using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Extensions.ValueTask;
using Soenneker.Telnyx.Client.Abstract;
using Soenneker.Telnyx.ClientUtil.Abstract;
using Soenneker.Telnyx.OpenApiClient;
using Soenneker.Utils.AsyncSingleton;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Telnyx.ClientUtil;

/// <inheritdoc cref="ITelnyxClientUtil" />
public sealed class TelnyxClientUtil : ITelnyxClientUtil
{
    private readonly AsyncSingleton<TelnyxOpenApiClient> _client;
    private readonly ITelnyxHttpClient _httpClientUtil;

    public TelnyxClientUtil(ITelnyxHttpClient httpClientUtil)
    {
        _httpClientUtil = httpClientUtil;
        _client = new AsyncSingleton<TelnyxOpenApiClient>(CreateClient);
    }

    private async ValueTask<TelnyxOpenApiClient> CreateClient(CancellationToken token)
    {
        HttpClient httpClient = await _httpClientUtil.Get(token).NoSync();

        var requestAdapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: httpClient);

        return new TelnyxOpenApiClient(requestAdapter);
    }

    public ValueTask<TelnyxOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return _client.Get(cancellationToken);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _client.DisposeAsync();
    }
}
