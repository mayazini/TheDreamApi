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


        [HttpPost("UploadResume/{userName}")]
        public async Task<IActionResult> Upload(IFormFile file,string userName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Users", userName, "Resume");
            var filePath = Path.Combine(folderPath, file.FileName);

            // Create the user's folder if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { FilePath = filePath });
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

        //GetApplicantsByProject with the application data class
        //[HttpGet("GetApplicantsByProject/{projectId}")]
        //public async Task<IActionResult> GetApplicantsByProject(int projectId)
        //{
        //    try
        //    {
        //        DataTable dt = ApplicationsBLL.GetApplicantsByProject(projectId);
        //        if (dt == null)
        //        {
        //            return NotFound(new { error = "No applications found for the project." });
        //        }

        //        // Convert DataTable to a list of ApplicationData objects
        //        List<ApplicationData> applicants = dt.AsEnumerable()
        //            .Select(row => new ApplicationData
        //            {
        //                ProjectId = Convert.ToInt32(row["ProjectId"]),
        //                UserName = Convert.ToString(row["UserName"]),
        //                Email = Convert.ToString(row["Email"]),
        //                Message = Convert.ToString(row["Message"]),
        //                ResumePath = Convert.ToString(row["ResumePath"]),
        //                Requirement = new Requirement
        //                {
        //                    Description = Convert.ToString(row["Description"]),
        //                    Amount = Convert.ToInt32(row["Amount"]),
        //                    Id = Convert.ToInt32(row["RequirementId"])
        //                },
        //                Status = Convert.ToString(row["Status"])
        //            })
        //            .ToList();

        //        // Return a success response with the list of applicants
        //        return Ok(applicants);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any errors that occur during the operation
        //        return StatusCode(500, new { error = "An error occurred while retrieving applicants." });
        //    }
        //}

        [HttpGet("DownloadResume/{userName}/{resumeFileName}")]
        public IActionResult Download(string userName,string resumeFileName)
        {
            // Get the file path
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Users", userName,"Resume", resumeFileName);

            // Check if file exists
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            // Get the file's mime type
            var mimeType = GetMimeType(path);

            // Return the file
            return File(System.IO.File.ReadAllBytes(path), mimeType, resumeFileName);
        }

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
