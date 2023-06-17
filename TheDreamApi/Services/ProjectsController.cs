using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json.Nodes;
using System.Text.Json;
using TheDreamApi.BLL;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using TheDreamApi.Models;

namespace TheDreamApi.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {

        [HttpGet("GetProjects/{spaceName}/{creatorName}")]
        public IActionResult GetProjectsBySpaceAndName(string spaceName, string creatorName)
        {
            try
            {
                // Get the user data
                List<Project> projects = ProjectsServiceBLL.GetProjectsBySpaceAndName(spaceName, creatorName);
                if (projects == null)
                {
                    return NotFound(new { error = "no projects" });
                }
                return Ok(projects);
            }
            catch (Exception ex)
            {
                // Log the error and return a 500 Internal Server Error
                return StatusCode(500, new { error = "An error occurred." });
            }
        }

        [HttpGet("GetProjects/{spaceName}")]
        public IActionResult GetProjectsBySpace(string spaceName)
        {
            try
            {
                // Get the user data
                List<Project> projects = ProjectsServiceBLL.GetProjectsBySpace(spaceName);
                if (projects == null)
                {
                    return NotFound(new { error = "no projects" });
                }
                // Return the serialized data
                return Ok(projects);
            }
            catch (Exception ex)
            {
                // Log the error and return a 500 Internal Server Error
                return StatusCode(500, new { error = "An error occurred." });
            }
        }



        [HttpPut("CreateNewProject")]
        public IActionResult CreateNewProject([FromBody] JsonElement value)
        {
            try
            {
                Project project = Project.BuildProjectFromClient(value);
                string response = ProjectsServiceBLL.CreateNewProject(project);
                if (response == "")
                {
                    return Ok();
                }
                if (response == "didnt work")
                {
                    return NotFound(new { error = "didn't work" });
                }
                else
                {

                    return StatusCode(500, new { error = response });
                }
            }
            catch (Exception ex) { return StatusCode(500, new { error = "An error occurred." }); }
        }
    }
}
