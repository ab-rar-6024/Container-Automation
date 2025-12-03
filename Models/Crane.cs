namespace ContainerAutomationApp.Models
{
    public class Crane
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; } // RTG or QC

        public ICollection<Worker> AssignedWorkers { get; set; } = new List<Worker>();
    }
}
