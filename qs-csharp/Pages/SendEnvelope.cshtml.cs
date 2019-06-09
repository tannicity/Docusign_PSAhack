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
            //    Create the document object
            SendEnvelopeModel pageInfo = new SendEnvelopeModel
            {
                signerName = "Tanny Ng",
                signerEmail = "zoomzoom.tmd@gmail.com",
                signerId = "1000",
                signerRouting = "1",
                accessToken = "eyJ0eXAiOiJNVCIsImFsZyI6IlJTMjU2Iiwia2lkIjoiNjgxODVmZjEtNGU1MS00Y2U5LWFmMWMtNjg5ODEyMjAzMzE3In0.AQoAAAABAAUABwAAxWPmeezWSAgAAAWH9Lzs1kgCAC5WseC6Pj1BpkJ7B9-e_VQVAAEAAAAYAAEAAAAFAAAADQAkAAAAZjBmMjdmMGUtODU3ZC00YTcxLWE0ZGEtMzJjZWNhZTNhOTc4EgABAAAACwAAAGludGVyYWN0aXZlMAAAxWPmeezWSDcA5MmzWuy8K0uaejoXWPZBcw.S5mtjFB8Cbr4JnB18EWMetaq0qcEcXX17rTl59iBl3dAsOwl-un-6a4xmCRB1Py81pit-tHpBf3CpgwHNfbBF5zCVIpCXWzFYIN4ySmG2YEDXu45edHuQhdya-DeZHGktH_19oJoSeRyN70mKXT7F5KmpqPVWW_9hC8bQ361fojkvC7F2hnbtoG0NDESq2I_suZoOBx0Ndv8oYSwpS65DT420rcQpSQsxBa5GdUt42zTlnJ5JwRsMUIzOCwY8yXfLjt0kAPeuiZbQ8E0NrRCqnMieR0ghAUwi4VC-LWn8NgikOyN2m74K0iTmnpfAMDayheM9qO3aEvT2ejPtK_jmw"
            };

            Document document = new Document
            {
                DocumentBase64 = Convert.ToBase64String(ReadContent(docName)),
                Name = "Petition_v1",
                FileExtension = "pdf",
                DocumentId = docId
            };
            Document[] documents = new Document[] { document };

            // Create the signer recipient object 
            Signer signer = new Signer
            {
                Email = pageInfo.signerEmail,
                Name = pageInfo.signerName,
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
            {
                EmailSubject = "Please sign the document",
                Documents = new List<Document>(documents),
                Recipients = recipients,
                Status = "sent"
            };

            // 2. Use the SDK to create and send the envelope
            ApiClient apiClient = new ApiClient(basePath);
            apiClient.Configuration.AddDefaultHeader("Authorization", "Bearer " + pageInfo.accessToken);
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
