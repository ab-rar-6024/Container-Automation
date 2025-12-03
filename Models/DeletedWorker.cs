using System;
namespace ContainerAutomationApp.Models
{
    public class DeletedWorker
    {
        public int Id { get; set; } // This ID is unique to this table
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Certification { get; set; }
        public string? Crane { get; set; }
        public string? Shift { get; set; }
        
        public DateTime DeletedAt { get; set; }
    }
}