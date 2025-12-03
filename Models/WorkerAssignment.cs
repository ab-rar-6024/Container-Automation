using System;
using System.ComponentModel.DataAnnotations;

namespace ContainerAutomationApp.Models
{
    public class WorkerAssignment
    {
        public int Id { get; set; }

        [Required]
        public int WorkerId { get; set; }
        public Worker? Worker { get; set; }

        [Required]
        public int CraneAssignmentId { get; set; }
        public CraneAssignment? CraneAssignment { get; set; }

        public TimeSpan WorkDuration { get; set; } = TimeSpan.FromHours(6);
        public TimeSpan RestDuration { get; set; } = TimeSpan.FromHours(2);

        public string? PunchInTime { get; set; }
        public string? PunchOutTime { get; set; }

        public string Status { get; set; } = "Assigned"; // or “Completed”
    }
}
