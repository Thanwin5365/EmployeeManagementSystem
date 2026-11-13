# Employee Management System

A full-stack Employee Management System built using Angular and ASP.NET Core Web API to streamline employee administration, joining report approvals, and attendance tracking.

## Overview

This application provides a centralized platform for managing employees, managers, joining reports, approval workflows, and attendance records.

## Features

- **Employee Authentication** – Secure employee login.
- **Manager Authentication** – Role-based manager login.
- **Employee Management** – View and manage employee information.
- **Joining Report Management** – Create and track joining reports.
- **Multi-Level Approval Workflow** – Level 1 Manager, Level 2 Manager, and HR approval/rejection.
- **Approval History** – Track approval decisions and rejection reasons.
- **Attendance Management** – Employee check-in, check-out, and daily attendance summaries.
- **REST API Integration** – Angular communicates with the backend through HTTP APIs.

## Technology Stack

| Layer | Technologies |
|---|---|
| Frontend | Angular, TypeScript, HTML, CSS |
| Backend | C#, ASP.NET Core Web API |
| Database | Microsoft SQL Server (original implementation) |
| Data Access | ADO.NET, SQL Server Stored Procedures |
| Email | MailKit |
| Tools | Visual Studio, VS Code, Git, GitHub |

*Note: PostgreSQL migration is currently in progress and is not yet represented as a completed feature.*

## Project Structure

```text
EmployeeManagementSystem/
│
├── Backend/
│   └── EmployeeJoiningAPI/
│       ├── Controllers/
│       ├── Models/
│       ├── Repositories/
│       ├── Services/
│       └── Program.cs
│
├── Frontend/
│   └── employee-joining-portal/
│       └── employee-joining-portal/
│           └── src/
│               ├── app/
│               └── environments/
│
└── .gitignore
```

## Application Architecture

```text
Angular Frontend
       |
       | HTTP REST API
       v
ASP.NET Core Web API
       |
       v
Repository Layer (ADO.NET)
       |
       v
SQL Server Database
```

## Running Locally

### Backend

```bash
cd Backend/EmployeeJoiningAPI/EmployeeJoiningAPI
dotnet restore
dotnet run
```

### Frontend

```bash
cd Frontend/employee-joining-portal/employee-joining-portal
npm install
ng serve
```

Open `http://localhost:4200` in your browser.

Configure the backend connection string and frontend API URL before running the application.

## Developer

**Thanwin Sre.N**

GitHub: [Thanwin5365](https://github.com/Thanwin5365)

---

This project was developed to demonstrate full-stack application development, REST API integration, database operations, authentication, and workflow management.
