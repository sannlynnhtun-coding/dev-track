using DevTrack.Database.Entities;
using DevTrack.Domain.Features.Batches;
using DevTrack.Domain.Features.Batches.Models;
using DevTrack.Shared;

namespace DevTrack.Domain.Tests;

public class BatchServiceTests
{
    [Fact]
    public async Task CreateBatch_ShouldGenerateCalendarEntries()
    {
        using var db = TestDbContextFactory.Create();
        var service = new BatchService(db);

        var response = await service.CreateBatchAsync(new BatchRequest
        {
            BatchName = "Batch 01",
            MentorName = "Mentor",
            StartDate = new DateOnly(2026, 5, 5),
            EndDate = new DateOnly(2026, 5, 7),
            TrainingMonths = 2,
            SelectedDays = [DayOfWeek.Tuesday, DayOfWeek.Thursday]
        });

        Assert.True(response.IsSuccess);
        Assert.Equal(3, db.TrainingCalendars.Count());
        Assert.Equal(2, db.TrainingCalendars.Count(x => x.IsAttendanceRequired == true));
    }

    [Fact]
    public async Task CreateBatch_ShouldFailWhenBatchNameMissing()
    {
        using var db = TestDbContextFactory.Create();
        var service = new BatchService(db);

        var response = await service.CreateBatchAsync(new BatchRequest
        {
            BatchName = "",
            StartDate = new DateOnly(2026, 5, 1),
            EndDate = new DateOnly(2026, 6, 1)
        });

        Assert.True(response.IsFailure);
        Assert.Equal("Batch name is required.", response.Message);
    }

    [Fact]
    public async Task UpdateBatchAssignments_ShouldReplaceAssignments()
    {
        using var db = TestDbContextFactory.Create();
        db.Batches.Add(new Batch { Id = 1, BatchName = "B1", StartDate = new DateOnly(2026, 1, 1), EndDate = new DateOnly(2026, 2, 1) });
        db.Developers.AddRange(
            new Developer { Id = 1, FullName = "A", IsActive = true },
            new Developer { Id = 2, FullName = "B", IsActive = true });
        db.BatchDevelopers.Add(new BatchDeveloper { BatchId = 1, DeveloperId = 1 });
        await db.SaveChangesAsync();

        var service = new BatchService(db);
        var result = await service.UpdateBatchAssignmentsAsync(1, [2]);

        Assert.True(result.IsSuccess);
        Assert.DoesNotContain(db.BatchDevelopers, x => x.BatchId == 1 && x.DeveloperId == 1);
        Assert.Contains(db.BatchDevelopers, x => x.BatchId == 1 && x.DeveloperId == 2);
    }

    [Fact]
    public async Task GetBatches_ShouldReturnPagedAndSortedByStartDateDesc()
    {
        using var db = TestDbContextFactory.Create();
        db.Batches.AddRange(
            new Batch { Id = 1, BatchName = "Old", StartDate = new DateOnly(2026, 1, 1), EndDate = new DateOnly(2026, 2, 1) },
            new Batch { Id = 2, BatchName = "New", StartDate = new DateOnly(2026, 3, 1), EndDate = new DateOnly(2026, 4, 1) });
        await db.SaveChangesAsync();

        var service = new BatchService(db);
        var result = await service.GetBatchesAsync(new PaginationRequest { PageNumber = 1, PageSize = 10 });

        Assert.Equal(2, result.Data.Count);
        Assert.Equal("New", result.Data[0].BatchName);
        Assert.Equal("Old", result.Data[1].BatchName);
    }
}
