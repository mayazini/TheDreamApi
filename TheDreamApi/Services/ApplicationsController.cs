using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheDreamApi.Models;
using TheDreamApi.BLL;
using System.Diagnostics;
using System.Reflection.Metadata;
using TheDreamApi.BLL;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

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
                        ProjectId = (int)requirementObj["projectId"]
                    };
                    application.Requirement = requirement;

                    // Assign other properties
                    application.ResumePath = (string)obj["ResumePath"];
                    application.Email = (string)obj["email"];
                    application.ProjectId = (int)(obj["projectId"]);
                    application.Message = (string)obj["message"];
                    application.UserName = (string)obj["userName"];
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
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Users", userName, "Resumes");
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

    }
}
