using Microsoft.AspNetCore.Mvc;
using ContainerAutomationApp.Data;
using ContainerAutomationApp.Models;
using ContainerAutomationApp.Filters;

namespace ContainerAutomationApp.Controllers
{
    [AdminOnly] // require login for dashboard
    public class HomeController : Controller
    {
        public IActionResult Dashboard()
        {
            ViewBag.Username = HttpContext.Session.GetString("AdminUsername");
            return View();
        }
    }
}
