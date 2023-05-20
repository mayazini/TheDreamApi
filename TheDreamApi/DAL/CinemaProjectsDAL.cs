using System.Data;

namespace TheDreamApi.DAL
{
    public class CinemaProjectsDAL
    {
        public static DataTable GetCinemaProjects()
        {
            string query = "select * from CinemaProjects";
            DataTable result = SQLHelper.SelectData(query);
            return result;
        }
    }
}
