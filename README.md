# Digital Certificate Verification System

A web-based certificate management and verification platform built with **ASP.NET Web Forms (C#)** and **Oracle Database (PL/SQL)**. Developed as a semester project for CSE 3110 — Database Systems Laboratory, KUET.

---

## Table of Contents

- [Project Overview](#project-overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [System Roles](#system-roles)
- [Database Schema](#database-schema)
- [PL/SQL Package](#plsql-package)
- [Triggers](#triggers)
- [Project Structure](#project-structure)
- [Pages](#pages)
- [Setup Instructions](#setup-instructions)
- [Seed Data](#seed-data)
- [SQL Concepts Demonstrated](#sql-concepts-demonstrated)
- [Screenshots](#screenshots)

---

## Project Overview

The Digital Certificate Verification System allows educational institutions to:

- Issue and manage academic certificates digitally
- Let students request certificates online
- Allow admins to approve or reject requests
- Enable anyone (employers, teachers, public) to verify a certificate using its unique code

---

## Features

| Feature | Description |
|---|---|
| Add Certificate | Admin issues a new certificate to a student |
| Update Certificate | Admin edits certificate details or status |
| Delete Certificate | Admin removes a certificate (blocked if verified) |
| Search Certificates | Search by certificate code or student name |
| Verify Certificate | Public verifier enters a code to confirm authenticity |
| Filter Certificates | Filter by department, type, status, institution, year, and date range using checkboxes |
| Certificate Requests | Students submit requests; admin approves or rejects |
| Dashboard Stats | Real-time counts of students, certificates, expired, and pending requests |
| Audit Log | Every certificate status change is logged automatically |

---

## Technology Stack

| Layer | Technology |
|---|---|
| Frontend | ASP.NET Web Forms, Bootstrap 5.3, Bootstrap Icons |
| Backend | C# (.NET Framework 4.7.2) |
| Database | Oracle Database (XE / XEPDB1) |
| Data Access | Oracle.ManagedDataAccess.Client (ODP.NET) |
| Stored Logic | PL/SQL Package (`cert_pkg`) |
| Database Events | Oracle Triggers (10 triggers) |

---

## System Roles

### Admin
- Login at `/Login.aspx`
- Full access to: Dashboard, Manage Students, Manage Certificates, Certificate List (with filters), Manage Requests

### Student
- Login at `/StudentLogin.aspx`
- Can submit certificate requests and track their status at `/MyRequests.aspx`

### Public / Verifier (Employer, Teacher, Anyone)
- No login required
- Accesses `/VerifyCertificate.aspx` and enters a certificate code to verify

---

## Database Schema

### Tables (7)
```
Departments
department_id   NUMBER  PK
department_name VARCHAR2(100)
Students
student_id      NUMBER  PK
student_name    VARCHAR2(100)
email           VARCHAR2(100)  UNIQUE
password        VARCHAR2(100)
phone           VARCHAR2(20)
department_id   NUMBER  FK → Departments
Certificate_Types
type_id         NUMBER  PK
type_name       VARCHAR2(100)
Certificates
certificate_id   NUMBER  PK
certificate_code VARCHAR2(50)  UNIQUE
student_id       NUMBER  FK → Students
type_id          NUMBER  FK → Certificate_Types
title            VARCHAR2(200)
issue_date       DATE
expiry_date      DATE
issuing_authority VARCHAR2(100)
status           VARCHAR2(20)  CHECK (Valid/Expired/Revoked)
Verifications
verification_id   NUMBER  PK
certificate_id    NUMBER  FK → Certificates
verifier_name     VARCHAR2(100)
verifier_email    VARCHAR2(100)
verification_date DATE
remarks           VARCHAR2(300)
Admins
admin_id  NUMBER  PK
username  VARCHAR2(50)  UNIQUE
password  VARCHAR2(100)
Certificate_Requests
request_id           NUMBER  PK
student_id           NUMBER  FK → Students
type_id              NUMBER  FK → Certificate_Types
title                VARCHAR2(200)
issuing_authority    VARCHAR2(100)
preferred_issue_date DATE
remarks              VARCHAR2(300)
requested_date       DATE
status               VARCHAR2(20)  CHECK (Pending/Approved/Rejected)
reviewed_by          NUMBER  FK → Admins
reviewed_date        DATE
certificate_id       NUMBER  FK → Certificates
Cert_Audit_Log  (for trigger 10)
log_id         NUMBER  PK
certificate_id NUMBER
old_status     VARCHAR2(20)
new_status     VARCHAR2(20)
changed_on     DATE
```

### ER Diagram (Text)
```
Departments  ──(1 to M)──  Students  ──(1 to M)──  Certificates  ──(M to 1)──  Certificate_Types
│
(1 to M)
│
Verifications
Students  ──(1 to M)──  Certificate_Requests  ──(M to 1)──  Certificate_Types
Certificate_Requests  ──(M to 1)──  Admins
Certificate_Requests  ──(M to 1)──  Certificates  (after approval)
Certificates  ──(1 to M)──  Cert_Audit_Log
```

---

## PL/SQL Package

All database logic is encapsulated inside a single PL/SQL package: `cert_pkg`

### Functions (2)
| Function | Purpose |
|---|---|
| `login_admin(username, password)` | Returns admin_id or -1 |
| `login_student(email, password)` | Returns student_id or -1 |

### Procedures (18)
| Procedure | Purpose |
|---|---|
| `get_departments` | Fetch all departments (for dropdowns) |
| `get_certificate_types` | Fetch all certificate types |
| `get_students_lookup` | Fetch student id + name (for dropdowns) |
| `add_student` | Insert new student |
| `update_student` | Update student details |
| `delete_student` | Delete a student |
| `get_all_students` | List all students with department name (JOIN) |
| `get_student_by_id` | Fetch single student for edit form |
| `add_certificate` | Insert new certificate |
| `update_certificate` | Update certificate details |
| `delete_certificate` | Delete certificate and its verifications |
| `get_all_certificates` | List all certificates with student + type (JOIN) |
| `get_certificate_by_id` | Fetch single certificate for edit form |
| `search_certificates` | LIKE search by code or student name |
| `filter_certificates` | Multi-filter by dept, type, status, institution, year, date range |
| `verify_certificate` | Verify by code, log verification, return result |
| `submit_request` | Student submits a certificate request |
| `get_student_requests` | List a student's own requests |
| `get_pending_requests` | Admin view: all pending requests |
| `approve_request` | Create certificate + mark request Approved |
| `reject_request` | Mark request as Rejected |
| `get_dashboard_stats` | Return 4 counts for dashboard cards |

---

## Triggers

| # | Trigger Name | Table | Event | Purpose |
|---|---|---|---|---|
| 1 | `trg_auto_expire_certificate` | Certificates | BEFORE INSERT OR UPDATE | Auto-sets status to Expired if expiry_date < SYSDATE |
| 2 | `trg_prevent_student_delete` | Students | BEFORE DELETE | Blocks delete if student has certificates |
| 3 | `trg_verification_date_stamp` | Verifications | BEFORE INSERT | Auto-stamps verification_date and default remarks |
| 4 | `trg_prevent_duplicate_cert_code` | Certificates | BEFORE INSERT | Blocks duplicate certificate codes with custom error |
| 5 | `trg_validate_cert_dates` | Certificates | BEFORE INSERT OR UPDATE | Ensures expiry_date > issue_date |
| 6 | `trg_prevent_duplicate_request` | Certificate_Requests | BEFORE INSERT | Blocks duplicate pending requests per student per type |
| 7 | `trg_lock_reviewed_request` | Certificate_Requests | BEFORE UPDATE | Prevents editing Approved/Rejected requests |
| 8 | `trg_prevent_cert_delete_if_verified` | Certificates | BEFORE DELETE | Blocks delete if verification records exist |
| 9 | `trg_uppercase_cert_code` | Certificates | BEFORE INSERT OR UPDATE | Forces certificate codes to uppercase |
| 10 | `trg_certificate_status_audit` | Certificates | AFTER UPDATE | Logs every status change to Cert_Audit_Log |

---



## Project Structure

~~~
DigitalCertSystem/
│
├── Database/
│   ├── 01_schema.sql               -- All CREATE TABLE and sequences
│   ├── 02_seed_data.sql            -- Sample data for all 7 tables
│   ├── 03_cert_pkg.sql             -- PL/SQL package spec + body
│   └── 04_triggers.sql             -- All 10 triggers
│
├── Site.Master                     -- Shared layout, navbar, footer
├── Site.master.cs
│
├── Login.aspx                      -- Admin login
├── Login.aspx.cs
├── StudentLogin.aspx               -- Student login
├── StudentLogin.aspx.cs
│
├── AdminDashboard.aspx             -- Stat cards dashboard
├── AdminDashboard.aspx.cs
├── ManageStudents.aspx             -- Add / Edit / Delete students
├── ManageStudents.aspx.cs
├── ManageCertificates.aspx         -- Add / Edit / Delete certificates
├── ManageCertificates.aspx.cs
├── CertificateList.aspx            -- Sidebar checkbox filters + search
├── CertificateList.aspx.cs
├── ManageRequests.aspx             -- Approve / Reject pending requests
├── ManageRequests.aspx.cs
│
├── RequestCertificate.aspx         -- Student: submit request
├── RequestCertificate.aspx.cs
├── MyRequests.aspx                 -- Student: track own requests
├── MyRequests.aspx.cs
│
├── VerifyCertificate.aspx          -- Public: verify by certificate code
├── VerifyCertificate.aspx.cs
│
├── DBHelper.cs                     -- Static connection + command helper
└── Web.config                      -- Oracle connection string
~~~

## Pages

| Page | Role | Access |
|---|---|---|
| `Login.aspx` | Admin | Public |
| `StudentLogin.aspx` | Student | Public |
| `VerifyCertificate.aspx` | Anyone | Public |
| `AdminDashboard.aspx` | Admin | Admin session required |
| `ManageStudents.aspx` | Admin | Admin session required |
| `ManageCertificates.aspx` | Admin | Admin session required |
| `CertificateList.aspx` | Admin | Admin session required |
| `ManageRequests.aspx` | Admin | Admin session required |
| `RequestCertificate.aspx` | Student | Student session required |
| `MyRequests.aspx` | Student | Student session required |

---

## Setup Instructions

### Prerequisites
- Visual Studio 2019 or later
- Oracle Database XE 21c (or any Oracle instance)
- Oracle.ManagedDataAccess NuGet package
- .NET Framework 4.7.2

### Step 1 — Database Setup

Open SQL\*Plus or Oracle SQL Developer and run the scripts in order:

```sql
@01_schema.sql
@02_seed_data.sql
@03_cert_pkg.sql
@04_triggers.sql
```

### Step 2 — Configure Connection String

Open `Web.config` and update with your Oracle credentials:

```xml
<connectionStrings>
    <add name="OracleDb"
         connectionString="User Id=YOUR_USER;Password=YOUR_PASSWORD;Data Source=localhost:1521/XEPDB1"
         providerName="Oracle.ManagedDataAccess.Client" />
</connectionStrings>
```

### Step 3 — Install NuGet Package

In Visual Studio → Package Manager Console:
Install-Package Oracle.ManagedDataAccess

### Step 4 — Run the Project

Press `F5` in Visual Studio. The browser will open at `VerifyCertificate.aspx`.

- Admin login → `/Login.aspx` — username: `admin`, password: `admin123`
- Student login → `/StudentLogin.aspx` — email: `john.doe@kuet.ac.bd`, password: `pass123`

---

## Seed Data

| Table | Rows |
|---|---|
| Departments | 5 (CSE, EEE, ME, CE, BME) |
| Certificate_Types | 5 (Workshop, Training, Internship, Competition, Course Completion) |
| Admins | 2 |
| Students | 10 |
| Certificates | 11 (mix of Valid, Expired, Revoked) |
| Verifications | 4 |
| Certificate_Requests | 5 (Pending, Approved, Rejected) |

---

## SQL Concepts Demonstrated

| Concept | Where Used |
|---|---|
| Primary Key | All 7 tables |
| Foreign Key | Students → Departments, Certificates → Students/Types, etc. |
| UNIQUE Constraint | Students.email, Certificates.certificate_code, Admins.username |
| CHECK Constraint | Certificates.status, Certificate_Requests.status |
| DEFAULT | Certificates.status, Verifications.verification_date, Certificate_Requests.status |
| JOIN (INNER) | get_all_students, get_all_certificates, filter_certificates |
| JOIN (LEFT OUTER) | filter_certificates (Departments may be NULL) |
| LIKE | search_certificates (wildcard search) |
| WHERE with multiple conditions | filter_certificates (AND chaining) |
| Aggregation (COUNT) | get_dashboard_stats |
| CASE expression | verify_certificate (compute effective_status) |
| NVL | approve_request (NVL preferred_issue_date, SYSDATE) |
| Sequences | All 8 sequences for PK auto-increment |
| REF CURSOR | All 15 procedures returning result sets |
| PL/SQL Package | cert_pkg (spec + body) |
| Stored Function | login_admin, login_student |
| Stored Procedure | All 18 data procedures |
| RAISE_APPLICATION_ERROR | Triggers 2, 4, 5, 6, 7, 8 |
| :OLD / :NEW | All 10 triggers |
| BEFORE / AFTER Trigger | BEFORE (1–9), AFTER (10) |
| INSERT / UPDATE / DELETE Trigger | All three DML events covered |
| Audit Logging | trg_certificate_status_audit → Cert_Audit_Log |
| Referential Integrity | ON DELETE restricted (default) across all FK pairs |

---

## Developer

| Field | Detail |
|---|---|
| Institution | Khulna University of Engineering & Technology (KUET) |
| Course | CSE 3110 — Database Systems Laboratory |
| Supervisor | Waliul Islam Sumon and Kaniz Fatema Isha|
| Academic Year | 2025–2026 |
