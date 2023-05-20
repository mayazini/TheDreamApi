using System.Data;
using TheDreamApi.DAL;

namespace TheDreamApi.BLL
{
    public class CinemaProjectsBLL
    { 
        public static DataTable GetCinemaProjects()
        {
            DataTable data = CinemaProjectsDAL.GetCinemaProjects();

            if (data == null || data.Rows.Count == 0)// check if login incorrect
            {
                return null;
            }
            return data;
        }

    }
}
