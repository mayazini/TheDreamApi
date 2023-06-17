using System.Text.Json.Nodes;
using System.Text.Json;

namespace TheDreamApi.Models
{
    public class Project
    {
        public Project()
        {
            ProjectId = 0;
            SpaceName = string.Empty;
            ProjectName = string.Empty;
            Description = string.Empty;
            CreatorName = string.Empty;
            Requirements = new List<Requirement>();
        }
        public int ProjectId { get; set; }
        public string SpaceName { get; set; }// not in data table but how client and server talk
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string CreatorName { get; set; }
        public List<Requirement> Requirements { get; set; }
        public static Project BuildProjectFromClient(JsonElement value)
        {
            dynamic obj = JsonNode.Parse(value.GetRawText());
            string spaceName = (string)obj["spaceName"];
            string projectName = (string)obj["projectName"];
            string description = (string)obj["description"];
            string creatorName = (string)obj["creatorName"];
            var requirements = obj["requirements"];

            // Create a new Project instance
            var project = new Project
            {
                SpaceName = spaceName,
                ProjectName = projectName,
                Description = description,
                CreatorName = creatorName,
                Requirements = new List<Requirement>()
            };

            foreach (var requirement in requirements)
            {
                string requirementName = (string)requirement["name"];
                int requirementAmount = Int32.Parse((string)requirement["amount"]);

                // Create a new Requirement instance
                var requirementObj = new Requirement
                {
                    Description = requirementName,
                    Amount = requirementAmount
                };

                // Add the requirement to the project's Requirements list
                project.Requirements.Add(requirementObj);
            }

            return project;
        }

    }

  
}
