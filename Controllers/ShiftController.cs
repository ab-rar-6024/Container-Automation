using Microsoft.AspNetCore.Mvc;
using ContainerAutomationApp.Data;
using ContainerAutomationApp.Models;

public class ShiftController : Controller
{
    private readonly ApplicationDbContext _context;

    public ShiftController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View(_context.Shifts.ToList());
    }

    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Shift shift)
    {
        _context.Shifts.Add(shift);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id) =>
        View(_context.Shifts.FirstOrDefault(s => s.Id == id));

    [HttpPost]
    public IActionResult Edit(Shift shift)
    {
        _context.Shifts.Update(shift);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id) =>
        View(_context.Shifts.FirstOrDefault(s => s.Id == id));

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var shift = _context.Shifts.Find(id);
        if (shift != null)
        {
            _context.Shifts.Remove(shift);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
