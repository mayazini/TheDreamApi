using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheDreamApi.Models;
using TheDreamApi.BLL;

namespace TheDreamApi.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        [HttpPost("Apply")]
        public IActionResult Apply([FromForm] ApplicationData applicationData)
        {
            try
            {
                // Perform the necessary operations with the application data
                // e.g., store it in the database, send emails, etc.

        // Save the resume file
                ApplicationsBLL.SaveResumeFile(applicationData.ResumeStream, Convert.ToInt32(applicationData.ProjectId), applicationData.UserName.ToString());

                // Return a success response
                return Ok(new { message = "Application submitted successfully." });
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the operation
                return StatusCode(500, new { error = "An error occurred while processing the application." });
            }
        }


    }
}
