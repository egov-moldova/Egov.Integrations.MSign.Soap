using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;

namespace Egov.Integrations.MSign.Soap.Tests;

public class MSignSoapOptionsPostConfigureTests
{
    private readonly MSignSoapOptionsPostConfigure _postConfigure;
    private readonly X509Certificate2 _testCertificate;

    public MSignSoapOptionsPostConfigureTests()
    {
        _postConfigure = new MSignSoapOptionsPostConfigure();
        // Create a dummy certificate for testing
        _testCertificate = new X509Certificate2();
    }

    [Fact]
    public void PostConfigure_ValidOptions_SetsDefaults()
    {
        // Arrange
        var options = new MSignSoapOptions
        {
            SystemCertificate = _testCertificate,
            ApiAddress = new Uri("https://api.example.com"),
            FrontendAddress = new Uri("https://frontend.example.com"),
            ServiceRootUrl = new Uri("https://service.example.com")
        };

        // Act
        _postConfigure.PostConfigure(Options.DefaultName, options);

        // Assert
        Assert.NotNull(options.BasicHttpsBinding);
        Assert.NotNull(options.EndpointAddress);
        Assert.Equal("https://api.example.com/", options.EndpointAddress.Uri.ToString());
    }

    [Fact]
    public void PostConfigure_MissingCertificate_ThrowsApplicationException()
    {
        // Arrange
        var options = new MSignSoapOptions();

        // Act & Assert
        var ex = Assert.Throws<ApplicationException>(() => _postConfigure.PostConfigure(Options.DefaultName, options));
        Assert.Contains("System certificate", ex.Message);
    }

    [Fact]
    public void PostConfigure_MissingApiAddress_ThrowsApplicationException()
    {
        // Arrange
        var options = new MSignSoapOptions
        {
            SystemCertificate = _testCertificate
        };

        // Act & Assert
        var ex = Assert.Throws<ApplicationException>(() => _postConfigure.PostConfigure(Options.DefaultName, options));
        Assert.Contains("MSign API Address", ex.Message);
    }

    [Fact]
    public void PostConfigure_MissingFrontendAddress_ThrowsApplicationException()
    {
        // Arrange
        var options = new MSignSoapOptions
        {
            SystemCertificate = _testCertificate,
            ApiAddress = new Uri("https://api.example.com")
        };

        // Act & Assert
        var ex = Assert.Throws<ApplicationException>(() => _postConfigure.PostConfigure(Options.DefaultName, options));
        Assert.Contains("frontend address not configured", ex.Message);
    }

    [Fact]
    public void PostConfigure_MissingServiceRootUrl_ThrowsApplicationException()
    {
        // Arrange
        var options = new MSignSoapOptions
        {
            SystemCertificate = _testCertificate,
            ApiAddress = new Uri("https://api.example.com"),
            FrontendAddress = new Uri("https://frontend.example.com")
        };

        // Act & Assert
        var ex = Assert.Throws<ApplicationException>(() => _postConfigure.PostConfigure(Options.DefaultName, options));
        Assert.Contains("ServiceRootUrl not configured", ex.Message);
    }
}
