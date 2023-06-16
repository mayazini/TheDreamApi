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
                string query = "exec spInsertApplication @requirementId='" + applicationData.Requirement.Id + "', @projectId='" + applicationData.ProjectId + "',@username='" + applicationData.UserName + "',@message='" + applicationData.Message + "',@email='" + applicationData.Email + "',@resumePath='" + applicationData.ResumePath+"'";
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

        public static DataTable GetApplicantsByProject(int projectId)
        {
            string query = "exec GetApplicantsByProject " + projectId;
            DataTable dt = SQLHelper.SelectData(query);
            return dt;

        }

        public static bool UpdateApplicationStatus(int applicationId, string status)
        {
            string query ="exec spUpdateApplicationStatus "+applicationId +","+status;
            int response = SQLHelper.DoQuery(query);
            return response==1;
        }
    }

}
