# 📘 Core Tables (Simple MSSQL Schema)

## 1️⃣ Developers

```sql
CREATE TABLE Developers (
    Id INT IDENTITY PRIMARY KEY,
    DeveloperCode NVARCHAR(50) NULL,
    FullName NVARCHAR(150) NOT NULL,
    Email NVARCHAR(150) NULL,
    Phone NVARCHAR(50) NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE()
);
```

---

## 2️⃣ Batches (Training Class)

Example: `.NET Batch 001`

```sql
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
```

---

## 3️⃣ BatchDevelopers (Batch Members)

One batch → many developers

```sql
CREATE TABLE BatchDevelopers (
    Id INT IDENTITY PRIMARY KEY,
    BatchId INT NOT NULL,
    DeveloperId INT NOT NULL,

    FOREIGN KEY (BatchId) REFERENCES Batches(Id),
    FOREIGN KEY (DeveloperId) REFERENCES Developers(Id)
);
```

---

## 4️⃣ TrainingCalendar (Important Table)

Controls:

* Class Day
* Assignment Day
* Close Day
* Holiday
* Replacement Day

```sql
CREATE TABLE TrainingCalendar (
    Id INT IDENTITY PRIMARY KEY,
    BatchId INT NOT NULL,
    TrainingDate DATE NOT NULL,

    DayType NVARCHAR(50) NOT NULL,
    -- Class
    -- Assignment
    -- Close
    -- Holiday
    -- Replacement

    IsAttendanceRequired BIT DEFAULT 0,

    AssignmentTitle NVARCHAR(250) NULL,
    AssignmentDueDate DATE NULL,

    Remark NVARCHAR(500) NULL,

    FOREIGN KEY (BatchId) REFERENCES Batches(Id)
);
```

Example:

| Date | Type       | Attendance |
| ---- | ---------- | ---------- |
| Mon  | Class      | Yes        |
| Wed  | Assignment | No         |
| Fri  | Close      | No         |

---

## 5️⃣ AttendanceRecords

Only for **Class Day**

```sql
CREATE TABLE AttendanceRecords (
    Id INT IDENTITY PRIMARY KEY,
    BatchId INT NOT NULL,
    DeveloperId INT NOT NULL,
    TrainingDate DATE NOT NULL,

    Status NVARCHAR(50) NOT NULL,
    -- Present
    -- Absent
    -- Leave
    -- Late

    Remark NVARCHAR(500) NULL,

    CreatedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (BatchId) REFERENCES Batches(Id),
    FOREIGN KEY (DeveloperId) REFERENCES Developers(Id)
);
```

---

## 6️⃣ AssignmentSubmissions

Only for **Assignment Day**

```sql
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
```

---

## 7️⃣ LeaveRecords

Monthly leave tracking

```sql
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
```

---

# 📊 Attendance % Calculation Example Query

```sql
SELECT
    d.FullName,
    COUNT(a.Id) AS TotalClasses,
    SUM(CASE WHEN a.Status = 'Present' THEN 1 ELSE 0 END) AS PresentDays,
    CAST(
        SUM(CASE WHEN a.Status = 'Present' THEN 1 ELSE 0 END) * 100.0
        / COUNT(a.Id)
    AS DECIMAL(5,2)) AS AttendancePercent
FROM AttendanceRecords a
JOIN Developers d ON d.Id = a.DeveloperId
WHERE a.BatchId = 1
GROUP BY d.FullName;
```

---

# ✅ What This Schema Supports

This structure already handles:

✔ 2-day/week training
✔ 2–3 month batches
✔ Dynamic schedule changes
✔ Assignment days
✔ Close days
✔ Leave limits
✔ 80% attendance validation
✔ Mentor tracking developers
✔ Submission scoring
✔ Attendance dashboard readiness

---
