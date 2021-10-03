using System.IO;
using System.Threading.Tasks;
using DotnetK8S.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DotnetK8S.Worker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
            
            await new HostBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .ConfigureServices(services =>
                {
                    services.Configure<Config>(config);
                    services.AddHostedService<WorkerService>();
                    services.AddEfCore(config, "PostgreSql");
                })
                .RunConsoleAsync();
        }
    }
}