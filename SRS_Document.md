# Software Requirements Specification (SRS)
## Expense Tracker Application

---

**Document Version:** 1.0  
**Date:** March 2026  
**Project:** Expense Tracker Web Application  
**Technology Stack:** ASP.NET Core 6 MVC, Entity Framework Core 6, SQL Server Express, Syncfusion EJ2  

---

## Table of Contents

1. Introduction
2. Overall Description
3. Functional Requirements
4. Non-Functional Requirements
5. System Architecture
6. Database Design
7. User Interface Requirements
8. Security Requirements
9. Constraints and Assumptions
10. Glossary

---

## 1. Introduction

### 1.1 Purpose
This document defines the software requirements for the Expense Tracker Web Application. It describes the functional and non-functional requirements, system architecture, and design constraints for developers, testers, and stakeholders.

### 1.2 Scope
The Expense Tracker is a web-based personal finance management application that allows registered users to:
- Track income and expense transactions
- Organize transactions by categories
- View financial summaries and charts on a dashboard
- Generate reports with date range filtering
- Manage their account settings

### 1.3 Intended Audience
- Developers maintaining or extending the application
- Testers validating application functionality
- Project stakeholders reviewing system capabilities

### 1.4 Definitions and Acronyms

| Term | Definition |
|------|-----------|
| MVC | Model-View-Controller architectural pattern |
| EF Core | Entity Framework Core — ORM for .NET |
| Identity | ASP.NET Core Identity — authentication framework |
| SRS | Software Requirements Specification |
| INR | Indian National Rupee (₹) |
| CRUD | Create, Read, Update, Delete operations |

---

## 2. Overall Description

### 2.1 Product Perspective
The Expense Tracker is a standalone web application hosted locally using ASP.NET Core 6. It uses SQL Server Express as the database backend and Syncfusion EJ2 components for rich UI elements including grids, charts, and sidebar navigation.

### 2.2 Product Functions (Summary)
- User registration and authentication
- Category management (Income / Expense types)
- Transaction management (Add, Edit, Delete)
- Dashboard with financial summary and charts
- Reports with date range filtering and visual analytics
- Account settings (profile update, password change)
- Navbar with notification, message, and profile panels

### 2.3 User Classes
| User Class | Description |
|------------|-------------|
| Registered User | A user who has created an account and can access all features |
| Guest | Unauthenticated visitor — redirected to login page |

### 2.4 Operating Environment
- OS: Windows 10/11
- Runtime: .NET 6 SDK
- Database: SQL Server Express (local instance: `(local)\sqlexpress`)
- Browser: Any modern browser (Chrome, Edge, Firefox)
- Port: http://localhost:5000

---

## 3. Functional Requirements

### 3.1 Authentication Module

#### FR-AUTH-01: User Registration
- The system shall allow a new user to register with a unique username, email address, and password.
- Password must be at least 6 characters.
- Confirm password field must match the password field.
- On successful registration, the user is automatically logged in and redirected to the Dashboard.
- Duplicate email or username shall display an appropriate error message.

#### FR-AUTH-02: User Login
- The system shall allow a registered user to log in using their email and password.
- The system shall look up the user by email and authenticate using their username internally.
- A "Remember me" option shall persist the session across browser restarts.
- Invalid credentials shall display "Invalid email or password." error.

#### FR-AUTH-03: User Logout
- The system shall allow a logged-in user to log out via the sidebar logout button or the profile dropdown in the navbar.
- On logout, the user is redirected to the Login page.
- All protected routes shall redirect unauthenticated users to the Login page.

---

### 3.2 Category Module

#### FR-CAT-01: View Categories
- The system shall display all categories in a sortable, paginated grid (5 per page).
- Each row shall show: Category name with icon, Type (Income/Expense badge), and action buttons.

#### FR-CAT-02: Create Category
- The system shall allow a user to create a new category with:
  - Type: Income or Expense (radio button selection)
  - Title: required text field (max 50 characters)
  - Icon: optional emoji or symbol (max 5 characters)
- On success, the user is redirected to the Category list.

#### FR-CAT-03: Edit Category
- The system shall allow a user to edit an existing category's Title, Icon, and Type.
- The form shall be pre-populated with existing values.

#### FR-CAT-04: Delete Category
- The system shall allow a user to delete a category via a confirmation prompt.
- Deleting a category shall cascade-delete all associated transactions (enforced at DB level).

---

### 3.3 Transaction Module

#### FR-TRN-01: View Transactions
- The system shall display all transactions in a sortable, paginated grid (10 per page).
- Each row shall show: Category with icon, Date, Formatted Amount (+ for income, - for expense in ₹), and action buttons.

#### FR-TRN-02: Create Transaction
- The system shall allow a user to create a transaction with:
  - Date: date picker (defaults to today)
  - Category: dropdown list of existing categories
  - Amount: numeric input (must be greater than 0)
  - Note: optional text (max 75 characters)

#### FR-TRN-03: Edit Transaction
- The system shall allow editing of all transaction fields.
- The form shall be pre-populated with existing values.

