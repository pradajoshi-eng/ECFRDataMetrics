using ECFR.Core.Helpers;
using ECFR.Core.Models;
using ECFR.Data.EFModels;
using ECFR.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECFR.WebApi.Controllers
{
    [ApiController]
    [Route("api/metrics")]
    public class MetricsController : ControllerBase
    {

        private readonly ILogger<MetricsController> _logger;
        private readonly AgencyMetricsService _metricsService;

        public MetricsController(ILogger<MetricsController> logger, AgencyMetricsService agencyMetricsService)
        {
            _logger = logger;
            _metricsService = agencyMetricsService;
        }

        // Get latest metrics for an agency
        [HttpGet("agency/{agencyId}/latest")]
        public async Task<IActionResult> GetLatest(int agencyId)
        {
            var snap = _metricsService.GetAgencySnapshotData(agencyId);
            if (snap != null)
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
            else
                return Ok($"No snapshots found for Agency with Id: {agencyId}");
        }

        // Timeseries: return snapshots between dates
        [HttpGet("agency/{agencyId}/series")]
        public async Task<IActionResult> GetSeries(int agencyId, [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            var snapshots = _metricsService.GetAllSnapshotsData(agencyId);
            if (!snapshots.Any() || snapshots.Count == 0) return Ok($"No snapshots found for Agency with Id: {agencyId}");
            if (from.HasValue) snapshots = snapshots.Where(s => s.RetrievedAt >= from.Value).ToList();
            if (to.HasValue) snapshots = snapshots.Where(s => s.RetrievedAt <= to.Value).ToList();
            var list = snapshots.OrderBy(s => s.RetrievedAt).Select(s => new
            {
                s.Id,
                s.RetrievedAt,
                s.WordCount,
                s.UniqueTermCount,
                s.Sha256Checksum,
                s.JaccardSimilarityWithPrevious,
                s.ChangeDensity
            }).ToList();
            return Ok(list);
        }

        // Diff summary between two snapshots
        [HttpGet("snapshot/diff")]
        public async Task<IActionResult> Diff([FromQuery] int leftId, [FromQuery] int rightId)
        {

            var left = _metricsService.GetAgencySnapshotbyIdData(leftId);
            var right = _metricsService.GetAgencySnapshotbyIdData(rightId);
            if (left == null || right == null) return NotFound();

            // Basic differences
            var leftTokens = TextAnalyzer.UniqueTerms(left.RawText);
            var rightTokens = TextAnalyzer.UniqueTerms(right.RawText);
            var added = rightTokens.Except(leftTokens).Take(200).ToArray();
            var removed = leftTokens.Except(rightTokens).Take(200).ToArray();

            return Ok(new
            {
                leftId = left.Id,
                rightId = right.Id,
                leftRetrievedAt = left.RetrievedAt,
                rightRetrievedAt = right.RetrievedAt,
                leftWordCount = left.WordCount,
                rightWordCount = right.WordCount,
                addedCount = added.Length,
                removedCount = removed.Length,
                addedSample = added,
                removedSample = removed,
                jaccard = TextAnalyzer.Jaccard(leftTokens, rightTokens)
            });
        }
    }
}