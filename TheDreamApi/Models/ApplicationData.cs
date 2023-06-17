namespace TheDreamApi.Models
{
    public class ApplicationData
    {
        public ApplicationData()
        {
            ProjectId = 0;
            ApplicantName= string.Empty;
            Email = string.Empty;
            Message = string.Empty;
            ResumePath = string.Empty;
            Status = string.Empty;
            Project = new Project();
            Requirement = new Requirement();
        }
        public int ProjectId { get; set; }
        public string ApplicantName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string ResumePath { get; set; }
        public string Status { get; set; }
        public Requirement Requirement { get; set; }

        public Project Project { get; set; }//to move information
    }
}
