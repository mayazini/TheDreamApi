using System.Data;
using System.Text.Json;
using TheDreamApi.DAL;

namespace TheDreamApi.BLL
{
    public class CinemaProjectsServiceBLL
    { 
        public static DataTable GetCinemaProjects()
        {
            DataTable data = CinemaProjectsServiceDAL.GetCinemaProjects();

            if (data == null || data.Rows.Count == 0)// check if login incorrect
            {
                return null;
            }
            return data;
        }
        public static DataTable GetCinemaProjectsByName(JsonElement json)
        {
            DataTable data = CinemaProjectsServiceDAL.GetCinemaProjectsByName(json);

            if (data == null || data.Rows.Count == 0)// check if login incorrect
            {
                return null;
            }
            return data;
        }
        public static string CreateNewCinemaProject(JsonElement json)
        {
            string response = CinemaProjectsServiceDAL.CreateCinemaNewProject(json);
            return response;
        }

    }
}
