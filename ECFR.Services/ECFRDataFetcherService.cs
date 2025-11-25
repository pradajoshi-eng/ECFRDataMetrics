using ECFR.Core.Helpers;
using ECFR.Core.Models;
using ECFR.Data.EFModels;
using ECFR.Repositories;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace ECFR.Services
{
    public class ECFRDataFetcherService
    {
        private const string BaseUrl = "https://www.ecfr.gov";
        private readonly ECFRDataStoreRepository _repository;
        private readonly AgencyRepository _agencyRepository;

        public ECFRDataFetcherService()
        {
            _repository = new ECFRDataStoreRepository();
            _agencyRepository = new AgencyRepository();
        }

        /// <summary>
        /// get data for all federal regulation agencies from https://www.ecfr.gov/api/admin/v1/agencies.json
        /// and save it in a file named federal_regulation_agencies.json in the Data folder
        /// </summary>
        public void FetchAndStoreAllAgencies()
        {
            try
            {
                string apiEndpoint = "/api/admin/v1/agencies.json";
                var responseJson = HttpClientHelper.GetAsync($"{BaseUrl}{apiEndpoint}");
                var fileHelper = new FileHelper();
                fileHelper.SaveDatatoFile(responseJson, "federal_regulation_agencies.json");

                var agencies = JsonConvert.DeserializeObject<AgenciesModel>(responseJson);
                if (agencies != null && agencies.Agencies != null && agencies.Agencies.Count > 0)
                    InsertAgencyData(agencies.Agencies);
                else
                    throw new ApplicationException("No agency data found in the response from the API.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error downloading agency data: " + ex.Message, ex);
            }
        }

        private void InsertAgencyData(List<AgencyModel> agencies)
        {
            try
            {
                _repository.InsertAgencyData(agencies);
                //insert children agencies with parent agency id to the database 
                foreach (var agency in agencies)
                {
                    if (agency.Children != null && agency.Children.Count > 0)
                    {
                        InsertAgencyData(agency.Children);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error inserting agency data: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Fetches content from a specified URL, processes it, and stores a snapshot of the data associated with a
        /// given agency in the database.
        /// </summary>
        /// <remarks>If the agency specified by <paramref name="agencyId"/> does not exist in the
        /// database, it will be created. The method ensures that the content is fetched successfully from the <paramref
        /// name="sourceUrl"/> and processes the content to extract plain text and various metrics. It also computes the
        /// similarity and change density with the previous snapshot, if available.</remarks>
        /// <param name="agencyId">The unique identifier for the agency in the eCFR system.</param>
        /// <param name="displayName">The display name of the agency, used if the agency does not already exist in the database.</param>
        /// <param name="sourceUrl">The URL from which to fetch the content.</param>
        /// <returns>An <see cref="AgencySnapshotModel"/> containing the processed data and metadata about the fetched content.</returns>
        public AgencySnapshotModel FetchAndStoreAgencyRelatedData(int agencyId, string displayName, string sourceUrl)
        {
            // Ensure agency exists
            var agencies = _agencyRepository.GetAgencyData(agencyId);
            var agency = agencies.FirstOrDefault();
            if (agency == null)
            {
                 agency = new AgencyModel { AgencyId = agencyId, Name=displayName, Display_name = displayName };
                _repository.InsertAgencyData(new List<AgencyModel>() { agency });
            }

            // Fetch content (GET)
            var res = HttpClientHelper.GetAsync($"{BaseUrl}{sourceUrl}");

            // You may want to strip HTML to plain text; a simple approach below:
            var plain = HtmlToPlainText(res);

            var snapshot = new AgencySnapshotModel
            {
                AgencyId = agency.Id,
                RetrievedAt = DateTime.UtcNow,
                SourceUrl = sourceUrl,
                RawText = plain,
                RawBytes = Encoding.UTF8.GetByteCount(plain),
                WordCount = TextAnalyzer.WordCount(plain),
                UniqueTermCount = TextAnalyzer.UniqueTerms(plain).Count,
                Sha256Checksum = TextAnalyzer.Sha256Hex(plain),
                FleschReadingEase = TextAnalyzer.FleschReadingEase(plain),
            };

            // Compute relation to previous snapshot (if any)
            var prevSnapshot = _agencyRepository.GetAgencySnapshot(agency.Id);

            if (prevSnapshot != null)
            {
                var prevTokens = TextAnalyzer.UniqueTerms(prevSnapshot.RawText);
                var currTokens = TextAnalyzer.UniqueTerms(plain);
                snapshot.JaccardSimilarityWithPrevious = TextAnalyzer.Jaccard(prevTokens, currTokens);

                // change density: bytes changed per 1000 words
                var prevRawBytes = prevSnapshot.RawBytes ?? 0;
                var currRawBytes = snapshot.RawBytes ?? 0;
                var bytesChanged = Math.Abs((double)(currRawBytes - prevRawBytes));
                snapshot.ChangeDensity = snapshot.WordCount > 0 ? (bytesChanged / (double)snapshot.WordCount) * 1000.0 : 0;
            }
            else
            {
                snapshot.JaccardSimilarityWithPrevious = 1.0;
                snapshot.ChangeDensity = 0;
            }

            _repository.InsertAgencySnapshotData(snapshot);

            return snapshot;
        }

        // Primitive HTML stripper; consider using AngleSharp or HtmlAgilityPack for robust parsing
        private string HtmlToPlainText(string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;
            // remove scripts/styles
            var withoutScripts = Regex.Replace(html, @"<script[^>]*>[\s\S]*?</script>|<style[^>]*>[\s\S]*?</style>", "", RegexOptions.IgnoreCase);
            // remove tags
            var text = Regex.Replace(withoutScripts, @"<[^>]+>", " ");
            // decode HTML entities
            text = System.Net.WebUtility.HtmlDecode(text);
            // normalize whitespace
            return Regex.Replace(text, @"\s+", " ").Trim();
        }
    }
}
