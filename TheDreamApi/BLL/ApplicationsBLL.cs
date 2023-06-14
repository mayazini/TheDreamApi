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

        
    }
}
