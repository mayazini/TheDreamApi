using System.Data;

namespace TheDreamApi.DAL
{
    public class EventsServiceDAL
    {

        public static DataTable GetCharts()
        {
            string query = "SELECT EventType, CONVERT(DATE, Time) AS EventDate, COUNT(*) AS EventCount\r\nFROM Events\r\nWHERE Time >= DATEADD(MONTH, -1, GETDATE()) AND Time < GETDATE()\r\nGROUP BY EventType, CONVERT(DATE, Time)\r\nORDER BY EventDate ASC";
            DataTable dt = SQLHelper.SelectData(query);
            return dt;
        }
    }
}
