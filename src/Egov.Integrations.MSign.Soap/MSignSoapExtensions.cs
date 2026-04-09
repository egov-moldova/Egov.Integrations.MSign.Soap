using Egov.Extensions.Configuration;
using Egov.Integrations.MSign.Soap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions methods to add MSign SOAP client implementation to an application.
/// </summary>
public static class MSignSoapExtensions
{
    /// <summary>
    /// Adds MSign SOAP client implementation.
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/> to add the client to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddMSignSoapClient(this IServiceCollection services)
        => services.AddMSignSoapClient(_ => { });

    /// <summary>
    /// Adds MSign SOAP client implementation.
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/> to add the client to.</param>
    /// <param name="config">The configuration being bound to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddMSignSoapClient(this IServiceCollection services, IConfiguration config) 
        => services.AddMSignSoapClient(config, _ => { });

    /// <summary>
    /// Adds MSign SOAP client implementation.
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/> to add the client to.</param>
    /// <param name="configureOptions">A delegate to configure <see cref="MSignSoapOptions"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddMSignSoapClient(this IServiceCollection services, Action<MSignSoapOptions> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddOptions<MSignSoapOptions>()
            .Configure<IOptions<SystemCertificateOptions>>((msignSoapOptions, systemCertificateOptions) =>
            {
                var systemCertificateOptionsValue = systemCertificateOptions.Value;
                msignSoapOptions.SystemCertificate ??= systemCertificateOptionsValue.Certificate;
                msignSoapOptions.SystemCertificateIntermediaries ??= systemCertificateOptionsValue.IntermediateCertificates;
            });

        services.TryAddSingleton<IPostConfigureOptions<MSignSoapOptions>, MSignSoapOptionsPostConfigure>();

        return services.AddScoped<IMSignSoapClient, MSignSoapClient>();
    }

    /// <summary>
    /// Adds MSign SOAP client implementation.
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/> to add the client to.</param>
    /// <param name="config">The configuration being bound to.</param>
    /// <param name="configureOptions">A delegate to configure <see cref="MSignSoapOptions"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddMSignSoapClient(this IServiceCollection services, IConfiguration config, Action<MSignSoapOptions> configureOptions)
    {
        services.Configure<MSignSoapOptions>(config);
        return services.AddMSignSoapClient(configureOptions);
    }
}