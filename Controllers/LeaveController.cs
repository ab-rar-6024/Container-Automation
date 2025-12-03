// ✅ Leave Controller: Auto-approval + Admin status update + Delete + Count
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContainerAutomationApp.Data;
using ContainerAutomationApp.Models;

namespace ContainerAutomationApp.Controllers
{
    public class LeaveController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ View all leave requests
        public IActionResult Index()
        {
            var requests = _context.LeaveRequests
                .Include(l => l.Worker)
                .OrderByDescending(l => l.Date)
                .ToList();

            return View(requests);
        }

        // ✅ Show Create form
        public IActionResult Create()
        {
            ViewBag.Workers = new SelectList(_context.Workers, "Id", "Name");
            return View();
        }

        // ✅ Handle Create POST with auto-approval logic
        [HttpPost]
        public IActionResult Create(int workerId, DateTime date, string leaveType)
        {
            if (workerId == 0 || date == default)
            {
                TempData["Error"] = "Invalid submission.";
                ViewBag.Workers = new SelectList(_context.Workers, "Id", "Name");
                return View();
            }

            var worker = _context.Workers.FirstOrDefault(w => w.Id == workerId);
            if (worker == null)
            {
                TempData["Error"] = "Worker not found.";
                ViewBag.Workers = new SelectList(_context.Workers, "Id", "Name");
                return View();
            }

            var leaveCount = _context.LeaveRequests
                .Include(l => l.Worker)
                .Count(l =>
                    l.Date.Date == date.Date &&
                    l.Status == "Approved" &&
                    l.Worker.AssignedShiftId == worker.AssignedShiftId
                );

            var status = leaveCount < 2 ? "Approved" : "Pending";

            var leave = new LeaveRequest
            {
                WorkerId = workerId,
                Date = date,
                LeaveType = leaveType,
                Status = status
            };

            _context.LeaveRequests.Add(leave);
            _context.SaveChanges();

            TempData["Message"] = $"Leave request {status}.";
            return RedirectToAction("Index");
        }

        // ✅ GET: Edit Leave
        public async Task<IActionResult> Edit(int id)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);
            if (leave == null) return NotFound();

            ViewBag.Workers = new SelectList(_context.Workers, "Id", "Name", leave.WorkerId);
            ViewBag.LeaveTypes = new SelectList(new[] { "Normal", "Special" }, leave.LeaveType);
            ViewBag.StatusOptions = new SelectList(new[] { "Pending", "Approved", "Rejected" }, leave.Status);

            return View(leave);
        }

        // ✅ POST: Edit Leave (admin can change status)
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WorkerId,Date,LeaveType,Status")] LeaveRequest model)
        {
            var existing = await _context.LeaveRequests.FindAsync(id);
            if (existing == null) return NotFound();

            if (ModelState.IsValid)
            {
                existing.WorkerId = model.WorkerId;
                existing.Date = model.Date;
                existing.LeaveType = model.LeaveType;
                existing.Status = model.Status;

                await _context.SaveChangesAsync();
                TempData["Message"] = "Leave updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Workers = new SelectList(_context.Workers, "Id", "Name", model.WorkerId);
            ViewBag.LeaveTypes = new SelectList(new[] { "Normal", "Special" }, model.LeaveType);
            ViewBag.StatusOptions = new SelectList(new[] { "Pending", "Approved", "Rejected" }, model.Status);

            return View(model);
        }

        // ✅ GET: Confirm Delete
        public async Task<IActionResult> Delete(int id)
        {
            var leave = await _context.LeaveRequests
                .Include(l => l.Worker)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (leave == null) return NotFound();
            return View(leave);
        }

        // ✅ POST: Confirm Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);
            if (leave != null)
            {
                _context.LeaveRequests.Remove(leave);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // ✅ Approve a pending leave
        public IActionResult Approve(int id)
        {
            var leave = _context.LeaveRequests.Find(id);
            if (leave != null && leave.Status == "Pending")
            {
                leave.Status = "Approved";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // ✅ Reject a pending leave
        public IActionResult Reject(int id)
        {
            var leave = _context.LeaveRequests.Find(id);
            if (leave != null && leave.Status == "Pending")
            {
                leave.Status = "Rejected";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // ✅ JSON badge count for pending leaves
        [HttpGet]
        public IActionResult PendingCount()
        {
            var count = _context.LeaveRequests.Count(l => l.Status == "Pending");
            return Json(new { count });
        }
    }
}