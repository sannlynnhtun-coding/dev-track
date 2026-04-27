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

    public async Task<Result<List<TrainingCalendarResponse>>> GetScheduleAsync(int batchId)
    {
        var calendar = await _db.TrainingCalendars
            .Where(c => c.BatchId == batchId)
            .OrderBy(c => c.TrainingDate)
            .ToListAsync();

        var markedDates = await _db.AttendanceRecords
            .Where(a => a.BatchId == batchId)
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

        return Result<List<TrainingCalendarResponse>>.Success(result);
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

    public async Task<Result<List<AttendanceSummaryResponse>>> GetAttendanceSummaryAsync(int batchId)
    {
        var totalClasses = await _db.TrainingCalendars
            .CountAsync(c => c.BatchId == batchId && c.DayType == "Class Day");

        var developers = await _db.BatchDevelopers
            .Where(bd => bd.BatchId == batchId)
            .Select(bd => new { bd.Developer.Id, bd.Developer.FullName, bd.Developer.DeveloperCode })
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

        return Result<List<AttendanceSummaryResponse>>.Success(result.OrderBy(r => r.FullName).ToList());
    }
}
