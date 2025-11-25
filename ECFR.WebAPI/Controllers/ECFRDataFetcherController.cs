using ECFR.Core.Models;
using ECFR.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECFR.WebApi.Controllers
{
    [ApiController]
    [Route("api/datafetcher")]
    public class ECFRDataFetcherController : ControllerBase
    {
        private readonly ILogger<ECFRDataFetcherController> _logger;
        private readonly ECFRDataFetcherService _dataFetcherService;
        private readonly AgencyService _agencyService;

        public ECFRDataFetcherController(ILogger<ECFRDataFetcherController> logger, ECFRDataFetcherService eCFRDataFetcherService, AgencyService agencyService)
        {
            _logger = logger;
            _dataFetcherService = eCFRDataFetcherService;
            _agencyService = agencyService;
        }

        /// <summary>
        /// Fetch data for all federal regulation agencies from  https://www.ecfr.gov/api/admin/v1/agencies.json
        /// and save it in a file named federal_regulation_agencies.json in the Data folder
        /// </summary>
        [HttpPost("FetchAndStoreAllAgencies", Name = "FetchAndStoreAllAgencies")]
        public void FetchAndStoreAllAgencies()
        {
            _dataFetcherService.FetchAndStoreAllAgencies();
        }
        // Trigger fetch for a single agency by providing an eCFR source URL
        [HttpPost("FetchAndStoreAllAgencyDataSnapshot")]
        public async Task<IActionResult> FetchAndStoreAllAgencySnapshotData()
        {

            var data = _agencyService.GetAgencyData();
            Dictionary<int, string> failedAgencies = new Dictionary<int, string>();
            foreach (var agency in data)
            {
                FetchRequest req = new FetchRequest
                {
                    AgencyId = agency.Id,
                    DisplayName = agency.Display_name,
                    Title = agency.Cfr_references != null && agency.Cfr_references.Count > 0 ? agency.Cfr_references[0].Title.ToString() : string.Empty
                };
                foreach(var title in agency.Cfr_references)
                {
                    req.Title = title.Title.ToString();
                    string date = (DateTime.Parse(req.Date)).ToString("yyyy-dd-MM");
                    date = "2025-03-11";
                    // req should contain ecfrAgencyId, displayName, sourceUrl
                    string sourceUrl = $"/api/renderer/v1/content/enhanced/{date}/title-{req.Title}";
                    if (!string.IsNullOrEmpty(title.Chapter))
                        sourceUrl += $"?chapter={title.Chapter}";
                    var snap = _dataFetcherService.FetchAndStoreAgencyRelatedData(req.AgencyId, req.DisplayName, sourceUrl);
                    if (snap == null)
                    {
                       failedAgencies.Add(req.AgencyId, req.Title);
                    }
                }
            }
            if(failedAgencies.Count>0)
            {
                return BadRequest(new
                {
                    Message = "Failed to fetch and store agency data snapshots for some agencies.",
                    FailedAgencies = failedAgencies
                });
            }
            return Ok("Request complete.");
            
        }

        // Trigger fetch for a single agency by providing an eCFR source URL
        [HttpPost("FetchAndStoreAgencyDataSnapshot")]
        public async Task<IActionResult> FetchAndStoreAgencyData([FromBody] FetchRequest req)
        {
            // req should contain ecfrAgencyId, displayName, sourceUrl
            string date = (DateTime.Parse(req.Date)).ToString("yyyy-dd-MM");
            string sourceUrl = $"/api/renderer/v1/content/enhanced/{date}/title-{req.Title}";
            //if additionalparams is provided, append to sourceUrl
            if (!string.IsNullOrEmpty(req.additionalQSParams))
            {
                sourceUrl +=$"?{req.additionalQSParams}";
            }            
            
            var snap = _dataFetcherService.FetchAndStoreAgencyRelatedData(req.AgencyId, req.DisplayName, sourceUrl);
            if(snap==null)
            {
                return BadRequest("Failed to fetch and store agency data snapshot.");
            }
            return Ok(new
            {
                snap.Id,
                snap.RetrievedAt,
                snap.WordCount,
                snap.UniqueTermCount,
                snap.Sha256Checksum,
                snap.JaccardSimilarityWithPrevious,
                snap.ChangeDensity,
                snap.FleschReadingEase
            });
        }

    }

    public class FetchRequest
    {
        public int AgencyId { get; set; }
        public string DisplayName { get; set; }
        /// <summary>
        /// by detault, use today's date in yyyy-MM-dd format
        /// </summary>
        public string Date { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
        /// <summary>
        /// Title number in the eCFR system
        /// </summary>
        public string Title { get; set; }
        public string? additionalQSParams { get; set; }
    }
}
