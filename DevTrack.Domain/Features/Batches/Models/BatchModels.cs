using DevTrack.Database.Entities;
using DevTrack.Shared;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Domain.Features.Batches.Models;

public class BatchRequest
{
    public string BatchName { get; set; } = string.Empty;
    public string? MentorName { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int TrainingMonths { get; set; }
    public int DaysPerWeek { get; set; } = 2; // Count
    public List<DayOfWeek> SelectedDays { get; set; } = new(); // Which days (Mon, Wed...)
    public int MinAttendancePercent { get; set; } = 80;
    public int MonthlyLeaveAllowed { get; set; } = 2;
}

public class BatchResponse
{
    public int Id { get; set; }
    public string BatchName { get; set; } = string.Empty;
    public string? MentorName { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int? TrainingMonths { get; set; }
    public string DateRange => $"{StartDate:MMM dd, yyyy} - {EndDate:MMM dd, yyyy}";
}

public class BatchAssignmentModel
{
    public int DeveloperId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? DeveloperCode { get; set; }
    public bool IsAssigned { get; set; }
}
