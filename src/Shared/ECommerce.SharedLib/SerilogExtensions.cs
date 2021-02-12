using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace ECommerce.SharedLib
{
    public static class SerilogExtensions
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
            (hostingContext, loggerConfiguration) =>
            {
                var env = hostingContext.HostingEnvironment;

                loggerConfiguration.MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("AppName", env.ApplicationName)
                    .Enrich.WithExceptionDetails()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                    .MinimumLevel.Override("ECommerce", LogEventLevel.Debug)
                    .WriteTo.Console();

                var elasticSearchUrl = hostingContext.Configuration.GetValue<string>("ElasticSearchUrl");

                if (!string.IsNullOrEmpty(elasticSearchUrl))
                {
                    loggerConfiguration.WriteTo.Elasticsearch(
                        new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
                        {
                            AutoRegisterTemplate = true,
                            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                            IndexFormat = "ecommerce-{0:yyyy.MM.dd}",
                            MinimumLogEventLevel = LogEventLevel.Debug
                        });
                }
            };
    }
}