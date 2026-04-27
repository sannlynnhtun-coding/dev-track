# 👨‍🏫 Mentor User Stories

## 1. Create Training Batch

**User Story**

> As a mentor, I want to create a training batch so that I can manage developers by group.

**Acceptance Criteria**

* Mentor can create batch name
* Select start date and end date
* Set training duration (2 or 3 months)
* Set teaching days per week
* Set minimum attendance (default 80%)

---

## 2. Assign Developers to Batch

**User Story**

> As a mentor, I want to assign developers to a batch so that I can track their attendance and progress.

**Acceptance Criteria**

* Mentor can add/remove developers
* One developer can belong to one batch
* Developer list visible in dashboard

---

## 3. Configure Training Calendar

**User Story**

> As a mentor, I want to define class days, assignment days, and close days so that attendance rules are applied correctly.

**Acceptance Criteria**

Mentor can mark a day as:

* Class Day ✅ attendance required
* Assignment Day 📝 submission required
* Close Day ❌ no attendance required
* Holiday 📅 optional

---

## 4. Mark Attendance

**User Story**

> As a mentor, I want to record attendance on class days so that developer participation is tracked.

**Acceptance Criteria**

Mentor can mark:

* Present
* Absent
* Leave
* Late

Only allowed on **Class Day**

---

## 5. Assign Weekly Assignments

**User Story**

> As a mentor, I want to assign tasks on assignment days so that developers can practice what they learned.

**Acceptance Criteria**

Mentor can:

* create assignment title
* set due date
* add description
* review submissions
* give score
* give feedback

---

## 6. Approve Leave Requests

**User Story**

> As a mentor, I want to approve leave requests so that leave limits are controlled properly.

**Acceptance Criteria**

Mentor can:

* approve leave
* reject leave
* see monthly leave usage
* see remaining leave balance

---

## 7. Monitor Attendance Percentage

**User Story**

> As a mentor, I want to see attendance percentages so that I can identify developers below the 80% requirement.

**Acceptance Criteria**

System shows:

* total class days
* present days
* leave days
* absent days
* attendance %

Status automatically:

* Good
* Warning
* Not Eligible

---

# 👨‍💻 Developer User Stories

## 8. View Training Schedule

**User Story**

> As a developer, I want to see my training schedule so that I know when to attend classes or submit assignments.

**Acceptance Criteria**

Developer can see:

* Class days
* Assignment days
* Close days
* Holidays

---

## 9. View Attendance Record

**User Story**

> As a developer, I want to check my attendance percentage so that I know whether I meet the 80% requirement.

**Acceptance Criteria**

Developer dashboard shows:

* attendance %
* present count
* leave count
* absent count
* warning status

---

## 10. Submit Assignment

**User Story**

> As a developer, I want to submit assignments online so that my mentor can review my work.

**Acceptance Criteria**

Developer can:

* upload submission
* resubmit before deadline
* view feedback
* view score

---

## 11. Request Leave

**User Story**

> As a developer, I want to request leave so that my absence is recorded properly.

**Acceptance Criteria**

Developer can:

* select leave date
* enter reason
* submit request
* see approval status
* see remaining leave quota

---

# 📊 System User Stories (Automation)

## 12. Auto Calculate Attendance %

**User Story**

> As a system, I want to calculate attendance percentage automatically so that eligibility status is accurate.

**Acceptance Criteria**

System calculates:

```
Attendance % = Present Days / Total Class Days × 100
```

Exclude:

* Close days
* Holidays

---

## 13. Auto Warning for Low Attendance

**User Story**

> As a system, I want to alert developers when attendance drops below 80% so that they can improve participation.

**Acceptance Criteria**

System marks status:

```
≥ 80% → Eligible
70–79% → Warning
< 70% → Not Eligible
```

---

## 14. Track Monthly Leave Limit

**User Story**

> As a system, I want to enforce monthly leave limits so that developers do not exceed allowed absences.

**Acceptance Criteria**

Example rule:

```
Allowed Leave = 2 days/month
```

System shows:

* used leave
* remaining leave
* exceeded warning

---

# ⭐ Optional (Recommended for Future Phase)

## 15. Replacement Class Support

**User Story**

> As a mentor, I want to add replacement classes so that missed sessions can be recovered.

---

## 16. Training Completion Eligibility

**User Story**

> As a mentor, I want to determine final eligibility so that only qualified developers complete training.

**Acceptance Criteria**

Completion requires:

* attendance ≥ 80%
* assignments submitted
* leave within limit

---
