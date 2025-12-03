using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContainerAutomationApp.Data;
using ContainerAutomationApp.Models;

namespace ContainerAutomationApp.Controllers
{
    public class ChartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var allWorkers = await _context.Workers.ToListAsync();
            var leaveWorkerIds = await _context.LeaveRequests
                .Where(l => l.Status == "Approved")
                .Select(l => l.WorkerId)
                .ToListAsync();

            var onLeave = allWorkers.Count(w => leaveWorkerIds.Contains(w.Id));
            var assigned = allWorkers.Count(w => w.AssignedCraneId != null && !leaveWorkerIds.Contains(w.Id));
            var unassigned = allWorkers.Count - assigned - onLeave;

            ViewBag.OnLeave = onLeave;
            ViewBag.Assigned = assigned;
            ViewBag.Unassigned = unassigned;

            return View();
        }
    }
}
