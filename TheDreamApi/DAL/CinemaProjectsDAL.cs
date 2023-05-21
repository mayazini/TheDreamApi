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
        public static DataTable GetCinemaProjectsById(JsonElement json)
        {
            dynamic obj = JsonNode.Parse(json.GetRawText());
            string id = (string)obj["Id"];
            string query = "select * from CinemaProjects where CreatorId = '"+id+"'";
            DataTable result = SQLHelper.SelectData(query);
            return result;
        }

        public static string CreateNewProject(JsonElement json)
        {
            dynamic obj = JsonNode.Parse(json.GetRawText());
            string projectName = (string)obj["projectName"];
            string description = (string)obj["description"];
            string query = $"INSERT INTO CinemaProjects (projectName,description, creatorid) VALUES ('{projectName}','{description}','1')";
            int result = SQLHelper.DoQuery(query);
            if(result == 0) { return "didnt work"; }
            return "";
        }
    }


}
