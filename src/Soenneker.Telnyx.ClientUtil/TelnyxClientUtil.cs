using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.Kiota.BearerAuthenticationProvider;
using Soenneker.Telnyx.Client.Abstract;
using Soenneker.Telnyx.ClientUtil.Abstract;
using Soenneker.Telnyx.OpenApiClient;
using Soenneker.Utils.AsyncSingleton;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Telnyx.ClientUtil;

/// <inheritdoc cref="ITelnyxClientUtil"/>
public sealed class TelnyxClientUtil : ITelnyxClientUtil
{
    private readonly AsyncSingleton<TelnyxOpenApiClient> _client;
    private readonly ITelnyxHttpClient _httpClientUtil;
    private readonly IConfiguration _configuration;

    public TelnyxClientUtil(ITelnyxHttpClient httpClientUtil, IConfiguration configuration, ILogger<TelnyxClientUtil> logger)
    {
        _httpClientUtil = httpClientUtil;
        _configuration = configuration;
        _ = logger;
        _client = new AsyncSingleton<TelnyxOpenApiClient>(CreateClient);
    }

    private async ValueTask<TelnyxOpenApiClient> CreateClient(CancellationToken token)
    {
        var telnyxToken = _configuration.GetValueStrict<string>("Telnyx:Token");

        HttpClient httpClient = await _httpClientUtil.Get(token).NoSync();

        var requestAdapter = new HttpClientRequestAdapter(new BearerAuthenticationProvider(telnyxToken), httpClient: httpClient);

        return new TelnyxOpenApiClient(requestAdapter);
    }

    public ValueTask<TelnyxOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return _client.Get(cancellationToken);
    }

    /// <summary>
    /// Releases resources used by the current instance.
    /// </summary>
    public void Dispose()
    {
        _client.Dispose();
    }

    /// <summary>
    /// Asynchronously releases resources used by the current instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public ValueTask DisposeAsync()
    {
        return _client.DisposeAsync();
    }
}
