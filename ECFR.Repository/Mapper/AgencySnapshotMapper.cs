using ECFR.Core.Models;
using ECFR.Data.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECFR.Repositories.Mapper
{
    public class AgencySnapshotMapper : IMapper<AgencySnapshotModel, AgencySnapshot>
    {
        public AgencySnapshot Map(AgencySnapshotModel source)
        {
            return new AgencySnapshot
            {

                Id = source.Id,
                RetrievedAt = source.RetrievedAt,
                SourceUrl = source.SourceUrl,
                AgencyId = source.AgencyId,
                Agency = source.Agency != null ? new AgencyMapper().Map(source.Agency) : null,
                ChangeDensity = source.ChangeDensity,
                FleschReadingEase = source.FleschReadingEase,
                JaccardSimilarityWithPrevious = source.JaccardSimilarityWithPrevious,
                RawBytes = source.RawBytes,
                RawText = source.RawText,
                Sha256Checksum = source.Sha256Checksum,
                WordCount = source.WordCount,
            };
        }

        public AgencySnapshotModel Map(AgencySnapshot destination)
        {
            return new AgencySnapshotModel
            {
                Id = destination.Id,
                RetrievedAt = destination.RetrievedAt,
                SourceUrl = destination.SourceUrl,
                AgencyId = destination.AgencyId,
                Agency = destination.Agency != null ? new AgencyMapper().Map(destination.Agency) : null,
                ChangeDensity = destination.ChangeDensity ?? 0, 
                FleschReadingEase = destination.FleschReadingEase ?? 0, 
                JaccardSimilarityWithPrevious = destination.JaccardSimilarityWithPrevious ?? 0, 
                RawBytes = destination.RawBytes ?? 0, 
                RawText = destination.RawText,
                Sha256Checksum = destination.Sha256Checksum,
                WordCount = destination.WordCount ?? 0, 
            };
        }
    }
}
