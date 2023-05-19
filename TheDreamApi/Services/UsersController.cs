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

        //public static string Register(JsonElement json)
        //{
        //    try
        //    {
        //        dynamic obj = JsonNode.Parse(json.GetRawText());
        //        string username = (string)obj["username"];
        //        string password = (string)obj["password"];
        //        string email = (string)obj["email"];
        //        int affected = 0;
        //        string checkQuery = $"select username from Users where username='{username}'";
        //        DataTable check = SQLHelper.SelectData(checkQuery);
        //        if (check.Rows.Count == 0)
        //        {

        //            string query = $"INSERT INTO Users (username, password, email) VALUES ('{username}',' {password}', '{email}')";
        //            affected = SQLHelper.DoQuery(query);

        //        }
        //        else
        //        {
        //            return ("username already taken");
        //        }
        //        if (affected > 0)
        //        {
        //            return ("ok");
        //        }
        //        else
        //        {
        //            return ("error in the query");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ("unkown error");
        //    }
        //}
    }
}
