namespace TheDreamApi.Models
{
    public class Inbox
    {
        public int Id { get; set; }
        public string Message { get; set; }    
        public string SenderName { get; set; }

        public string RecieverName { get; set; }
        public DateTime Time { get; set; }
        public string Subject { get; set; }
        public bool IsTrash { get; set; }

    }
}
