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
    public class SendEnvelopeModel : PageModel
    {
        // Constants need to be set:
        private const string accessToken = "";
        private const string accountId = "";
        private const string signerName = "Jim Smith";
        private const string signerEmail = "larry@example.com";
        private const string docName = "World_Wide_Corp_lorem.pdf";

        // Additional constants
        private const string basePath = "https://demo.docusign.net/restapi";

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Send envelope. Signer's request to sign is sent by email.
            // 1. Create envelope request obj
            // 2. Use the SDK to create and send the envelope

            // 1. Create envelope request object
            //    Start with the different components of the request
            Document document = new Document
            {
                DocumentBase64 = Convert.ToBase64String(ReadContent(docName)),
                Name = "Lorem Ipsum",
                FileExtension = "pdf",
                DocumentId = "1"
            };
            Document[] documents = new Document[] { document };

            SignHere signHereTab = new SignHere
            {
                DocumentId = "1",
                PageNumber = "1",
                RecipientId = "1",
                TabLabel = "Sign Here Tab",
                XPosition = "195",
                YPosition = "147"
            };
            SignHere[] signHereTabs = new SignHere[] { signHereTab };

            Signer signer = new Signer
            {
                Email = signerEmail,
                Name = signerName,
                RecipientId = "1",
                RoutingOrder = "1",
                Tabs = new Tabs { SignHereTabs = new List<SignHere>(signHereTabs) }
            };
            Signer[] signers = new Signer[] { signer };
            Recipients recipients = new Recipients { Signers = new List<Signer>(signers) };

            // Bring them together in the EnvelopeDefinition
            EnvelopeDefinition envelopeDefinition = new EnvelopeDefinition
            {
                EmailSubject = "Please sign the document",
                Documents = new List<Document>(documents),
                Recipients = recipients,
                Status = "sent"
            };

            // 2. Use the SDK to create and send the envelope
            ApiClient apiClient = new ApiClient(basePath);
            apiClient.Configuration.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            EnvelopesApi envelopesApi = new EnvelopesApi(apiClient.Configuration);
            EnvelopeSummary results = envelopesApi.CreateEnvelope(accountId, envelopeDefinition);
            ViewData["results"] = $"Envelope status: {results.Status}. Envelope ID: {results.EnvelopeId}";

            return new PageResult();
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