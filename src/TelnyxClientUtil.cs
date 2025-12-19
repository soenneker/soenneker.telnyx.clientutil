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
using Soenneker.HttpClients.LoggingHandler;

namespace Soenneker.Telnyx.ClientUtil;

/// <inheritdoc cref="ITelnyxClientUtil"/>
public sealed class TelnyxClientUtil : ITelnyxClientUtil
{
    private readonly AsyncSingleton<TelnyxOpenApiClient> _client;

    private HttpClient? _httpClient;

    public TelnyxClientUtil(ITelnyxHttpClient httpClientUtil, IConfiguration configuration, ILogger<TelnyxClientUtil> logger)
    {
        _client = new AsyncSingleton<TelnyxOpenApiClient>(async token =>
        {
            var telnyxToken = configuration.GetValueStrict<string>("Telnyx:Token");

            var logging = configuration.GetValue<bool>("Telnyx:RequestResponseLogging");

            if (logging)
            {
                var loggingHandler = new HttpClientLoggingHandler(logger, new HttpClientLoggingOptions
                {
                    LogLevel = LogLevel.Debug
                });

                loggingHandler.InnerHandler = new HttpClientHandler();

                _httpClient = new HttpClient(loggingHandler);
            }
            else
            {
                _httpClient = await httpClientUtil.Get(token).NoSync();
            }

            var requestAdapter = new HttpClientRequestAdapter(new BearerAuthenticationProvider(telnyxToken), httpClient: _httpClient);

            return new TelnyxOpenApiClient(requestAdapter);
        });
    }

    public ValueTask<TelnyxOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return _client.Get(cancellationToken);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();

        _client.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        _httpClient?.Dispose();

        return _client.DisposeAsync();
    }
}