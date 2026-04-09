namespace Egov.Integrations.MSign.Soap;

/// <summary>
/// Represents the interface for MSign SOAP client.
/// </summary>
public interface IMSignSoapClient : IMSign
{
    /// <summary>
    /// Builds a redirect address to MSign frontend.
    /// </summary>
    /// <param name="requestID">The request ID returned by <see cref="IMSign.PostSignRequestAsync(SignRequest)"/></param>
    /// <param name="relativeReturnUrl">The URL to return on signature result.</param>
    /// <param name="relayState">The relay state to return on signature result.</param>
    /// <param name="lang">Language to be used by MSign user interface. Known values: "ro", "ru", "en".</param>
    /// <param name="instrument">The signing instrument to be used, skipping signing instrument selection page. Known values: "mobile", "moldsign", "nationalid", "mobisign".</param>
    /// <param name="msisdn">User's mobile phone number, if known.</param>
    /// <returns>Absolute URL to redirect the user's browser to.</returns>
    string BuildRedirectAddress(string requestID, string relativeReturnUrl, string? relayState = null, string? lang = null, string? instrument = null, string? msisdn = null);
}
