using System.Data;

namespace TheDreamApi.DAL
{
    public class EventsDAL
    {

        public static DataTable GetCharts()
        {
            string query = "SELECT EventType, CONVERT(DATE, Time) AS EventDate, COUNT(*) AS EventCount\r\nFROM Events\r\nWHERE Time >= CONVERT(DATE, GETDATE()) AND Time <= DATEADD(MONTH, 1, GETDATE())\r\nGROUP BY EventType, CONVERT(DATE, Time)";
            DataTable dt = SQLHelper.SelectData(query);
            return dt;
        }
    }
}
