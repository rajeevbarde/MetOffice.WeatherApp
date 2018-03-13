using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Dapper;
using Dapper.Contrib.Extensions;
using WeatherShared;

namespace WeatherORM
{
    public class WeatherDB
    {
        public static readonly string connString = @"Data Source=(localdb)\MSSQLLocalDB;Database=MetOfficeDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public IDbConnection connection;

        public WeatherDB(IDbConnection IDBcon)
        {
            if (IDBcon is SqlConnection)
                connection = new SqlConnection(connString);

            connection.Open();
        }

        public WeatherDB()
        {
            connection = new SqlConnection(connString);
            connection.Open();
        }

        public void Insert(WeatherDataEntity wd)
        {
            try
            {
                connection.Insert<WeatherDataEntity>(wd);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Update(WeatherDataEntity wd)
        {
            try
            {
                connection.Update<WeatherDataEntity>(wd);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public bool RecordExist(WeatherDataEntity wd)
        {
            WeatherDataEntity data;
            try
            {
                string sql1 = @"SELECT TOP 1 * FROM WeatherData WHERE ClimateType = @ct AND Region = @rg AND year = @yr";                
                data = connection.QuerySingle<WeatherDataEntity>(sql1, new { ct = wd.ClimateType, rg = wd.Region, yr = wd.Year });                
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        private bool matchRecord(WeatherDataEntity wd)
        {            
            string sql1 = @"SELECT TOP 1 * FROM WeatherData WHERE ClimateType = @ClimateType AND Region = @Region AND year = @year";
            WeatherDataEntity newRecord = connection.QuerySingle<WeatherDataEntity>(sql1, new { ClimateType = wd.ClimateType, Region = wd.Region, Year = wd.Year });

            if (newRecord.Annual == wd.Annual &&
                newRecord.April == wd.April &&
                newRecord.August == wd.August &&
                newRecord.Autumn == wd.Autumn &&
                newRecord.December == wd.December &&
                newRecord.February == wd.February &&
                newRecord.January == wd.January &&
                newRecord.July == wd.July &&
                newRecord.June == wd.June &&
                newRecord.March == wd.March &&
                newRecord.May == wd.May &&
                newRecord.November == wd.November &&
                newRecord.October == wd.October &&
                newRecord.September == wd.September &&
                newRecord.Spring == wd.Spring &&
                newRecord.Summer == wd.Summer &&
                newRecord.Winter == wd.Winter)
                return true;

            return false;
        }

        public Dictionary<string, int> BulkInsertOrUpdate(List<WeatherDataEntity> wd)
        {
            Dictionary<string, int> count = new Dictionary<string, int>();
            count["insert"] = 0;
            count["update"] = 0;

            foreach (var item in wd)
            {
                if (!RecordExist(item))
                {
                    this.Insert(item);
                    count["insert"]++;
                }
                else
                {
                    bool isMatch = matchRecord(item);

                    if (!isMatch)
                    {
                        this.Update(item);
                        count["update"]++;
                    }
                }                
            }

            return count;
        }

        public void ResetFileDownloadTable()
        {
            connection.DeleteAll<WeatherFileDownloadEntity>();
            
            foreach (Region region in Enum.GetValues(typeof(Region)))
                foreach (ClimateType ct in Enum.GetValues(typeof(ClimateType)))
                {
                    WeatherFileDownloadEntity wfd = new WeatherFileDownloadEntity()
                    {
                        Region = region,
                        ClimateType = ct,
                        TimeStamp = DateTime.Now.AddDays(-50),
                        Filename = FileDownloadHelper.getDBFileName(ct, region)
                    };
                    
                    connection.Insert(wfd);
                }
        }

        public DateTime getFileTimeStamp(string filename)
        {
            string sql = @"SELECT TOP 1 * FROM WeatherFileDownload WHERE filename = '" + filename + "'";
            WeatherFileDownloadEntity dt = connection.QuerySingle<WeatherFileDownloadEntity>(sql);
            var fileDate = dt.TimeStamp.Date;
            
            return fileDate;
        }

        public void UpdateFileTimeStamp(string filename)
        {
            string sql = "UPDATE WeatherFileDownload SET timestamp = @ts WHERE filename = @fn";
            var affectedRows = connection.Execute(sql, new { ts = DateTime.Now, fn = filename });
        }
    }//class
}