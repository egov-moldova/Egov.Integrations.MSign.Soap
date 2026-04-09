using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace Egov.Integrations.MSign.Soap;

/// <summary>
/// Options for MSign SOAP client.
/// </summary>
public class MSignSoapOptions
{
    /// <summary>
    /// Address of MSign SOAP service (i.e. https://msign.gov.md:8443/MSign.svc).
    /// </summary>
    public Uri ApiAddress { get; set; } = default!;

    /// <summary>
    /// Address of MSign Frontend (i.e. https://msign.gov.md).
    /// </summary>
    public Uri FrontendAddress { get; set; } = default!;

    /// <summary>
    /// URL for service.
    /// </summary>
    public Uri ServiceRootUrl { get; set; } = default!;

    /// <summary>
    /// The client certificate to use to authenticate with MSign Soap API. Can be set directly, if needed.
    /// </summary>
    public X509Certificate2? SystemCertificate { get; set; }

    /// <summary>
    /// The intermediate certificates to use to authenticate with MSign Soap API. Can be set directly, if needed.
    /// </summary>
    public X509Certificate2Collection? SystemCertificateIntermediaries { get; set; }

    internal BasicHttpsBinding? BasicHttpsBinding { get; set; }

    internal EndpointAddress? EndpointAddress { get; set; }
}
