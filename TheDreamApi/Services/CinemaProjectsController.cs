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
    public class CinemaProjectsController : ControllerBase
    {


        [HttpGet("GetCinemaProjects")]
        public IActionResult GetCinemaProjects()
        {
            try
            {
                // Get the user data
                var dt = CinemaProjectsServiceBLL.GetCinemaProjects();
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

        [HttpPost("GetCinemaProjectsByName")]
        public IActionResult GetCinemaProjectsByName(JsonElement value)
        {
            try
            {
                // Get the user data
                var dt = CinemaProjectsServiceBLL.GetCinemaProjectsByName(value);
                if (dt == null)
                {
                    return NotFound(new { error = "no projects" });
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
                // Log the error and return a 500 Internal Server Error
                return StatusCode(500, new { error = "An error occurred." });
            }
        }

        [HttpGet("GetRetailProjects")]
        public IActionResult GetRetailProjects()
        {
            try
            {
                // Get the user data
                var dt = CinemaProjectsServiceBLL.GetCinemaProjects();
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

        [HttpPost("GetRetailProjectsByName")]
        public IActionResult GetRetailProjectsByName(JsonElement value)
        {
            try
            {
                // Get the user data
                var dt = CinemaProjectsServiceBLL.GetCinemaProjectsByName(value);
                if (dt == null)
                {
                    return NotFound(new { error = "no projects" });
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
                // Log the error and return a 500 Internal Server Error
                return StatusCode(500, new { error = "An error occurred." });
            }
        }


        [HttpPut("CreateNewProject")]
        public IActionResult CreateNewProject([FromBody] JsonElement value)
        {
            try
            {
                string response = CinemaProjectsServiceBLL.CreateNewCinemaProject(value);
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
