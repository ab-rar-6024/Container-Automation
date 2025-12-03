using System;
using System.Linq;
using System.Collections.Generic;
using ContainerAutomationApp.Data;
using ContainerAutomationApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ContainerAutomationApp.Services
{
    public class CraneAssignmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly Random _random = new();

        public CraneAssignmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AutoAssign(DateTime forDate)
        {
            var shifts = _context.Shifts.ToList();
            var cranes = _context.Cranes.ToList();
            var workers = _context.Workers
                .Include(w => w.History)
                .Where(w => !_context.LeaveRequests.Any(l => l.WorkerId == w.Id && l.Date == forDate && l.Status == "Approved"))
                .ToList();

            // Track used workers
            var usedWorkerIds = new HashSet<int>();

            foreach (var shift in shifts)
            {
                var availableWorkers = workers
                    .Where(w => !usedWorkerIds.Contains(w.Id))
                    .OrderBy(w => _random.Next())
                    .ToList();

                foreach (var crane in cranes)
                {
                    var compatible = availableWorkers
                        .Where(w => w.Certification == crane.Type || w.Certification == "Both")
                        .GroupBy(w => w.Role)
                        .ToDictionary(g => g.Key, g => g.ToList());

                    if (!compatible.ContainsKey("Senior") || compatible["Senior"].Count < 2 || !compatible.ContainsKey("Junior") || compatible["Junior"].Count < 1)
                        continue;

                    var selected = new List<Worker>();
                    selected.AddRange(compatible["Senior"].Take(2));
                    selected.AddRange(compatible["Junior"].Take(1));

                    foreach (var worker in selected)
                    {
                        // Save history
                        _context.AssignmentHistories.Add(new AssignmentHistory
                        {
                            WorkerId = worker.Id,
                            CraneId = crane.Id,
                            ShiftId = shift.Id,
                            Date = forDate
                        });

                        // Mark used
                        usedWorkerIds.Add(worker.Id);
                    }
                }
            }

            _context.SaveChanges();
        }
    }
}
