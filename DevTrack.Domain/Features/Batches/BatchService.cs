using DevTrack.Database.Entities;
using DevTrack.Domain.Features.Batches.Models;
using DevTrack.Shared;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Domain.Features.Batches;

public class BatchService : IBatchService
{
    private readonly DevTrackDbContext _db;

    public BatchService(DevTrackDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<BatchResponse>> GetBatchesAsync(PaginationRequest request)
    {
        var query = _db.Batches.AsNoTracking();
        
        var totalCount = await query.CountAsync();
        
        var batches = await query
            .OrderByDescending(b => b.StartDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(b => new BatchResponse
            {
                Id = b.Id,
                BatchName = b.BatchName,
                MentorName = b.MentorName,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                TrainingMonths = b.TrainingMonths
            })
            .ToListAsync();

        var pagination = new Pagination(request.PageNumber, request.PageSize, totalCount);
        return PagedResult<BatchResponse>.Success(batches, pagination);
    }

    public async Task<Result<BatchResponse>> GetBatchByIdAsync(int id)
    {
        var batch = await _db.Batches
            .Select(b => new BatchResponse
            {
                Id = b.Id,
                BatchName = b.BatchName,
                MentorName = b.MentorName,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                TrainingMonths = b.TrainingMonths
            })
            .FirstOrDefaultAsync(b => b.Id == id);

        if (batch == null) return Result<BatchResponse>.Failure("Batch not found.");
        return Result<BatchResponse>.Success(batch);
    }

    public async Task<Result<BatchResponse>> CreateBatchAsync(BatchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.BatchName))
            return Result<BatchResponse>.Failure("Batch name is required.");

        var batch = new Batch
        {
            BatchName = request.BatchName,
            MentorName = request.MentorName,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TrainingMonths = request.TrainingMonths,
            DaysPerWeek = request.DaysPerWeek,
            MinAttendancePercent = request.MinAttendancePercent,
            MonthlyLeaveAllowed = request.MonthlyLeaveAllowed,
            CreatedAt = DateTime.Now
        };

        _db.Batches.Add(batch);
        await _db.SaveChangesAsync();

        // Auto-generate Calendar
        await GenerateCalendarAsync(batch.Id, request.StartDate, request.EndDate, request.SelectedDays);

        return Result<BatchResponse>.Success(new BatchResponse
        {
            Id = batch.Id,
            BatchName = batch.BatchName,
            MentorName = batch.MentorName,
            StartDate = batch.StartDate,
            EndDate = batch.EndDate,
            TrainingMonths = batch.TrainingMonths
        }, "Batch created successfully.");
    }

    public async Task<Result<List<BatchAssignmentModel>>> GetBatchDevelopersAsync(int batchId)
    {
        var assigned = await _db.BatchDevelopers
            .Where(bd => bd.BatchId == batchId)
            .Select(bd => new BatchAssignmentModel
            {
                DeveloperId = bd.DeveloperId,
                FullName = bd.Developer.FullName,
                DeveloperCode = bd.Developer.DeveloperCode,
                IsAssigned = true
            })
            .ToListAsync();

        var assignedIds = assigned.Select(a => a.DeveloperId).ToList();
        var globallyAssignedIds = await _db.BatchDevelopers
            .Select(bd => bd.DeveloperId)
            .Distinct()
            .ToListAsync();

        var available = await _db.Developers
            .Where(d => (d.IsActive ?? true) &&
                        !assignedIds.Contains(d.Id) &&
                        !globallyAssignedIds.Contains(d.Id))
            .Select(d => new BatchAssignmentModel
            {
                DeveloperId = d.Id,
                FullName = d.FullName,
                DeveloperCode = d.DeveloperCode,
                IsAssigned = false
            })
            .ToListAsync();

        var result = assigned.Concat(available).OrderBy(x => x.FullName).ToList();
        return Result<List<BatchAssignmentModel>>.Success(result);
    }

    public async Task<Result> UpdateBatchAssignmentsAsync(int batchId, List<int> selectedDeveloperIds)
    {
        var existing = await _db.BatchDevelopers.Where(bd => bd.BatchId == batchId).ToListAsync();
        _db.BatchDevelopers.RemoveRange(existing);

        foreach (var devId in selectedDeveloperIds)
        {
            _db.BatchDevelopers.Add(new BatchDeveloper
            {
                BatchId = batchId,
                DeveloperId = devId
            });
        }

        await _db.SaveChangesAsync();
        return Result.Success("Assignments updated successfully.");
    }

    private async Task GenerateCalendarAsync(int batchId, DateOnly start, DateOnly end, List<DayOfWeek> trainingDays)
    {
        var currentDate = start;
        while (currentDate <= end)
        {
            var isClassDay = trainingDays.Contains(currentDate.DayOfWeek);
            var calendarEntry = new TrainingCalendar
            {
                BatchId = batchId,
                TrainingDate = currentDate,
                DayType = isClassDay ? "Class Day" : "Holiday",
                Remark = isClassDay ? "Regular Training Session" : "Weekend/Holiday",
                IsAttendanceRequired = isClassDay
            };

            _db.TrainingCalendars.Add(calendarEntry);
            currentDate = currentDate.AddDays(1);
        }

        await _db.SaveChangesAsync();
    }
}
