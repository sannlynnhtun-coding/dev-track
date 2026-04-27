using System;
using System.Collections.Generic;

namespace DevTrack.Database.Entities;

public partial class TrainingCalendar
{
    public int Id { get; set; }

    public int BatchId { get; set; }

    public DateOnly TrainingDate { get; set; }

    public string DayType { get; set; } = null!;

    public bool? IsAttendanceRequired { get; set; }

    public string? AssignmentTitle { get; set; }

    public DateOnly? AssignmentDueDate { get; set; }

    public string? Remark { get; set; }

    public virtual Batch Batch { get; set; } = null!;
}
