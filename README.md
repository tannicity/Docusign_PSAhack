# DocuSign Quick Start examples for C#

Repository: [qs-csharp](https://github.com/docusign/qs-csharp)

These quick start examples provide straight-forward
code examples for quickly
trying the DocuSign eSignature API with the 
[C# SDK](https://github.com/docusign/docusign-csharp-client).

### Screencast
Watch the 4 minute [installation video][installVideo]

[![Installation Video][installVideoThumb]][installVideo]

[installVideo]:http://tiny.cc/csharpvideo1
[installVideoThumb]:https://raw.githubusercontent.com/docusign/qs-csharp/master/documentation/C_sharp_qs_video1_thumb_200.png


The repo includes:

1. [Pages\EmbeddedSigning.cshtml.cs](https://github.com/docusign/qs-csharp/blob/master/qs-csharp/Pages/EmbeddedSigning.cshtml.cs)
   -- Embedding a signing ceremony in your web application.
2. [Pages\SendEnvelope.cshtml.cs](https://github.com/docusign/qs-csharp/blob/master/qs-csharp/Pages/SendEnvelope.cshtml.cs)
   -- Sending a signing request via an email to the signer.
3. [Pages\ListEnvelopes.cshtml.cs](https://github.com/docusign/qs-csharp/blob/master/qs-csharp/Pages/ListEnvelopes.cshtml.cs)
   -- Listing the envelopes in the user's account, including their status.

These examples do not include authentication. Instead,
use the DocuSign DevCenter's
[OAuth token generator](https://developers.docusign.com/oauth-token-generator)
to create an access token.

For a C# JWT authentication example, see the
example repositories:

* .NET Core: [eg-01-csharp-jwt-core](https://github.com/docusign/eg-01-csharp-jwt-core)
* .NET Framework: [eg-01-csharp-jwt-framework](https://github.com/docusign/eg-01-csharp-jwt-framework)

An OAuth Authorization Code Grant example is
also being developed.

For more information, see the
[DocuSign DevCenter Examples section](https://developers.docusign.com/esign-rest-api/code-examples).

## Installation

### Compatibility
This example uses .NET Core 2.1 with the Microsoft Razor pages web framework.
The SDK itself is compatible with .NET Core 2.0 and .NET Framework 4.5 or later versions.

The DocuSign example files will work with .NET Core or Framework.

### Installation steps
Download or clone this repository. Then open the project in Visual Studio.

For Windows, Microsoft's 
[Introduction to Razor](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-2.1&tabs=visual-studio)
article notes that the Visual Studio **ASP.NET and web development** 
and **.NET Core cross-platform development** workloads must be installed.

### Troubleshooting problems
If the solution doesn't start in Visual Studio, here are some suggestions:

* If you see core errors, update to the current version of Visual Studio
  and install the 
  [.NET Core 2.1 SDK](https://www.microsoft.com/net/download/thank-you/dotnet-sdk-2.1.403-windows-x64-installer).
  
  The free Visual Studio Community version can be used for this solution.
* More errors? Open the Visual Studio installer and install
  Visual Studio **ASP.NET and web development** 
  and **.NET Core cross-platform development** workloads.

### Configure the example files' settings
Each quick start example is a self-contained file. You will configure
each of the three example files listed above with:

 * **Access token:** Use the [OAuth Token Generator](https://developers.docusign.com/oauth-token-generator).
   To use the token generator, you'll need a
   [free DocuSign Developer's account.](https://go.docusign.com/o/sandbox/)

   Each access token lasts 8 hours, you will need to repeat this process
   when the token expires. You can use the same access token for
   multiple examples.

 * **Account Id:** After logging into the [DocuSign Sandbox system](https://demo.docusign.net),
   you can copy your Account Id from the dropdown menu by your name. See the figure:

   ![Figure](https://raw.githubusercontent.com/docusign/qs-csharp/master/documentation/account_id.png)
 * **Signer name and email:** Remember to try the DocuSign signing ceremony using both a mobile phone and a regular
   email client.

## Run the examples

Build and run the examples in Visual Studio. IIS Express will be automatically configured and started.

Your default browser will be automatically opened to the index page for the solution. 

## Support, Contributions, License

Submit support questions to [StackOverflow](https://stackoverflow.com). Use tag `docusignapi`.

Contributions via Pull Requests are appreciated.
All contributions must use the MIT License.

This repository uses the MIT license, see the
LICENSE file.

