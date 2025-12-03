using Microsoft.AspNetCore.Mvc;
using ContainerAutomationApp.Helpers;
using ContainerAutomationApp.Models;

namespace ContainerAutomationApp.Controllers
{
    public class WorkerUploadController : Controller
    {
        [HttpGet]
        public IActionResult Upload()
        {
            return View(); // Shows the file upload form
        }

        [HttpPost]
        public IActionResult Upload(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                // Step 1: Parse Excel file into C# models
                var workers = ExcelHelper.ReadWorkerExcel(excelFile); // List<WorkerInput>

                // Step 2: Run scheduling logic
                var scheduledResults = SchedulerHelper.ProcessWorkerSchedule(workers); // List<WorkerScheduleResult>

                // Step 3: Generate output Excel with color codes
                string outputPath = ExcelHelper.GenerateOutputExcel(scheduledResults);


                // Step 4: Return the Excel file as download
                byte[] fileBytes = System.IO.File.ReadAllBytes(outputPath);
                return File(fileBytes, 
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                    "Final_Schedule.xlsx");
            }
            catch (Exception ex)
            {
                // Optional: Log error here
                return StatusCode(500, "Error processing schedule: " + ex.Message);
            }
        }
    }
}
