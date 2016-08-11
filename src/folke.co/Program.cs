using System.IO;
using Microsoft.AspNetCore.Hosting;
using Folke;
using Microsoft.Extensions.Configuration;

namespace folke.co
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // We load the configuration here so that the app will listen on a port configurable from the appsettings.json file ("server.urls" section)
            var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddJsonFile("appsettings.Local.json", optional: true)
                    .Build();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
