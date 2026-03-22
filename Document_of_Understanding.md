# Document of Understanding (DOU)
## Expense Tracker Web Application

---

**Document Version:** 1.0  
**Date:** March 2026  
**Project Name:** Expense Tracker  
**Platform:** ASP.NET Core 6 MVC  
**Database:** SQL Server Express  

---

## Table of Contents

1. Project Overview
2. How the Application Works
3. Project Structure
4. Module-by-Module Walkthrough
5. Data Flow
6. Technology Decisions
7. Known Limitations
8. How to Run the Project
9. How to Extend the Application

---

## 1. Project Overview

The Expense Tracker is a personal finance web application built with ASP.NET Core 6 MVC. It allows a single registered user (or multiple users) to record their daily income and expense transactions, organize them into categories, and visualize their financial health through charts and reports.

The application was originally sourced from the CodAffection GitHub repository and extended with the following features:
- User registration and login (ASP.NET Core Identity)
- Indian Rupee (₹) currency formatting
- Reports page with date range filtering
- Settings page (profile update + password change)
- Navbar notification, message, and profile dropdowns

---

## 2. How the Application Works

### User Journey

```
1. User visits http://localhost:5000
        ↓
2. Not logged in → Redirected to /Account/Login
        ↓
3. New user? → Click "Register" → Fill form → Auto-logged in
   Existing user? → Enter email + password → Login
        ↓
4. Lands on Dashboard → Sees last 7 days summary
        ↓
5. Creates Categories (e.g., 🍔 Food - Expense, 💼 Salary - Income)
        ↓
6. Adds Transactions (links to a category, enters amount + date)
        ↓
7. Dashboard updates with new data
        ↓
8. Views Reports → Filters by date → Sees charts and breakdowns
        ↓
9. Updates profile/password in Settings
        ↓
10. Logs out via sidebar or profile dropdown
```

---

## 3. Project Structure

```
Expense Tracker/
│
├── Controllers/                    ← Handle HTTP requests and business logic
│   ├── AccountController.cs        ← Register, Login, Logout
│   ├── CategoryController.cs       ← CRUD for categories
│   ├── DashboardController.cs      ← Dashboard data aggregation
│   ├── TransactionController.cs    ← CRUD for transactions
│   ├── ReportsController.cs        ← Date-filtered analytics
│   └── SettingsController.cs       ← Profile + password update
│
├── Models/                         ← Data models and view models
│   ├── ApplicationDbContext.cs     ← EF Core DbContext (inherits IdentityDbContext)
│   ├── Category.cs                 ← Category entity
│   ├── Transaction.cs              ← Transaction entity
│   ├── AccountViewModels.cs        ← RegisterViewModel, LoginViewModel
│   └── SettingsViewModel.cs        ← SettingsViewModel
│
├── Views/                          ← Razor HTML templates
│   ├── Account/                    ← Login.cshtml, Register.cshtml
│   ├── Category/                   ← Index.cshtml, AddOrEdit.cshtml
│   ├── Dashboard/                  ← Index.cshtml
│   ├── Transaction/                ← Index.cshtml, AddOrEdit.cshtml
│   ├── Reports/                    ← Index.cshtml
│   ├── Settings/                   ← Index.cshtml
│   └── Shared/
│       ├── _Layout.cshtml          ← Main layout with navbar
│       ├── _AuthLayout.cshtml      ← Minimal layout for login/register
│       └── _SideBar.cshtml         ← Sidebar navigation
│
├── Migrations/                     ← EF Core database migrations
│   ├── 20220531_Initial Create     ← Original Categories + Transactions tables
│   └── 20260322_AddIdentity        ← ASP.NET Identity tables
│
├── wwwroot/                        ← Static files
│   ├── css/site.css                ← All custom styles
│   ├── js/site.js                  ← Custom JavaScript
│   └── lib/                        ← Bootstrap, jQuery
│
├── appsettings.json                ← Connection string configuration
├── Program.cs                      ← App startup, DI registration, middleware
└── Expense Tracker.csproj          ← Project dependencies
```

---

## 4. Module-by-Module Walkthrough

### 4.1 Authentication (AccountController)

**How it works:**
- Uses ASP.NET Core Identity which stores users in the `AspNetUsers` SQL table.
- Passwords are never stored in plain text — Identity hashes them using PBKDF2.
- On login, the system finds the user by email first (since `PasswordSignInAsync` requires a username), then authenticates.
- A cookie is issued on successful login. All subsequent requests carry this cookie.
- The `[Authorize]` attribute on controllers checks for this cookie. If missing, the user is redirected to `/Account/Login`.