#### FR-TRN-04: Delete Transaction
- The system shall allow deletion of a transaction with a confirmation prompt.

---

### 3.4 Dashboard Module

#### FR-DASH-01: Summary Widgets
- The system shall display three summary cards for the last 7 days:
  - Total Income (₹)
  - Total Expense (₹)
  - Net Balance (₹)

#### FR-DASH-02: Doughnut Chart
- The system shall display a doughnut chart showing expense breakdown by category for the last 7 days.

#### FR-DASH-03: Spline Chart
- The system shall display a spline (line) chart showing daily Income vs Expense for the last 7 days.

#### FR-DASH-04: Recent Transactions
- The system shall display the 5 most recent transactions in a grid showing Category, Date, and Amount.

---

### 3.5 Reports Module

#### FR-RPT-01: Date Range Filter
- The system shall allow the user to filter all report data by a custom start and end date.
- Default range shall be the last 30 days.

#### FR-RPT-02: Summary Cards
- The system shall display Total Income, Total Expense, and Net Balance for the selected date range.

#### FR-RPT-03: Monthly Bar Chart
- The system shall display a grouped bar chart showing Income vs Expense grouped by month.

#### FR-RPT-04: Expense Doughnut Chart
- The system shall display a doughnut chart of expense breakdown by category for the selected range.

#### FR-RPT-05: Category Breakdown Table
- The system shall display a sortable table showing each category's type, transaction count, and total amount.

#### FR-RPT-06: All Transactions Table
- The system shall display a paginated (10 per page) table of all transactions in the selected date range.

---

### 3.6 Settings Module

#### FR-SET-01: Update Profile
- The system shall allow the user to update their username and email address.
- Changes are reflected immediately in the sidebar and navbar.

#### FR-SET-02: Change Password
- The system shall allow the user to change their password by providing:
  - Current password (for verification)
  - New password (min 6 characters)
  - Confirm new password (must match)
- Incorrect current password shall display an error message.

#### FR-SET-03: Logout from Settings
- The system shall provide a logout button in the Danger Zone section of Settings.

---

### 3.7 Navbar Module

#### FR-NAV-01: Notification Panel
- The system shall display a bell icon with a red badge showing unread notification count.
- Clicking the icon opens a dropdown panel with recent activity notifications.
- Opening the panel shall auto-dismiss the badge after 1.5 seconds.
- A "Mark all read" link shall clear all unread highlights and hide the badge.

#### FR-NAV-02: Message Panel
- The system shall display a message icon with a red badge showing unread message count.
- Clicking opens a dropdown with system messages.
- Same mark-as-read and auto-dismiss behavior as notifications.

#### FR-NAV-03: Profile Dropdown
- The system shall display a profile icon in the navbar.
- Clicking opens a dropdown showing the logged-in username with quick links to Settings, Reports, Transactions, and a Logout button.

---

## 4. Non-Functional Requirements

### 4.1 Performance
- NFR-01: Dashboard page shall load within 3 seconds under normal conditions.
- NFR-02: All database queries shall use Entity Framework Include() for eager loading to avoid N+1 query problems.

### 4.2 Usability
- NFR-03: The application shall use a consistent dark theme across all pages.
- NFR-04: All currency values shall be displayed in Indian Rupee (₹) format using en-IN culture.
- NFR-05: All forms shall display inline validation error messages.

### 4.3 Security
- NFR-06: All pages except Login and Register shall require authentication.
- NFR-07: Passwords shall be hashed using ASP.NET Core Identity's default PBKDF2 algorithm.
- NFR-08: All POST forms shall include Anti-Forgery tokens to prevent CSRF attacks.
- NFR-09: Password minimum length is 6 characters; uppercase and special characters are not required.

### 4.4 Reliability
- NFR-10: The application shall use EF Core migrations to maintain database schema consistency.
- NFR-11: Cascade delete shall be enforced at the database level for Category → Transaction relationships.

### 4.5 Maintainability
- NFR-12: The application shall follow the MVC pattern with clear separation of concerns.
- NFR-13: All controllers shall use dependency injection for database context and Identity services.

---

## 5. System Architecture

