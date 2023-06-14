using System.Data;
using TheDreamApi.DAL;

namespace TheDreamApi.BLL
{
    public class EventsServiceBLL
    {

        public static DataTable GetCharts()
        {
           return EventsServiceDAL.GetCharts();
        }
    }
}
