using Expense_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public SettingsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new SettingsViewModel
            {
                UserName = user!.UserName ?? "",
                Email = user.Email ?? ""
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(SettingsViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            user.UserName = model.UserName;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["Success"] = "Profile updated successfully.";
            }
            else
            {
                foreach (var error in result.Errors)
                    TempData["Error"] = error.Description;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(SettingsViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            if (model.NewPassword != model.ConfirmNewPassword)
            {
                TempData["Error"] = "New passwords do not match.";
                return RedirectToAction("Index");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword!, model.NewPassword!);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["Success"] = "Password changed successfully.";
            }
            else
            {
                foreach (var error in result.Errors)
                    TempData["Error"] = error.Description;
            }

            return RedirectToAction("Index");
        }
    }
}
