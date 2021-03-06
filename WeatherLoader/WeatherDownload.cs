﻿using System;
using System.Net;
using System.IO;
using WeatherShared;
using Serilog;

namespace WeatherLoader
{
    /// <summary>
    /// Download weather data from MetOffice
    /// </summary>
    public static class WeatherDownload
    {
        public static string filepath = "./";
        public static string filename = "country.txt";
        public static string baseURL = @"https://www.metoffice.gov.uk/pub/data/weather/uk/climate/datasets";
        static readonly ILogger Log = Logger.Initialize();

        /// <summary>
        /// Download the climate file based on region and climate type
        /// </summary>
        /// <param name="region">Name of region to be downloaded</param>
        /// <param name="climateType">Climate type that needs to be downloaded</param>
        /// <returns>Download status</returns>
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

                Log.Information($"{filename} got downloaded");
            }
            catch (Exception e)
            {
                Log.Error(e, " : error");
            }
            
            if (txtData != String.Empty)
                isDownloaded = true;

            return isDownloaded;
        }

        /// <summary>
        /// Download all the data from MetOffice
        /// </summary>
        /// <returns>Download status</returns>
        public static bool DownloadAll()
        {
            try
            {
                foreach (Region region in Enum.GetValues(typeof(Region)))
                    foreach (ClimateType ct in Enum.GetValues(typeof(ClimateType)))
                    {
                        bool isDownloaded = Download(region, ct);                        
                    }
            }
            catch (Exception e)
            {
                Log.Error(e, " : error");
            }

            return true;
        }

        /// <summary>
        /// Generate URL for MetOffice data based on region and climate type.
        /// </summary>
        /// <param name="region">Name of region to be downloaded</param>
        /// <param name="climateType">Climate type that needs to be downloaded</param>
        /// <returns>URL string of the data</returns>
        public static string getWebURL(Region region, ClimateType climateType)
        {
            string climateTypeURL = FileDownloadHelper.getTypeURLPart(climateType);
            string filename = FileDownloadHelper.getSiteFileName(region);
            return String.Concat(baseURL, "/" + climateTypeURL, "/date", "/" + filename);
        }
    }
}
