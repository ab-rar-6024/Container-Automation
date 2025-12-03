using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ContainerAutomationApp.Models;
using Microsoft.AspNetCore.Http;

namespace ContainerAutomationApp.Helpers
{
    public static class ExcelHelper
    {
        // ✅ For Worker model (simple import)
        public static List<Worker> ParseWorkersFromExcel(IFormFile file)
        {
            List<Worker> workers = new();
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null) return workers;

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    string name = worksheet.Cells[row, 1].Text?.Trim();
                    string phone = worksheet.Cells[row, 2].Text?.Trim();
                    string certification = worksheet.Cells[row, 3].Text?.Trim();

                    if (!string.IsNullOrEmpty(name))
                    {
                        workers.Add(new Worker
                        {
                            Name = name,
                            Phone = phone,
                            Certification = certification
                        });
                    }
                }
            }
            return workers;
        }

        // ✅ Read full worker input (ID, Skill, Availability, etc.)
        public static List<WorkerInput> ReadWorkerExcel(IFormFile file)
        {
            var workers = new List<WorkerInput>();
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets.First();
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    workers.Add(new WorkerInput
                    {
                        WorkerId = int.TryParse(worksheet.Cells[row, 1].Text, out var id) ? id : 0,
                        Name = worksheet.Cells[row, 2].Text?.Trim(),
                        Skill = worksheet.Cells[row, 3].Text?.Trim(),
                        Availability = worksheet.Cells[row, 4].Text?.Trim(),
                        LeaveRequest = worksheet.Cells[row, 5].Text?.Trim().ToLower() == "yes",
                        LeaveType = worksheet.Cells[row, 6].Text?.Trim()
                    });
                }
            }
            return workers;
        }

        // ✅ Output final Excel schedule
        public static string GenerateOutputExcel(List<WorkerScheduleResult> workers)

        {
            var colorMap = new Dictionary<string, string>
            {
                { "Green", "90EE90" },
                { "Red", "FF9999" },
                { "Blue", "ADD8E6" },
                { "Yellow", "FFFF99" }
            };

            var outputPath = Path.Combine(Path.GetTempPath(), $"FinalSchedule_{DateTime.Now:yyyyMMddHHmmss}.xlsx");

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("ShiftAssignment");
                var headers = new[] { "Worker ID", "Name", "Assigned Crane", "Shift", "Leave Status", "Color Code", "Work Hours", "Rest Hours" };

                for (int i = 0; i < headers.Length; i++)
                    ws.Cells[1, i + 1].Value = headers[i];

                for (int i = 0; i < workers.Count; i++)
                {
                    var w = workers[i];
                    int row = i + 2;
                    ws.Cells[row, 1].Value = w.WorkerId;
                    ws.Cells[row, 2].Value = w.Name;
                    ws.Cells[row, 3].Value = w.AssignedCrane;
                    ws.Cells[row, 4].Value = w.Shift;
                    ws.Cells[row, 5].Value = w.LeaveStatus;
                    ws.Cells[row, 6].Value = w.ColorCode;
                    ws.Cells[row, 7].Value = w.WorkHours;
                    ws.Cells[row, 8].Value = w.RestHours;

                    string fillHex = colorMap.ContainsKey(w.ColorCode) ? colorMap[w.ColorCode] : "FFFFFF";
                    var fillColor = System.Drawing.ColorTranslator.FromHtml("#" + fillHex);

                    for (int col = 1; col <= 8; col++)
                        ws.Cells[row, col].Style.Fill.BackgroundColor.SetColor(fillColor);
                }

                package.SaveAs(new FileInfo(outputPath));
            }

            return outputPath;
        }
    }
}
