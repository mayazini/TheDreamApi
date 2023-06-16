using Microsoft.AspNetCore.Mvc;
using TheDreamApi.BLL;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Data;
using Microsoft.AspNetCore.Cors;
using System.Text.Json.Nodes;
using System.Text.Json;
using TheDreamApi.Models;

namespace TheDreamApi.web_service
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //[EnableCors("AllowSpecificOrigin")]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserCredentials userCredentials)
        {
            try
            {
                string username = userCredentials.Username;
                string password = userCredentials.Password;

                // Get the user data and error message
                (User user, string errorMessage) = UsersServiceBLL.Login(username, password);

                // If user is not null, return the user data
                if (user != null)
                {
                    return Ok(user);
                }

                // If user is null, return the error message
                return BadRequest(new { error = errorMessage });
            }
            catch (Exception ex)
            {
                // Log the error and return a 500 Internal Server Error
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("Register")]
        public IActionResult Register([FromBody] JsonElement value)
        {
            try
            {
                dynamic obj = JsonNode.Parse(value.GetRawText());
                User user = new User();
                user.Email = (string)obj["username"];
                user.UserName = (string)obj["email"];
                user.Password = (string)obj["password"];
                //user.Age = (int)obj["age"];
                string response = UsersServiceBLL.Register(user);
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

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var dt = UsersServiceBLL.GetAllUsers();
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
            catch (Exception ex) { return StatusCode(500, new { error = "An error occurred." }); }

        }
    }
    public class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
