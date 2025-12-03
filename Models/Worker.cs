using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContainerAutomationApp.Models
{
    public class Worker
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? Certification { get; set; } // RTG / QC / Both

        public string? Role { get; set; } // Senior / Junior

        public int? AssignedShiftId { get; set; }

        [ForeignKey("AssignedShiftId")]
        public Shift? AssignedShift { get; set; }

        public int? AssignedCraneId { get; set; }

        [ForeignKey("AssignedCraneId")]
        public Crane? AssignedCrane { get; set; }

        public List<LeaveRequest> LeaveRequests { get; set; } = new();

        public ICollection<AssignmentHistory> History { get; set; } = new List<AssignmentHistory>();

        // Computed property (not mapped to DB)
        [NotMapped]
        public bool IsAvailable => AssignedCraneId.HasValue && AssignedShiftId.HasValue;
    }
}
