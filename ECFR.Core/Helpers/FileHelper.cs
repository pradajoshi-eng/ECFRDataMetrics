using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECFR.Core.Helpers
{
    public class FileHelper
    {
        private readonly string BaseFolder;
        private string DataFolder => $"{BaseFolder}/Data";

        public FileHelper()
        {
            BaseFolder = AppContext.BaseDirectory;
        }

        /// <summary>
        /// method to save data to a file
        /// </summary>
        public void SaveDatatoFile(string data, string fileName)
        {
            try
            {
                System.IO.File.WriteAllText($"{DataFolder}/{fileName}", data);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error saving agency data: " + ex.Message);
            }
        }
    }
}
