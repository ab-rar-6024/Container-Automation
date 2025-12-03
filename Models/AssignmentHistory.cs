using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContainerAutomationApp.Models
{
    public class AssignmentHistory
    {
        [Key]
        public int Id { get; set; }

        public int WorkerId { get; set; }
        [ForeignKey("WorkerId")]
        public Worker Worker { get; set; }

        public int CraneId { get; set; }
        [ForeignKey("CraneId")]
        public Crane Crane { get; set; }

        public int ShiftId { get; set; }
        [ForeignKey("ShiftId")]
        public Shift Shift { get; set; }

        public DateTime Date { get; set; } // ✅ Add this line

        public string Role { get; set; } = string.Empty;

        public string Status { get; set; } = "Assigned";
    }
}
