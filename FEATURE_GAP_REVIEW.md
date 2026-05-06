# DevTrack Feature Gap Review

**Source of truth:** [User Stories.md](User%20Stories.md)  
**Scope:** `DevTrack.WebApp` (MVC + Refit), `DevTrack.Api` (HTTP API), `DevTrack.Domain` (services), `DevTrack.Database` (EF entities)  
**Date:** 2026-05-06

This document maps each user story to the current implementation and calls out **missing**, **partial**, or **contract/behavior** gaps. It is a read-only assessment; it does not change product behavior.

---

## Summary

| Category | Count (approx.) |
|----------|-----------------|
| Largely implemented (mentor batch/attendance path) | 4–5 stories |
| Partial (UI or data exists; rules or API incomplete) | 5–6 stories |
| Missing (no end-to-end flow) | 6+ stories + optional future items |

**Highest-impact gaps**

1. **API vs WebApp contract:** `IBatchApiClient` exposes `POST /api/batches`, but `BatchesController` has no create action—batch create cannot succeed through the API client used by the WebApp.
2. **Calendar (US3):** Initial calendar is auto-generated in `BatchService.GenerateCalendarAsync` (class vs “Holiday” only). There is no update path for Assignment / Close / Holiday semantics after creation.
3. **Assignments (US5) & Leave (US6, US11, US14):** `AssignmentSubmission` and `LeaveRecord` exist in the database; there are no domain services, API endpoints, or MVC screens for CRUD, approval, or enforcement.
4. **Developer role (US8–US11):** The app is mentor-centric (batches, attendance). There is no developer self-service, identity, or “my batch” resolution.
5. **Rules:** Class-day-only attendance, one-batch-per-developer, monthly leave limits, and composite completion (US16) are not fully enforced in code.

---

## Story-by-story status

### Mentor stories

| # | Story | Status | Notes |
|---|--------|--------|--------|
| **1** | Create Training Batch | **Partial** | **Domain:** `BatchService.CreateBatchAsync` creates batch + calendar. **WebApp:** `BatchesController.Create` posts to `IBatchService` → Refit `CreateBatchAsync`. **API:** [`DevTrack.Api/Controllers/BatchesController.cs`](DevTrack.Api/Controllers/BatchesController.cs) has **no** `POST` create endpoint—Refit call will fail against API-only deployments. Create form captures duration via `TrainingMonths`; **DaysPerWeek** exists on entity/request but is not surfaced consistently in UI vs story wording (“teaching days per week”). |
| **2** | Assign Developers to Batch | **Partial** | **Implemented:** `ManageDevelopers` UI, `UpdateBatchAssignmentsAsync` replaces links for one batch. **Missing:** Enforcing **one developer → one batch** globally (no unique constraint on `DeveloperId` in `BatchDeveloper`; reassignment across batches not validated). |
| **3** | Configure Training Calendar | **Partial** | **Implemented:** Read-only schedule in [`Views/Attendance/Schedule.cshtml`](DevTrack.WebApp/Views/Attendance/Schedule.cshtml) using `TrainingCalendar` rows; badges support multiple day types if data existed. **Missing:** No POST/PATCH to edit day type, assignment title/due date, or close days after generation. `TrainingCalendarRequest` in [`TrainingModels.cs`](DevTrack.Domain/Features/Training/Models/TrainingModels.cs) is **unused**. Generation only produces **Class Day** vs **Holiday** (see `GenerateCalendarAsync` in [`BatchService.cs`](DevTrack.Domain/Features/Batches/BatchService.cs)). |
| **4** | Mark Attendance | **Partial** | **Implemented:** Bulk mark UI [`Views/Attendance/Mark.cshtml`](DevTrack.WebApp/Views/Attendance/Mark.cshtml), API [`TrainingController`](DevTrack.Api/Controllers/TrainingController.cs), persistence in [`TrainingService.MarkBulkAttendanceAsync`](DevTrack.Domain/Features/Training/TrainingService.cs). **Missing:** Server-side guarantee that marking is only on **class days** (`IsAttendanceRequired`); records can be written for any calendar date found. |
| **5** | Assign Weekly Assignments | **Missing** | DB entity [`AssignmentSubmission`](DevTrack.Database/Entities/AssignmentSubmission.cs) and calendar fields (`AssignmentTitle`, `AssignmentDueDate`) exist; no mentor flows for create assignment, review submissions, score, or feedback. |
| **6** | Approve Leave Requests | **Missing** | Entity [`LeaveRecord`](DevTrack.Database/Entities/LeaveRecord.cs) only; no approve/reject/list/monthly usage UI or API. No explicit **rejected** state (only `ApprovedBy` / `ApprovedAt`). |
| **7** | Monitor Attendance Percentage | **Partial** | **Implemented:** Summary table + thresholds Eligible / Warning / Not Eligible in [`TrainingService.GetAttendanceSummaryAsync`](DevTrack.Domain/Features/Training/TrainingService.cs) and [`Views/Attendance/Index.cshtml`](DevTrack.WebApp/Views/Attendance/Index.cshtml). **Gaps:** “Late” is not broken out in summary; percent uses `Present` only (see below). Status labels in doc use “Good” vs code “Eligible”—naming only. |

