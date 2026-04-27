USE DevTrack;
GO

-- Insert Developers
SET IDENTITY_INSERT Developers ON;
INSERT INTO Developers (Id, DeveloperCode, FullName, Email, Phone, IsActive, CreatedAt) VALUES
(1, 'DEV001', 'Alice Johnson', 'alice.j@example.com', '555-0101', 1, GETDATE()),
(2, 'DEV002', 'Bob Smith', 'bob.smith@example.com', '555-0102', 1, GETDATE()),
(3, 'DEV003', 'Charlie Davis', 'charlie.d@example.com', '555-0103', 1, GETDATE()),
(4, 'DEV004', 'Diana Prince', 'diana.p@example.com', '555-0104', 1, GETDATE()),
(5, 'DEV005', 'Ethan Hunt', 'ethan.h@example.com', '555-0105', 1, GETDATE()),
(6, 'DEV006', 'Fiona Gallagher', 'fiona.g@example.com', '555-0106', 1, GETDATE()),
(7, 'DEV007', 'George Miller', 'george.m@example.com', '555-0107', 1, GETDATE()),
(8, 'DEV008', 'Hannah Abbott', 'hannah.a@example.com', '555-0108', 1, GETDATE());
SET IDENTITY_INSERT Developers OFF;

-- Insert Batches
SET IDENTITY_INSERT Batches ON;
INSERT INTO Batches (Id, BatchName, MentorName, StartDate, EndDate, TrainingMonths, DaysPerWeek, MinAttendancePercent, MonthlyLeaveAllowed, CreatedAt) VALUES
(1, '.NET Full Stack - Spring 2026', 'John Doe', '2026-03-01', '2026-05-31', 3, 2, 80, 2, GETDATE()),
(2, 'React & Node.js - Summer 2026', 'Jane Smith', '2026-06-01', '2026-08-31', 3, 3, 85, 1, GETDATE());
SET IDENTITY_INSERT Batches OFF;

-- Assign Developers to Batches
SET IDENTITY_INSERT BatchDevelopers ON;
INSERT INTO BatchDevelopers (Id, BatchId, DeveloperId) VALUES
(1, 1, 1), -- Alice in .NET
(2, 1, 2), -- Bob in .NET
(3, 1, 3), -- Charlie in .NET
(4, 1, 4), -- Diana in .NET
(5, 2, 5), -- Ethan in React
(6, 2, 6), -- Fiona in React
(7, 2, 7), -- George in React
(8, 2, 8); -- Hannah in React
SET IDENTITY_INSERT BatchDevelopers OFF;

-- Insert some Training Calendar entries (for the first week of .NET batch)
SET IDENTITY_INSERT TrainingCalendar ON;
INSERT INTO TrainingCalendar (Id, BatchId, TrainingDate, DayType, IsAttendanceRequired, AssignmentTitle, Remark) VALUES
(1, 1, '2026-03-02', 'Class Day', 1, 'Intro to C#', 'First day of training'),
(2, 1, '2026-03-04', 'Class Day', 1, 'Generics & Collections', 'Advanced C# topics'),
(3, 1, '2026-03-06', 'Class Day', 1, 'LINQ Basics', 'Introduction to querying'),
(4, 1, '2026-03-07', 'Weekend', 0, NULL, 'Saturday Holiday'),
(5, 1, '2026-03-08', 'Weekend', 0, NULL, 'Sunday Holiday');
SET IDENTITY_INSERT TrainingCalendar OFF;

-- Insert some Attendance Records
SET IDENTITY_INSERT AttendanceRecords ON;
INSERT INTO AttendanceRecords (Id, BatchId, DeveloperId, TrainingDate, Status, Remark, CreatedAt) VALUES
(1, 1, 1, '2026-03-02', 'Present', 'Arrived on time', GETDATE()),
(2, 1, 2, '2026-03-02', 'Present', NULL, GETDATE()),
(3, 1, 3, '2026-03-02', 'Absent', 'No reason provided', GETDATE()),
(4, 1, 4, '2026-03-02', 'Present', NULL, GETDATE()),
(5, 1, 1, '2026-03-04', 'Present', NULL, GETDATE()),
(6, 1, 2, '2026-03-04', 'Leave', 'Medical issues', GETDATE()),
(7, 1, 3, '2026-03-04', 'Present', NULL, GETDATE()),
(8, 1, 4, '2026-03-04', 'Present', NULL, GETDATE());
SET IDENTITY_INSERT AttendanceRecords OFF;

