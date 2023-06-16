using System.Data;
using TheDreamApi.DAL;
using TheDreamApi.Models;

namespace TheDreamApi.BLL
{
    public class EventsServiceBLL
    {

        public static (string,List<ChartData>) GetCharts()
        {

            try
            {
               return ("successfull",EventsServiceDAL.GetCharts());
            }
            catch(Exception ex)
            {
                if(ex.Message == "internal error")
                {
                    return (ex.Message, null);
                }
                else
                {
                    return ("unknown", null);
                }
            }
        }
    }
}