### Developer stories

| # | Story | Status | Notes |
|---|--------|--------|--------|
| **8** | View Training Schedule | **Missing (developer)** | Schedule exists for a **selected batch** (mentor path), not “my schedule” for a logged-in developer. No auth, no developer context. |
| **9** | View Attendance Record | **Missing (developer)** | Same: mentor batch attendance analytics only; no personal dashboard for a developer identity. |
| **10** | Submit Assignment | **Missing** | No file/upload flow, no submission API, no linkage UI despite `AssignmentSubmission` table. |
| **11** | Request Leave | **Missing** | No request form, quota display tied to batch rules, or approval status for developers. |

### System / automation stories

| # | Story | Status | Notes |
|---|--------|--------|--------|
| **12** | Auto Calculate Attendance % | **Partial** | Formula in summary: `Present / TotalClasses × 100` where `TotalClasses` counts calendar rows with `IsAttendanceRequired`. Matches spirit of US12. **Late** does not count as present unless manually stored as Present; summary does not count Late separately. |
| **13** | Auto Warning for Low Attendance | **Partial** | Thresholds ≥80 / 70–79 / &lt;70 implemented as Eligible / Warning / Not Eligible in summary. No proactive **notification** (email/in-app) beyond displaying status. |
| **14** | Track Monthly Leave Limit | **Missing** | `MonthlyLeaveAllowed` on batch (defaults in [`DevTrackDbContext`](DevTrack.Database/Entities/DevTrackDbContext.cs)) is not enforced or surfaced in calculations. |
| **15** | Replacement Class (optional) | **Missing** | Comment only on `TrainingCalendarRequest.DayType`; no model or workflow. |
| **16** | Training Completion Eligibility | **Missing** | No composite check (attendance + assignments + leave). |

---

## Additional findings

### Contract mismatch (batch create)

- [`IBatchApiClient`](DevTrack.WebApp/Services/IBatchApiClient.cs): `Post("/api/batches")` → `CreateBatchAsync`
- [`BatchesController`](DevTrack.Api/Controllers/BatchesController.cs): only `GET`, `GET/{id}`, `GET/{id}/developers`, `POST/{id}/assignments` — **no** `POST` for batch creation.

### Dashboard metric vs story formula

- [`DashboardService`](DevTrack.Domain/Features/Dashboard/DashboardService.cs) `OverallAttendanceRate` = present count / **all** attendance rows — not `present / total class days` per developer/batch as in US12.

### Security / roles

- [`DevTrack.Api/Program.cs`](DevTrack.Api/Program.cs): `UseAuthorization()` with no authentication scheme registered; endpoints are effectively open.
- WebApp: same pattern — no mentor vs developer separation.

