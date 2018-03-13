using System;
using System.Collections.Generic;
using WeatherLoader;
using WeatherORM;
using WeatherShared;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {

            WeatherDownload.DownloadAll();

            WeatherDB wdb = new WeatherDB();
            //wdb.ResetFileDownloadTable();

            foreach (Region region in Enum.GetValues(typeof(Region)))
                foreach (ClimateType ct in Enum.GetValues(typeof(ClimateType)))
                {
                    var filename = FileDownloadHelper.getDBFileName(ct, region);
                    bool isNewFile = WeatherParser.isNewFile(filename, wdb);

                    if (isNewFile)
                    {
                        var list = WeatherParser.FormatWeatherData(filename, ct, region);
                        var count = wdb.BulkInsertOrUpdate(list);
                        wdb.UpdateFileTimeStamp(filename);
                    }
                }

            //update the file table with timestamp
            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
