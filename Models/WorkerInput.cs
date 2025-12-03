public class WorkerInput
{
    public int WorkerId { get; set; }
    public string? Name { get; set; }
    public string? Skill { get; set; } // RTG / QC / Both
    public string? Availability { get; set; }
    public bool LeaveRequest { get; set; }
    public string? LeaveType { get; set; } // Normal / Special
}
