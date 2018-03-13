using System;
using Dapper.Contrib.Extensions;
using WeatherShared;

namespace WeatherORM
{    
    [Table("WeatherData")]
    public class WeatherDataEntity
    {
        [Key]
        public int WeatherDataId { get; set; }
        public int Year { get; set; }

        public float? January { get; set; }
        public float? February { get; set; }
        public float? March { get; set; }
        public float? April { get; set; }
        public float? May { get; set; }
        public float? June { get; set; }
        public float? July { get; set; }
        public float? August { get; set; }
        public float? September { get; set; }
        public float? October { get; set; }
        public float? November { get; set; }
        public float? December { get; set; }

        public float? Winter { get; set; }
        public float? Spring { get; set; }
        public float? Summer { get; set; }
        public float? Autumn { get; set; }
        public float? Annual { get; set; }

        public Region Region { get; set; }
        public ClimateType ClimateType { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
