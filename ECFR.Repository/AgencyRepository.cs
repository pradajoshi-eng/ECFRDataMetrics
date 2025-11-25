using ECFR.Core.Models;
using ECFR.Data.EFModels;
using ECFR.Repositories.Mapper;
using Microsoft.EntityFrameworkCore;

namespace ECFR.Repositories
{
    public class AgencyRepository
    {
        /// <summary>
        /// Get agency related data and map to model from the database using Entity Framework or Dapper. 
        /// This method will read the data from the database, map it to a list of AgencyModel objects using the AgencyMapper, and return that list.
        /// </summary>        
        public List<AgencyModel> GetAgencyData(int? agencyId = null)
        {
            try
            {
                using (var context = new EcfrdbMdfContext())
                {
                    var agencyRawList = agencyId.HasValue?context.Agencies.Where(a => a.Id == agencyId.Value): context.Agencies;
                    if(!agencyRawList.Any() || agencyRawList.Count() == 0) { return null; }

                    var agencyMapper = new AgencyMapper();
                    var agencyModelList = agencyRawList.Select(a => agencyMapper.Map(a)).ToList();
                    // For each agency, get the related cfr_references and map them to CfrReferenceModel objects
                    foreach (var agencyModel in agencyModelList)
                    {
                        var cfrReferenceRelList = context.AgencyCfrReferenceRels.Where(r => r.AgencyId == agencyModel.Id);
                        if(!cfrReferenceRelList.Any() || cfrReferenceRelList.Count() == 0) { continue; }

                        var cfrReferenceIds = cfrReferenceRelList.Select(r => r.TitleId).ToList();
                        var cfrReferenceRawList = context.CfrReferences.Where(c => cfrReferenceIds.Contains(c.Id));
                        if(!cfrReferenceRawList.Any() || cfrReferenceRawList.Count() == 0) { continue; }

                        var cfrReferenceMapper = new CfrReferenceMapper();
                        agencyModel.Cfr_references = cfrReferenceRawList.Select(c => cfrReferenceMapper.Map(c)).ToList();
                    }
                    return agencyModelList;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while getting agency data: " + ex.Message);
            }
        }

        /// <summary>
        /// get snapshot for a given agency from the database
        /// sorted by RetrievedAt in descending order
        /// </summary> 
        public AgencySnapshotModel GetAgencySnapshot(int agencyId)
        {
            try
            {
                using (var context = new EcfrdbMdfContext())
                {
                    var snapshot = context.AgencySnapshots
                                    .Where(s => s.AgencyId == agencyId)
                                    .OrderByDescending(s => s.RetrievedAt)
                                    .FirstOrDefaultAsync();
                    if(snapshot.Result == null)
                    {
                        return null;
                    }
                    var agencySnapshotMapper = new AgencySnapshotMapper();
                    return agencySnapshotMapper.Map(snapshot.Result);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while getting snapshot: " + ex.Message);
            }
        }

        /// <summary>
        /// get snapshot from the database based on Id
        /// </summary> 
        public AgencySnapshotModel GetSnapshotById(int Id)
        {
            try
            {
                using (var context = new EcfrdbMdfContext())
                {
                    var snapshot = context.AgencySnapshots.FindAsync(Id);
                    if (snapshot == null)
                    {
                        return null;
                    }
                    var agencySnapshotMapper = new AgencySnapshotMapper();
                    return agencySnapshotMapper.Map(snapshot.Result);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while getting snapshot: " + ex.Message);
            }
        }

        /// <summary>
        /// get all snapshot for a given agency from the database
        /// sorted by RetrievedAt in descending order
        /// </summary> 
        public List<AgencySnapshotModel> GetAllSnapshots(int agencyId)
        {
            try
            {
                using (var context = new EcfrdbMdfContext())
                {
                    var snapshots = context.AgencySnapshots
                                    .Where(s => s.AgencyId == agencyId)
                                    .OrderByDescending(s => s.RetrievedAt);
                    if (!snapshots.Any() || snapshots.Count() == 0)
                    {
                        return null;
                    }
                    var agencySnapshotMapper = new AgencySnapshotMapper();
                    return snapshots.Select(s=> agencySnapshotMapper.Map(s)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while getting snapshots: " + ex.Message);
            }
        }

    }
}
