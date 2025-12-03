using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContainerAutomationApp.Models
{
    public class Leave
    {
        public int Id { get; set; }

        [Required]
        public int WorkerId { get; set; }

        [ForeignKey("WorkerId")]
        public Worker? Worker { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string LeaveType { get; set; } = "Normal"; // ✅ ADDED: LeaveType field (e.g., Normal, Emergency)

        public string Status { get; set; } = "Pending";
    }
}
