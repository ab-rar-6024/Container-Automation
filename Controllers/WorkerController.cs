using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContainerAutomationApp.Data;
using ContainerAutomationApp.Models;
using OfficeOpenXml;
using System.Text;

namespace ContainerAutomationApp.Controllers
{
    public class WorkerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ INDEX: Show all workers + leave status mapping
        public IActionResult Index(string search, string leaveStatus)
        {
            var today = DateTime.Today;

            var workers = _context.Workers
                .Include(w => w.AssignedCrane)
                .Include(w => w.AssignedShift)
                .Include(w => w.LeaveRequests)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                workers = workers.Where(w => w.Name.Contains(search));
            }

            var leaveMap = new Dictionary<int, string>();
            foreach (var w in workers)
            {
                var todayLeave = w.LeaveRequests.FirstOrDefault(l => l.Date.Date == today);
                if (todayLeave != null)
                    leaveMap[w.Id] = todayLeave.Status!;
            }

            if (!string.IsNullOrEmpty(leaveStatus))
            {
                workers = workers.Where(w => leaveMap.ContainsKey(w.Id) && leaveMap[w.Id] == leaveStatus);
            }

            ViewBag.LeaveStatusMap = leaveMap;
            ViewBag.SelectedStatus = leaveStatus;
            ViewBag.Search = search;

