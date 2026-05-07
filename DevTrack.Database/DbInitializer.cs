using System;
using System.Linq;
using DevTrack.Database.Entities;

namespace DevTrack.Database;

public static class DbInitializer
{
    public static void Initialize(DevTrackDbContext context)
    {
        // Look for any developers.
        if (context.Developers.Any())
        {
            return;   // DB has been seeded
        }

        var developers = new Developer[]
        {
            new Developer { DeveloperCode = "DEV001", FullName = "Alice Johnson", Email = "alice.j@example.com", Phone = "555-0101", IsActive = true, CreatedAt = DateTime.Now },
            new Developer { DeveloperCode = "DEV002", FullName = "Bob Smith", Email = "bob.smith@example.com", Phone = "555-0102", IsActive = true, CreatedAt = DateTime.Now },
            new Developer { DeveloperCode = "DEV003", FullName = "Charlie Davis", Email = "charlie.d@example.com", Phone = "555-0103", IsActive = true, CreatedAt = DateTime.Now },
            new Developer { DeveloperCode = "DEV004", FullName = "Diana Prince", Email = "diana.p@example.com", Phone = "555-0104", IsActive = true, CreatedAt = DateTime.Now },
            new Developer { DeveloperCode = "DEV005", FullName = "Ethan Hunt", Email = "ethan.h@example.com", Phone = "555-0105", IsActive = true, CreatedAt = DateTime.Now },
            new Developer { DeveloperCode = "DEV006", FullName = "Fiona Gallagher", Email = "fiona.g@example.com", Phone = "555-0106", IsActive = true, CreatedAt = DateTime.Now },
            new Developer { DeveloperCode = "DEV007", FullName = "George Miller", Email = "george.m@example.com", Phone = "555-0107", IsActive = true, CreatedAt = DateTime.Now },
            new Developer { DeveloperCode = "DEV008", FullName = "Hannah Abbott", Email = "hannah.a@example.com", Phone = "555-0108", IsActive = true, CreatedAt = DateTime.Now }
        };

        context.Developers.AddRange(developers);
        context.SaveChanges();

        var batches = new Batch[]
        {
            new Batch { BatchName = ".NET Full Stack - Spring 2026", MentorName = "John Doe", StartDate = new DateOnly(2026, 3, 1), EndDate = new DateOnly(2026, 5, 31), TrainingMonths = 3, DaysPerWeek = 2, MinAttendancePercent = 80, MonthlyLeaveAllowed = 2, CreatedAt = DateTime.Now },
            new Batch { BatchName = "React & Node.js - Summer 2026", MentorName = "Jane Smith", StartDate = new DateOnly(2026, 6, 1), EndDate = new DateOnly(2026, 8, 31), TrainingMonths = 3, DaysPerWeek = 3, MinAttendancePercent = 85, MonthlyLeaveAllowed = 1, CreatedAt = DateTime.Now }
        };

        context.Batches.AddRange(batches);
        context.SaveChanges();

        var batchDevelopers = new BatchDeveloper[]
        {
            new BatchDeveloper { BatchId = batches[0].Id, DeveloperId = developers[0].Id },
            new BatchDeveloper { BatchId = batches[0].Id, DeveloperId = developers[1].Id },
            new BatchDeveloper { BatchId = batches[0].Id, DeveloperId = developers[2].Id },
            new BatchDeveloper { BatchId = batches[0].Id, DeveloperId = developers[3].Id },
            new BatchDeveloper { BatchId = batches[1].Id, DeveloperId = developers[4].Id },
            new BatchDeveloper { BatchId = batches[1].Id, DeveloperId = developers[5].Id },
            new BatchDeveloper { BatchId = batches[1].Id, DeveloperId = developers[6].Id },
            new BatchDeveloper { BatchId = batches[1].Id, DeveloperId = developers[7].Id }
        };

        context.BatchDevelopers.AddRange(batchDevelopers);
        context.SaveChanges();

        var calendars = new TrainingCalendar[]
        {
            new TrainingCalendar { BatchId = batches[0].Id, TrainingDate = new DateOnly(2026, 3, 2), DayType = "Class Day", IsAttendanceRequired = true, AssignmentTitle = "Intro to C#", Remark = "First day of training" },
            new TrainingCalendar { BatchId = batches[0].Id, TrainingDate = new DateOnly(2026, 3, 4), DayType = "Class Day", IsAttendanceRequired = true, AssignmentTitle = "Generics & Collections", Remark = "Advanced C# topics" },
            new TrainingCalendar { BatchId = batches[0].Id, TrainingDate = new DateOnly(2026, 3, 6), DayType = "Class Day", IsAttendanceRequired = true, AssignmentTitle = "LINQ Basics", Remark = "Introduction to querying" },
            new TrainingCalendar { BatchId = batches[0].Id, TrainingDate = new DateOnly(2026, 3, 7), DayType = "Weekend", IsAttendanceRequired = false, Remark = "Saturday Holiday" },
            new TrainingCalendar { BatchId = batches[0].Id, TrainingDate = new DateOnly(2026, 3, 8), DayType = "Weekend", IsAttendanceRequired = false, Remark = "Sunday Holiday" },
            new TrainingCalendar { BatchId = batches[0].Id, TrainingDate = new DateOnly(2026, 3, 9), DayType = "Class Day", IsAttendanceRequired = true, Remark = "SQL Fundamentals" },
            new TrainingCalendar { BatchId = batches[0].Id, TrainingDate = new DateOnly(2026, 3, 11), DayType = "Assignment Day", IsAttendanceRequired = false, AssignmentTitle = "C# Logic Test", AssignmentDueDate = new DateOnly(2026, 3, 15), Remark = "Weekly test" },
            new TrainingCalendar { BatchId = batches[0].Id, TrainingDate = new DateOnly(2026, 3, 13), DayType = "Class Day", IsAttendanceRequired = true, Remark = "SQL Advanced" },
            new TrainingCalendar { BatchId = batches[1].Id, TrainingDate = new DateOnly(2026, 6, 1), DayType = "Class Day", IsAttendanceRequired = true, Remark = "Intro to Web Development" },
            new TrainingCalendar { BatchId = batches[1].Id, TrainingDate = new DateOnly(2026, 6, 3), DayType = "Class Day", IsAttendanceRequired = true, Remark = "HTML5 & CSS3 Layouts" },
            new TrainingCalendar { BatchId = batches[1].Id, TrainingDate = new DateOnly(2026, 6, 5), DayType = "Assignment Day", IsAttendanceRequired = false, AssignmentTitle = "First Portfolio Website", AssignmentDueDate = new DateOnly(2026, 6, 12), Remark = "Design your own portfolio" }
        };

        context.TrainingCalendars.AddRange(calendars);
        context.SaveChanges();

        var attendances = new AttendanceRecord[]
        {
            new AttendanceRecord { BatchId = batches[0].Id, DeveloperId = developers[0].Id, TrainingDate = new DateOnly(2026, 3, 2), Status = "Present", Remark = "Arrived on time", CreatedAt = DateTime.Now },
            new AttendanceRecord { BatchId = batches[0].Id, DeveloperId = developers[1].Id, TrainingDate = new DateOnly(2026, 3, 2), Status = "Present", CreatedAt = DateTime.Now },
            new AttendanceRecord { BatchId = batches[0].Id, DeveloperId = developers[2].Id, TrainingDate = new DateOnly(2026, 3, 2), Status = "Absent", Remark = "No reason provided", CreatedAt = DateTime.Now },
            new AttendanceRecord { BatchId = batches[0].Id, DeveloperId = developers[3].Id, TrainingDate = new DateOnly(2026, 3, 2), Status = "Present", CreatedAt = DateTime.Now },
            new AttendanceRecord { BatchId = batches[0].Id, DeveloperId = developers[0].Id, TrainingDate = new DateOnly(2026, 3, 4), Status = "Present", CreatedAt = DateTime.Now },
            new AttendanceRecord { BatchId = batches[0].Id, DeveloperId = developers[1].Id, TrainingDate = new DateOnly(2026, 3, 4), Status = "Leave", Remark = "Medical issues", CreatedAt = DateTime.Now },
            new AttendanceRecord { BatchId = batches[0].Id, DeveloperId = developers[2].Id, TrainingDate = new DateOnly(2026, 3, 4), Status = "Present", CreatedAt = DateTime.Now },
            new AttendanceRecord { BatchId = batches[0].Id, DeveloperId = developers[3].Id, TrainingDate = new DateOnly(2026, 3, 4), Status = "Present", CreatedAt = DateTime.Now },
            new AttendanceRecord { BatchId = batches[1].Id, DeveloperId = developers[4].Id, TrainingDate = new DateOnly(2026, 6, 1), Status = "Present", Remark = "Excited to start", CreatedAt = DateTime.Now },
            new AttendanceRecord { BatchId = batches[1].Id, DeveloperId = developers[5].Id, TrainingDate = new DateOnly(2026, 6, 1), Status = "Present", CreatedAt = DateTime.Now },
            new AttendanceRecord { BatchId = batches[1].Id, DeveloperId = developers[6].Id, TrainingDate = new DateOnly(2026, 6, 1), Status = "Present", CreatedAt = DateTime.Now },
            new AttendanceRecord { BatchId = batches[1].Id, DeveloperId = developers[7].Id, TrainingDate = new DateOnly(2026, 6, 1), Status = "Present", CreatedAt = DateTime.Now }
        };

        context.AttendanceRecords.AddRange(attendances);
        context.SaveChanges();

        var leaves = new LeaveRecord[]
        {
            new LeaveRecord { BatchId = batches[0].Id, DeveloperId = developers[1].Id, LeaveDate = new DateOnly(2026, 3, 9), Reason = "Sick leave - Flu", ApprovedBy = "John Doe", ApprovedAt = DateTime.Now },
            new LeaveRecord { BatchId = batches[1].Id, DeveloperId = developers[5].Id, LeaveDate = new DateOnly(2026, 6, 3), Reason = "Family Emergency", ApprovedBy = "Jane Smith", ApprovedAt = DateTime.Now }
        };

        context.LeaveRecords.AddRange(leaves);
        context.SaveChanges();

        var submissions = new AssignmentSubmission[]
        {
            new AssignmentSubmission { BatchId = batches[0].Id, DeveloperId = developers[0].Id, TrainingDate = new DateOnly(2026, 3, 11), IsSubmitted = true, SubmissionDate = new DateTime(2026, 3, 11, 14, 0, 0), Score = 95, Feedback = "Excellent logic implementation!" },
            new AssignmentSubmission { BatchId = batches[0].Id, DeveloperId = developers[1].Id, TrainingDate = new DateOnly(2026, 3, 11), IsSubmitted = true, SubmissionDate = new DateTime(2026, 3, 12, 9, 30, 0), Score = 88, Feedback = "Good work, but watch out for edge cases." },
            new AssignmentSubmission { BatchId = batches[1].Id, DeveloperId = developers[4].Id, TrainingDate = new DateOnly(2026, 6, 5), IsSubmitted = true, SubmissionDate = new DateTime(2026, 6, 6, 10, 0, 0), Score = 90, Feedback = "Very creative design!" }
        };

        context.AssignmentSubmissions.AddRange(submissions);
        context.SaveChanges();
    }
}
