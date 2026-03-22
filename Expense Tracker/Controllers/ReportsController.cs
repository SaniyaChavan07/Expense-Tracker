using Expense_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Expense_Tracker.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CultureInfo _inCulture;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
            _inCulture = CultureInfo.CreateSpecificCulture("en-IN");
            _inCulture.NumberFormat.CurrencyNegativePattern = 1;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            DateTime start = startDate ?? DateTime.Today.AddDays(-29);
            DateTime end = endDate ?? DateTime.Today;

            ViewBag.StartDate = start.ToString("yyyy-MM-dd");
            ViewBag.EndDate = end.ToString("yyyy-MM-dd");

            var transactions = await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.Date >= start && t.Date <= end)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            int totalIncome = transactions.Where(t => t.Category!.Type == "Income").Sum(t => t.Amount);
            int totalExpense = transactions.Where(t => t.Category!.Type == "Expense").Sum(t => t.Amount);
            int balance = totalIncome - totalExpense;

            ViewBag.TotalIncome = String.Format(_inCulture, "{0:C0}", totalIncome);
            ViewBag.TotalExpense = String.Format(_inCulture, "{0:C0}", totalExpense);
            ViewBag.Balance = String.Format(_inCulture, "{0:C0}", balance);

            // Monthly bar chart data
            ViewBag.MonthlyData = transactions
                .GroupBy(t => new { t.Date.Year, t.Date.Month })
                .Select(g => new
                {
                    month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
                    income = g.Where(t => t.Category!.Type == "Income").Sum(t => t.Amount),
                    expense = g.Where(t => t.Category!.Type == "Expense").Sum(t => t.Amount),
                })
                .OrderBy(x => x.month)
                .ToList();

            // Category breakdown
            ViewBag.CategoryBreakdown = transactions
                .GroupBy(t => new { t.Category!.CategoryId, t.Category.Title, t.Category.Icon, t.Category.Type })
                .Select(g => new
                {
                    title = g.Key.Icon + " " + g.Key.Title,
                    type = g.Key.Type,
                    total = g.Sum(t => t.Amount),
                    formattedTotal = String.Format(_inCulture, "{0:C0}", g.Sum(t => t.Amount)),
                    count = g.Count()
                })
                .OrderByDescending(x => x.total)
                .ToList();

            // Category breakdown - expenses only for doughnut
            ViewBag.ExpenseCategoryBreakdown = transactions
                .Where(t => t.Category!.Type == "Expense")
                .GroupBy(t => new { t.Category!.CategoryId, t.Category.Title, t.Category.Icon })
                .Select(g => new
                {
                    title = g.Key.Icon + " " + g.Key.Title,
                    total = g.Sum(t => t.Amount),
                    formattedTotal = String.Format(_inCulture, "{0:C0}", g.Sum(t => t.Amount)),
                })
                .OrderByDescending(x => x.total)
                .ToList();

            ViewBag.Transactions = transactions;

            return View();
        }
    }
}
