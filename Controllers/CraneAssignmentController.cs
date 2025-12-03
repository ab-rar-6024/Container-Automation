using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContainerAutomationApp.Data;
using ContainerAutomationApp.Models;
using ContainerAutomationApp.Services;
using System;
using System.Linq;

namespace ContainerAutomationApp.Controllers
{
    public class CraneAssignmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CraneAssignmentService _assignmentService;

        public CraneAssignmentController(ApplicationDbContext context, CraneAssignmentService assignmentService)
        {
            _context = context;
            _assignmentService = assignmentService;
        }

        // View all crane assignments
        public IActionResult Index()
        {
            var assignments = _context.CraneAssignments
                .Include(a => a.Crane)
                .Include(a => a.Shift)
                .Include(a => a.WorkerAssignments)
                    .ThenInclude(w => w.Worker)
                .OrderByDescending(a => a.Date)
                .ToList();

            return View(assignments);
        }

        // Auto-assign manpower
        public IActionResult AutoAssign()
        {
            _assignmentService.AutoAssign(DateTime.Today);
            TempData["Message"] = "Auto-assignment completed.";
            return RedirectToAction("Index");
        }

        // GET: Manual crane assignment form
        [HttpGet]
        public IActionResult Assign(int id)
        {
            var worker = _context.Workers
                .Include(w => w.AssignedCrane)
                .FirstOrDefault(w => w.Id == id);

            if (worker == null)
                return NotFound();

            // Show compatible cranes only
            var compatibleCranes = _context.Cranes
                .Where(c =>
                    (worker.Certification == "Both" || c.Type == worker.Certification)
                )
                .ToList();

            ViewBag.Cranes = compatibleCranes;
            return View(worker);
        }

        // POST: Save manual crane assignment
        [HttpPost]
        public IActionResult Assign(int workerId, int craneId)
        {
            var worker = _context.Workers.Find(workerId);
            var crane = _context.Cranes.Find(craneId);

            if (worker != null && crane != null)
            {
                worker.AssignedCraneId = crane.Id;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
