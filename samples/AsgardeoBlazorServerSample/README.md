# Blazor Server Sample with Asgardeo Authentication

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Requirements](#requirements)
- [Setup](#setup)
- [Running the Application](#running-the-application)
- [Screenshots](#screenshots)
- [Troubleshooting](#troubleshooting)
- [License](#license)

## Introduction
This is a sample Blazor Server application that demonstrates how to integrate [Asgardeo](https://wso2.com/asgardeo) for authentication using the ASP.NET Authentication SDK. The application allows users to sign in, view their profile, and sign out, utilizing OAuth2.0/OIDC protocols.

## Features
- Blazor Server application with Asgardeo authentication.
- OAuth2.0 and OIDC-based authentication flow.
- Token management and user session handling.

## Requirements
- .NET 6.0 or later
- An active Asgardeo account with a registered application
- ASP.NET Authentication SDK (included in the [mono repo](../README.md))

## Asgardeo Configuration:

1. Sign up or log in to your Asgardeo account.
2. Create a new application and configure it for your application.
3. Obtain the client ID, base URL, and other necessary credentials.

## Setup
1. Clone this repository and navigate to the `samples/AsgardeoBlazorServerSample` directory:

    ```bash
    git clone <repository-url>
    cd samples/AsgardeoBlazorServerSample
    ```

2. Update the configuration settings in `appsettings.json`:

    ```json
    {
        "Asgardeo": {
            "Authority": "https://api.asgardeo.io/t/<YOUR_ORGANIZATION_NAME>/oauth2/token",
            "ClientId": "<YOUR_CLIENT_ID>",
            "ClientSecret": "<YOUR_CLIENT_SECRET>",
            "ResponseType": "code",
            "Scopes": ["openid", "profile"],
        }
    }
    ```

3. Restore the NuGet packages:

    ```bash
    dotnet restore
    ```

## Running the Application
To run the application locally:

```bash
dotnet run
```

Note: The application will be accessible at https://localhost:5285. You can sign in using your Asgardeo account, view the authenticated user's profile, and log out.

## Preview

Here are some example screenshots of the running application:

1. Login page
2. Welcome page

## Troubleshooting

If you encounter any issues with the setup or running the application, refer to the following:

- Ensure that your Asgardeo application configuration matches the settings in appsettings.json.
- Check if you have installed all the required NuGet packages.
- Make sure the redirect URIs in Asgardeo match the URIs configured in the application.
- For more help, refer to the official Asgardeo documentation.

## Contributing

Contributions are welcome! Please refer to our contribution guidelines for details on how to get started.

## License

Licenses this source under the Apache License, Version 2.0 (LICENSE), You may not use this file except in compliance with the License.