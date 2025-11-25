using System;
using System.Collections.Generic;

namespace ECFR.Data.EFModels;

public partial class AgencySnapshot
{
    public int Id { get; set; }

    public int AgencyId { get; set; }

    public DateTime RetrievedAt { get; set; }

    public string? SourceUrl { get; set; }

    public string? RawText { get; set; }

    public long? RawBytes { get; set; }

    public int? WordCount { get; set; }

    public int? UniqueTermCount { get; set; }

    public string? Sha256Checksum { get; set; }

    public double? JaccardSimilarityWithPrevious { get; set; }

    public double? ChangeDensity { get; set; }

    public double? FleschReadingEase { get; set; }

    public virtual Agency Agency { get; set; } = null!;
}
