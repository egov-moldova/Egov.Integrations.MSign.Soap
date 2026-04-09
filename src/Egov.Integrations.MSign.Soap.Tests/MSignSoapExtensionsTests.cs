using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Egov.Integrations.MSign.Soap.Tests;

public class MSignSoapExtensionsTests
{
    [Fact]
    public void AddMSignSoapClient_RegistersServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["MSignSoap:ApiAddress"] = "https://api.example.com",
                ["MSignSoap:FrontendAddress"] = "https://frontend.example.com",
                ["MSignSoap:ServiceRootUrl"] = "https://service.example.com"
            })
            .Build();

        // Act
        services.AddMSignSoapClient(configuration.GetSection("MSignSoap"), options =>
        {
            options.SystemCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2();
        });
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var options = serviceProvider.GetService<IOptions<MSignSoapOptions>>();
        Assert.NotNull(options);
        Assert.Equal("https://api.example.com/", options.Value.ApiAddress.ToString());

        var client = serviceProvider.GetService<IMSignSoapClient>();
        Assert.NotNull(client);
    }
}
