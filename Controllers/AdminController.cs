using Microsoft.AspNetCore.Mvc;
using ContainerAutomationApp.Data;
using ContainerAutomationApp.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ContainerAutomationApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ========== LOGIN ==========

        // Show login page
        public IActionResult Login()
        {
            return View();
        }

        // Handle login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
public IActionResult Login(string username, string password)
{
    var admin = _context.Admins
        .FirstOrDefault(a => a.Username == username && a.Password == password);

    if (admin != null)
    {
        HttpContext.Session.SetString("AdminUsername", admin.Username);
        return RedirectToAction("Dashboard", "Home");
    }

    ViewBag.Error = "Invalid credentials.";
    return View();
}


        // ========== REGISTER ==========

        // Show register page
        public IActionResult Register()
        {
            return View();
        }

        // Handle register POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(string username, string password)
        {
            if (_context.Admins.Any(a => a.Username == username))
            {
                ViewBag.Error = "Username already exists.";
                return View();
            }

            var admin = new Admin
            {
                Username = username,
                Password = password // ⚠️ Consider hashing for production
            };

            _context.Admins.Add(admin);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // ========== LOGOUT ==========

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
