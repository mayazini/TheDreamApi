using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;

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

            // Iterate over the requirements and insert them into the database
            foreach (var requirement in requirements)
            {
                string requirementName = (string)requirement["name"];
                int requirementAmount = (int)requirement["amount"];

                // Insert the requirement into the database
                string query = $"INSERT INTO Requirements (projectName, requirementName, requirementAmount) VALUES ('{projectName}', '{requirementName}', {requirementAmount})";
                int result = SQLHelper.DoQuery(query);

                if (result == 0)
                {
                    return "Failed to insert requirement into the database";
                }
            }

            string projectQuery = $"INSERT INTO CinemaProjects (projectName,description, CreatorName) VALUES ('{projectName}','{description}','{creatorName}')";
            int projectResult = SQLHelper.DoQuery(projectQuery);
            if(projectResult == 0) { return "didnt work"; }
            return "";
        }
    }


}
