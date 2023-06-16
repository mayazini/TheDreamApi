using System.Data;
using TheDreamApi.Models;

namespace TheDreamApi.DAL
{
    public class EventsServiceDAL
    {

        public static List<ChartData> GetCharts()
        {
            try
            {
                string query = "SELECT EventType, CONVERT(DATE, Time) AS EventDate, COUNT(*) AS EventCount\r\nFROM Events\r\nWHERE Time >= DATEADD(MONTH, -1, GETDATE()) AND Time < GETDATE()\r\nGROUP BY EventType, CONVERT(DATE, Time)\r\nORDER BY EventDate ASC";
                DataTable dt = SQLHelper.SelectData(query);
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
                    return (chartData);
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception("internal error");
            }
        }
    }
}
