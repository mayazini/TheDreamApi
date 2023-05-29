using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json.Nodes;
using System.Text.Json;
using TheDreamApi.BLL;

namespace TheDreamApi.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class InboxController : ControllerBase
    {
        [HttpPut("SendMessage")]
        public IActionResult Register([FromBody] JsonElement value)
        {
            try
            {
                string response = InboxBLL.SendMessage(value);
                if (response == "ok")
                {
                    return Ok();
                }
                if (response == "user doesn't exist")
                {
                    return NotFound(new { error = response });
                }
                else
                {

                    return StatusCode(500, new { error = response });
                }
            }
            catch (Exception ex) { return StatusCode(500, new { error = "An error occurred." }); }

        }

        [HttpPost("Inbox")]
        public IActionResult GetUserMessages(JsonElement value)
        {
            try
            {
                dynamic obj = JsonNode.Parse(value.GetRawText());
                string username = (string)obj["username"];
                var dt = InboxBLL.GetUserMessages(username);
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
    }
}
