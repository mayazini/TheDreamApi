﻿namespace TheDreamApi.Models
{
    public class ApplicationData
    {
        public int ProjectId { get; set; }
        public string ApplicantName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string ResumePath { get; set; }
        public string Status { get; set; }
        public Requirement Requirement { get; set; }
    }
}
