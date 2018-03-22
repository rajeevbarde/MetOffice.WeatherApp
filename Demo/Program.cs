using System;
using System.Collections.Generic;
using WeatherLoader;
using WeatherShared;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            ClimateLoader.InitialLoad();

            Console.WriteLine("Done !");
            Console.ReadLine();            
        }
    }
}