**Key files:**
- `AccountController.cs` — Register, Login, Logout actions
- `AccountViewModels.cs` — Form data models with validation attributes
- `Views/Account/Login.cshtml` and `Register.cshtml` — Use `_AuthLayout` (no sidebar)

---

### 4.2 Categories (CategoryController)

**How it works:**
- Categories are the foundation of the app. Every transaction must belong to a category.
- Each category has a Type: either "Income" or "Expense". This determines how transactions are counted in summaries.
- The Icon field accepts any emoji (e.g., 🍔, 💼) which is displayed alongside the title.
- Deleting a category cascades to delete all its transactions (configured in EF migration).

**Key files:**
- `CategoryController.cs` — Index, AddOrEdit (GET + POST), Delete
- `Category.cs` — Model with TitleWithIcon computed property
- `Views/Category/Index.cshtml` — Syncfusion Grid with type badge template
- `Views/Category/AddOrEdit.cshtml` — Form with radio buttons for type

---

### 4.3 Transactions (TransactionController)

**How it works:**
- Transactions record a financial event: a date, a category, an amount, and an optional note.
- The `FormattedAmount` computed property on the Transaction model formats the amount as `+ ₹1,000` for income or `- ₹1,000` for expense using the `en-IN` culture.
- The transaction list is sorted by date descending by default.

**Key files:**
- `TransactionController.cs` — Index, AddOrEdit (GET + POST), Delete
- `Transaction.cs` — Model with FormattedAmount and CategoryTitleWithIcon computed properties
- `Views/Transaction/Index.cshtml` — Syncfusion Grid with action column template
- `Views/Transaction/AddOrEdit.cshtml` — Syncfusion DatePicker, DropDownList, NumericTextBox

---

### 4.4 Dashboard (DashboardController)

**How it works:**
- Queries all transactions from the last 7 days (today minus 6 days).
- Calculates Total Income, Total Expense, and Balance using LINQ.
- Builds two chart datasets:
  - **Doughnut chart**: Groups expenses by category, sums amounts.
  - **Spline chart**: Groups by date, creates a 7-day array and joins income/expense summaries.
- Passes all data to the view via ViewBag.
- Currency is formatted using `en-IN` culture (₹).

**Key files:**
- `DashboardController.cs` — Single Index action with all data aggregation
- `Views/Dashboard/Index.cshtml` — Syncfusion AccumulationChart + Chart components

---

### 4.5 Reports (ReportsController)

**How it works:**
- Accepts optional `startDate` and `endDate` query parameters (defaults to last 30 days).
- Queries all transactions in the date range with their categories.
- Builds 4 datasets for the view:
  1. Summary totals (income, expense, balance)
  2. Monthly grouped data for bar chart
  3. Category breakdown (all types) for the table
  4. Expense-only category breakdown for the doughnut chart
- The date filter form submits via GET so the URL is bookmarkable.

**Key files:**
- `ReportsController.cs` — Single Index action with date parameters
- `Views/Reports/Index.cshtml` — Date filter form + 4 data visualizations

---

### 4.6 Settings (SettingsController)

**How it works:**
- Loads the current user via `UserManager.GetUserAsync(User)`.
- **Update Profile**: Updates `UserName` and `Email` on the IdentityUser, then calls `RefreshSignInAsync` to update the auth cookie with the new username.
- **Change Password**: Calls `UserManager.ChangePasswordAsync` which verifies the current password before updating.
- Uses `TempData` for success/error messages that survive the redirect.

**Key files:**
- `SettingsController.cs` — Index (GET), UpdateProfile (POST), ChangePassword (POST)
- `SettingsViewModel.cs` — Combined view model for both forms
- `Views/Settings/Index.cshtml` — Two separate forms on the same page

---

### 4.7 Navbar (\_Layout.cshtml)

**How it works:**
- The navbar is part of `_Layout.cshtml` which wraps every authenticated page.
- Three Bootstrap dropdown components are used for notifications, messages, and profile.
- Notification and message badges are red circles with counts.
- JavaScript handles mark-as-read: removes the `unread` CSS class from items and hides the badge.
- The profile dropdown shows the logged-in username via `@User.Identity?.Name` (Razor server-side rendering).

---

### 4.8 Sidebar (\_SideBar.cshtml)

**How it works:**
- Uses Syncfusion EJ2 Sidebar component with dock mode (collapses to icon-only mode).
- Menu items are defined as a C# List of anonymous objects passed to the Syncfusion Menu component.
- The sidebar toggler button expands/collapses the sidebar using the EJ2 JavaScript API.
- Shows the logged-in username and a logout button in the profile section.

