# Egov.Integrations.MSign.Soap

[![NuGet](https://img.shields.io/nuget/v/Egov.Integrations.MSign.Soap.svg)](https://www.nuget.org/packages/Egov.Integrations.MSign.Soap)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A .NET library that provides a reusable client for connecting with MSign digital signature service using the SOAP protocol. It is built for ASP.NET Core 10.0+ applications and integrates seamlessly with the eGov platform's shared configuration and certificate management.

---

## Table of Contents

- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
  - [Dependency Injection (Recommended)](#using-dependency-injection-recommended)
  - [Creating Signature Requests](#creating-signature-requests)
  - [Handling Responses](#handling-responses)
- [Supported Content Types](#supported-content-types)
- [Error Handling](#error-handling)
- [Testing](#testing)
- [Contributing](#contributing)
- [Code of Conduct](#code-of-conduct)
- [AI Assistance](#ai-assistance)
- [License](#license)

---

## Features

- Full support for MSign SOAP API operations
- Seamless integration with `Egov.Extensions.Configuration` for certificate management
- Built-in helper for building MSign redirect addresses
- Async-first API design
- Automatic handling of certificate chains and intermediate certificates

---

## Prerequisites

- .NET 10.0 or later
- A valid Service Provider certificate registered in MSign (issued by [STISC](https://stisc.gov.md/))
- Access to MSign SOAP API endpoints

---

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/Egov.Integrations.MSign.Soap):

```shell
dotnet add package Egov.Integrations.MSign.Soap
```

Or via the Package Manager Console:

```shell
Install-Package Egov.Integrations.MSign.Soap
```

**Note:** This package depends on [`Egov.Extensions.Configuration`](https://www.nuget.org/packages/Egov.Extensions.Configuration/).

---

## Configuration

Add the following sections to your **appsettings.json**:

```json
{
  "Certificate": {
    "Path": "Files/Certificates/your-certificate.pfx",
    "Password": "your-certificate-password"
  },
  "MSignSoap": {
    "ApiAddress": "https://msign.staging.egov.md:8443/MSign.svc",
    "FrontendAddress": "https://msign.staging.egov.md",
    "ServiceRootUrl": "https://localhost:44379"
  }
}
```

- **`ApiAddress`**: The endpoint of the MSign SOAP service.
- **`FrontendAddress`**: The base URL of the MSign user interface.
- **`ServiceRootUrl`**: The base URL of your application (must be HTTPS).

---

## Usage

### Using Dependency Injection (Recommended)

Register the services in **Program.cs**:

```csharp
builder.Services.AddSystemCertificate(builder.Configuration.GetSection("Certificate"));
builder.Services.AddMSignSoapClient(builder.Configuration.GetSection("MSignSoap"));
```

### Creating Signature Requests

Inject `IMSignSoapClient` into your controller or service:

```csharp
public class HomeController : ControllerBase
{
    private readonly IMSignSoapClient _msignClient;

    public HomeController(IMSignSoapClient msignClient)
    {
        _msignClient = msignClient;
    }

    public async Task<IActionResult> Sign()
    {
        var requestID = await _msignClient.PostSignRequestAsync(new SignRequest
        {
            ShortContentDescription = "Sample HASH signature",
            ContentType = ContentType.Hash,
            Contents = new[]
            {
                new SignContent
                {
                    Content = SHA1.HashData(new byte[] { 0xDE, 0xAD, 0xBE, 0xEF })
                }
            }
        });

        var relayState = Guid.NewGuid().ToString();
        var redirectUrl = _msignClient.BuildRedirectAddress(requestID, Url.Action(nameof(Callback))!, relayState);
        
        return Redirect(redirectUrl);
    }
}
```

### Handling Responses

```csharp
public async Task<IActionResult> Callback(string requestID, string relayState)
{
    var response = await _msignClient.GetSignResponseAsync(requestID, null);

    if (response.Status == SignStatus.Success)
    {
        // Process successful signature
        return Ok();
    }

    return BadRequest(response.Message);
}
```

---

## Supported Content Types

| Content Type | Description |
|--------------|-------------|
| **Hash** | Signature on a provided hash (SHA-1, SHA-256, etc.) |
| **PDF** | Native PDF signature |

---

## Error Handling

| Exception | Scenario |
|-----------|----------|
| `ArgumentNullException` | `requestID` or `relativeReturnUrl` is missing |
| `CommunicationException` | Network or SOAP communication issues |
| `SecurityNegotiationException` | Certificate authentication failures |

---

## Testing

The solution includes a test project `Test/` that demonstrates usage and can be used for integration testing.

```shell
dotnet test
```

---

## Contributing

Contributions are welcome! Please read [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines on how to get started.

---

## Code of Conduct

This project adheres to the [Contributor Covenant Code of Conduct](CODE_OF_CONDUCT.md). By participating, you are expected to uphold this code.

---

## AI Assistance

This repository contains an [AGENTS.md](AGENTS.md) file with instructions and context for AI coding agents to assist in development, ensuring consistency in code style and project structure.

---

## License

This project is licensed under the [MIT License](LICENSE).
