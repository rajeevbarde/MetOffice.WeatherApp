using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using WeatherORM;
using WeatherShared;

namespace WeatherLoader
{
    /// <summary>
    /// MetOffice data loader with high level operations
    /// </summary>
    public static class ClimateLoader
    {
        static readonly ILogger Log = Logger.Initialize();

        /// <summary>
        /// Reset entire database and Load data
        /// </summary>
        public static bool InitialLoad()
        {
            bool isLoaded = false;
            WeatherDB wdb = new WeatherDB();

            //Truncate tables
            wdb.TruncateTable("WeatherFileDownload");
            wdb.TruncateTable("WeatherData");

            //Reset WeatherFileDownload table
            wdb.ResetFileDownloadTable();

            //Download all data
            WeatherDownload.DownloadAll();

            //Insert or update data
            foreach (Region region in Enum.GetValues(typeof(Region)))
                foreach (ClimateType ct in Enum.GetValues(typeof(ClimateType)))
                {
                    var filename = FileDownloadHelper.getDBFileName(ct, region); // get filename from table based on region & climate
                    bool isLatestFile = WeatherParser.isLatestFile(filename, wdb); // Verify if downloaded file is latest

                    if (isLatestFile)
                    {
                        var list = WeatherParser.FormatWeatherData(filename, ct, region); // Parse the latest file into WeatherDataEntity

                        Log.Information($"\n Loading  : {ct} , {region}");
                        var count = wdb.BulkInsertOrUpdate(list); //Insert or Update data

                        Log.Information(count["insert"] + " inserted");
                        Log.Information(count["update"] + " updated");
                        Log.Information(count["nochange"] + " no change");

                        wdb.UpdateFileTimeStamp(filename);
                    }
                }            

            return isLoaded;
        }
    }
}
