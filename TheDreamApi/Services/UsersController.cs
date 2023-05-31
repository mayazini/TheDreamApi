using Microsoft.AspNetCore.Mvc;
using TheDreamApi.BLL;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Data;
using Microsoft.AspNetCore.Cors;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace TheDreamApi.web_service
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //[EnableCors("AllowSpecificOrigin")]
        [HttpPost("GetUserData")]
        public IActionResult GetUserData(JsonElement json)
        {
            try
            {
                dynamic obj = JsonNode.Parse(json.GetRawText());
                string username = (string)obj["username"];
                string password = (string)obj["password"];

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

        [HttpPut("Register")]
        public IActionResult Register([FromBody] JsonElement value)
        {
            try
            {
                string response = UsersBLL.Register(value);
                if (response == "ok")
                {
                    return Ok();
                }
                if (response == "username already taken")
                {
                    return NotFound(new { error = "Username taken." });
                }
                else
                {

                    return StatusCode(500, new { error = response });
                }
            }
           catch(Exception ex) { return StatusCode(500, new { error = "An error occurred." }); }

        }

        [HttpPut("Register")]
        public IActionResult Register([FromBody] JsonElement value)
        {
            try
            {
                string response = UsersBLL.Register(value);
                if (response == "ok")
                {
                    return Ok();
                }
                if (response == "username already taken")
                {
                    return NotFound(new { error = "Username taken." });
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
