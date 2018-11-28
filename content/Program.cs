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
                .ConfigureServices(services => services.AddSingleton<ScheduleMonitor, FileSystemScheduleMonitor>());
#endif
#if Empty
                .ConfigureServices(services => services.AddHostedService<Worker>());
#endif
    }
}
