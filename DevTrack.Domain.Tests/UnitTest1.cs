using DevTrack.Database.Entities;
using DevTrack.Domain.Features.Batches;
using DevTrack.Domain.Features.Training;
using DevTrack.Domain.Features.Training.Models;
using DevTrack.Shared;

namespace DevTrack.Domain.Tests;

public class FeatureRegressionTests
{
    [Fact]
    public async Task GetAttendanceSummary_ShouldComputePercentAndStatus()
    {
        using var db = TestDbContextFactory.Create();
        var batchId = await SeedBatchWithSingleDeveloperAsync(db);

        db.TrainingCalendars.AddRange(
            new TrainingCalendar { BatchId = batchId, TrainingDate = new DateOnly(2026, 5, 1), DayType = "Class Day", IsAttendanceRequired = true },
            new TrainingCalendar { BatchId = batchId, TrainingDate = new DateOnly(2026, 5, 2), DayType = "Holiday", IsAttendanceRequired = false },
            new TrainingCalendar { BatchId = batchId, TrainingDate = new DateOnly(2026, 5, 3), DayType = "Class Day", IsAttendanceRequired = true });

        db.AttendanceRecords.AddRange(
            new AttendanceRecord { BatchId = batchId, DeveloperId = 1, TrainingDate = new DateOnly(2026, 5, 1), Status = "Present" },
            new AttendanceRecord { BatchId = batchId, DeveloperId = 1, TrainingDate = new DateOnly(2026, 5, 3), Status = "Absent" });
        await db.SaveChangesAsync();

        var service = new TrainingService(db);
        var result = await service.GetAttendanceSummaryAsync(batchId, new PaginationRequest { PageNumber = 1, PageSize = 10 });
        var summary = Assert.Single(result.Data);

        Assert.Equal(2, summary.TotalClasses);
        Assert.Equal(1, summary.PresentDays);
        Assert.Equal(1, summary.AbsentDays);
        Assert.Equal(50m, summary.AttendancePercent);
        Assert.Equal("Not Eligible", summary.Status);
    }

    [Fact]
    public async Task MarkBulkAttendance_ShouldRejectNonClassDay()
    {
        using var db = TestDbContextFactory.Create();
        var batchId = await SeedBatchWithSingleDeveloperAsync(db);
        db.TrainingCalendars.Add(new TrainingCalendar
        {
            BatchId = batchId,
            TrainingDate = new DateOnly(2026, 5, 4),
            DayType = "Holiday",
            IsAttendanceRequired = false
        });
        await db.SaveChangesAsync();

        var service = new TrainingService(db);
        var result = await service.MarkBulkAttendanceAsync(new BulkAttendanceRequest
        {
            BatchId = batchId,
            TrainingDate = new DateOnly(2026, 5, 4),
            Markings =
            [
                new AttendanceMarkingModel { DeveloperId = 1, FullName = "Dev One", Status = "Present" }
            ]
        });

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task GetBatchDevelopers_ShouldNotListDeveloperAssignedToAnotherBatchAsAvailable()
    {
        using var db = TestDbContextFactory.Create();
        db.Batches.AddRange(
            new Batch { Id = 1, BatchName = "Batch A", StartDate = new DateOnly(2026, 1, 1), EndDate = new DateOnly(2026, 3, 1) },
            new Batch { Id = 2, BatchName = "Batch B", StartDate = new DateOnly(2026, 4, 1), EndDate = new DateOnly(2026, 6, 1) });
        db.Developers.Add(new Developer { Id = 1, FullName = "Dev One", DeveloperCode = "DEV001", IsActive = true });
        db.BatchDevelopers.Add(new BatchDeveloper { BatchId = 1, DeveloperId = 1 });
        await db.SaveChangesAsync();

        var service = new BatchService(db);
        var result = await service.GetBatchDevelopersAsync(2);
        Assert.NotNull(result.Data);
        Assert.DoesNotContain(result.Data, x => x.DeveloperId == 1 && !x.IsAssigned);
    }

    private static async Task<int> SeedBatchWithSingleDeveloperAsync(DevTrackDbContext db)
    {
        db.Batches.Add(new Batch
        {
            Id = 1,
            BatchName = "Batch A",
            StartDate = new DateOnly(2026, 5, 1),
            EndDate = new DateOnly(2026, 7, 1)
        });
        db.Developers.Add(new Developer
        {
            Id = 1,
            FullName = "Dev One",
            DeveloperCode = "DEV001",
            IsActive = true
        });
        db.BatchDevelopers.Add(new BatchDeveloper { BatchId = 1, DeveloperId = 1 });
        await db.SaveChangesAsync();
        return 1;
    }

}
