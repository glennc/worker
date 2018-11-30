using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
#if WebJobs
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Azure.WebJobs.Host;
#endif

namespace workerproto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
#if WebJobs
                .ConfigureWebJobs(webJobsBuilder => webJobsBuilder.AddTimers())
#endif
                .ConfigureServices(services => {
                    services.AddHealthChecks()
                            .AddSocketListener(8080);
#if WebJobs
                    services.AddSingleton<ScheduleMonitor, FileSystemScheduleMonitor>();
#endif
#if Empty
                    services.AddHostedService<Worker>();
#endif
                });
    }
}
