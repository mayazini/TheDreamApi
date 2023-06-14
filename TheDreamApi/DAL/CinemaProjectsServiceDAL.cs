using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;
using TheDreamApi.Models;

namespace TheDreamApi.DAL
{
    public class CinemaProjectsServiceDAL
    {
        public static DataTable GetCinemaProjects()
        {
            string query = "SELECT p.*, r.Description AS RequirementDescription, r.Amount, r.ProjectId FROM Projects p JOIN Requirements r ON p.Id = r.ProjectId WHERE p.SpaceId = (SELECT Id FROM Spaces WHERE Space = 'cinema')";
            DataTable result = SQLHelper.SelectData(query);
            return result;
        }
        public static DataTable GetCinemaProjectsByName(JsonElement json)
        {
            dynamic obj = JsonNode.Parse(json.GetRawText());
            string creatorName = (string)obj["CreatorName"];
            string query = $"SELECT p.*, r.Description AS RequirementDescription, r.Amount, r.ProjectId FROM Projects p JOIN Requirements r ON p.Id = r.ProjectId WHERE p.SpaceId = (SELECT Id FROM Spaces WHERE Space = 'cinema') AND p.CreatorName = '{creatorName}'";
            DataTable result = SQLHelper.SelectData(query);
            return result;
        }

        public static string CreateCinemaNewProject(JsonElement json)
        {
            dynamic obj = JsonNode.Parse(json.GetRawText());
            string projectName = (string)obj["projectName"];
            string description = (string)obj["description"];
            string creatorName = (string)obj["creatorName"];
            var requirements = obj["requirements"];

            // Insert the project information into the CinemaProjects table
            string projectQuery = $"INSERT INTO Projects (projectName, description, CreatorName, SpaceId) VALUES ('{projectName}', '{description}', '{creatorName}', (SELECT Id FROM Spaces WHERE Space = 'cinema')); SELECT SCOPE_IDENTITY();";
            int projectId = SQLHelper.SelectScalarToInt32(projectQuery);

            if (projectId == 0)
            {
                return "Failed to insert project into the database";
            }

            // Iterate over the requirements and insert them into the database
            foreach (var requirement in requirements)
            {
                string requirementName = (string)requirement["name"];
                int requirementAmount = Int32.Parse((string)requirement["amount"]);

                // Insert the requirement into the database with the associated project ID
                bool worked = RequirementsDAL.CreateNewRequirment(projectId, requirementName, requirementAmount);

                if (worked)
                {
                    return "Failed to insert requirement into the database";
                }
                else
                {
                    // Insert the event into the Events table
                    string eventQuery = $"INSERT INTO Events (EventType, Time, projectId) VALUES ('{EventTypes.createNewProject}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{projectId}')";
                    int result = SQLHelper.DoQuery(eventQuery);

                    return result > 0 ? "" : "error in the query";
                }
            }

            return "";
        }


    }


}
