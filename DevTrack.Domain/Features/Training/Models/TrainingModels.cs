using DevTrack.Database.Entities;
using DevTrack.Shared;

namespace DevTrack.Domain.Features.Training.Models;

public class TrainingCalendarRequest
{
    public int BatchId { get; set; }
    public DateOnly TrainingDate { get; set; }
    public string DayType { get; set; } = "Class Day"; // Class Day, Assignment, Close Day, Holiday, Replacement
    public bool IsAttendanceRequired { get; set; } = true;
    public string? AssignmentTitle { get; set; }
    public DateOnly? AssignmentDueDate { get; set; }
    public string? Remark { get; set; }
}

public class AttendanceRequest
{
    public int BatchId { get; set; }
    public int DeveloperId { get; set; }
    public DateOnly TrainingDate { get; set; }
    public string Status { get; set; } = "Present"; // Present, Absent, Leave, Late
    public string? Remark { get; set; }
}

public class AttendanceSummaryResponse
{
    public int DeveloperId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? DeveloperCode { get; set; }
    public int TotalClasses { get; set; }
    public int PresentDays { get; set; }
    public int AbsentDays { get; set; }
    public int LeaveDays { get; set; }
    public decimal AttendancePercent { get; set; }
    public string Status { get; set; } = "Eligible"; // Eligible, Warning, Not Eligible
}

public class AttendanceMarkingModel
{
    public int DeveloperId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Status { get; set; } = "Present";
    public string? Remark { get; set; }
}

public class BulkAttendanceRequest
{
    public int BatchId { get; set; }
    public DateOnly TrainingDate { get; set; }
    public List<AttendanceMarkingModel> Markings { get; set; } = new();
}

public class TrainingCalendarResponse
{
    public int Id { get; set; }
    public int BatchId { get; set; }
    public DateOnly TrainingDate { get; set; }
    public string DayType { get; set; } = string.Empty;
    public bool IsAttendanceRequired { get; set; }
    public string? AssignmentTitle { get; set; }
    public DateOnly? AssignmentDueDate { get; set; }
    public string? Remark { get; set; }
    public bool IsAttendanceMarked { get; set; }
}
