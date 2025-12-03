using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContainerAutomationApp.Data;
using ContainerAutomationApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ContainerAutomationApp.Controllers
{
    public class CraneController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CraneController(ApplicationDbContext context)
        {
            _context = context;
        }

        // List all cranes
        public IActionResult Index()
        {
            var cranes = _context.Cranes.ToList();
            return View(cranes);
        }

        // Create crane - GET
        public IActionResult Create()
        {
            return View();
        }

        // Create crane - POST
        [HttpPost]
        public IActionResult Create(Crane crane)
        {
            if (ModelState.IsValid)
            {
                _context.Cranes.Add(crane);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crane);
        }

        // Edit crane - GET
        public IActionResult Edit(int id)
        {
            var crane = _context.Cranes.Find(id);
            return crane == null ? NotFound() : View(crane);
        }

        // Edit crane - POST
        [HttpPost]
        public IActionResult Edit(Crane crane)
        {
            if (ModelState.IsValid)
            {
                _context.Cranes.Update(crane);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crane);
        }

        // Delete crane - GET
        public IActionResult Delete(int id)
        {
            var crane = _context.Cranes.Find(id);
            return crane == null ? NotFound() : View(crane);
        }

        // Delete crane - POST
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var crane = _context.Cranes.Find(id);
            if (crane != null)
            {
                _context.Cranes.Remove(crane);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // 🗺️ Map view to show Crane → Assigned Workers
        public async Task<IActionResult> Map()
        {
            var cranes = await _context.Cranes
                .Include(c => c.AssignedWorkers)
                .ToListAsync();

            return View(cranes);
        }
    }
}
