using DevTrack.Database.Entities;
using DevTrack.Domain.Features.Dashboard.Models;
using DevTrack.Shared;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Domain.Features.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly DevTrackDbContext _db;

    public DashboardService(DevTrackDbContext db)
    {
        _db = db;
    }

    public async Task<Result<DashboardResponse>> GetDashboardDataAsync()
    {
        var totalBatches = await _db.Batches.CountAsync();
        var totalDevelopers = await _db.Developers.CountAsync();
        
        var today = DateOnly.FromDateTime(DateTime.Today);
        var activeBatches = await _db.Batches.CountAsync(b => b.StartDate <= today && b.EndDate >= today);

        var allAttendance = await _db.AttendanceRecords.ToListAsync();
        var overallRate = allAttendance.Any() 
            ? (decimal)allAttendance.Count(a => a.Status == "Present") * 100 / allAttendance.Count 
            : 0;

        var recentBatches = await _db.Batches
            .OrderByDescending(b => b.CreatedAt)
            .Take(5)
            .Select(b => new BatchSummaryModel
            {
                Id = b.Id,
                BatchName = b.BatchName,
                MentorName = b.MentorName ?? "Unassigned",
                DeveloperCount = _db.BatchDevelopers.Count(bd => bd.BatchId == b.Id)
            })
            .ToListAsync();

        // Trend for last 7 active days
        var trends = await _db.AttendanceRecords
            .GroupBy(a => a.TrainingDate)
            .OrderByDescending(g => g.Key)
            .Take(7)
            .Select(g => new AttendanceTrendModel
            {
                Date = g.Key.ToString("MM/dd"),
                PresentCount = g.Count(a => a.Status == "Present")
            })
            .Reverse()
            .ToListAsync();

        var response = new DashboardResponse
        {
            TotalBatches = totalBatches,
            TotalDevelopers = totalDevelopers,
            ActiveBatches = activeBatches,
            OverallAttendanceRate = Math.Round(overallRate, 1),
            RecentBatches = recentBatches,
            AttendanceTrends = trends
        };

        return Result<DashboardResponse>.Success(response);
    }
}
