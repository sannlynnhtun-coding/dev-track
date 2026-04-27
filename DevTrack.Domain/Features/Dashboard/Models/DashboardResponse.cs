namespace DevTrack.Domain.Features.Dashboard.Models;

public class DashboardResponse
{
    public int TotalBatches { get; set; }
    public int TotalDevelopers { get; set; }
    public int ActiveBatches { get; set; }
    public decimal OverallAttendanceRate { get; set; }
    public List<BatchSummaryModel> RecentBatches { get; set; } = new();
    public List<AttendanceTrendModel> AttendanceTrends { get; set; } = new();
}

public class BatchSummaryModel
{
    public int Id { get; set; }
    public string BatchName { get; set; } = string.Empty;
    public string MentorName { get; set; } = string.Empty;
    public int DeveloperCount { get; set; }
    public decimal AverageAttendance { get; set; }
}

public class AttendanceTrendModel
{
    public string Date { get; set; } = string.Empty;
    public int PresentCount { get; set; }
}
