using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using System.Text.Json.Nodes;
using TheDreamApi.Models;

namespace TheDreamApi.DAL
{
    public class ProjectsServiceDAL
    {
        //public static DataTable GetCinemaProjects()
        //{
        //    string query = "SELECT p.*, r.Description AS RequirementDescription, r.Amount, r.ProjectId FROM Projects p JOIN Requirements r ON p.Id = r.ProjectId WHERE p.SpaceId = (SELECT Id FROM Spaces WHERE Space = 'cinema')";
        //    DataTable result = SQLHelper.SelectData(query);
        //    return result;
        //}
        public static Project BuildProject(IEnumerable<DataRow> rows)
        {
            Project project = new Project();

            if (rows != null && rows.Any())
            {
                DataRow firstRow = rows.First();
                project.ProjectId = firstRow.IsNull("Id") ? 0 : Convert.ToInt32(firstRow["Id"]);
                project.ProjectName = firstRow.IsNull("projectName") ? string.Empty : firstRow.Field<string>("projectName");
                project.Description = firstRow.IsNull("description") ? string.Empty : firstRow.Field<string>("description");
                project.CreatorName = firstRow.IsNull("creatorName") ? string.Empty : firstRow.Field<string>("creatorName");

                foreach (DataRow row in rows)
                {
                    if (!row.IsNull("RequirementId"))
                    {
                        Requirement requirement = new Requirement();
                        requirement.Description = row.IsNull("RequirementDescription") ? string.Empty : row.Field<string>("RequirementDescription");
                        requirement.Amount = row.IsNull("Amount") ? 0 : row.Field<int>("Amount");
                        requirement.ProjectId = row.IsNull("ProjectId") ? 0 : row.Field<int>("ProjectId");
                        requirement.Id = row.IsNull("RequirementId") ? 0 : row.Field<int>("RequirementId");

                        project.Requirements.Add(requirement);
                    }
                }
            }

            return project;
        }


        public static List<Project> GetProjectsBySpace(string spaceName)
        {
            try
            {
                string query = "exec spGetProjectsBySpace @SpaceName = "+ spaceName;
                List<Project> projectList = new List<Project>();

                DataTable dt = SQLHelper.SelectData(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    var groupedRows = dt.AsEnumerable().GroupBy(row => row.Field<int>("Id"));

                    foreach (var group in groupedRows)
                    {
                        var project = BuildProject(group);
                        projectList.Add(project);
                    }
                }

                if (projectList.Count > 0)
                {
                    return projectList;
                }
                else
                {
                    throw new Exception("No projects found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unknown error");
            }
        }



        public static DataTable GetCinemaProjectsByName(string spaceName, string creatorName)
        {
            string query = $"SELECT p.*, r.Description AS RequirementDescription, r.Amount, r.ProjectId FROM Projects p JOIN Requirements r ON p.Id = r.ProjectId WHERE p.SpaceId = (SELECT Id FROM Spaces WHERE Space = '{spaceName}') AND p.CreatorName = '{creatorName}'";
            DataTable result = SQLHelper.SelectData(query);
            return result;
        }

        public static string CreateNewProject(Project project)
        {
            // Insert the project information into the CinemaProjects table
            string projectQuery = $"exec spInsertNewProject @spaceName ='" + project.SpaceName + "' ,@projectName= '" + project.ProjectName + "',@description='" + project.Description + "',@creatorName='" + project.CreatorName+"'";
            int projectId = SQLHelper.SelectScalarToInt32(projectQuery);

            if (projectId == 0)
            {
                return "Failed to insert project into the database";
            }

            // Iterate over the requirements and insert them into the database
            foreach (var requirement in project.Requirements)
            {

                // Insert the requirement into the database with the associated project ID
                bool worked = RequirementsDAL.CreateNewRequirment(projectId, requirement.Description, requirement.Amount);

                if (!worked)
                {
                    return "Failed to insert requirement into the database";
                }
            }
            string eventQuery = $"INSERT INTO Events (EventType, Time, projectId) VALUES ('{EventTypes.createNewProject}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{projectId}')";
            int result = SQLHelper.DoQuery(eventQuery);

            return result > 0 ? "" : "error in the query";
        }


    }


}
