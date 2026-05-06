using DevTrack.Database.Entities;
using DevTrack.Domain.Features.Training;
using DevTrack.Domain.Features.Training.Models;
using DevTrack.Shared;

namespace DevTrack.Domain.Tests;

public class TrainingServiceTests
{
    [Fact]
    public async Task GetClassDays_ShouldReturnOnlyAttendanceRequiredDates()
    {
        using var db = TestDbContextFactory.Create();
        await SeedBatchAndDeveloper(db);
        db.TrainingCalendars.AddRange(
            new TrainingCalendar { BatchId = 1, TrainingDate = new DateOnly(2026, 5, 2), DayType = "Holiday", IsAttendanceRequired = false },
            new TrainingCalendar { BatchId = 1, TrainingDate = new DateOnly(2026, 5, 1), DayType = "Class Day", IsAttendanceRequired = true },
            new TrainingCalendar { BatchId = 1, TrainingDate = new DateOnly(2026, 5, 3), DayType = "Class Day", IsAttendanceRequired = true });
        await db.SaveChangesAsync();

        var service = new TrainingService(db);
        var result = await service.GetClassDaysAsync(1);
        Assert.NotNull(result.Data);

        Assert.Equal(2, result.Data.Count);
        Assert.Equal(new DateOnly(2026, 5, 3), result.Data[0]);
        Assert.Equal(new DateOnly(2026, 5, 1), result.Data[1]);
    }

    [Fact]
    public async Task GetAttendanceForDate_ShouldPrefillExistingAndDefaults()
    {
        using var db = TestDbContextFactory.Create();
        await SeedBatchAndDeveloper(db);
        db.Developers.Add(new Developer { Id = 2, FullName = "Dev Two", IsActive = true });
        db.BatchDevelopers.Add(new BatchDeveloper { BatchId = 1, DeveloperId = 2 });
        db.TrainingCalendars.Add(new TrainingCalendar { BatchId = 1, TrainingDate = new DateOnly(2026, 5, 1), DayType = "Class Day", IsAttendanceRequired = true });
        db.AttendanceRecords.Add(new AttendanceRecord { BatchId = 1, DeveloperId = 1, TrainingDate = new DateOnly(2026, 5, 1), Status = "Late", Remark = "Traffic" });
        await db.SaveChangesAsync();

        var service = new TrainingService(db);
        var result = await service.GetAttendanceForDateAsync(1, new DateOnly(2026, 5, 1));

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Markings.Count);
        Assert.Contains(result.Data.Markings, x => x.DeveloperId == 1 && x.Status == "Late" && x.Remark == "Traffic");
        Assert.Contains(result.Data.Markings, x => x.DeveloperId == 2 && x.Status == "Present");
    }

    [Fact]
    public async Task MarkBulkAttendance_ShouldReplacePreviousRecords()
    {
        using var db = TestDbContextFactory.Create();
        await SeedBatchAndDeveloper(db);
        db.TrainingCalendars.Add(new TrainingCalendar { BatchId = 1, TrainingDate = new DateOnly(2026, 5, 1), DayType = "Class Day", IsAttendanceRequired = true });
        db.AttendanceRecords.Add(new AttendanceRecord { BatchId = 1, DeveloperId = 1, TrainingDate = new DateOnly(2026, 5, 1), Status = "Absent" });
        await db.SaveChangesAsync();

        var service = new TrainingService(db);
        var result = await service.MarkBulkAttendanceAsync(new BulkAttendanceRequest
        {
            BatchId = 1,
            TrainingDate = new DateOnly(2026, 5, 1),
            Markings = [new AttendanceMarkingModel { DeveloperId = 1, FullName = "Dev One", Status = "Present" }]
        });

        Assert.True(result.IsSuccess);
        Assert.Single(db.AttendanceRecords);
        Assert.Equal("Present", db.AttendanceRecords.Single().Status);
    }

    [Fact]
    public async Task GetSchedule_ShouldMarkDatesWithRecordedAttendance()
    {
        using var db = TestDbContextFactory.Create();
        await SeedBatchAndDeveloper(db);
        db.TrainingCalendars.AddRange(
            new TrainingCalendar { BatchId = 1, TrainingDate = new DateOnly(2026, 5, 1), DayType = "Class Day", IsAttendanceRequired = true },
            new TrainingCalendar { BatchId = 1, TrainingDate = new DateOnly(2026, 5, 2), DayType = "Class Day", IsAttendanceRequired = true });
        db.AttendanceRecords.Add(new AttendanceRecord { BatchId = 1, DeveloperId = 1, TrainingDate = new DateOnly(2026, 5, 1), Status = "Present" });
        await db.SaveChangesAsync();

        var service = new TrainingService(db);
        var result = await service.GetScheduleAsync(1, new PaginationRequest { PageNumber = 1, PageSize = 10 });

        Assert.Equal(2, result.Data.Count);
        Assert.True(result.Data.Single(x => x.TrainingDate == new DateOnly(2026, 5, 1)).IsAttendanceMarked);
        Assert.False(result.Data.Single(x => x.TrainingDate == new DateOnly(2026, 5, 2)).IsAttendanceMarked);
    }

    private static async Task SeedBatchAndDeveloper(DevTrackDbContext db)
    {
        db.Batches.Add(new Batch { Id = 1, BatchName = "Batch", StartDate = new DateOnly(2026, 5, 1), EndDate = new DateOnly(2026, 6, 1) });
        db.Developers.Add(new Developer { Id = 1, FullName = "Dev One", IsActive = true });
        db.BatchDevelopers.Add(new BatchDeveloper { BatchId = 1, DeveloperId = 1 });
        await db.SaveChangesAsync();
    }
}
