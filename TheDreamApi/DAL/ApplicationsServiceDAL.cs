using System.Data;
using TheDreamApi.Models;

namespace TheDreamApi.DAL
{
    public class ApplicationsServiceDAL
    {
        public static string Apply(ApplicationData applicationData)
        {
            try
            {
                string query = $"INSERT INTO Applications (RequirementId, Status, ProjetId,Username) VALUES ('{applicationData.Requirement.Id}', 'sent', '{applicationData.ProjectId}',)";
                int affected = SQLHelper.DoQuery(query);

                if (affected > 0)
                {
                }
            }
            catch (Exception ex)
            {
                return ("unkown error");
            }

        }
    }

}
