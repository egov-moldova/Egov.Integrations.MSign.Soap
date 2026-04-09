using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using Microsoft.Extensions.Options;

namespace Egov.Integrations.MSign.Soap;

internal class MSignSoapOptionsPostConfigure : IPostConfigureOptions<MSignSoapOptions>
{
    public void PostConfigure(string? name, MSignSoapOptions options)
    {
        if (options.SystemCertificate == null)
        {
            throw new ApplicationException("System certificate for MSign not configured or not available");
        }

        // TODO: Can we skip this somehow?
        if (options.SystemCertificateIntermediaries != null)
        {
            using var intermediateStore = new X509Store(StoreName.CertificateAuthority, StoreLocation.CurrentUser, OpenFlags.ReadWrite);
            foreach (var intermediateCertificate in options.SystemCertificateIntermediaries)
            {
                if (intermediateCertificate.Issuer != intermediateCertificate.Subject &&
                    !intermediateStore.Certificates.Contains(intermediateCertificate))
                {
                    intermediateStore.Add(intermediateCertificate);
                }
            }
        }

        options.BasicHttpsBinding ??= new BasicHttpsBinding
        {
            Security =
            {
                Mode = BasicHttpsSecurityMode.Transport,
                Transport =
                {
                    ClientCredentialType = HttpClientCredentialType.Certificate
                }
            },
            MaxReceivedMessageSize = 2147483647
        };

        if (options.EndpointAddress == null && options.ApiAddress != null)
        {
            options.EndpointAddress = new EndpointAddress(options.ApiAddress);
        }

        if (options.EndpointAddress == null)
        {
            throw new ApplicationException("Neither MSign API Address nor endpoint address configured");
        }

        if (options.FrontendAddress == null)
        {
            throw new ApplicationException("MSign frontend address not configured");
        }

        if (options.ServiceRootUrl == null)
        {
            throw new ApplicationException("ServiceRootUrl not configured for MSign");
        }
    }
}
