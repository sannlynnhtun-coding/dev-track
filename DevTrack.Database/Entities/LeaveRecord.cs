using System;
using System.Collections.Generic;

namespace DevTrack.Database.Entities;

public partial class LeaveRecord
{
    public int Id { get; set; }

    public int BatchId { get; set; }

    public int DeveloperId { get; set; }

    public DateOnly LeaveDate { get; set; }

    public string? Reason { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public virtual Batch Batch { get; set; } = null!;

    public virtual Developer Developer { get; set; } = null!;
}
