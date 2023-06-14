namespace TheDreamApi.Models
{
    public class Applications
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string UserName { get; set; }
        public string Space { get; set; }
        public byte[] Resume { get; set; }
        public bool IsApproved { get; set; }
    }
}
