using System;
using System.Collections.Generic;

namespace DevTrack.Database.Entities;

public partial class AttendanceRecord
{
    public int Id { get; set; }

    public int BatchId { get; set; }

    public int DeveloperId { get; set; }

    public DateOnly TrainingDate { get; set; }

    public string Status { get; set; } = null!;

    public string? Remark { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Batch Batch { get; set; } = null!;

    public virtual Developer Developer { get; set; } = null!;
}
