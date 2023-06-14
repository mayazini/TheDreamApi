namespace TheDreamApi.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public string Amount { get; set; }
        public int ItemId { get; set; }
        public string StatusId { get; set; }
    }
}
