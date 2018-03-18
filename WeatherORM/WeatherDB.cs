using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Dapper;
using Dapper.Contrib.Extensions;
using WeatherShared;

namespace WeatherORM
{
    /// <summary>
    /// ORM class containing database operations.
    /// </summary>
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

        /// <summary>
        /// Insert weather data into table
        /// </summary>
        /// <param name="wd">Row data which needs to be inserted</param>
        /// <returns>identity id of the record inserted</returns>
        public long Insert(WeatherDataEntity wd)
        {
            long id = -1;
            try
            {
                id = connection.Insert<WeatherDataEntity>(wd);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return id;
        }

        /// <summary>
        /// Update weather data into table
        /// </summary>
        /// <param name="wd">Row data which needs to be updated</param>
        /// <returns>record update status</returns>
        public bool Update(WeatherDataEntity wd)
        {
            bool isUpdated = false;
            try
            {
                isUpdated = connection.Update<WeatherDataEntity>(wd);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return isUpdated;
        }

        /// <summary>
        /// Check Whether the record exist in the database
        /// </summary>
        /// <param name="wd">Row data which needs to be verified if it exist</param>
        /// <returns>record exist  status</returns>
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

        /// <summary>
        /// Match all the column in WeatherData table with entity passed in the parameter
        /// </summary>
        /// <param name="wd">Row data which needs to be matched with database</param>
        /// <returns>record match  status</returns>
        private bool matchRecord(WeatherDataEntity wd)
        {
            try
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
            }
            catch (Exception e) { }

            return false;
        }

        /// <summary>
        /// Bulk insert or update Weather data.
        /// </summary>
        /// <param name="wd">List of Weather data</param>
        /// <returns>Dictionary containing count of inserted, update and no change</returns>
        public Dictionary<string, int> BulkInsertOrUpdate(List<WeatherDataEntity> wd)
        {
            Dictionary<string, int> count = new Dictionary<string, int>();
            count["insert"] = 0;
            count["update"] = 0;
            count["nochange"] = 0;

            foreach (var item in wd)
            {
                if (!RecordExist(item))
                {
                    long val = this.Insert(item);
                    if(val != -1)
                        count["insert"]++;
                }
                else
                {
                    bool isMatch = matchRecord(item);

                    if (!isMatch)
                    {
                        bool isUpdated = this.Update(item);
                        if(isUpdated)
                            count["update"]++;
                    }
                }                
            }//for

            count["nochange"] = wd.Count - (count["insert"] + count["update"]);
            return count;
        }

        /// <summary>
        /// Reset table WeatherFileDownload to default values.
        /// </summary>        
        /// <returns>Status of table reset</returns>
        public bool ResetFileDownloadTable()
        {
            bool isReset = false;
            try
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

                isReset = true;
            }
            catch (Exception e) { }

            return isReset;
        }

        /// <summary>
        /// Get the timestamp of the file name from the table - WeatherFileDownload
        /// </summary>      
        /// <param name="filename">name of the file which the timestamp is needed</param>
        /// <returns>time stamp for the file name</returns>
        public DateTime getFileTimeStamp(string filename)
        {
            string sql = @"SELECT TOP 1 * FROM WeatherFileDownload WHERE filename = '" + filename + "'";
            WeatherFileDownloadEntity dt = connection.QuerySingle<WeatherFileDownloadEntity>(sql);
            var fileDate = dt.TimeStamp.Date;
            
            return fileDate;
        }

        /// <summary>
        /// Updates file's timestamp to current data and time within the database.
        /// </summary>      
        /// <param name="filename">name of the file which the timestamp is updated</param>
        /// <returns>status of update</returns>
        public bool UpdateFileTimeStamp(string filename)
        {
            bool isUpdated = false;
            try
            {
                string sql = "UPDATE WeatherFileDownload SET timestamp = @ts WHERE filename = @fn";
                var affectedRows = connection.Execute(sql, new { ts = DateTime.Now, fn = filename });
                isUpdated = true;
            }
            catch (Exception e) { }

            return isUpdated;
        }

        /// <summary>
        /// Truncate Table
        /// </summary>      
        /// <param name="filename">name of the file which the timestamp is updated</param>
        /// <returns>status of update</returns>
        public bool TruncateTable(string table)
        {
            bool isTruncated = false;
            try
            {
                string sql = $"TRUNCATE TABLE {table}";
                var affectedRows = connection.Execute(sql);
                isTruncated = true;
            }
            catch (Exception e) { }

            return isTruncated;
        }
    }//class
}