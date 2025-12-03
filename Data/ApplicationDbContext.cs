using Microsoft.EntityFrameworkCore;
using ContainerAutomationApp.Models;

namespace ContainerAutomationApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Worker> Workers { get; set; }
        public DbSet<Crane> Cranes { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<CraneAssignment> CraneAssignments { get; set; }
        public DbSet<WorkerAssignment> WorkerAssignments { get; set; }
        public DbSet<AssignmentHistory> AssignmentHistories { get; set; }


        public DbSet<DeletedWorker> DeletedWorkers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Worker → Crane: Many workers assigned to one crane
            modelBuilder.Entity<Worker>()
                .HasOne(w => w.AssignedCrane)
                .WithMany(c => c.AssignedWorkers)
                .HasForeignKey(w => w.AssignedCraneId)
                .OnDelete(DeleteBehavior.SetNull);

            // Worker → Shift: Many workers assigned to one shift
            modelBuilder.Entity<Worker>()
                .HasOne(w => w.AssignedShift)
                .WithMany()
                .HasForeignKey(w => w.AssignedShiftId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
