using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherShared
{
    /// <summary>
    /// A helper class for files containing climate data.
    /// </summary>
    public static class FileDownloadHelper
    {
        /// <summary>
        /// Gets name of the file based on the region.
        /// </summary>
        /// <param name="region">Enum type which contain regions are part of the project</param>
        /// <returns>file name for the region</returns>
        public static string getSiteFileName(Region region)
        {
            string filename = "";

            if (region == Region.UK) filename = "UK.txt";
            else if (region == Region.England) filename = "England.txt";
            else if (region == Region.Scotland) filename = "Scotland.txt";
            else if (region == Region.Wales) filename = "Wales.txt";

            return filename;
        }

        /// <summary>
        /// Gets the part of URL based on the climate type.
        /// </summary>
        /// <param name="climateType">Enum type which contain climate tpyes are part of the project</param>
        /// <returns>URL part for the climate</returns>
        public static string getTypeURLPart(ClimateType climateType)
        {
            string climateTypeURLPart = "";

            if (climateType == ClimateType.max) climateTypeURLPart = "Tmax";
            else if (climateType == ClimateType.min) climateTypeURLPart = "Tmin";
            else if (climateType == ClimateType.mean) climateTypeURLPart = "Tmean";
            else if (climateType == ClimateType.rainfall) climateTypeURLPart = "Rainfall";
            else if (climateType == ClimateType.sunshine) climateTypeURLPart = "Sunshine";

            return climateTypeURLPart;
        }

        /// <summary>
        /// Gets name of the file based on the region and climate which is stored in database.
        /// </summary>
        /// <param name="climateType">Enum type which contain climate tpyes are part of the project</param>
        /// <param name="region">Enum type which contain regions are part of the project</param>
        /// <returns>file name for the region and climate type</returns>
        public static string getDBFileName(ClimateType climateType, Region region)
        {
            return getTypeURLPart(climateType) + "_" + getSiteFileName(region);
        }

    }
}
