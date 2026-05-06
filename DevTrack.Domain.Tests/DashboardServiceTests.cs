using DevTrack.Database.Entities;
using DevTrack.Domain.Features.Dashboard;

namespace DevTrack.Domain.Tests;

public class DashboardServiceTests
{
    [Fact]
    public async Task GetDashboardData_ShouldReturnTotalsRatesAndRecentData()
    {
        using var db = TestDbContextFactory.Create();
        var today = DateOnly.FromDateTime(DateTime.Today);

        db.Batches.AddRange(
            new Batch { Id = 1, BatchName = "Active", MentorName = "M1", StartDate = today.AddDays(-1), EndDate = today.AddDays(10), CreatedAt = DateTime.Today.AddDays(-1) },
            new Batch { Id = 2, BatchName = "Past", MentorName = "M2", StartDate = today.AddDays(-30), EndDate = today.AddDays(-10), CreatedAt = DateTime.Today.AddDays(-2) });
        db.Developers.AddRange(
            new Developer { Id = 1, FullName = "Dev A", IsActive = true },
            new Developer { Id = 2, FullName = "Dev B", IsActive = true });
        db.BatchDevelopers.Add(new BatchDeveloper { BatchId = 1, DeveloperId = 1 });
        db.AttendanceRecords.AddRange(
            new AttendanceRecord { BatchId = 1, DeveloperId = 1, TrainingDate = today.AddDays(-1), Status = "Present" },
            new AttendanceRecord { BatchId = 1, DeveloperId = 1, TrainingDate = today, Status = "Absent" });
        await db.SaveChangesAsync();

        var service = new DashboardService(db);
        var result = await service.GetDashboardDataAsync();

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.TotalBatches);
        Assert.Equal(2, result.Data.TotalDevelopers);
        Assert.Equal(1, result.Data.ActiveBatches);
        Assert.Equal(50m, result.Data.OverallAttendanceRate);
        Assert.NotEmpty(result.Data.RecentBatches);
        Assert.NotEmpty(result.Data.AttendanceTrends);
    }
}
