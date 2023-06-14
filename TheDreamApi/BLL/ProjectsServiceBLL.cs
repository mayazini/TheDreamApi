using System.Data;
using System.Text.Json;
using TheDreamApi.DAL;

namespace TheDreamApi.BLL
{
    public class ProjectsServiceBLL
    { 
        public static DataTable GetProjectsBySpace(string spaceName)
        {
            DataTable data = ProjectsServiceDAL.GetProjectsBySpace(spaceName);

            if (data == null || data.Rows.Count == 0)// check if login incorrect
            {
                return null;
            }
            return data;
        }
        public static DataTable GetProjectsBySpaceAndName(string spaceName, string creatorName)
        {
            DataTable data = ProjectsServiceDAL.GetCinemaProjectsByName(spaceName, creatorName);

            if (data == null || data.Rows.Count == 0)// check if login incorrect
            {
                return null;
            }
            return data;
        }
        public static string CreateNewProject(JsonElement json)
        {
            string response = ProjectsServiceDAL.CreateNewProject(json);
            return response;
        }

    }
}
