namespace ECFR.Core.Models
{
    public class AgencySnapshotModel
    {
        public int Id { get; set; }
        public int AgencyId { get; set; }
        public AgencyModel Agency { get; set; }

        // Data captured from eCFR content
        public DateTime RetrievedAt { get; set; } // when we fetched
        public string? SourceUrl { get; set; }     // the eCFR endpoint used
        public string? RawText { get; set; }       // full plain text content (or trimmed)
        public long? RawBytes { get; set; }        // bytes length of RawText (UTF8)

        // For data Analysis
        public int? WordCount { get; set; }
        public int? UniqueTermCount { get; set; }
        public string? Sha256Checksum { get; set; } // checksum of RawText
        public double? JaccardSimilarityWithPrevious { get; set; } // custom / computed later
        public double? ChangeDensity { get; set; } // bytes changed per 1000 words (custom)
        public double? FleschReadingEase { get; set; } // readability score (optional)

    }
}