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
                var dt = ProjectsServiceBLL.GetProjectsBySpaceAndName(spaceName, creatorName);
                if (dt == null)
                {
                    return NotFound(new { error = "no projects" });
                }

                var projects = dt.AsEnumerable().Select(row => new Project
                {
                    ProjectId = row.Field<int>("Id"),
                    ProjectName = row.Field<string>("ProjectName"),
                    Description = row.Field<string>("Description"),
                    CreatorName = row.Field<string>("CreatorName"),
                    Requirements = dt.AsEnumerable()
                .Where(r => r.Field<int>("ProjectId") == row.Field<int>("Id"))
                .Select(r => new Requirement
                {
                    Description = r.Field<string>("RequirementDescription"),
                    Amount = r.Field<int>("Amount")
                })
                .ToList()
                });
                // Return the serialized data
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
                var dt = ProjectsServiceBLL.GetProjectsBySpace(spaceName);
                if (dt == null)
                {
                    return NotFound(new { error = "no projects" });
                }

                var projects = dt.AsEnumerable().Select(row => new Project
                {
                    ProjectId = row.Field<int>("Id"),
                    ProjectName = row.Field<string>("ProjectName"),
                    Description = row.Field<string>("Description"),
                    CreatorName = row.Field<string>("CreatorName"),
                    Requirements = dt.AsEnumerable()
                .Where(r => r.Field<int>("ProjectId") == row.Field<int>("Id"))
                .Select(r => new Requirement
                {
                    Description = r.Field<string>("RequirementDescription"),
                    Amount = r.Field<int>("Amount"),
                    ProjectId = row.Field<int>("ProjectId"),
                    Id = r.Field<int>("RequirementId")
                })
                .ToList()
                });
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
                string response = ProjectsServiceBLL.CreateNewProject(value);
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
