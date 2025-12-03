using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContainerAutomationApp.Data;
using System.Linq;

namespace ContainerAutomationApp.Controllers
{
    public class AssignmentHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssignmentHistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var history = _context.AssignmentHistories
                .Include(a => a.Worker)
                .Include(a => a.Crane)
                .Include(a => a.Shift)
                .Where(h => h.Date == DateTime.Today)
                .ToList();

            return View(history);
        }
    }
}
