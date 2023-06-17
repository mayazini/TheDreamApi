using System.Data;
using TheDreamApi.Models;
using System.Text.Json;
using TheDreamApi.DAL;

namespace TheDreamApi.BLL
{
    public class ProjectsServiceBLL
    { 
        public static List<Project> GetProjectsBySpace(string spaceName)
        {
            List<Project> projects = ProjectsServiceDAL.GetProjectsBySpace(spaceName);

            if (projects == null || projects.Count == 0)// check if login incorrect
            {
                return null;
            }
            return projects;
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
        public static string CreateNewProject(Project project)
        {
            string response = ProjectsServiceDAL.CreateNewProject(project);
            return response;
        }

    }
}
