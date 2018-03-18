using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using WeatherORM;
using WeatherShared;

namespace WeatherLoader
{
    /// <summary>
    /// Parse the Weather's data from the downloaded file
    /// </summary>
    public static class WeatherParser
    {
        /// <summary>
        /// Verify if the downloaded file is latest or old.
        /// </summary>
        /// <param name="fileName">Name of the file which is verified</param>
        /// <param name="wdb">The database object. It is needed for persistent connection</param>
        /// <returns>result of file verification</returns>
        public static bool isLatestFile(string fileName, WeatherDB wdb)
        {
            IEnumerable<string> TextLines = File.ReadLines(WeatherDownload.filepath + fileName);

            foreach (var line in TextLines)
            {
                if (line.Contains("Last updated"))
                {
                    DateTime dateLatest = getDateFromFile(line);
                    DateTime dateDB = wdb.getFileTimeStamp(fileName);

                    if (dateLatest.Date > dateDB.Date)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        private static DateTime getDateFromFile(string line)
        {
            string[] textSplit = line.Split(" ");
            string dateDirty = (textSplit[textSplit.Length - 1]);
            string dateClean = dateDirty.Remove(dateDirty.Length - 1);

            IFormatProvider formatProvider = new CultureInfo("en-GB");
            DateTime date = DateTime.Parse(dateClean, formatProvider);

            return date;

        }

        /// <summary>
        /// Format the weather data from text file in class object.
        /// </summary>
        /// <param name="fileName">Name of the file which needs to be formatted</param>
        /// <param name="ct">The climate type filter</param>
        /// <param name="region">The Region filter</param>
        /// <returns>List of data in WeatherDataEntity</returns>
        public static List<WeatherDataEntity> FormatWeatherData(string fileName,ClimateType ct, Region region)
        {
            IEnumerable<string> TextLines = File.ReadLines(WeatherDownload.filepath + fileName);
            List<WeatherDataEntity> wdeList = new List<WeatherDataEntity>();

            int count = 0;
            foreach (var line in TextLines)
            {
                count++;
                if (count <= 8)
                    continue;

                string[] lineArr = line.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (Convert.ToInt32(lineArr[0]) == 2018)
                {
                    WeatherDataEntity wde2018 = new WeatherDataEntity()
                    {
                        Year = Convert.ToInt32(lineArr[0]),
                        January = toFloat(lineArr[1]),
                        February = toFloat(lineArr[2]),
                        Winter = toFloat(lineArr[3]),
                        ClimateType = ct,
                        Region = region,
                        TimeStamp = DateTime.Now

                    };

                    wdeList.Add(wde2018);
                    continue;
                }
                WeatherDataEntity wde = new WeatherDataEntity()
                {
                    Year = Convert.ToInt32(lineArr[0]),

                    January = toFloat(lineArr[1]),
                    February = toFloat(lineArr[2]),
                    March = toFloat(lineArr[3]),
                    April = toFloat(lineArr[4]),
                    May = toFloat(lineArr[5]),
                    June = toFloat(lineArr[6]),
                    July = toFloat(lineArr[7]),
                    August = toFloat(lineArr[8]),
                    September = toFloat(lineArr[9]),
                    October = toFloat(lineArr[10]),
                    November = toFloat(lineArr[11]),
                    December = toFloat(lineArr[12]),

                    Winter = toFloat(lineArr[13]),
                    Spring = toFloat(lineArr[14]),
                    Summer = toFloat(lineArr[15]),
                    Autumn = toFloat(lineArr[16]),
                    Annual = toFloat(lineArr[17]),

                    ClimateType = ct,
                    Region = region,
                    TimeStamp = DateTime.Now
                };

                wdeList.Add(wde);
            }

            return wdeList;
        }

        private static float? toFloat(string data)
        {
            float res;
            try
            {
                res = float.Parse(data, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch (Exception e)
            {
                return null;
            }
            return res;
        }
    }//class
}
