using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json.Nodes;
using System.Text.Json;
using TheDreamApi.BLL;
using TheDreamApi.Models;

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

        [HttpGet("GetMessage/{messageId}")]
        public IActionResult GetMessage(int messageId)
        {
            try
            {
                // Retrieve the message from the database based on the messageId
                // Replace this with your actual database query or logic
                var message = InboxBLL.GetMessageById(messageId);

                if (message == null)
                {
                    return NotFound(new { error = "Message not found" });
                }

                // Convert DataTable to a list of dictionaries
                var rows = message.AsEnumerable()
                    .Select(row => message.Columns.Cast<DataColumn>()
                        .ToDictionary(column => column.ColumnName, column => row[column]));

                // Return the serialized data
                return Ok(rows);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred." });
            }
        }



        [HttpPost("GetSentMessages")]
        public IActionResult GetSentMessagesByName(JsonElement value)
        {
            try
            {
                dynamic obj = JsonNode.Parse(value.GetRawText());
                string username = (string)obj["username"];
                var dt = InboxBLL.GetSentMessagesByName(username);
                if (dt == null)
                {
                    return NotFound(new { error = "no messages" });
                }

                // Convert DataTable to a list of Inbox objects
                List<Inbox> messages = dt.AsEnumerable()
                .Select(row => new Inbox
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Message = Convert.ToString(row["Message"]),
                    SenderName = Convert.ToString(row["SenderName"]),
                    RecieverName = Convert.ToString(row["RecieverName"]),
                    Time = Convert.ToDateTime(row["Time"]),
                    Subject = Convert.ToString(row["Subject"]),
                    IsTrash = Convert.ToBoolean(row["IsTrash"])
                })
                .ToList();

                // Return the serialized data
                return Ok(messages);
            }
            catch (Exception ex)
            {
                // Log the error and return a 500 Internal Server Error
                return StatusCode(500, new { error = "An error occurred." });
            }
        }

        [HttpPatch("MoveToTrash/{messageId}")]
        public IActionResult MoveToTrash(int messageId)
        {
            try
            {
                // Delete the message from the inbox data table using the messageId
                bool worked = InboxBLL.MoveToTrash(messageId);

                if (worked)
                {
                    return Ok(new { message = "Message moved to trash successfully" });
                }
                else
                {
                    return NotFound(new { error = "Message not found" });
                }
            }
            catch (Exception ex)
            {
                // Log the error and return a 500 Internal Server Error
                return StatusCode(500, new { error = "An error occurred." });
            }
        }

        [HttpDelete("DeleteMessage/{messageId}")]
        public IActionResult DeleteMessage(int messageId)
        {
            try
            {
                // Delete the message from the inbox data table using the messageId
                bool isDeleted = InboxBLL.DeleteMessage(messageId);

                if (isDeleted)
                {
                    return Ok(new { message = "Message deleted successfully" });
                }
                else
                {
                    return NotFound(new { error = "Message not found" });
                }
            }
            catch (Exception ex)
            {
                // Log the error and return a 500 Internal Server Error
                return StatusCode(500, new { error = "An error occurred." });
            }
        }

        [HttpPost("GetTrashByName")]
        public IActionResult GetTrash(JsonElement value)
        {
            try
            {
                dynamic obj = JsonNode.Parse(value.GetRawText());
                string username = (string)obj["username"];
                var dt = InboxBLL.GetTrashByName(username);
                if (dt == null)
                {
                    return NotFound(new { error = "no messages" });
                }

                // Convert DataTable to a list of Inbox objects
                List<Inbox> messages = dt.AsEnumerable()
                .Select(row => new Inbox
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Message = Convert.ToString(row["Message"]),
                    SenderName = Convert.ToString(row["SenderName"]),
                    RecieverName = Convert.ToString(row["RecieverName"]),
                    Time = Convert.ToDateTime(row["Time"]),
                    Subject = Convert.ToString(row["Subject"]),
                    IsTrash = Convert.ToBoolean(row["IsTrash"])
                })
                .ToList();

                // Return the serialized data
                return Ok(messages);
            }
            catch (Exception ex)
            {
                // Log the error and return a 500 Internal Server Error
                return StatusCode(500, new { error = "An error occurred." });
            }
        }

    }
}
