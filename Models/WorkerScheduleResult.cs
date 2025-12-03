namespace ContainerAutomationApp.Models
{
    public class WorkerScheduleResult
    {
        public int WorkerId { get; set; }
        public string? Name { get; set; }
        public string? AssignedCrane { get; set; }
        public string? Shift { get; set; }
        public string? LeaveStatus { get; set; }
        public string? ColorCode { get; set; }
        public int WorkHours { get; set; }
        public int RestHours { get; set; }
    }
}
