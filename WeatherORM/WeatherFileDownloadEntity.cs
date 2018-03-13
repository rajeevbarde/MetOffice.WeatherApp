using System;
using Dapper.Contrib.Extensions;
using WeatherShared;

namespace WeatherORM
{
    [Table("WeatherFileDownload")]
    class WeatherFileDownloadEntity
    {
        [Key]
        public int Id { get; set; }
        public Region Region { get; set; }
        public ClimateType ClimateType { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Filename { get; set; }
    }
}
