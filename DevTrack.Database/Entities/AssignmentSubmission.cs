using System;
using System.Collections.Generic;

namespace DevTrack.Database.Entities;

public partial class AssignmentSubmission
{
    public int Id { get; set; }

    public int BatchId { get; set; }

    public int DeveloperId { get; set; }

    public DateOnly TrainingDate { get; set; }

    public bool? IsSubmitted { get; set; }

    public DateTime? SubmissionDate { get; set; }

    public int? Score { get; set; }

    public string? Feedback { get; set; }

    public virtual Batch Batch { get; set; } = null!;

    public virtual Developer Developer { get; set; } = null!;
}
