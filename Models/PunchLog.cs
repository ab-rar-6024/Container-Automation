using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ContainerAutomationApp.Models; // ✅ This line is required if Worker/Crane are in same namespace

namespace ContainerAutomationApp.Models
{
    public class PunchLog
    {
        public int Id { get; set; }

        public int WorkerId { get; set; }
        public Worker? Worker { get; set; }

        public int CraneId { get; set; }
        public Crane? Crane { get; set; }

        public DateTime PunchInTime { get; set; }
        public DateTime? PunchOutTime { get; set; }
    }
}
