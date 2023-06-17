using Microsoft.AspNetCore.Http;
using System.Data;
using TheDreamApi.Models;

namespace TheDreamApi.DAL
{
    public class ApplicationsServiceDAL
    {
        public static ApplicationData BuildApplication(DataRow row)
        {
            ApplicationData application = new ApplicationData();
            application.Status = row["Status"] != DBNull.Value ? row["Status"].ToString() : null;
            application.ApplicantName = row["ApplicantName"] != DBNull.Value ? row["ApplicantName"].ToString() : null;
            application.ProjectId = row["ProjectId"] != DBNull.Value ? Convert.ToInt32(row["ProjectId"]) : 0;
            application.Requirement.Description = row["RequirementDescription"] != DBNull.Value ? row["RequirementDescription"].ToString() : null;
            application.Message = row["Message"] != DBNull.Value ? row["Message"].ToString() : null;
            application.ResumePath = row["ResumePath"] != DBNull.Value ? row["ResumePath"].ToString() : null;
            return application;
        }


        public static string Apply(ApplicationData applicationData)
        {
            try
            {
                string query = "exec spInsertApplication @requirementId='" + applicationData.Requirement.Id + "', @projectId='" + applicationData.ProjectId + "',@username='" + applicationData.ApplicantName + "',@message='" + applicationData.Message + "',@email='" + applicationData.Email + "',@resumePath='" + applicationData.ResumePath+"'";
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

        public static List<ApplicationData> GetApplicationsByApplicantName(string spaceName, string applicantName)
        {
            try
            {
                string query = "exec GetApplicationsByApplicantName " + applicantName +","+ spaceName;
                DataTable dt = SQLHelper.SelectData(query); 
                List<ApplicationData> applications = new List<ApplicationData>();

                foreach (DataRow row in dt.Rows)
                {
                    ApplicationData application = BuildApplication(row);
                    application.Project.CreatorName = row["CreatorName"] != DBNull.Value ? row["CreatorName"].ToString() : null;
                    application.Project.ProjectName = row["ProjectName"] != DBNull.Value ? row["ProjectName"].ToString() : null;
                    applications.Add(application);
                }

                if (applications != null && applications.Count > 0)
                {
                    return applications;
                }
                else
                {
                    throw new Exception("No applications found");
                }
            }
            catch(Exception ex) { throw new Exception(ex.Message); }
           
        }
    }

}
