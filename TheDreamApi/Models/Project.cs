namespace TheDreamApi.Models
{
    public class Project
    {
        int ProjectId { get; set; }
        string ProjectName { get; set; }
        string Description { get; set; }
        string CreatorName { get; set; }
        List<Requirement> Requirments {get;set;}
}
}