### Data configuration

- [`DevTrackDbContext.OnConfiguring`](DevTrack.Database/Entities/DevTrackDbContext.cs) embeds a SQL Server connection string (scaffold warning); API uses `Configuration` connection string — avoid relying on both paths in production.

### UI nit

- [`_Layout.cshtml`](DevTrack.WebApp/Views/Shared/_Layout.cshtml): “New Batch” button has no `asp-controller`/`asp-action` link (cosmetic).

---

## Recommended implementation phases

These phases turn gaps into shippable increments. Order assumes you want the existing MVC + API architecture to stay aligned.

### Phase 1 — Foundation and API parity (unblock batch create)

1. Add **`POST /api/batches`** to `BatchesController`, delegating to `IBatchService.CreateBatchAsync` (same as domain already implements).
2. Smoke-test WebApp **Create Batch** end-to-end against the API.
3. Optionally fix layout “New Batch” to link to `Batches/Create`.

### Phase 2 — Training calendar editing (US3)

1. Add domain method(s) to update a `TrainingCalendar` row (day type, `IsAttendanceRequired`, assignment fields, remark).
2. Expose `PUT`/`PATCH` (or POST bulk) on API; add mentor UI (e.g. from Schedule view) to edit days.
3. Enforce **mark attendance only on class days** in `MarkBulkAttendanceAsync` (and optionally hide/disable Mark UI on non-class days).

### Phase 3 — Assignments (US5, US10)

1. Define assignment identity (calendar row vs separate `Assignment` table if needed).
2. Mentor: create/edit assignment metadata; review pipeline with score + feedback on `AssignmentSubmission` (or extend schema if file storage needed).
3. Developer: submission upload/resubmit — requires auth + storage strategy (blob/wwwroot/API).

### Phase 4 — Leave (US6, US11, US14)

1. Extend `LeaveRecord` if **reject** needs persistence (e.g. `Status` or `RejectedAt`).
2. Developer request flow + mentor approve/reject + monthly quota from `MonthlyLeaveAllowed`.
3. Tie approved leave to attendance (optional: auto “Leave” on class days) per product rules.

### Phase 5 — Identity and developer portal (US8, US9)

1. Add authentication (e.g. ASP.NET Core Identity or external provider) and roles (`Mentor`, `Developer`).
2. Map users to `Developer` records; “my batch” from `BatchDeveloper`.
3. Build developer-only pages: schedule, attendance summary, assignments, leave.

### Phase 6 — Completion and optional features (US15, US16)

1. Composite eligibility report per developer/batch.
2. Replacement class workflow if still desired.

---

## Files referenced (quick index)

| Area | Files |
|------|--------|
| API | `DevTrack.Api/Controllers/BatchesController.cs`, `TrainingController.cs`, `Program.cs` |
| Domain | `DevTrack.Domain/Features/Batches/BatchService.cs`, `Training/TrainingService.cs`, `Dashboard/DashboardService.cs`, `Training/Models/TrainingModels.cs` |
| WebApp | `DevTrack.WebApp/Controllers/BatchesController.cs`, `AttendanceController.cs`, `Services/IBatchApiClient.cs`, `Services/ITrainingApiClient.cs`, `Views/Attendance/*.cshtml`, `Views/Shared/_Layout.cshtml` |
| Database | `DevTrack.Database/Entities/*.cs`, `DevTrackDbContext.cs` |

---

## Conclusion

The codebase delivers a **mentor-oriented MVP**: batches, developer registry, generated calendar, attendance marking, and attendance summary with eligibility bands. **Assignments, leave, developer self-service, strict business rules, and API completeness** are the main gaps relative to [User Stories.md](User%20Stories.md). Start with **Phase 1** to align the API with the WebApp contract, then **Phase 2** for calendar configuration and attendance rule enforcement.
