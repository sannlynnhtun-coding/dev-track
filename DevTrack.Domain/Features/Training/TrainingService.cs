using DevTrack.Database.Entities;
using DevTrack.Domain.Features.Training.Models;
using DevTrack.Shared;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Domain.Features.Training;

public class TrainingService : ITrainingService
{
    private readonly DevTrackDbContext _db;

    public TrainingService(DevTrackDbContext db)
    {
        _db = db;
    }

    public async Task<Result<List<DateOnly>>> GetClassDaysAsync(int batchId)
    {
        var dates = await _db.TrainingCalendars
            .Where(c => c.BatchId == batchId && c.IsAttendanceRequired == true)
            .Select(c => c.TrainingDate)
            .OrderByDescending(d => d)
            .ToListAsync();
        return Result<List<DateOnly>>.Success(dates);
    }

    public async Task<PagedResult<TrainingCalendarResponse>> GetScheduleAsync(int batchId, PaginationRequest request)
    {
        var query = _db.TrainingCalendars
            .Where(c => c.BatchId == batchId);

        var totalCount = await query.CountAsync();

        var calendar = await query
            .OrderBy(c => c.TrainingDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var dates = calendar.Select(c => c.TrainingDate).ToList();

        var markedDates = await _db.AttendanceRecords
            .Where(a => a.BatchId == batchId && dates.Contains(a.TrainingDate))
            .Select(a => a.TrainingDate)
            .Distinct()
            .ToListAsync();

        var result = calendar.Select(c => new TrainingCalendarResponse
        {
            Id = c.Id,
            BatchId = c.BatchId,
            TrainingDate = c.TrainingDate,
            DayType = c.DayType,
            IsAttendanceRequired = c.IsAttendanceRequired ?? false,
            AssignmentTitle = c.AssignmentTitle,
            AssignmentDueDate = c.AssignmentDueDate,
            Remark = c.Remark,
            IsAttendanceMarked = markedDates.Contains(c.TrainingDate)
        }).ToList();

        var pagination = new Pagination(request.PageNumber, request.PageSize, totalCount);
        return PagedResult<TrainingCalendarResponse>.Success(result, pagination);
    }

    public async Task<Result<BulkAttendanceRequest>> GetAttendanceForDateAsync(int batchId, DateOnly date)
    {
        var calendar = await _db.TrainingCalendars
            .FirstOrDefaultAsync(c => c.BatchId == batchId && c.TrainingDate == date);

        if (calendar == null) return Result<BulkAttendanceRequest>.Failure("Date not found in calendar.");

        var batchDevelopers = await _db.BatchDevelopers
            .Where(bd => bd.BatchId == batchId)
            .Select(bd => bd.Developer)
            .ToListAsync();

        var existingAttendance = await _db.AttendanceRecords
            .Where(a => a.BatchId == batchId && a.TrainingDate == date)
            .ToDictionaryAsync(a => a.DeveloperId);

        var request = new BulkAttendanceRequest
        {
            BatchId = batchId,
            TrainingDate = date,
            Markings = batchDevelopers.Select(d => new AttendanceMarkingModel
            {
                DeveloperId = d.Id,
                FullName = d.FullName,
                Status = existingAttendance.ContainsKey(d.Id) ? existingAttendance[d.Id].Status : "Present",
                Remark = existingAttendance.ContainsKey(d.Id) ? existingAttendance[d.Id].Remark : ""
            }).OrderBy(m => m.FullName).ToList()
        };

        return Result<BulkAttendanceRequest>.Success(request);
    }

    public async Task<Result> MarkBulkAttendanceAsync(BulkAttendanceRequest request)
    {
        var calendar = await _db.TrainingCalendars
            .FirstOrDefaultAsync(c => c.BatchId == request.BatchId && c.TrainingDate == request.TrainingDate);

        if (calendar == null)
        {
            return Result.Failure("Date not found in calendar.");
        }

        if (!(calendar.IsAttendanceRequired ?? false))
        {
            return Result.Failure("Attendance can only be marked on class days.");
        }

        var existing = await _db.AttendanceRecords
            .Where(a => a.BatchId == request.BatchId && a.TrainingDate == request.TrainingDate)
            .ToListAsync();

        _db.AttendanceRecords.RemoveRange(existing);

        foreach (var item in request.Markings)
        {
            _db.AttendanceRecords.Add(new AttendanceRecord
            {
                BatchId = request.BatchId,
                DeveloperId = item.DeveloperId,
                TrainingDate = request.TrainingDate,
                Status = item.Status,
                Remark = item.Remark,
                CreatedAt = DateTime.Now
            });
        }

        await _db.SaveChangesAsync();
        return Result.Success("Attendance marked successfully.");
    }

    public async Task<PagedResult<AttendanceSummaryResponse>> GetAttendanceSummaryAsync(int batchId, PaginationRequest request)
    {
        var totalClasses = await _db.TrainingCalendars
            .CountAsync(c => c.BatchId == batchId && (c.IsAttendanceRequired ?? false));

        var query = _db.BatchDevelopers
            .Where(bd => bd.BatchId == batchId)
            .Select(bd => new { bd.Developer.Id, bd.Developer.FullName, bd.Developer.DeveloperCode });

        var totalCount = await query.CountAsync();
        
        var developers = await query
            .OrderBy(bd => bd.FullName)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var developerIds = developers.Select(d => d.Id).ToList();

        var attendance = await _db.AttendanceRecords
            .Where(a => a.BatchId == batchId && developerIds.Contains(a.DeveloperId))
            .ToListAsync();

        var result = new List<AttendanceSummaryResponse>();

        foreach (var dev in developers)
        {
            var devAttendance = attendance.Where(a => a.DeveloperId == dev.Id).ToList();
            var presentCount = devAttendance.Count(a => a.Status == "Present");
            var percent = totalClasses > 0 ? (decimal)presentCount * 100 / totalClasses : 0;

            result.Add(new AttendanceSummaryResponse
            {
                DeveloperId = dev.Id,
                FullName = dev.FullName,
                DeveloperCode = dev.DeveloperCode,
                TotalClasses = totalClasses,
                PresentDays = presentCount,
                AbsentDays = devAttendance.Count(a => a.Status == "Absent"),
                LeaveDays = devAttendance.Count(a => a.Status == "Leave"),
                AttendancePercent = Math.Round(percent, 2),
                Status = percent >= 80 ? "Eligible" : (percent >= 70 ? "Warning" : "Not Eligible")
            });
        }

        var pagination = new Pagination(request.PageNumber, request.PageSize, totalCount);
        return PagedResult<AttendanceSummaryResponse>.Success(result, pagination);
    }

    public async Task<Result<List<AttendanceSummaryResponse>>> GetFullAttendanceSummaryAsync(int batchId)
    {
        var totalClasses = await _db.TrainingCalendars
            .CountAsync(c => c.BatchId == batchId && (c.IsAttendanceRequired ?? false));

        var developers = await _db.BatchDevelopers
            .Where(bd => bd.BatchId == batchId)
            .Select(bd => new { bd.Developer.Id, bd.Developer.FullName, bd.Developer.DeveloperCode })
            .OrderBy(bd => bd.FullName)
            .ToListAsync();

        var attendance = await _db.AttendanceRecords
            .Where(a => a.BatchId == batchId)
            .ToListAsync();

        var result = new List<AttendanceSummaryResponse>();

        foreach (var dev in developers)
        {
            var devAttendance = attendance.Where(a => a.DeveloperId == dev.Id).ToList();
            var presentCount = devAttendance.Count(a => a.Status == "Present");
            var percent = totalClasses > 0 ? (decimal)presentCount * 100 / totalClasses : 0;

            result.Add(new AttendanceSummaryResponse
            {
                DeveloperId = dev.Id,
                FullName = dev.FullName,
                DeveloperCode = dev.DeveloperCode,
                TotalClasses = totalClasses,
                PresentDays = presentCount,
                AbsentDays = devAttendance.Count(a => a.Status == "Absent"),
                LeaveDays = devAttendance.Count(a => a.Status == "Leave"),
                AttendancePercent = Math.Round(percent, 2),
                Status = percent >= 80 ? "Eligible" : (percent >= 70 ? "Warning" : "Not Eligible")
            });
        }

        return Result<List<AttendanceSummaryResponse>>.Success(result);
    }
}
