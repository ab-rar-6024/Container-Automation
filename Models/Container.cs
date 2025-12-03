using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContainerAutomationApp.Models
{
    public class Container
    {
        public int Id { get; set; }

        [Required]
        public string ContainerNumber { get; set; } = string.Empty;

        [Required]
        public string Size { get; set; } = string.Empty;  // e.g., 20ft or 40ft

        public string? Location { get; set; }

        // ✅ These two lines were likely missing:
        public int? AssignedCraneId { get; set; }

        [ForeignKey("AssignedCraneId")]
        public Crane? AssignedCrane { get; set; }

        public DateTime AssignedTime { get; set; } = DateTime.Now;
    }
}
