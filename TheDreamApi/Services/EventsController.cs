using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TheDreamApi.BLL;
using TheDreamApi.DAL;
using TheDreamApi.Models;

namespace TheDreamApi.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        [HttpGet("GetEventChart")]
        public IActionResult GetEventChart()
        {
            try
            {
                DataTable dt = EventsServiceBLL.GetCharts();
                if (dt != null && dt.Rows.Count > 0)
                {
                    // Prepare the chart data
                    var chartData = new List<ChartData>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var eventType = row["EventType"].ToString();
                        var eventDate = DateTime.Parse(row["EventDate"].ToString());
                        var eventCount = int.Parse(row["EventCount"].ToString());

                        // Create a new chart data object
                        var data = new ChartData
                        {
                            EventType = eventType,
                            EventDate = eventDate,
                            EventCount = eventCount
                        };

                        // Add the chart data to the list
                        chartData.Add(data);
                    }

                    // Return the chart data
                    return Ok(chartData);
                }
                else
                {
                    return NotFound(new { error = "No chart data found" });
                }
            }
            catch (Exception ex)
            {
                // Log the error and return a 500 Internal Server Error
                return StatusCode(500, new { error = "An error occurred." });
            }
        }

    }
}
