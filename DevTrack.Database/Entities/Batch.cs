using System;
using System.Collections.Generic;

namespace DevTrack.Database.Entities;

public partial class Batch
{
    public int Id { get; set; }

    public string BatchName { get; set; } = null!;

    public string? MentorName { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int? TrainingMonths { get; set; }

    public int? DaysPerWeek { get; set; }

    public int? MinAttendancePercent { get; set; }

    public int? MonthlyLeaveAllowed { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; } = new List<AssignmentSubmission>();

    public virtual ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();

    public virtual ICollection<BatchDeveloper> BatchDevelopers { get; set; } = new List<BatchDeveloper>();

    public virtual ICollection<LeaveRecord> LeaveRecords { get; set; } = new List<LeaveRecord>();

    public virtual ICollection<TrainingCalendar> TrainingCalendars { get; set; } = new List<TrainingCalendar>();
}
