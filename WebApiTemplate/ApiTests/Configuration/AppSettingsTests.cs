using Api.Configuration;
using Microsoft.Extensions.Configuration;

namespace ApiTests.Configuration
{
    public class AppSettingsTests
    {
        [Fact]
        public void AppSettings_BindConfiguration_Success()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string> {
            {"AppSettings:ProjectInfo:Name", "InventoryManager"},
            {"AppSettings:ProjectInfo:Version", "1.0.0"},
            {"AppSettings:ProjectInfo:Description", "API for managing inventory operations."},
            {"AppSettings:ProjectInfo:Company", "Acme Corp"},
            {"AppSettings:ProjectInfo:Product", "Inventory Suite"},
            {"AppSettings:ProjectInfo:Contact:Name", "infoteam Hellas MIKE"},
            {"AppSettings:ProjectInfo:Contact:Email", "info@infoteam-software.gr"},
            {"AppSettings:ProjectInfo:Contact:Url", "https://infoteam-software.gr"}
        };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Act
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

            // Assert
            Assert.NotNull(appSettings);
            Assert.Equal("InventoryManager", appSettings.ProjectInfo.Name);
            Assert.Equal("1.0.0", appSettings.ProjectInfo.Version);
            Assert.Equal("API for managing inventory operations.", appSettings.ProjectInfo.Description);
            Assert.Equal("Acme Corp", appSettings.ProjectInfo.Company);
            Assert.Equal("Inventory Suite", appSettings.ProjectInfo.Product);
            Assert.Equal("infoteam Hellas MIKE", appSettings.ProjectInfo.Contact.Name);
            Assert.Equal("info@infoteam-software.gr", appSettings.ProjectInfo.Contact.Email);
            Assert.Equal("https://infoteam-software.gr", appSettings.ProjectInfo.Contact.Url);
        }
    }
}