-- More Training Calendar entries (Batch 1: SQL and Tests, Batch 2: Initial Schedule)
SET IDENTITY_INSERT TrainingCalendar ON;
INSERT INTO TrainingCalendar (Id, BatchId, TrainingDate, DayType, IsAttendanceRequired, AssignmentTitle, AssignmentDueDate, Remark) VALUES
(6, 1, '2026-03-09', 'Class Day', 1, NULL, NULL, 'SQL Fundamentals'),
(7, 1, '2026-03-11', 'Assignment Day', 0, 'C# Logic Test', '2026-03-15', 'Weekly test'),
(8, 1, '2026-03-13', 'Class Day', 1, NULL, NULL, 'SQL Advanced'),
(9, 1, '2026-03-14', 'Weekend', 0, NULL, NULL, 'Saturday Holiday'),
(10, 1, '2026-03-15', 'Weekend', 0, NULL, NULL, 'Sunday Holiday'),
(11, 1, '2026-03-16', 'Close Day', 0, NULL, NULL, 'Public Holiday'),
(12, 2, '2026-06-01', 'Class Day', 1, NULL, NULL, 'Intro to Web Development'),
(13, 2, '2026-06-03', 'Class Day', 1, NULL, NULL, 'HTML5 & CSS3 Layouts'),
(14, 2, '2026-06-05', 'Assignment Day', 0, 'First Portfolio Website', '2026-06-12', 'Design your own portfolio');
SET IDENTITY_INSERT TrainingCalendar OFF;

-- More Attendance Records
SET IDENTITY_INSERT AttendanceRecords ON;
INSERT INTO AttendanceRecords (Id, BatchId, DeveloperId, TrainingDate, Status, Remark, CreatedAt) VALUES
(9, 1, 1, '2026-03-09', 'Present', NULL, GETDATE()),
(10, 1, 3, '2026-03-09', 'Present', NULL, GETDATE()),
(11, 1, 4, '2026-03-09', 'Present', NULL, GETDATE()),
(12, 1, 1, '2026-03-13', 'Present', NULL, GETDATE()),
(13, 1, 2, '2026-03-13', 'Late', 'Heavy traffic', GETDATE()),
(14, 1, 3, '2026-03-13', 'Present', NULL, GETDATE()),
(15, 1, 4, '2026-03-13', 'Present', NULL, GETDATE()),
(16, 2, 5, '2026-06-01', 'Present', 'Excited to start', GETDATE()),
(17, 2, 6, '2026-06-01', 'Present', NULL, GETDATE()),
(18, 2, 7, '2026-06-01', 'Present', NULL, GETDATE()),
(19, 2, 8, '2026-06-01', 'Present', NULL, GETDATE()),
(20, 2, 5, '2026-06-03', 'Present', NULL, GETDATE()),
(21, 2, 7, '2026-06-03', 'Present', NULL, GETDATE()),
(22, 2, 8, '2026-06-03', 'Present', NULL, GETDATE());
SET IDENTITY_INSERT AttendanceRecords OFF;

-- Insert Leave Records
SET IDENTITY_INSERT LeaveRecords ON;
INSERT INTO LeaveRecords (Id, BatchId, DeveloperId, LeaveDate, Reason, ApprovedBy, ApprovedAt) VALUES
(1, 1, 2, '2026-03-09', 'Sick leave - Flu', 'John Doe', GETDATE()),
(2, 2, 6, '2026-06-03', 'Family Emergency', 'Jane Smith', GETDATE());
SET IDENTITY_INSERT LeaveRecords OFF;

-- Insert Assignment Submissions
SET IDENTITY_INSERT AssignmentSubmissions ON;
INSERT INTO AssignmentSubmissions (Id, BatchId, DeveloperId, TrainingDate, IsSubmitted, SubmissionDate, Score, Feedback) VALUES
(1, 1, 1, '2026-03-11', 1, '2026-03-11 14:00:00', 95, 'Excellent logic implementation!'),
(2, 1, 2, '2026-03-11', 1, '2026-03-12 09:30:00', 88, 'Good work, but watch out for edge cases.'),
(3, 1, 3, '2026-03-11', 1, '2026-03-15 23:55:00', 70, 'Submitted just before deadline.'),
(4, 2, 5, '2026-06-05', 1, '2026-06-06 10:00:00', 90, 'Very creative design!'),
(5, 2, 6, '2026-06-05', 0, NULL, NULL, NULL);
SET IDENTITY_INSERT AssignmentSubmissions OFF;

