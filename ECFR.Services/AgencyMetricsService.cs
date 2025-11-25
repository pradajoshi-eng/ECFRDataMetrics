using ECFR.Core.Models;
using ECFR.Repositories;

namespace ECFR.Services
{
    public class AgencyMetricsService
    {

        private readonly AgencyRepository _repository;
        public AgencyMetricsService()
        {
            _repository = new AgencyRepository();
        }

        /// <summary>
        /// get agency snapshot from repository and return AgencySnapshotModel 
        /// </summary>
        /// <param name="agencyId"></param>
        /// <returns>AgencySnapshotModel</returns>
        /// <remarks>Throws ApplicationException if there is an error getting agency data.</remarks>
        public AgencySnapshotModel GetAgencySnapshotData(int agencyId)
        {
            try
            {
                return _repository.GetAgencySnapshot(agencyId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting agency data: " + ex.Message);
            }
        }
        
        /// <summary>
        /// get all snapshots for given agency from repository and return list of AgencySnapshotModel 
        /// </summary>
        /// <param name="agencyId"></param>
        /// <returns>List<AgencySnapshotModel></returns>
        /// <remarks>Throws ApplicationException if there is an error getting agency data.</remarks>
        public List<AgencySnapshotModel> GetAllSnapshotsData(int agencyId)
        {
            try
            {
                return _repository.GetAllSnapshots(agencyId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting agency data: " + ex.Message);
            }
        }

        /// <summary>
        /// get agency snapshot from repository based on id and return AgencySnapshotModel 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>AgencySnapshotModel</returns>
        /// <remarks>Throws ApplicationException if there is an error getting agency data.</remarks>
        public AgencySnapshotModel GetAgencySnapshotbyIdData(int id)
        {
            try
            {
                return _repository.GetSnapshotById(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting agency data: " + ex.Message);
            }
        }


    }
}
