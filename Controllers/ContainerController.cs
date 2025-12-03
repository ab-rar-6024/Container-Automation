using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContainerAutomationApp.Data;
using ContainerAutomationApp.Models;
using System.Linq;

namespace ContainerAutomationApp.Controllers
{
    public class ContainerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContainerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Container/
        public IActionResult Index()
        {
            var containers = _context.Containers
                .Include(c => c.AssignedCrane)
                .ToList();
            return View(containers);
        }

        // GET: /Container/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Container/Create
        [HttpPost]
        public IActionResult Create(Container container)
        {
            var craneType = container.Location == "Ship" ? "QC" : "RTG";

            var availableCrane = _context.Cranes
                .FirstOrDefault(c => c.Type == craneType &&
                    !_context.Containers.Any(cont => cont.AssignedCraneId == c.Id));

            if (availableCrane != null)
                container.AssignedCraneId = availableCrane.Id;

            _context.Containers.Add(container);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Container/Assign/{id}
        [HttpGet]
        public IActionResult Assign(int id)
        {
            var container = _context.Containers
                .Include(c => c.AssignedCrane)
                .FirstOrDefault(c => c.Id == id);

            if (container == null)
                return NotFound();

            var craneType = container.Location == "Ship" ? "QC" : "RTG";
            var compatibleCranes = _context.Cranes
                .Where(c => c.Type == craneType)
                .ToList();

            ViewBag.Cranes = compatibleCranes;
            return View(container);
        }

        // POST: /Container/Assign
        [HttpPost]
        public IActionResult Assign(int containerId, int craneId)
        {
            var container = _context.Containers.Find(containerId);
            if (container != null)
            {
                container.AssignedCraneId = craneId;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
