using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherShared
{
    public static class FileDownloadHelper
    {
        public static string getSiteFileName(Region region)
        {
            string filename = "";
            if (region == Region.UK)
                filename = "UK.txt";
            else if (region == Region.England)
                filename = "England.txt";
            else if (region == Region.Scotland)
                filename = "Scotland.txt";
            else if (region == Region.Wales)
                filename = "Wales.txt";

            return filename;
        }

        public static string getTypeURLPart(ClimateType climateType)
        {
            string climateTypeURLPart = "";
            if (climateType == ClimateType.max)
                climateTypeURLPart = "Tmax";
            else if (climateType == ClimateType.min)
                climateTypeURLPart = "Tmin";
            else if (climateType == ClimateType.mean)
                climateTypeURLPart = "Tmean";
            else if (climateType == ClimateType.rainfall)
                climateTypeURLPart = "Rainfall";
            else if (climateType == ClimateType.sunshine)
                climateTypeURLPart = "Sunshine";

            return climateTypeURLPart;
        }

        public static string getDBFileName(ClimateType climateType, Region region)
        {
            return getTypeURLPart(climateType) + "_" + getSiteFileName(region);
        }

    }
}
