namespace TheDreamApi.Models
{
    public class RetailProject:Project
    {
        public Requirement Investor { set; get; }
        public Requirement Supplier { set; get; }
    }
}
