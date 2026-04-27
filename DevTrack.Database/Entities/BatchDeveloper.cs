using System;
using System.Collections.Generic;

namespace DevTrack.Database.Entities;

public partial class BatchDeveloper
{
    public int Id { get; set; }

    public int BatchId { get; set; }

    public int DeveloperId { get; set; }

    public virtual Batch Batch { get; set; } = null!;

    public virtual Developer Developer { get; set; } = null!;
}
