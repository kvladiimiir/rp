using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration.Json;

namespace RequestClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()) + "/config/host.json";
            
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(path, true, true)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseUrls(config["urls"])
                .UseStartup<Startup>();
        }
    }
}
