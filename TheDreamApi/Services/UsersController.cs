using Microsoft.AspNetCore.Mvc;
using TheDreamApi.BLL;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Data;

namespace TheDreamApi.web_service
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("GetUserData")]
        public IActionResult GetUserData(string username, string password)
        {
            try
            {
                // Get the user data
                var dt = UsersBLL.GetUserDataBLL(username, password);
                if (dt == null)
                {
                    return NotFound(new { error = "User not found." });
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

        // Rest of the code...
    }
}
