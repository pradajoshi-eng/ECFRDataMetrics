using ECFR.Core.Models;
using ECFR.Data.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECFR.Repositories.Mapper
{
    public class AgencyMapper : IMapper<AgencyModel, Agency>
    {
        public Agency Map(AgencyModel source)
        {
            return new Agency
            {
                Id = source.Id,
                Name = source.Name,
                ShortName = source.Short_name,
                DisplayName = source.Display_name,
                SortableName = source.Sortable_name,
                Slug = source.Slug,
                InverseAgencyNavigation = source.Agencies != null ? source.Agencies.Select(a => Map(a)).ToList() : new List<Agency>(),
                //CfrReferences = source.Cfr_references != null ? source.Cfr_references.Select(c => new CfrReferenceRaw
                //{
                //    Title = c.Title,
                //    Chapter = c.Chapter
                //}).ToList() : new List<CfrReferenceRaw>(),
                AgencySnapshots = source.Snapshots != null ? 
                                    source.Snapshots.Select(s => new AgencySnapshotMapper().Map(s)).ToList() : 
                                    new List<AgencySnapshot>(),

            };
        }

        public AgencyModel Map(Agency destination)
        {
            return new AgencyModel
            {
                Id = destination.Id,
                Name = destination.Name,
                Short_name = destination.ShortName,
                Display_name = destination.DisplayName,
                Sortable_name = destination.SortableName,
                Slug = destination.Slug,
                Agencies = destination.InverseAgencyNavigation != null ? destination.InverseAgencyNavigation.Select(a => Map(a)).ToList() : new List<AgencyModel>(),
                //Cfr_references = destination.CfrReferences != null ? destination.CfrReferences.Select(c => new CfrReferenceModel
                //{
                //    Title = c.Title,
                //    Chapter = c.Chapter
                //}).ToList() : new List<CfrReferenceModel>(),
                Snapshots = destination.AgencySnapshots != null ? 
                            destination.AgencySnapshots.Select(s => new AgencySnapshotMapper().Map(s)).ToList() : 
                            new List<AgencySnapshotModel>(),
            };
        }
    }
}
