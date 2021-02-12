using System;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.SharedLib.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace ECommerce.SharedLib
{
    public class RedisHealthCheck : IHealthCheck
    {
        private readonly RedisSettings _settings;

        public RedisHealthCheck(RedisSettings settings)
        {
            _settings = settings;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var connection = await ConnectionMultiplexer.ConnectAsync(new ConfigurationOptions
                {
                    EndPoints = {{_settings.Host, _settings.Port}},
                    Password = _settings.Password
                });

                return connection.IsConnected ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(HealthStatus.Unhealthy, exception: ex);
            }
        }
    }
}