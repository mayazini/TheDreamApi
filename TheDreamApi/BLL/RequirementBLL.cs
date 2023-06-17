using TheDreamApi.DAL;

namespace TheDreamApi.BLL
{
    public class RequirementBLL
    {

        public static bool HasSpace(int requirementId)
        {
            if(RequirementsDAL.GetRequirementCountByApplication(requirementId) > 0) { return true; }
            return false;
        }
    }
}
