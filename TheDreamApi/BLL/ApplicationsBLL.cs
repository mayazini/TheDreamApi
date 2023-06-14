namespace TheDreamApi.BLL
{
    public class ApplicationsBLL
    {
        public static void SaveResumeFile(Stream resumeStream, int projectId, string username)
        {
            string fileName = $"{projectId}_{username}_resume.pdf"; // Generate a unique file name
            string filePath = Path.Combine("Users_Resumes", fileName); // Specify the directory where resumes will be stored

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                resumeStream.CopyTo(fileStream);
            }
        }
    }
}
