using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECFR.Core.Helpers
{
    public class HttpClientHelper
    {
        /// <summary>
        /// get data from a given uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetAsync(string uri)
        {
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    throw new ApplicationException($"Unable to get response. Response Code: {response.StatusCode}.");
                }
            }
        }
    }
}
