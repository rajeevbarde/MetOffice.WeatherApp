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
            ClimateLoader.InitialLoad();
            Console.ReadLine();
        }
    }
}
