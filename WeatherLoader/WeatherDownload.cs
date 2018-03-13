using System;
using System.Net;
using System.IO;
using WeatherShared;

namespace WeatherLoader
{
    public static class WeatherDownload
    {
        public static string filepath = "./";
        public static string filename = "country.txt";
        public static string baseURL = @"https://www.metoffice.gov.uk/pub/data/weather/uk/climate/datasets";

        public static bool Download(Region region, ClimateType climateType)
        {
            string URL = getWebURL(region, climateType);
            bool isDownloaded = false;
            string txtData = String.Empty;

            try
            {
                using (var webClient = new WebClient())
                {
                    txtData = webClient.DownloadString(URL);
                }

                filename = FileDownloadHelper.getDBFileName(climateType, region);
                File.WriteAllText(filepath + filename, txtData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            if (txtData != String.Empty)
                isDownloaded = true;

            return isDownloaded;
        }

        public static bool DownloadAll()
        {
            try
            {
                foreach (Region region in Enum.GetValues(typeof(Region)))
                    foreach (ClimateType ct in Enum.GetValues(typeof(ClimateType)))
                    {
                        bool isDownloaded = Download(region, ct);
                        Console.WriteLine(region.ToString() + " " + ct.ToString() + ":" + isDownloaded);
                    }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public static string getWebURL(Region region, ClimateType climateType)
        {
            string climateTypeURL = FileDownloadHelper.getTypeURLPart(climateType);
            string filename = FileDownloadHelper.getSiteFileName(region);
            return String.Concat(baseURL, "/" + climateTypeURL, "/date", "/" + filename);
        }
    }
}
