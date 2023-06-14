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
                string query = $"INSERT INTO Applications (RequirementId, Status, ProjectId,Username,Message,Email,ResumePath) VALUES ('{applicationData.Requirement.Id}', 'sent', '{applicationData.ProjectId}','{applicationData.UserName}','{applicationData.Message}','{applicationData.Email}',N'{applicationData.ResumePath}')";
                int affected = SQLHelper.DoQuery(query);

                if (affected > 0)
                {
                    return "application sent successfuly";
                }
                else
                {
                    return ("error with input query");
                }
            }
            catch (Exception ex)
            {
                return ("unkown error");
            }

        }
    }

}
