using System;
using System.Collections.Generic;

namespace DevTrack.Database.Entities;

public partial class Developer
{
    public int Id { get; set; }

    public string? DeveloperCode { get; set; }

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; } = new List<AssignmentSubmission>();

    public virtual ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();

    public virtual ICollection<BatchDeveloper> BatchDevelopers { get; set; } = new List<BatchDeveloper>();

    public virtual ICollection<LeaveRecord> LeaveRecords { get; set; } = new List<LeaveRecord>();
}
