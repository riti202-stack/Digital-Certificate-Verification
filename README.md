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
