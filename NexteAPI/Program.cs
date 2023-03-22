using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NexteAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logo = @"
========================================================

    _   _________  ______________   ___    ____  ____
   / | / / ____/ |/ /_  __/ ____/  /   |  / __ \/  _/
  /  |/ / __/  |   / / / / __/    / /| | / /_/ // /  
 / /|  / /___ /   | / / / /___   / ___ |/ ____// /   
/_/ |_/_____//_/|_|/_/ /_____/  /_/  |_/_/   /___/   
                                                     
            Create by FourFeatures 2023 

========================================================
"
            ;

            byte[] array = Encoding.UTF8.GetBytes(logo);
            Stream stream = Console.OpenStandardOutput();
            stream.Write(array, 0, array.Length);
            stream.Flush();
            stream.Dispose();
            stream.Close();

            var host = CreateHostBuilder(args).Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false).Build();

            var webHost = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddLog4Net();
                });

            return webHost;
        }
    }
}