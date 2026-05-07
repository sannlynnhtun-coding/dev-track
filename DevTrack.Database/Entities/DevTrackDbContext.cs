using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Database.Entities;

public partial class DevTrackDbContext : DbContext
{
    public DevTrackDbContext()
    {
    }

    public DevTrackDbContext(DbContextOptions<DevTrackDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }

    public virtual DbSet<AttendanceRecord> AttendanceRecords { get; set; }

    public virtual DbSet<Batch> Batches { get; set; }

    public virtual DbSet<BatchDeveloper> BatchDevelopers { get; set; }

    public virtual DbSet<Developer> Developers { get; set; }

    public virtual DbSet<LeaveRecord> LeaveRecords { get; set; }

    public virtual DbSet<TrainingCalendar> TrainingCalendars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlServer("Server=.;Database=DevTrack;User Id=sa;Password=sasa@123;TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssignmentSubmission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Assignme__3214EC07C7160389");

            entity.Property(e => e.Feedback).HasMaxLength(500);
            entity.Property(e => e.IsSubmitted).HasDefaultValue(false);
            entity.Property(e => e.SubmissionDate).HasColumnType("datetime");

            entity.HasOne(d => d.Batch).WithMany(p => p.AssignmentSubmissions)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Assignmen__Batch__4E88ABD4");

            entity.HasOne(d => d.Developer).WithMany(p => p.AssignmentSubmissions)
                .HasForeignKey(d => d.DeveloperId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Assignmen__Devel__4F7CD00D");
        });

        modelBuilder.Entity<AttendanceRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attendan__3214EC07B2CC7D85");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Remark).HasMaxLength(500);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Batch).WithMany(p => p.AttendanceRecords)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attendanc__Batch__49C3F6B7");

            entity.HasOne(d => d.Developer).WithMany(p => p.AttendanceRecords)
                .HasForeignKey(d => d.DeveloperId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attendanc__Devel__4AB81AF0");
        });

        modelBuilder.Entity<Batch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Batches__3214EC07255BD5CC");

            entity.Property(e => e.BatchName).HasMaxLength(150);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DaysPerWeek).HasDefaultValue(2);
            entity.Property(e => e.MentorName).HasMaxLength(150);
            entity.Property(e => e.MinAttendancePercent).HasDefaultValue(80);
            entity.Property(e => e.MonthlyLeaveAllowed).HasDefaultValue(2);
        });

        modelBuilder.Entity<BatchDeveloper>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BatchDev__3214EC07CA06462F");

            entity.HasOne(d => d.Batch).WithMany(p => p.BatchDevelopers)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BatchDeve__Batch__412EB0B6");

            entity.HasOne(d => d.Developer).WithMany(p => p.BatchDevelopers)
                .HasForeignKey(d => d.DeveloperId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BatchDeve__Devel__4222D4EF");
        });

        modelBuilder.Entity<Developer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Develope__3214EC07B9DBC095");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeveloperCode).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Phone).HasMaxLength(50);
        });

        modelBuilder.Entity<LeaveRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveRec__3214EC074BB7C5D7");

            entity.Property(e => e.ApprovedAt).HasColumnType("datetime");
            entity.Property(e => e.ApprovedBy).HasMaxLength(150);
            entity.Property(e => e.Reason).HasMaxLength(500);

            entity.HasOne(d => d.Batch).WithMany(p => p.LeaveRecords)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LeaveReco__Batch__52593CB8");

            entity.HasOne(d => d.Developer).WithMany(p => p.LeaveRecords)
                .HasForeignKey(d => d.DeveloperId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LeaveReco__Devel__534D60F1");
        });

        modelBuilder.Entity<TrainingCalendar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Training__3214EC07D6993CB5");

            entity.ToTable("TrainingCalendar");

            entity.Property(e => e.AssignmentTitle).HasMaxLength(250);
            entity.Property(e => e.DayType).HasMaxLength(50);
            entity.Property(e => e.IsAttendanceRequired).HasDefaultValue(false);
            entity.Property(e => e.Remark).HasMaxLength(500);

            entity.HasOne(d => d.Batch).WithMany(p => p.TrainingCalendars)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TrainingC__Batch__45F365D3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
