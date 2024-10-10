# ASP.NET Authentication SDK for Integrating Asgardeo

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [Blazor Sample Application](#blazor-sample-application)
- [Contributing](#contributing)
- [License](#license)

## Introduction
This SDK provides seamless integration of [Asgardeo](https://wso2.com/asgardeo) with ASP.NET applications. It offers authentication functionalities based on the OAuth2.0 and OpenID Connect (OIDC) protocols, enabling secure user authentication and token management.

## Features
- OAuth2.0 and OIDC compliant.
- Easy integration with ASP.NET applications.
- Token management (access tokens, refresh tokens, ID tokens).
- Secure authentication with Asgardeo Identity and Access Management (IAM) platform.

## Installation

You can clone this repository and include the SDK in your project directly.

## Configuration

To configure the SDK for your project, follow these steps:

1. Add the Asgardeo-specific settings to your `appsettings.json` file:

```json
{
    "Asgardeo": {
        "ClientId": "YOUR_CLIENT_ID",
        "ClientSecret": "YOUR_CLIENT_SECRET",
        "Authority": "https://api.asgardeo.io/t/<your-tenant-name>/oauth2",
        "RedirectUri": "https://localhost:5001/signin-oidc",
        "PostLogoutRedirectUri": "https://localhost:5001/signout-callback-oidc"
    }
}
```

2. Update your Startup.cs/Program.cs to configure authentication:

```cs
builder.Services.AddAsgardeoAuthentication(options =>
{
    builder.Configuration.GetSection("Asgardeo").Bind(options);
});
```

3. Usage

Once the SDK is configured, you can access authenticated users in your controllers or services:

You can also manage tokens and user sessions using the provided methods from the SDK. Refer to the API documentation for more advanced usage scenarios.

## Blazor Sample Application

To see a working example of this SDK, check out the Blazor Sample Application included in this repository. This sample demonstrates how to use the SDK in a Blazor Server application with Asgardeo authentication.

## Contributing

Contributions are welcome! Please refer to our contribution guidelines for details on how to get started.

## License

Licenses this source under the Apache License, Version 2.0 (LICENSE), You may not use this file except in compliance with the License.