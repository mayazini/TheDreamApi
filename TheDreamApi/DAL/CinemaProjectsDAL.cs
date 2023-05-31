using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;
using TheDreamApi.Models;

namespace TheDreamApi.DAL
{
    public class CinemaProjectsDAL
    {
        public static DataTable GetCinemaProjects()
        {
            string query = "SELECT p.*, r.Description AS RequirementDescription, r.Amount,r.ProjectId FROM CinemaProjects p JOIN Requirements r ON p.Id = r.projectId";
            DataTable result = SQLHelper.SelectData(query);
            return result;
        }
        public static DataTable GetCinemaProjectsByName(JsonElement json)
        {
            dynamic obj = JsonNode.Parse(json.GetRawText());
            string creatorName = (string)obj["CreatorName"];
            string query = "select * from CinemaProjects where CreatorName = '" + creatorName + "'";
            DataTable result = SQLHelper.SelectData(query);
            return result;
        }

        public static string CreateNewProject(JsonElement json)
        {
            dynamic obj = JsonNode.Parse(json.GetRawText());
            string projectName = (string)obj["projectName"];
            string description = (string)obj["description"];
            string creatorName = (string)obj["creatorName"];
            var requirements = obj["requirements"];

            // Insert the project information into the CinemaProjects table
            string projectQuery = $"INSERT INTO CinemaProjects (projectName, description, CreatorName) VALUES ('{projectName}', '{description}', '{creatorName}'); SELECT SCOPE_IDENTITY();";
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
                string requirementQuery = $"INSERT INTO Requirements (projectId, Description, Amount) VALUES ({projectId}, '{requirementName}', {requirementAmount})";
                int result = SQLHelper.DoQuery(requirementQuery);

                if (result == 0)
                {
                    return "Failed to insert requirement into the database";
                }
                else
                {
                    // Insert the event into the Events table
                    string eventQuery = $"INSERT INTO Events (EventType, Time,projectId) VALUES ('{EventTypes.createNewProject}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{projectId}')";
                    result = SQLHelper.DoQuery(eventQuery);

                    return result > 0 ? "" : "error in the query";
                }
            }

            return "";
        }

    }


}
