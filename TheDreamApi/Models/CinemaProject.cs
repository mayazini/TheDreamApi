namespace TheDreamApi.Models
{
    public class CinemaProject:Project
    {
        public Requirement Director { set; get; }
        public Requirement Writer { set; get; }
        public Requirement Filmer { set; get; }
    }
}
