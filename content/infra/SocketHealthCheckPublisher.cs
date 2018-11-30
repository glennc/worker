using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace workerproto
{
    public class SocketHealthCheckPublisher : IHealthCheckPublisher
    {
        private TcpListener _listener;

        public SocketHealthCheckPublisher(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
        {
            if (report.Status == HealthStatus.Healthy)
            {
                _listener.Start();
            }
            else
            {
                _listener.Stop();
            }
            return Task.CompletedTask;
        }
    }

    public static class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddSocketListener(this IHealthChecksBuilder builder, int port)
        {
            builder.Services.AddSingleton<IHealthCheckPublisher>(new SocketHealthCheckPublisher(port));
            return builder;
        }
    }
}
