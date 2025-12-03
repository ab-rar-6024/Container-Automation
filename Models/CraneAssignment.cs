using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContainerAutomationApp.Models
{
    public class CraneAssignment
    {
        public int Id { get; set; }

        [Required]
        public int CraneId { get; set; }
        public Crane? Crane { get; set; }

        [Required]
        public int ShiftId { get; set; }
        public Shift? Shift { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public List<WorkerAssignment> WorkerAssignments { get; set; } = new();
    }
}
