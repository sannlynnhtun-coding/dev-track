using DevTrack.Database.Entities;
using DevTrack.Domain.Features.Developers.Models;
using DevTrack.Shared;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Domain.Features.Developers;

public class DeveloperService : IDeveloperService
{
    private readonly DevTrackDbContext _db;

    public DeveloperService(DevTrackDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<DeveloperResponse>> GetDevelopersAsync(PaginationRequest request)
    {
        var query = _db.Developers.AsNoTracking();
        
        var totalCount = await query.CountAsync();
        
        var developers = await query
            .OrderBy(d => d.FullName)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(d => new DeveloperResponse
            {
                Id = d.Id,
                FullName = d.FullName,
                DeveloperCode = d.DeveloperCode,
                Email = d.Email,
                IsActive = d.IsActive ?? true
            })
            .ToListAsync();

        var pagination = new Pagination(request.PageNumber, request.PageSize, totalCount);
        return PagedResult<DeveloperResponse>.Success(developers, pagination);
    }

    public async Task<Result<DeveloperDetailResponse>> GetDeveloperByIdAsync(int id)
    {
        var developer = await _db.Developers
            .AsNoTracking()
            .Where(d => d.Id == id)
            .Select(d => new DeveloperDetailResponse
            {
                Id = d.Id,
                FullName = d.FullName,
                DeveloperCode = d.DeveloperCode,
                Email = d.Email,
                Phone = d.Phone,
                IsActive = d.IsActive ?? true,
                CreatedAt = d.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (developer == null)
        {
            return Result<DeveloperDetailResponse>.Failure("Developer not found.");
        }

        var currentBatch = await _db.BatchDevelopers
            .AsNoTracking()
            .Where(bd => bd.DeveloperId == id)
            .OrderByDescending(bd => bd.Batch.StartDate)
            .Select(bd => new DeveloperBatchSummary
            {
                BatchId = bd.Batch.Id,
                BatchName = bd.Batch.BatchName,
                MentorName = bd.Batch.MentorName,
                StartDate = bd.Batch.StartDate,
                EndDate = bd.Batch.EndDate,
                MinAttendancePercent = bd.Batch.MinAttendancePercent
            })
            .FirstOrDefaultAsync();

        developer.CurrentBatch = currentBatch;

        if (currentBatch == null)
        {
            return Result<DeveloperDetailResponse>.Success(developer);
        }

        var totalClasses = await _db.TrainingCalendars
            .AsNoTracking()
            .CountAsync(c => c.BatchId == currentBatch.BatchId && (c.IsAttendanceRequired ?? false));

        var attendance = await _db.AttendanceRecords
            .AsNoTracking()
            .Where(a => a.BatchId == currentBatch.BatchId && a.DeveloperId == id)
            .ToListAsync();

        var presentDays = attendance.Count(a => a.Status == "Present");
        var attendancePercent = totalClasses > 0 ? (decimal)presentDays * 100 / totalClasses : 0;

        developer.Attendance = new DeveloperAttendanceSummary
        {
            TotalClasses = totalClasses,
            PresentDays = presentDays,
            AbsentDays = attendance.Count(a => a.Status == "Absent"),
            LeaveDays = attendance.Count(a => a.Status == "Leave"),
            LateDays = attendance.Count(a => a.Status == "Late"),
            AttendancePercent = Math.Round(attendancePercent, 2),
            Status = attendancePercent >= 80 ? "Eligible" : (attendancePercent >= 70 ? "Warning" : "Not Eligible")
        };

        developer.RecentAttendance = attendance
            .OrderByDescending(a => a.TrainingDate)
            .Take(10)
            .Select(a => new DeveloperAttendanceRecordResponse
            {
                TrainingDate = a.TrainingDate,
                Status = a.Status,
                Remark = a.Remark
            })
            .ToList();

        return Result<DeveloperDetailResponse>.Success(developer);
    }

    public async Task<Result<DeveloperResponse>> CreateDeveloperAsync(DeveloperRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
            return Result<DeveloperResponse>.Failure("Full name is required.");

        var developer = new Developer
        {
            FullName = request.FullName,
            DeveloperCode = request.DeveloperCode,
            Email = request.Email,
            Phone = request.Phone,
            IsActive = request.IsActive,
            CreatedAt = DateTime.Now
        };

        _db.Developers.Add(developer);
        await _db.SaveChangesAsync();

        return Result<DeveloperResponse>.Success(new DeveloperResponse
        {
            Id = developer.Id,
            FullName = developer.FullName,
            DeveloperCode = developer.DeveloperCode,
            Email = developer.Email,
            IsActive = developer.IsActive ?? true
        }, "Developer created successfully.");
    }
}
