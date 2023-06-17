using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheDreamApi.Models;
using TheDreamApi.BLL;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using System.Data;
using Microsoft.AspNetCore.StaticFiles;

namespace TheDreamApi.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        [HttpPut("Apply")]
        public IActionResult Apply([FromBody] JsonElement applicationData)
        {
            try
            {
                dynamic obj = JsonNode.Parse(applicationData.GetRawText());
                ApplicationData application = new ApplicationData();
                try
                {
                    // Convert the requirement object
                    dynamic requirementObj = obj["selectedRequirement"];
                    Requirement requirement = new Requirement()
                    {
                        Description = (string)requirementObj["description"],
                        Amount = (int)requirementObj["amount"],
                        Id = (int)requirementObj["id"]
                    };
                    application.Requirement = requirement;

                    // Assign other properties
                    application.ResumePath = (string)obj["ResumePath"];
                    application.Email = (string)obj["email"];
                    application.ProjectId = (int)(obj["projectId"]);
                    application.Message = (string)obj["message"];
                    application.UserName = (string)obj["ApplicantName"];
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during the operation
                    return StatusCode(500, new { error = "invalid input." });
                }
                string response = ApplicationsBLL.Apply(application);
                // Return a success response
                return Ok(new { message = "Application submitted successfully." });
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the operation
                return StatusCode(500, new { error = "An error occurred while processing the application." });
            }
        }


        //[HttpPost("UploadResume/{userName}")]
        //public async Task<IActionResult> Upload(IFormFile file,string userName)
        //{
        //    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Users", userName, "Resume");
        //    var filePath = Path.Combine(folderPath, file.FileName);

        //    // Create the user's folder if it doesn't exist
        //    if (!Directory.Exists(folderPath))
        //    {
        //        Directory.CreateDirectory(folderPath);
        //        if (!System.IO.File.Exists(folderPath))
        //        {
        //            return NotFound();
        //        }
        //    } 
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    return Ok(new { FilePath = filePath });
        //}

        [HttpPost("UploadResume/{userName}")]
        public async Task<IActionResult> UploadResume(IFormFile file, string userName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Users", userName, "Resume");
            var resumePath = Path.Combine(folderPath, "resume.pdf");

            // Create the user's folder if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (System.IO.File.Exists(resumePath))
            {
                // Delete the existing resume file
                System.IO.File.Delete(resumePath);
            }

            using (var stream = new FileStream(resumePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { FilePath = resumePath });
        }




        [HttpGet("GetApplicantsByProject/{projectId}")]
        public async Task<IActionResult> GetApplicantsByProject(int projectId)
        {
            try
            {
                DataTable dt = ApplicationsBLL.GetApplicantsByProject(projectId);
                if (dt == null)
                {
                    return NotFound(new { error = "No applications found for the project." });
                }

                // Convert DataTable to a list of dictionaries
                var rows = dt.AsEnumerable()
                    .Select(row => dt.Columns.Cast<DataColumn>()
                        .ToDictionary(column => column.ColumnName, column => row[column]));

                // Return the serialized data
                return Ok(rows);
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the operation
                return StatusCode(500, new { error = "An error occurred while retrieving applicants." });
            }
        }

        [HttpPut("UpdateApplicationStatus/{applicationId}")]
        public async Task<IActionResult> UpdateApplicationStatus(int applicationId, [FromBody] StatusUpdateRequest request)
        {
            try
            {
                bool response = ApplicationsBLL.UpdateApplicationStatus(applicationId, request.Status);
                if (!response)
                {
                    return NotFound(new { error = "Status update didnt work" });
                }

                return Ok("Updated application status");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the operation
                return StatusCode(500, new { error = "An error occurred while retrieving applicants." });
            }
        }

        [HttpGet("DownloadResume/{userName}/{resumeFileName}")]
        public IActionResult Download(string userName,string resumeFileName)
        {
            // Get the file path
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Users", userName,"Resume", "resume.pdf");

            // Check if file exists
            if (!System.IO.File.Exists(path))
            {
                return NotFound("file doesnt exist");
            }

            // Get the file's mime type
            var mimeType = GetMimeType(path);

            // Return the file
            return File(System.IO.File.ReadAllBytes(path), mimeType, resumeFileName);
        }

        //[HttpGet("DownloadResume/{userName}/{resumeFileName}")]
        //public IActionResult Download(string userName, string resumeFileName)
        //{
        //    // Get the file path
        //    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Users", userName, "Resume", resumeFileName);

        //    // Check if file exists
        //    if (!System.IO.File.Exists(path))
        //    {
        //        return NotFound("file doesnt exist");
        //    }

        //    // Get the file's mime type
        //    var mimeType = GetMimeType(path);

        //    // Return the file
        //    return File(System.IO.File.ReadAllBytes(path), mimeType, resumeFileName);
        //}

        // Helper method to get the mime type of a file. This is used to ensure the browser knows what type of file it's downloading.
        private string GetMimeType(string filePath)
        {
            var provider = new FileExtensionContentTypeProvider();
            string mimeType;
            if (!provider.TryGetContentType(filePath, out mimeType))
            {
                mimeType = "application/octet-stream";
            }
            return mimeType;
        }

    }
}
