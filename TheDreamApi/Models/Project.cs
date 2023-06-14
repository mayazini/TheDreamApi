namespace TheDreamApi.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string CreatorName { get; set; }
        public List<Requirement> Requirements { get;set;}

        public int SpaceId { get; set; }
    }
}
