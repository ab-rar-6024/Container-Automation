using ContainerAutomationApp.Models;

namespace ContainerAutomationApp.Helpers
{
    public static class SchedulerHelper
    {
        public static List<WorkerScheduleResult> ProcessWorkerSchedule(List<WorkerInput> workers)
        {
            var result = new List<WorkerScheduleResult>();
            var shifts = new[] { "Morning", "Evening", "Night" };
            var cranes = new Dictionary<string, List<string>>
            {
                { "QC", Enumerable.Range(1, 6).Select(i => $"QC-{i}").ToList() },
                { "RTG", Enumerable.Range(1, 6).Select(i => $"RTG-{i}").ToList() }
            };

            foreach (var shift in shifts)
            {
                var availableWorkers = workers
                    .Where(w => w.Availability == shift && !w.LeaveRequest)
                    .ToList();

                var leaveRequests = workers
                    .Where(w => w.Availability == shift && w.LeaveRequest)
                    .ToList();

                // ✅ Assign 2 normal leaves
                var approvedLeaves = leaveRequests
                    .Where(l => l.LeaveType == "Normal")
                    .Take(2)
                    .ToList();

                foreach (var leave in approvedLeaves)
                {
                    result.Add(new WorkerScheduleResult
                    {
                        WorkerId = leave.WorkerId,
                        Name = leave.Name,
                        Shift = shift,
                        AssignedCrane = "",
                        LeaveStatus = "Approved Leave",
                        ColorCode = "Red",
                        WorkHours = 0,
                        RestHours = 0
                    });
                }

                // ✅ Remaining leave requests need manager approval
                var pendingLeaves = leaveRequests.Except(approvedLeaves).ToList();
                foreach (var leave in pendingLeaves)
                {
                    result.Add(new WorkerScheduleResult
                    {
                        WorkerId = leave.WorkerId,
                        Name = leave.Name,
                        Shift = shift,
                        AssignedCrane = "",
                        LeaveStatus = "Pending Leave",
                        ColorCode = "Blue",
                        WorkHours = 0,
                        RestHours = 0
                    });
                }

                // ✅ Assign 12 workers to cranes
                var assignedWorkers = new List<WorkerInput>();
                int assignedCount = 0;
                foreach (var craneType in cranes.Keys)
                {
                    foreach (var crane in cranes[craneType])
                    {
                        var eligibleWorker = availableWorkers
                            .FirstOrDefault(w =>
                                w.Skill == craneType || w.Skill == "Both");

                        if (eligibleWorker != null && assignedCount < 12)
                        {
                            result.Add(new WorkerScheduleResult
                            {
                                WorkerId = eligibleWorker.WorkerId,
                                Name = eligibleWorker.Name,
                                Shift = shift,
                                AssignedCrane = crane,
                                LeaveStatus = "Assigned",
                                ColorCode = "Green",
                                WorkHours = 6,
                                RestHours = 0
                            });

                            assignedWorkers.Add(eligibleWorker);
                            availableWorkers.Remove(eligibleWorker);
                            assignedCount++;
                        }
                    }
                }

                // ✅ Assign 4 workers to rest
                var restingWorkers = availableWorkers.Take(4).ToList();
                foreach (var restWorker in restingWorkers)
                {
                    result.Add(new WorkerScheduleResult
                    {
                        WorkerId = restWorker.WorkerId,
                        Name = restWorker.Name,
                        Shift = shift,
                        AssignedCrane = "",
                        LeaveStatus = "Rest",
                        ColorCode = "Yellow",
                        WorkHours = 6,
                        RestHours = 2
                    });
                }
            }

            return result;
        }
    }
}
