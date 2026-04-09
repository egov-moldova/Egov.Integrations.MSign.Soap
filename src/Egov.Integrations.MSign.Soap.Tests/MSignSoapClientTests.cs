using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;
using Moq;

namespace Egov.Integrations.MSign.Soap.Tests;

public class MSignSoapClientTests
{
    private readonly Mock<IOptions<MSignSoapOptions>> _optionsMock;
    private readonly MSignSoapOptions _options;

    public MSignSoapClientTests()
    {
        _optionsMock = new Mock<IOptions<MSignSoapOptions>>();
        _options = new MSignSoapOptions
        {
            FrontendAddress = new Uri("https://msign.staging.egov.md"),
            ServiceRootUrl = new Uri("https://localhost:44379"),
            ApiAddress = new Uri("https://msign.staging.egov.md:8443/MSign.svc"),
            BasicHttpsBinding = new System.ServiceModel.BasicHttpsBinding(),
            EndpointAddress = new System.ServiceModel.EndpointAddress("https://msign.staging.egov.md:8443/MSign.svc")
        };
        _optionsMock.Setup(o => o.Value).Returns(_options);
    }

    [Fact]
    public void BuildRedirectAddress_Basic_ReturnsCorrectUrl()
    {
        // Arrange
        var client = new MSignSoapClient(_optionsMock.Object);
        var requestID = "REQ-123";
        var relativeReturnUrl = "/callback";

        // Act
        var result = client.BuildRedirectAddress(requestID, relativeReturnUrl);

        // Assert
        Assert.Equal("https://msign.staging.egov.md/REQ-123?returnUrl=https%3A%2F%2Flocalhost%3A44379%2Fcallback", result);
    }

    [Fact]
    public void BuildRedirectAddress_WithAllParams_ReturnsCorrectUrl()
    {
        // Arrange
        var client = new MSignSoapClient(_optionsMock.Object);
        var requestID = "REQ-123";
        var relativeReturnUrl = "/callback";
        var relayState = "state123";
        var lang = "ro";
        var instrument = "mobile";
        var msisdn = "37360000000";

        // Act
        var result = client.BuildRedirectAddress(requestID, relativeReturnUrl, relayState, lang, instrument, msisdn);

        // Assert
        var expected = "https://msign.staging.egov.md/REQ-123?returnUrl=https%3A%2F%2Flocalhost%3A44379%2Fcallback" +
                       "&relayState=state123&lang=ro&instrument=mobile&msisdn=37360000000";
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void BuildRedirectAddress_InvalidRequestID_ThrowsArgumentNullException(string? requestID)
    {
        // Arrange
        var client = new MSignSoapClient(_optionsMock.Object);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => client.BuildRedirectAddress(requestID!, "/callback"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void BuildRedirectAddress_InvalidRelativeReturnUrl_ThrowsArgumentNullException(string? relativeReturnUrl)
    {
        // Arrange
        var client = new MSignSoapClient(_optionsMock.Object);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => client.BuildRedirectAddress("REQ-123", relativeReturnUrl!));
    }
}
