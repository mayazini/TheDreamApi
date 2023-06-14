namespace TheDreamApi.Models
{
    public class Requirement
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public int ProjectId { get; set; }
        
    }
}
