# AI Assistance

This repository is designed to be AI-friendly. If you are using an AI coding assistant (like GitHub Copilot, Cursor, or Junie), this file provides context to help the agent understand the project's structure and coding standards.

## Project Overview

- **Name**: Egov.Integrations.MSign.Soap
- **Technology Stack**: .NET 10, ASP.NET Core, SOAP, WCF (System.ServiceModel)
- **Purpose**: A reusable library for integrating with the MSign digital signature service using the SOAP protocol.

## Coding Standards

- **Language**: C# 14
- **Formatting**: Use standard C# conventions.
- **Nullability**: Enabled. Always handle potential null values.
- **Documentation**: Provide XML documentation for all public members.

## Key Components

- `IMSignSoapClient`: The main interface for interacting with MSign.
- `MSignSoapOptions`: Configuration options for the SOAP client.
- `AddMSignSoapClient`: Extension methods for dependency injection.
- `SystemCertificateOptions`: Shared certificate configuration (from `Egov.Extensions.Configuration`).

## Instructions for AI Agents

1. **Maintain Consistency**: Follow the existing patterns for dependency injection and options configuration.
2. **Error Handling**: Ensure that all API calls are properly wrapped in error handling as per the project's standards.
3. **Testing**: When adding new features, update or add corresponding tests in the test project (if applicable).
4. **Security**: Handle certificates and private keys with care, following the established patterns in `MSignSoapOptionsPostConfigure`.
