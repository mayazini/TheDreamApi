namespace TheDreamApi.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public int ProjectId { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public byte[] Image { get; set; }
    }
}
