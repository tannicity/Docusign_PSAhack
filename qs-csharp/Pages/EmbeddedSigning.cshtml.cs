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
        private string accessToken { get; set; }
        private string signerName { get; set; }
        private string signerEmail { get; set; }
        private string signerId { get; set; }
        private string signerRouting { get; set; }

        // Constants need to be set:
        private const string docName = "World_Wide_Corp_lorem.pdf";
        private const string docId = "1";

        // Additional constants
        private const string accountId = "8503012";
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
            //    Create the document object
            EmbeddedSigning pageInfo = new EmbeddedSigning
            {
                signerName = "Tanny Ng",
                signerEmail = "zoomzoom.tmd@gmail.com",
                signerId = "1000",
                signerRouting = "1",
                accessToken = "eyJ0eXAiOiJNVCIsImFsZyI6IlJTMjU2Iiwia2lkIjoiNjgxODVmZjEtNGU1MS00Y2U5LWFmMWMtNjg5ODEyMjAzMzE3In0.AQoAAAABAAUABwCALdLYwezWSAgAgG315gTt1kgCAC5WseC6Pj1BpkJ7B9-e_VQVAAEAAAAYAAkAAAAFAAAAKwAAAC0AAAAvAAAAMQAAADIAAAA4AAAAMwAAADUAAAANACQAAABmMGYyN2YwZS04NTdkLTRhNzEtYTRkYS0zMmNlY2FlM2E5NzgSAAEAAAALAAAAaW50ZXJhY3RpdmUwAIAAodfB7NZINwDkybNa7LwrS5p6OhdY9kFz.LHHS7H4GTayO31-USuesGb--00NMcrOqO0KnzoKMhM55ClXR2vw2OzKShqI3yhIjHc0CyGoyOtNrEW0MN0o8rRZuctb5hNtR9RAtbuNZ-hITpjyL9LBFZWxV91dYAmlgrBAcM2LtrZTWHolkqGLUNQMpD_vI8BqqT3UEO9zBL5OUz4WwSZgBoCmdejMVq-zOq-ALPpD6YoX0HoHiZHVl4_DTwTiJ_lB6I3z72fh3-i6f7iD_kaJyc2nA5jAtRXVGvX_gNUPhA4aDIZ8tJ8TTZW9hbgQ2BkFlwm69xqvRjMUuQlg6xnfW7vlvtI6tQ7GAUlVhQS5KX9FveUDYogidQw"
            };

            Document document = new Document
            { DocumentBase64 = Convert.ToBase64String(ReadContent(docName)),
              Name = "Petition Sample", FileExtension = "pdf", DocumentId = docId
            };
            Document[] documents = new Document[] { document };

            // Create the signer recipient object 
            Signer signer = new Signer
            {
                Email = pageInfo.signerEmail,
                Name = pageInfo.signerName,
                ClientUserId = pageInfo.signerId,
                RecipientId = pageInfo.signerId,
                RoutingOrder = pageInfo.signerRouting
            };

            // Create the sign here tab (signing field on the document)
            SignHere signHereTab = new SignHere
            {
                DocumentId = docId,
                PageNumber = "1",
                RecipientId = pageInfo.signerId,
                TabLabel = "Sign Here Tab",
                XPosition = "195",
                YPosition = "147"
            };
            SignHere[] signHereTabs = new SignHere[] { signHereTab };

            // Add the sign here tab array to the signer object.
            signer.Tabs = new Tabs { SignHereTabs = new List<SignHere>(signHereTabs) };
            // Create array of signer objects
            Signer[] signers = new Signer[] { signer };
            // Create recipients object
            Recipients recipients = new Recipients { Signers = new List<Signer>(signers) };
            // Bring the objects together in the EnvelopeDefinition
            EnvelopeDefinition envelopeDefinition = new EnvelopeDefinition
            { EmailSubject = "Please sign the document",
              Documents = new List<Document>( documents ),
              Recipients = recipients,
              Status = "sent"
            };

            // 2. Use the SDK to create and send the envelope
            ApiClient apiClient = new ApiClient(basePath);
            apiClient.Configuration.AddDefaultHeader("Authorization", "Bearer " + pageInfo.accessToken);
            EnvelopesApi envelopesApi = new EnvelopesApi(apiClient.Configuration);
            EnvelopeSummary results = envelopesApi.CreateEnvelope(accountId, envelopeDefinition);

            // 3. Create Envelope Recipient View request obj
            string envelopeId = results.EnvelopeId;
            RecipientViewRequest viewOptions = new RecipientViewRequest
            { ReturnUrl = returnUrl, ClientUserId = pageInfo.signerId,
              AuthenticationMethod = "none",
              UserName = pageInfo.signerName, Email = pageInfo.signerEmail
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
