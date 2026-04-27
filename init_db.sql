USE master;
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'DevTrack')
BEGIN
    CREATE DATABASE DevTrack;
END;
GO

USE DevTrack;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Developers')
BEGIN
    CREATE TABLE Developers (
        Id INT IDENTITY PRIMARY KEY,
        DeveloperCode NVARCHAR(50) NULL,
        FullName NVARCHAR(150) NOT NULL,
        Email NVARCHAR(150) NULL,
        Phone NVARCHAR(50) NULL,
        IsActive BIT DEFAULT 1,
        CreatedAt DATETIME DEFAULT GETDATE()
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Batches')
BEGIN
    CREATE TABLE Batches (
        Id INT IDENTITY PRIMARY KEY,
        BatchName NVARCHAR(150) NOT NULL,
        MentorName NVARCHAR(150),
        StartDate DATE NOT NULL,
        EndDate DATE NOT NULL,
        TrainingMonths INT,
        DaysPerWeek INT DEFAULT 2,
        MinAttendancePercent INT DEFAULT 80,
        MonthlyLeaveAllowed INT DEFAULT 2,
        CreatedAt DATETIME DEFAULT GETDATE()
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BatchDevelopers')
BEGIN
    CREATE TABLE BatchDevelopers (
        Id INT IDENTITY PRIMARY KEY,
        BatchId INT NOT NULL,
        DeveloperId INT NOT NULL,

        FOREIGN KEY (BatchId) REFERENCES Batches(Id),
        FOREIGN KEY (DeveloperId) REFERENCES Developers(Id)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TrainingCalendar')
BEGIN
    CREATE TABLE TrainingCalendar (
        Id INT IDENTITY PRIMARY KEY,
        BatchId INT NOT NULL,
        TrainingDate DATE NOT NULL,
        DayType NVARCHAR(50) NOT NULL,
        IsAttendanceRequired BIT DEFAULT 0,
        AssignmentTitle NVARCHAR(250) NULL,
        AssignmentDueDate DATE NULL,
        Remark NVARCHAR(500) NULL,
        FOREIGN KEY (BatchId) REFERENCES Batches(Id)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AttendanceRecords')
BEGIN
    CREATE TABLE AttendanceRecords (
        Id INT IDENTITY PRIMARY KEY,
        BatchId INT NOT NULL,
        DeveloperId INT NOT NULL,
        TrainingDate DATE NOT NULL,
        Status NVARCHAR(50) NOT NULL,
        Remark NVARCHAR(500) NULL,
        CreatedAt DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (BatchId) REFERENCES Batches(Id),
        FOREIGN KEY (DeveloperId) REFERENCES Developers(Id)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AssignmentSubmissions')
BEGIN
    CREATE TABLE AssignmentSubmissions (
        Id INT IDENTITY PRIMARY KEY,
        BatchId INT NOT NULL,
        DeveloperId INT NOT NULL,
        TrainingDate DATE NOT NULL,
        IsSubmitted BIT DEFAULT 0,
        SubmissionDate DATETIME NULL,
        Score INT NULL,
        Feedback NVARCHAR(500) NULL,
        FOREIGN KEY (BatchId) REFERENCES Batches(Id),
        FOREIGN KEY (DeveloperId) REFERENCES Developers(Id)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LeaveRecords')
BEGIN
    CREATE TABLE LeaveRecords (
        Id INT IDENTITY PRIMARY KEY,
        BatchId INT NOT NULL,
        DeveloperId INT NOT NULL,
        LeaveDate DATE NOT NULL,
        Reason NVARCHAR(500),
        ApprovedBy NVARCHAR(150),
        ApprovedAt DATETIME NULL,
        FOREIGN KEY (BatchId) REFERENCES Batches(Id),
        FOREIGN KEY (DeveloperId) REFERENCES Developers(Id)
    );
END
