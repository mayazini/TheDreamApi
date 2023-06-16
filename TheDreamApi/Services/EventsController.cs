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
                (string message , List<ChartData> chartData) = EventsServiceBLL.GetCharts();
                if (message == "successfull") { return Ok(chartData); }                               
                else if(message == "internal error")
                {
                    return NotFound(new { error = "No chart data found" });
                }
                else
                {
                    return StatusCode(500, new { error = "An error occurred." });
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
