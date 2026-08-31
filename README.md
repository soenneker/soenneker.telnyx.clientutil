[![](https://img.shields.io/nuget/v/soenneker.telnyx.clientutil.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.telnyx.clientutil/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.telnyx.clientutil/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.telnyx.clientutil/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.telnyx.clientutil.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.telnyx.clientutil/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.telnyx.clientutil/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.telnyx.clientutil/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Telnyx.ClientUtil

Provides a lazily initialized `TelnyxOpenApiClient` backed by the authenticated, cached Telnyx `HttpClient`.

## Installation

```bash
dotnet add package Soenneker.Telnyx.ClientUtil
```

## Configuration

```json
{
  "Telnyx": {
    "Token": "KEY..."
  }
}
```

## Usage

```csharp
using Soenneker.Telnyx.ClientUtil.Abstract;
using Soenneker.Telnyx.ClientUtil.Registrars;
using Soenneker.Telnyx.OpenApiClient;
using Soenneker.Telnyx.OpenApiClient.Models;

services.AddTelnyxClientUtilAsScoped();

TelnyxOpenApiClient client = await telnyxClientUtil.Get(cancellationToken);
ListMessagingProfilesResponse? response = await client.Messaging_profiles.GetAsync(
    cancellationToken: cancellationToken);
```

The scoped registration uses a singleton HTTP provider. Disposing the scoped utility releases its generated client wrapper without removing the shared authenticated `HttpClient`; the HTTP provider disposes that client at application shutdown.

The generated client follows Telnyx's OpenAPI operation and schema names, including underscore-separated members such as `Messaging_profiles`.
