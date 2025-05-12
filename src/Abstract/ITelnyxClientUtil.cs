using Soenneker.Telnyx.OpenApiClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Telnyx.ClientUtil.Abstract;

/// <summary>
/// An async thread-safe singleton for the Telnyx OpenApiClient
/// </summary>
public interface ITelnyxClientUtil : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Gets a configured Telnyx.so OpenAPI client instance
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A configured Telnyx OpenAPI client</returns>
    ValueTask<TelnyxOpenApiClient> Get(CancellationToken cancellationToken = default);
}