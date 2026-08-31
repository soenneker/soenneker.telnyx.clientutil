using Soenneker.Telnyx.OpenApiClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Telnyx.ClientUtil.Abstract;

/// <summary>
/// Provides a lazily initialized Telnyx OpenAPI client backed by the shared authenticated HTTP client.
/// </summary>
public interface ITelnyxClientUtil : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Releases the generated client wrapper owned by this utility without disposing the shared HTTP provider.
    /// </summary>
    new void Dispose();

    /// <summary>
    /// Asynchronously releases the generated client wrapper owned by this utility without disposing the shared HTTP provider.
    /// </summary>
    new ValueTask DisposeAsync();

    /// <summary>
    /// Gets the cached Telnyx OpenAPI client, creating it on first use.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A configured Telnyx OpenAPI client</returns>
    ValueTask<TelnyxOpenApiClient> Get(CancellationToken cancellationToken = default);
}
