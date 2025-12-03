using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContainerAutomationApp.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }

        public int WorkerId { get; set; }

        [ForeignKey("WorkerId")]
        public Worker? Worker { get; set; } = null!;

        public DateTime Date { get; set; } = DateTime.Now;

        public string LeaveType { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
       
}

}