```
┌─────────────────────────────────────────────────────┐
│                    Browser (Client)                  │
│         HTML + Bootstrap 5 + Syncfusion EJ2          │
└──────────────────────┬──────────────────────────────┘
                       │ HTTP Requests
┌──────────────────────▼──────────────────────────────┐
│              ASP.NET Core 6 MVC Server               │
│                                                      │
│  ┌─────────────┐  ┌──────────────┐  ┌────────────┐  │
│  │ Controllers │  │    Models    │  │   Views    │  │
│  │─────────────│  │──────────────│  │────────────│  │
│  │ Account     │  │ Transaction  │  │ Dashboard  │  │
│  │ Dashboard   │  │ Category     │  │ Category   │  │
│  │ Category    │  │ AppDbContext │  │ Transaction│  │
│  │ Transaction │  │ ViewModels   │  │ Reports    │  │
│  │ Reports     │  │              │  │ Settings   │  │
│  │ Settings    │  │              │  │ Account    │  │
│  └──────┬──────┘  └──────────────┘  └────────────┘  │
│         │                                            │
│  ┌──────▼──────────────────────────────────────┐    │
│  │         ASP.NET Core Identity                │    │
│  │   UserManager / SignInManager / RoleManager  │    │
│  └──────┬──────────────────────────────────────┘    │
│         │                                            │
│  ┌──────▼──────────────────────────────────────┐    │
│  │      Entity Framework Core 6 (ORM)           │    │
│  └──────┬──────────────────────────────────────┘    │
└─────────┼───────────────────────────────────────────┘
          │
┌─────────▼───────────────────────────────────────────┐
│           SQL Server Express (TransactionDB)         │
│                                                      │
│  Categories | Transactions | AspNetUsers | ...       │
└─────────────────────────────────────────────────────┘
```

---

## 6. Database Design

### 6.1 Tables

#### Categories
| Column | Type | Constraints |
|--------|------|-------------|
| CategoryId | int | PK, Identity |
| Title | nvarchar(50) | NOT NULL |
| Icon | nvarchar(5) | NOT NULL, Default '' |
| Type | nvarchar(10) | NOT NULL, Default 'Expense' |

#### Transactions
| Column | Type | Constraints |
|--------|------|-------------|
| TransactionId | int | PK, Identity |
| CategoryId | int | FK → Categories(CategoryId) CASCADE DELETE |
| Amount | int | NOT NULL, > 0 |
| Note | nvarchar(75) | NULL |
| Date | datetime2 | NOT NULL |

#### AspNetUsers (Identity)
| Column | Type | Description |
|--------|------|-------------|
| Id | nvarchar(450) | PK |
| UserName | nvarchar(256) | Unique |
| NormalizedUserName | nvarchar(256) | Unique Index |
| Email | nvarchar(256) | |
| NormalizedEmail | nvarchar(256) | Index |
| PasswordHash | nvarchar(max) | PBKDF2 hashed |
| SecurityStamp | nvarchar(max) | |

> Additional Identity tables: AspNetRoles, AspNetUserRoles, AspNetUserClaims, AspNetUserLogins, AspNetUserTokens, AspNetRoleClaims

### 6.2 Entity Relationships
```
Categories (1) ──────< Transactions (Many)
    CategoryId (PK)        CategoryId (FK)
```

---

## 7. User Interface Requirements

### 7.1 Layout
- The application uses a persistent sidebar navigation with collapsible dock behavior.
- A sticky top navbar contains search, notification, message, and profile icons.
- All pages use a dark theme with background color `#12161d`.

### 7.2 Pages

| Page | Route | Description |
|------|-------|-------------|
| Login | /Account/Login | Email + password login form |
| Register | /Account/Register | New user registration form |
| Dashboard | / | Summary widgets + charts + recent transactions |
| Categories | /Category | Category list grid |
| Add/Edit Category | /Category/AddOrEdit | Category form |
| Transactions | /Transaction | Transaction list grid |
| Add/Edit Transaction | /Transaction/AddOrEdit | Transaction form |
| Reports | /Reports | Date-filtered analytics and charts |
| Settings | /Settings | Profile update + password change |

### 7.3 Navigation Sidebar Items
- Dashboard
- Categories
- Transactions
- Reports
- Settings

---

## 8. Security Requirements

| Requirement | Implementation |
|-------------|---------------|
| Authentication | ASP.NET Core Identity with cookie-based auth |
| Authorization | [Authorize] attribute on all controllers except Account |
| Password Storage | PBKDF2 hashing via Identity |
| CSRF Protection | ValidateAntiForgeryToken on all POST actions |
| Session Redirect | Unauthenticated requests redirect to /Account/Login |
| Cookie Config | LoginPath: /Account/Login, LogoutPath: /Account/Logout |

---

## 9. Constraints and Assumptions

- The application runs on a single machine (localhost) and is not designed for multi-user concurrent access at scale.
- SQL Server Express is required; the connection string targets `(local)\sqlexpress`.
- The Syncfusion EJ2 license key is embedded in Program.cs and is valid for the current version (20.1.0.58).
- Currency is fixed to Indian Rupee (₹) using `en-IN` culture; no multi-currency support.
- No email verification is implemented for registration.
- No role-based access control (all authenticated users have equal access).

---

## 10. Glossary

| Term | Meaning |
|------|---------|
| Transaction | A single financial record of income or expense |
| Category | A classification label for transactions (e.g., Food, Salary) |
| Dashboard | The home page showing financial summary and charts |
| Balance | Total Income minus Total Expense for a given period |
| EF Migration | A versioned database schema change managed by EF Core |
| Identity | ASP.NET Core's built-in user authentication system |
| Syncfusion EJ2 | A UI component library used for grids, charts, and sidebar |
| en-IN | .NET culture code for Indian English (uses ₹ currency symbol) |
