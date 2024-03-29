﻿using System.Data;
using System.Text.Json;
using TheDreamApi.DAL;
using TheDreamApi.Models;

namespace TheDreamApi.BLL
{
    public class ApplicationsBLL
    {
        public static string Apply(ApplicationData applicationData)
        {
            return ApplicationsServiceDAL.Apply(applicationData);

        }
        public static DataTable GetApplicantsByProject(int projectId)
        {
            return ApplicationsServiceDAL.GetApplicantsByProject(projectId);

        }

        public static bool UpdateApplicationStatus(int applicationId, string status)
        {
            try
            {
                if (status == "Pending" || status == "Declined")
                {
                    return ApplicationsServiceDAL.UpdateApplicationStatus(applicationId, status);
                }
                else if (status == "Accepted"  && RequirementBLL.HasSpace(applicationId))
                {
                    return ApplicationsServiceDAL.UpdateApplicationStatus(applicationId, status);
                }
                if (status == "Accepted")
                {
                    throw new Exception("No more space");

                }
                throw new Exception("Requested status value invalid.");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<ApplicationData> GetApplicationsByApplicantName(string spaceName, string applicantName)
        {
            try
            {
                return ApplicationsServiceDAL.GetApplicationsByApplicantName(spaceName,applicantName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
