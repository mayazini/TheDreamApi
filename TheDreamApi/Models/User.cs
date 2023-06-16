using Microsoft.AspNetCore.Hosting.Server;

namespace TheDreamApi.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string LinkedInLink { get; set; }
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public string HashSalt { get; set; }
    }

 
}
