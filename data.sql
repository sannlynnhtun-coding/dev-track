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
