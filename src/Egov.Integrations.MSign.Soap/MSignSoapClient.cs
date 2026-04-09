using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Encodings.Web;

namespace Egov.Integrations.MSign.Soap;

internal sealed class MSignSoapClient : MSignClient, IMSignSoapClient
{
    private readonly MSignSoapOptions _settings;

    public MSignSoapClient(IOptions<MSignSoapOptions> options)
        : base(options.Value.BasicHttpsBinding, options.Value.EndpointAddress)
    {
        _settings = options.Value;
        ClientCredentials.ClientCertificate.Certificate = _settings.SystemCertificate;
    }

    public string BuildRedirectAddress(string requestID, string relativeReturnUrl, string? relayState = null, string? lang = null, string? instrument = null, string? msisdn = null)
    {
        if (string.IsNullOrWhiteSpace(requestID))
        {
            throw new ArgumentNullException(nameof(requestID));
        }

        if (string.IsNullOrWhiteSpace(relativeReturnUrl))
        {
            throw new ArgumentNullException(nameof(relativeReturnUrl));
        }

        var result = new StringBuilder(_settings.FrontendAddress.AbsoluteUri);
        result.Append(requestID);

        result.Append("?returnUrl=").Append(UrlEncoder.Default.Encode(new Uri(_settings.ServiceRootUrl, relativeReturnUrl).AbsoluteUri));

        if (relayState != null)
        {
            result.Append("&relayState=").Append(UrlEncoder.Default.Encode(relayState));
        }

        if (lang != null)
        {
            result.Append("&lang=").Append(UrlEncoder.Default.Encode(lang));
        }

        if (instrument != null)
        {
            result.Append("&instrument=").Append(UrlEncoder.Default.Encode(instrument));
        }

        if (msisdn != null)
        {
            result.Append("&msisdn=").Append(UrlEncoder.Default.Encode(msisdn));
        }

        return result.ToString();
    }
}
