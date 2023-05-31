using System.Data;
using TheDreamApi.DAL;

namespace TheDreamApi.BLL
{
    public class EventsBLL
    {

        public static DataTable GetCharts()
        {
           return EventsDAL.GetCharts();
        }
    }
}
