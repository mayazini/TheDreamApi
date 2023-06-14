namespace TheDreamApi.Models
{
    public class ApplicationData
    {
        public int ProjectId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public byte[] ResumeData { get; set; }
        public Stream ResumeStream { get; set; }
        public Requirement Requirement { get; set; }
    }
}
