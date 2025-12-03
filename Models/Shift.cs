using System.ComponentModel.DataAnnotations;
namespace ContainerAutomationApp.Models
{
    public class Shift
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        // ✅ Fix: Add this
        public ICollection<Worker> AssignedWorkers { get; set; } = new List<Worker>();
    }
}
