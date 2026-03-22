# 💰 Expense Tracker

A personal finance web application built with **ASP.NET Core 6 MVC** that helps you track income and expenses, visualize spending patterns, and manage your financial health — all in Indian Rupees (₹).

---

## ✨ Features

- 🔐 **User Authentication** — Register, login, logout with ASP.NET Core Identity
- 📂 **Category Management** — Create Income/Expense categories with custom icons
- 💸 **Transaction Tracking** — Add, edit, and delete income/expense transactions
- 📊 **Dashboard** — 7-day summary with doughnut and spline charts
- 📈 **Reports** — Date-range filtered analytics with monthly bar charts and category breakdowns
- ⚙️ **Settings** — Update profile (username/email) and change password
- 🔔 **Navbar** — Notification panel, message panel, and profile dropdown
- 🇮🇳 **INR Currency** — All amounts displayed in ₹ using Indian number formatting

---

## 🖥️ Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core 6 MVC |
| ORM | Entity Framework Core 6 |
| Authentication | ASP.NET Core Identity |
| Database | SQL Server Express |
| UI Components | Syncfusion EJ2 (v20.1.0.58) |
| CSS Framework | Bootstrap 5 |
| Icons | Font Awesome 6.1.1 |
| Language | C# 10 / Razor |

---

## 📋 Prerequisites

Before running this project, make sure you have the following installed:

| Requirement | Version | Download |
|-------------|---------|----------|
| .NET SDK | 6.0.x | [dotnet.microsoft.com](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) |
| SQL Server Express | 2017+ | [microsoft.com/sql-server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) |

> During SQL Server Express installation, choose **Basic** setup — this automatically creates the `(local)\sqlexpress` instance.

---

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/CodAffection/Expense-Tracker-App-in-Asp.Net-Core-MVC.git
cd "Expense-Tracker-App"
```

### 2. Configure the Database Connection

The connection string is already configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "DevConnection": "Server=(local)\\sqlexpress;Database=TransactionDB;Trusted_Connection=True;MultipleActiveResultSets=True;"
}
```

> If your SQL Server instance name is different, update the `Server` value accordingly.

### 3. Apply Database Migrations

```cmd
set PATH=C:\Program Files\dotnet;C:\Users\<YourUsername>\.dotnet\tools;%PATH%
cd "Expense Tracker"
dotnet ef database update
```

This creates the `TransactionDB` database with all required tables automatically.

### 4. Run the Application

**Option A — Double-click the batch file (easiest):**
```
RunApp.bat
```

**Option B — Command line:**
```cmd
set PATH=C:\Program Files\dotnet;%PATH%
cd "Expense Tracker"
dotnet run --urls http://localhost:5000
```

### 5. Open in Browser

```
http://localhost:5000
```

You will be redirected to the **Login** page. Click **Register** to create your first account.

---

## 📁 Project Structure

```
Expense-Tracker-App/
│
├── Expense Tracker/
│   ├── Controllers/
│   │   ├── AccountController.cs       # Register, Login, Logout
│   │   ├── CategoryController.cs      # Category CRUD
│   │   ├── DashboardController.cs     # Dashboard data & charts
│   │   ├── TransactionController.cs   # Transaction CRUD
│   │   ├── ReportsController.cs       # Date-filtered reports
│   │   └── SettingsController.cs      # Profile & password update
│   │
│   ├── Models/
│   │   ├── ApplicationDbContext.cs    # EF Core DbContext
│   │   ├── Category.cs                # Category entity
│   │   ├── Transaction.cs             # Transaction entity
│   │   ├── AccountViewModels.cs       # Register & Login view models
│   │   └── SettingsViewModel.cs       # Settings view model
│   │
│   ├── Views/
│   │   ├── Account/                   # Login, Register pages
│   │   ├── Category/                  # Category list & form
│   │   ├── Dashboard/                 # Dashboard page
│   │   ├── Transaction/               # Transaction list & form
│   │   ├── Reports/                   # Reports page
│   │   ├── Settings/                  # Settings page
│   │   └── Shared/                    # Layout, Sidebar, Auth Layout
│   │
│   ├── Migrations/                    # EF Core database migrations
│   ├── wwwroot/                       # Static files (CSS, JS, images)
│   ├── appsettings.json               # App configuration
│   └── Program.cs                     # App startup & DI registration
│
├── SRS_Document.md                    # Software Requirements Specification
├── Document_of_Understanding.md       # Technical understanding document
├── RunApp.bat                         # One-click run script
└── README.md                          # This file
```

