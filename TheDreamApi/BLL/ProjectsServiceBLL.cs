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

            if (projects == null || projects.Count == 0)
            {
                return null;
            }
            return projects;
        }
        public static List<Project> GetProjectsBySpaceAndName(string spaceName, string creatorName)
        {
            List<Project> projetcs = ProjectsServiceDAL.GetCinemaProjectsByName(spaceName, creatorName);

            if (projetcs == null || projetcs.Count == 0)
            {
                return null;
            }
            return projetcs;
        }
        public static string CreateNewProject(Project project)
        {
            string response = ProjectsServiceDAL.CreateNewProject(project);
            return response;
        }

    }
}
