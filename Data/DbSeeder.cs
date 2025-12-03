using ContainerAutomationApp.Data;
using ContainerAutomationApp.Models;


public static class DbSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (!context.Admins.Any())
        {
            context.Admins.Add(new Admin { Username = "admin", Password = "admin123" });
        }

        if (!context.Cranes.Any())
        {
            context.Cranes.AddRange(
                new Crane { Name = "Crane-A", Type = "RTG" },
                new Crane { Name = "Crane-B", Type = "QC" }
            );
        }

        if (!context.Shifts.Any())
        {
            context.Shifts.AddRange(
                new Shift { Name = "Morning", StartTime = DateTime.Today.AddHours(6), EndTime = DateTime.Today.AddHours(14) },
                new Shift { Name = "Evening", StartTime = DateTime.Today.AddHours(14), EndTime = DateTime.Today.AddHours(22) }
            );
        }

        context.SaveChanges();
    }
}
