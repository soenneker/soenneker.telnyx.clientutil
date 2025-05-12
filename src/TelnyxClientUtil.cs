using Microsoft.Extensions.Configuration;
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

    public TelnyxClientUtil(ITelnyxHttpClient httpClientUtil, IConfiguration configuration)
    {
        _client = new AsyncSingleton<TelnyxOpenApiClient>(async (token, _) =>
        {
            HttpClient httpClient = await httpClientUtil.Get(token).NoSync();

            var telnyxToken = configuration.GetValueStrict<string>("Telnyx:Token");

            var requestAdapter = new HttpClientRequestAdapter(new BearerAuthenticationProvider(telnyxToken), httpClient: httpClient);

            return new TelnyxOpenApiClient(requestAdapter);
        });
    }

    public ValueTask<TelnyxOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return _client.Get(cancellationToken);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _client.DisposeAsync();
    }
}