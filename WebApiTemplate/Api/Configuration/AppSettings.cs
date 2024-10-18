namespace Api.Configuration
{
    /// <summary>
    /// Configuration settings for the application
    /// </summary>
    public class AppSettings
    {
        public ProjectInfo ProjectInfo { get; set; } = new ProjectInfo();
    }

    /// <summary>
    /// General Information about the project
    /// </summary>
    public class ProjectInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public Contact Contact { get; set; } = new Contact();
    }

    /// <summary>
    /// Contact information for the project
    /// </summary>
    public class Contact
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}