---

## 5. Data Flow

### Adding a Transaction (Example)

```
User fills Transaction form
        ↓
POST /Transaction/AddOrEdit
        ↓
TransactionController.AddOrEdit(Transaction model)
        ↓
ModelState validation (Amount > 0, CategoryId > 0)
        ↓
_context.Add(transaction) or _context.Update(transaction)
        ↓
_context.SaveChangesAsync() → SQL INSERT/UPDATE
        ↓
RedirectToAction("Index") → GET /Transaction
        ↓
TransactionController.Index()
        ↓
_context.Transactions.Include(t => t.Category).ToListAsync()
        ↓
View(transactions) → Renders Syncfusion Grid
```

### Login Flow

```
User submits Login form (email + password)
        ↓
POST /Account/Login
        ↓
_userManager.FindByEmailAsync(email) → Gets IdentityUser
        ↓
_signInManager.PasswordSignInAsync(username, password, ...)
        ↓
Identity verifies PBKDF2 hash
        ↓
Success → Issues auth cookie → Redirect to /
Failure → ModelState error → Re-render Login view
```

---

## 6. Technology Decisions

| Decision | Choice | Reason |
|----------|--------|--------|
| Web Framework | ASP.NET Core 6 MVC | Mature, well-supported, strong typing |
| ORM | Entity Framework Core 6 | Code-first migrations, LINQ queries |
| Authentication | ASP.NET Core Identity | Built-in, secure, integrates with EF Core |
| UI Components | Syncfusion EJ2 | Rich grid, chart, sidebar components |
| CSS Framework | Bootstrap 5 | Responsive layout, dropdown components |
| Database | SQL Server Express | Free, local, compatible with EF Core |
| Currency | en-IN (₹) | Application targets Indian users |
| Icons | Font Awesome 6.1.1 | Wide icon library, CDN hosted |

---

## 7. Known Limitations

| Limitation | Description |
|------------|-------------|
| No email verification | Users can register with any email without verification |
| No multi-currency | Currency is hardcoded to ₹ (en-IN culture) |
| No roles | All authenticated users have equal access to all data |
| Shared data | All users see all categories and transactions (no per-user data isolation) |
| No export | Reports cannot be exported to PDF or Excel |
| Static notifications | Notification and message panel content is hardcoded, not dynamic |
| Local only | App runs on localhost; not configured for production deployment |
| Syncfusion license | The embedded license key may expire; a valid key is required |

---

## 8. How to Run the Project

### Prerequisites
- .NET 6 SDK installed at `C:\Program Files\dotnet`
- SQL Server Express installed with instance `(local)\sqlexpress`

### Steps

**Option 1: Double-click the batch file**
```
c:\Users\Sam\Desktop\Expense Tracker\RunApp.bat
```

**Option 2: Manual terminal commands**
```cmd
set PATH=C:\Program Files\dotnet;%PATH%
cd "c:\Users\Sam\Desktop\Expense Tracker\Expense-Tracker-App\Expense Tracker"
dotnet run --urls http://localhost:5000
```

**Option 3: Run database migration (first time only)**
```cmd
set PATH=C:\Program Files\dotnet;C:\Users\Sam\.dotnet\tools;%PATH%
cd "c:\Users\Sam\Desktop\Expense Tracker\Expense-Tracker-App\Expense Tracker"
dotnet ef database update
dotnet run
```

Then open: **http://localhost:5000**

---

## 9. How to Extend the Application

### Add a New Page/Feature
1. Create a new Controller in `Controllers/`
2. Add `[Authorize]` attribute to the controller class
3. Create a folder in `Views/` matching the controller name
4. Add the view `.cshtml` file
5. Add a menu item in `Views/Shared/_SideBar.cshtml`

### Add a New Database Table
1. Create a new Model class in `Models/`
2. Add a `DbSet<YourModel>` property to `ApplicationDbContext.cs`
3. Run: `dotnet ef migrations add YourMigrationName`
4. Run: `dotnet ef database update`

### Add Per-User Data Isolation
1. Add a `UserId` string property to `Category` and `Transaction` models
2. Create a new migration
3. In each controller, filter queries with `.Where(x => x.UserId == _userManager.GetUserId(User))`
4. Set `UserId` when creating records

### Add PDF/Excel Export to Reports
1. Install NuGet package: `ClosedXML` (Excel) or `iTextSharp` (PDF)
2. Add an export action in `ReportsController.cs`
3. Add export buttons to `Views/Reports/Index.cshtml`

---

*End of Document of Understanding*
