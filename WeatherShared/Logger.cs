using System;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace WeatherShared
{
    public static class Logger
    {
        public static ILogger Initialize()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                //.WriteTo.Console()
                .WriteTo.File("logs\\myapp.txt", shared: true)
                .CreateLogger();

            return Log.Logger;
        }
    }
}