            return View(workers.ToList());
        }

        // CREATE
        public IActionResult Create()
        {
            ViewBag.CraneList = new SelectList(_context.Cranes, "Id", "Name");
            ViewBag.ShiftList = new SelectList(_context.Shifts, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Worker worker)
        {
            if (ModelState.IsValid)
            {
                worker.AssignedCraneId = GetCompatibleCraneId(worker.Certification);
                worker.AssignedShiftId = GetAvailableShiftId();
                _context.Workers.Add(worker);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CraneList = new SelectList(_context.Cranes, "Id", "Name", worker.AssignedCraneId);
            ViewBag.ShiftList = new SelectList(_context.Shifts, "Id", "Name", worker.AssignedShiftId);
            return View(worker);
        }

        // EDIT
        public IActionResult Edit(int id)
        {
            var worker = _context.Workers.Find(id);
            if (worker == null) return NotFound();

            ViewBag.CraneList = new SelectList(_context.Cranes, "Id", "Name", worker.AssignedCraneId);
            ViewBag.ShiftList = new SelectList(_context.Shifts, "Id", "Name", worker.AssignedShiftId);
            return View(worker);
        }

        [HttpPost]
        public IActionResult Edit(int id, Worker model)
        {
            var worker = _context.Workers.Find(id);
            if (worker == null) return NotFound();

            if (ModelState.IsValid)
            {
                worker.Name = model.Name;
                worker.Phone = model.Phone;
                worker.Certification = model.Certification;
                worker.AssignedCraneId = model.AssignedCraneId;
                worker.AssignedShiftId = model.AssignedShiftId;

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CraneList = new SelectList(_context.Cranes, "Id", "Name", model.AssignedCraneId);
            ViewBag.ShiftList = new SelectList(_context.Shifts, "Id", "Name", model.AssignedShiftId);
            return View(model);
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            var worker = _context.Workers
                .Include(w => w.AssignedCrane)
                .Include(w => w.AssignedShift)
                .FirstOrDefault(w => w.Id == id);

            if (worker == null) return NotFound();
            return View(worker);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var worker = _context.Workers
                .Include(w => w.AssignedCrane)
                .Include(w => w.AssignedShift)
                .FirstOrDefault(w => w.Id == id);

            if (worker != null)
            {
                _context.DeletedWorkers.Add(new DeletedWorker
                {
                    Name = worker.Name,
                    Phone = worker.Phone,
                    Certification = worker.Certification,
                    Crane = worker.AssignedCrane?.Name ?? "Unassigned",
                    Shift = worker.AssignedShift?.Name ?? "Unassigned",
                    DeletedAt = DateTime.Now
                });

                _context.Workers.Remove(worker);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // DELETE ALL
        [HttpPost]
        public IActionResult DeleteAll()
        {
            var allWorkers = _context.Workers
                .Include(w => w.AssignedCrane)
                .Include(w => w.AssignedShift)
                .ToList();

            foreach (var w in allWorkers)
            {
                _context.DeletedWorkers.Add(new DeletedWorker
                {
                    Name = w.Name,
                    Phone = w.Phone,
                    Certification = w.Certification,
                    Crane = w.AssignedCrane?.Name ?? "Unassigned",
                    Shift = w.AssignedShift?.Name ?? "Unassigned",
                    DeletedAt = DateTime.Now
                });
            }

            _context.Workers.RemoveRange(allWorkers);
            _context.SaveChanges();
            _context.Database.ExecuteSqlRaw("ALTER TABLE Workers AUTO_INCREMENT = 1;");

            TempData["Message"] = "All workers deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // ================= Excel Upload =================

        [HttpGet]
        public IActionResult UploadExcel() => View();

        [HttpPost]
        public async Task<IActionResult> PreviewExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please upload a valid Excel file.";
                return RedirectToAction("UploadExcel");
            }

            var workers = new List<Worker>();
            var shifts = await _context.Shifts.Include(s => s.AssignedWorkers).ToListAsync();
            var cranes = await _context.Cranes.Include(c => c.AssignedWorkers).ToListAsync();

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            var sheet = package.Workbook.Worksheets.FirstOrDefault();
            if (sheet == null)
            {
                TempData["Error"] = "Sheet not found.";
                return RedirectToAction("UploadExcel");
            }

            for (int row = 2; row <= sheet.Dimension.End.Row; row++)
            {
                var name = sheet.Cells[row, 1].Text?.Trim();
                var phone = sheet.Cells[row, 2].Text?.Trim() ?? "0000000000";
                var cert = sheet.Cells[row, 3].Text?.Trim() ?? "RTG";

                if (string.IsNullOrEmpty(name)) continue;

                var shiftId = shifts.OrderBy(s => s.AssignedWorkers.Count + workers.Count(w => w.AssignedShiftId == s.Id)).FirstOrDefault()?.Id;
                var compatibleCranes = cranes
                    .Where(c => cert == "Both" || c.Type == cert)
                    .OrderBy(c => c.AssignedWorkers.Count + workers.Count(w => w.AssignedCraneId == c.Id))
                    .ToList();
                var craneId = compatibleCranes.FirstOrDefault()?.Id;

                workers.Add(new Worker
                {
                    Name = name,
                    Phone = phone,
                    Certification = cert,
                    AssignedShiftId = shiftId,
                    AssignedCraneId = craneId
                });
            }

            TempData["ExcelWorkers"] = System.Text.Json.JsonSerializer.Serialize(workers);
            return View("PreviewExcel", workers);
        }

        [HttpPost]
        public IActionResult ConfirmExcelUpload()
        {
            if (TempData["ExcelWorkers"] == null)
                return RedirectToAction(nameof(Index));

            var data = TempData["ExcelWorkers"].ToString();
            var workers = System.Text.Json.JsonSerializer.Deserialize<List<Worker>>(data);

            if (workers != null)
            {
                _context.Workers.AddRange(workers);
                _context.SaveChanges();
            }

            TempData["Message"] = "Workers uploaded successfully.";
            return RedirectToAction(nameof(Index));
        }

        // ============== Helper Methods ==============

        private int? GetAvailableShiftId()
        {
            var shifts = _context.Shifts.Include(s => s.AssignedWorkers).ToList();
            return shifts.OrderBy(s => s.AssignedWorkers.Count()).FirstOrDefault()?.Id;
        }

        private int? GetCompatibleCraneId(string cert)
        {
            var cranes = _context.Cranes.Include(c => c.AssignedWorkers).ToList();
            var eligible = (cert == "Both") ? cranes : cranes.Where(c => c.Type == cert).ToList();
            return eligible.OrderBy(c => c.AssignedWorkers.Count()).FirstOrDefault()?.Id;
        }
    }
}
