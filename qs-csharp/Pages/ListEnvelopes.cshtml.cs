using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using static DocuSign.eSign.Api.EnvelopesApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace qs_csharp.Pages
{
    public class ListEnvelopesModel : PageModel
    {
        // Constants need to be set:
        private const string accessToken = "{ACCESS_TOKEN}";
        private const string accountId = "{ACCOUNT_ID}";
        private const int envelopesAgeDays = -10;

        // Additional constants
        private const string basePath = "https://demo.docusign.net/restapi";

        public void OnGet()
        {
            // List the user's envelopes created in the last 10 days
            // 1. Create request options
            // 2. Use the SDK to list the envelopes

            // 1. Create request options
            ListStatusChangesOptions options = new ListStatusChangesOptions();
            DateTime date = DateTime.Now.AddDays(envelopesAgeDays);
            options.fromDate = date.ToString("yyyy/MM/dd");

            // 2. Use the SDK to list the envelopes
            ApiClient apiClient = new ApiClient(basePath);
            apiClient.Configuration.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            EnvelopesApi envelopesApi = new EnvelopesApi(apiClient.Configuration);
            EnvelopesInformation results = envelopesApi.ListStatusChanges(accountId, options);

            // Prettyprint the results
            string json = JsonConvert.SerializeObject(results);
            string jsonFormatted = JValue.Parse(json).ToString(Formatting.Indented);
            ViewData["results"] = jsonFormatted;

            return;
        }
    }
}
