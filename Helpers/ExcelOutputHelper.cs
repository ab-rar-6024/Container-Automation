using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;

namespace ContainerAutomationApp.Helpers
{
    public class WorkerShiftOutput
    {
        public int WorkerID { get; set; }
        public string Name { get; set; }
        public string AssignedCrane { get; set; }
        public string Shift { get; set; }
        public string LeaveStatus { get; set; }
        public string ColorCode { get; set; } // Green, Red, Blue, Yellow
        public int WorkHours { get; set; }
        public int RestHours { get; set; }
    }

    public static class ExcelOutputHelper
    {
        public static string GenerateExcel(List<WorkerShiftOutput> workers, string outputPath)
        {
            var colorMap = new Dictionary<string, string>
            {
                { "Green", "#90EE90" },    // Light Green
                { "Red", "#FF9999" },      // Light Red
                { "Blue", "#ADD8E6" },     // Light Blue
                { "Yellow", "#FFFF99" }    // Light Yellow
            };

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("ShiftAssignment");

            // Headers
            var headers = new[] {
                "Worker ID", "Name", "Assigned Crane", "Shift", "Leave Status", "Color Code", "Work Hours", "Rest Hours"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            }

            int row = 2;
            foreach (var worker in workers)
            {
                worksheet.Cell(row, 1).Value = worker.WorkerID;
                worksheet.Cell(row, 2).Value = worker.Name;
                worksheet.Cell(row, 3).Value = worker.AssignedCrane;
                worksheet.Cell(row, 4).Value = worker.Shift;
                worksheet.Cell(row, 5).Value = worker.LeaveStatus;
                worksheet.Cell(row, 6).Value = worker.ColorCode;
                worksheet.Cell(row, 7).Value = worker.WorkHours;
                worksheet.Cell(row, 8).Value = worker.RestHours;

                // Apply fill color
                if (colorMap.TryGetValue(worker.ColorCode, out string hex))
                {
                    var fillColor = XLColor.FromHtml(hex);
                    for (int col = 1; col <= 8; col++)
                    {
                        worksheet.Cell(row, col).Style.Fill.BackgroundColor = fillColor;
                    }
                }

                row++;
            }

            workbook.SaveAs(outputPath);
            return outputPath;
        }
    }
}
