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
                user.UserName = (string)obj["username"];
                user.Email = (string)obj["email"];
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
                List<User> users = UsersServiceBLL.GetAllUsers();
                if (users == null)
                {
                    return NotFound(new { error = "User not found." });
                }
                // Return the serialized data
                return Ok(users);
            }
            catch (Exception ex) { return StatusCode(500, new { error = "An error occurred." }); }

        }

        [HttpDelete("DeleteUser/{userName}")]
        public IActionResult DeleteUser(string userName)
        {
            try
            {
                bool deleted = UsersServiceBLL.DeleteUser(userName);

                if (deleted)
                {
                    return Ok("User deleted successfully.");
                }

                return BadRequest("Failed to delete user or user file was deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the user: {ex.Message}");
            }
        }

    }
    public class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
