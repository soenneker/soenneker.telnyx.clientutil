using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Telnyx.Client.Registrars;
using Soenneker.Telnyx.ClientUtil.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.Telnyx.ClientUtil.Registrars;

/// <summary>
/// An async thread-safe singleton for the Telnyx OpenApiClient
/// </summary>
public static class TelnyxClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="ITelnyxClientUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddTelnyxClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton().AddTelnyxHttpClientAsSingleton().TryAddSingleton<ITelnyxClientUtil, TelnyxClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="ITelnyxClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddTelnyxClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton().AddTelnyxHttpClientAsSingleton().TryAddScoped<ITelnyxClientUtil, TelnyxClientUtil>();


        return services;
    }
}