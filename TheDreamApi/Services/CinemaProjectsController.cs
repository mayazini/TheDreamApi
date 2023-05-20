using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json.Nodes;
using System.Text.Json;
using TheDreamApi.BLL;
using Newtonsoft.Json.Linq;

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
                var dt = CinemaProjectsBLL.GetCinemaProjects();
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
                string response = CinemaProjectsBLL.CreateNewProject(value);
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
