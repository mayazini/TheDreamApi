using System.Runtime.CompilerServices;
using TheDreamApi.Models;

namespace TheDreamApi.DAL
{
    public class RequirementsDAL
    {

        public static bool CreateNewRequirment(int projectId, string requirementName, int requirementAmount )
        {
            string requirementQuery = $"INSERT INTO Requirements (projectId, Description, Amount) VALUES ({projectId}, '{requirementName}', {requirementAmount})";
            int result = SQLHelper.DoQuery(requirementQuery);
            return result > 0;
        }

        public static int GetRequirementCountByApplication(int applicationId)
        {
            string requirementQuery = "exec spGetRequirementCountByApplication " + applicationId;
            int result = SQLHelper.SelectScalarToInt32(requirementQuery);
            return result ;
        }
    }
}
