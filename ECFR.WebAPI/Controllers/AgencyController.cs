using ECFR.Core.Models;
using ECFR.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECFR.WebApi.Controllers
{
    [ApiController]
    [Route("api/agency")]
    public class AgencyController : ControllerBase
    {

        private readonly ILogger<AgencyController> _logger;
        private readonly AgencyService _agencyService;

        public AgencyController(ILogger<AgencyController> logger, AgencyService agencyService)
        {
            _logger = logger;
            _agencyService = agencyService;
        }

        /// <summary>
        /// Get agency data from the federal_regulation_agencies.json file in the Data folder
        /// </summary>  
        [HttpGet("GetAgenciesData", Name = "GetAgenciesData")]
        public async Task<IActionResult> GetAgenciesData()
        {
            var data = _agencyService.GetAgencyData();
            var selectedDataOnly = data.Select(d => new
            {
                AgencyId = d.Id,
                Name = d.Name,
                Display_Name = d.Display_name,
                Titles = d.Cfr_references?.Select(t => t.Title).ToList()
            }).ToList();
            return Ok(selectedDataOnly);
        }

    }
}
