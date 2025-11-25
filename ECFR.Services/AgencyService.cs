using ECFR.Core.Models;
using ECFR.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECFR.Services
{
    public class AgencyService
    {
        private readonly AgencyRepository _repository;
        public AgencyService()
        {
            _repository = new AgencyRepository();
        }
        /// <summary>
        /// get data for all agencies from through repository and return a list of AgencyModel objects
        /// </summary>
        public List<AgencyModel> GetAgencyData()
        {
            try
            {
                return _repository.GetAgencyData();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting agency data: " + ex.Message);
            }
        }

    }
}