---

## 🗄️ Database Schema

```
┌─────────────────┐         ┌──────────────────────┐
│   Categories    │         │     Transactions      │
├─────────────────┤         ├──────────────────────┤
│ CategoryId (PK) │◄────────│ TransactionId (PK)   │
│ Title           │  1 : N  │ CategoryId (FK)       │
│ Icon            │         │ Amount                │
│ Type            │         │ Note                  │
└─────────────────┘         │ Date                  │
                            └──────────────────────┘

┌──────────────────────────────────────┐
│           AspNetUsers                │
│        (Identity Tables)             │
├──────────────────────────────────────┤
│ Id, UserName, Email, PasswordHash    │
│ + AspNetRoles, AspNetUserRoles, etc. │
└──────────────────────────────────────┘
```

---

## 📸 Pages Overview

| Page | Route | Description |
|------|-------|-------------|
| Login | `/Account/Login` | Email + password authentication |
| Register | `/Account/Register` | Create a new account |
| Dashboard | `/` | 7-day financial summary + charts |
| Categories | `/Category` | Manage income/expense categories |
| Transactions | `/Transaction` | View and manage all transactions |
| Reports | `/Reports` | Date-filtered analytics and charts |
| Settings | `/Settings` | Update profile and change password |

---

## 🔒 Security

- All routes (except Login/Register) are protected with `[Authorize]`
- Passwords are hashed using **PBKDF2** via ASP.NET Core Identity
- All POST forms use **Anti-Forgery tokens** to prevent CSRF attacks
- Unauthenticated users are automatically redirected to `/Account/Login`

---

## 📦 NuGet Packages

| Package | Version |
|---------|---------|
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 6.0.5 |
| Microsoft.EntityFrameworkCore | 6.0.5 |
| Microsoft.EntityFrameworkCore.SqlServer | 6.0.5 |
| Microsoft.EntityFrameworkCore.Tools | 6.0.5 |
| Syncfusion.EJ2.AspNet.Core | 20.1.0.58 |
| Swashbuckle.AspNetCore | 6.3.0 |

---

## ⚙️ Configuration

### Connection String (`appsettings.json`)
```json
{
  "ConnectionStrings": {
    "DevConnection": "Server=(local)\\sqlexpress;Database=TransactionDB;Trusted_Connection=True;MultipleActiveResultSets=True;"
  }
}
```

### Password Policy (`Program.cs`)
```csharp
options.Password.RequireNonAlphanumeric = false;
options.Password.RequireUppercase = false;
// Minimum length: 6 characters
```

---

## 🛠️ Common Commands

```cmd
# Restore packages
dotnet restore

# Build the project
dotnet build

# Run the project
dotnet run --urls http://localhost:5000

# Add a new migration
dotnet ef migrations add MigrationName

# Apply migrations to database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

> Prefix all `dotnet` commands with `set PATH=C:\Program Files\dotnet;%PATH% &&` if dotnet is not on your system PATH.

---

## 📄 Documentation

| Document | Description |
|----------|-------------|
| [SRS_Document.md](./SRS_Document.md) | Full Software Requirements Specification |
| [Document_of_Understanding.md](./Document_of_Understanding.md) | Technical walkthrough of how the app works |

---

## 🙏 Credits

- Original project by [CodAffection](https://github.com/CodAffection/Expense-Tracker-App-in-Asp.Net-Core-MVC)
- Extended with authentication, reports, settings, INR currency, and UI improvements

---

## 📝 License

This project is for educational purposes. Refer to the original repository for license details.
