using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;


namespace qs_csharp.Pages
{
    public class EmbeddedSigning : PageModel
    {
        // Constants need to be set:
        private const string accessToken = "{ACCESS_TOKEN}";
        private const string accountId = "{ACCOUNT_ID}";
        private const string signerName = "{USER_FULLNAME}";
        private const string signerEmail = "{USER_EMAIL}";
        private const string docName = "World_Wide_Corp_lorem.pdf";

        // Additional constants
        private const string signerClientId = "1000";
        private const string basePath = "https://demo.docusign.net/restapi";

        // Change the port number in the Properties / launchSettings.json file:
        //     "iisExpress": {
        //        "applicationUrl": "http://localhost:5050",
        private const string returnUrl = "http://localhost:5050/DSReturn";

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Embedded Signing Ceremony
            // 1. Create envelope request obj
            // 2. Use the SDK to create and send the envelope
            // 3. Create Envelope Recipient View request obj
            // 4. Use the SDK to obtain a Recipient View URL
            // 5. Redirect the user's browser to the URL

            // 1. Create envelope request object
            //    Start with the different components of the request
            Document document = new Document
            { DocumentBase64 = Convert.ToBase64String(ReadContent(docName)),
              Name = "Lorem Ipsum", FileExtension = "pdf", DocumentId = "1"
            };
            Document[] documents = new Document[] { document };

            SignHere signHereTab = new SignHere
            { DocumentId = "1", PageNumber = "1", RecipientId = "1",
                TabLabel = "Sign Here Tab", XPosition = "195", YPosition = "147"
            };
            SignHere[] signHereTabs = new SignHere[] { signHereTab };

            Signer signer = new Signer
            { Email = signerEmail, Name = signerName, ClientUserId = signerClientId,
              RecipientId = "1", RoutingOrder = "1",
              Tabs = new Tabs { SignHereTabs = new List<SignHere>(signHereTabs) }
            };
            Signer[] signers = new Signer[] { signer };
            Recipients recipients = new Recipients { Signers = new List<Signer>(signers) };

            // Bring them together in the EnvelopeDefinition
            EnvelopeDefinition envelopeDefinition = new EnvelopeDefinition
            { EmailSubject = "Please sign the document",
              Documents = new List<Document>( documents ),
              Recipients = recipients,
              Status = "sent"
            };

            // 2. Use the SDK to create and send the envelope
            ApiClient apiClient = new ApiClient(basePath);
            apiClient.Configuration.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            EnvelopesApi envelopesApi = new EnvelopesApi(apiClient.Configuration);
            EnvelopeSummary results = envelopesApi.CreateEnvelope(accountId, envelopeDefinition);

            // 3. Create Envelope Recipient View request obj
            string envelopeId = results.EnvelopeId;
            RecipientViewRequest viewOptions = new RecipientViewRequest
            { ReturnUrl = returnUrl, ClientUserId = signerClientId,
              AuthenticationMethod = "none",
              UserName = signerName, Email = signerEmail
            };

            // 4. Use the SDK to obtain a Recipient View URL
            ViewUrl viewUrl = envelopesApi.CreateRecipientView(accountId, envelopeId, viewOptions);

            // 5. Redirect the user's browser to the URL
            return Redirect(viewUrl.Url);
        }

        /// <summary>
        /// This method read bytes content from the project's Resources directory
        /// </summary>
        /// <param name="fileName">resource path</param>
        /// <returns>return bytes array</returns>
        internal static byte[] ReadContent(string fileName)
        {
            byte[] buff = null;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    long numBytes = new FileInfo(path).Length;
                    buff = br.ReadBytes((int)numBytes);
                }
            }
            return buff;
        }
    }
}
