using DevTrack.Database.Entities;
using DevTrack.Shared;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Domain.Features.Developers.Models;

public class DeveloperRequest
{
    public string FullName { get; set; } = string.Empty;
    public string? DeveloperCode { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; } = true;
}

public class DeveloperResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? DeveloperCode { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
}

public class DeveloperDetailResponse : DeveloperResponse
{
    public string? Phone { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DeveloperBatchSummary? CurrentBatch { get; set; }
    public DeveloperAttendanceSummary Attendance { get; set; } = new();
    public List<DeveloperAttendanceRecordResponse> RecentAttendance { get; set; } = new();
}

public class DeveloperBatchSummary
{
    public int BatchId { get; set; }
    public string BatchName { get; set; } = string.Empty;
    public string? MentorName { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int? MinAttendancePercent { get; set; }
    public string DateRange => $"{StartDate:MMM dd, yyyy} - {EndDate:MMM dd, yyyy}";
}

public class DeveloperAttendanceSummary
{
    public int TotalClasses { get; set; }
    public int PresentDays { get; set; }
    public int AbsentDays { get; set; }
    public int LeaveDays { get; set; }
    public int LateDays { get; set; }
    public decimal AttendancePercent { get; set; }
    public string Status { get; set; } = "No Batch";
}

public class DeveloperAttendanceRecordResponse
{
    public DateOnly TrainingDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Remark { get; set; }
}
