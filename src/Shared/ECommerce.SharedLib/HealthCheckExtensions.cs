using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.SharedLib.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ECommerce.SharedLib
{
    public static class HealthCheckExtensions
    {
        public static void MapDefaultHealthChecks(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks("/healthcheck/live", new HealthCheckOptions
            {
                Predicate = _ => false,
                ResponseWriter = WriteJsonResponse
            });

            endpoints.MapHealthChecks("/healthcheck/ready", new HealthCheckOptions
            {
                ResponseWriter = WriteJsonResponse
            });
        }

        public static IHealthChecksBuilder AddRedisHealthCheck(this IHealthChecksBuilder builder,
            RedisSettings settings, string name)
        {
            return builder.AddCheck(name, new RedisHealthCheck(settings));
        }

        private static Task WriteJsonResponse(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var options = new JsonWriterOptions {Indented = true};

            using var writer = new Utf8JsonWriter(context.Response.BodyWriter, options);

            writer.WriteStartObject();
            writer.WriteString("systemStatus", report.Status.ToString());

            if (report.Entries.Count > 0)
            {
                writer.WriteStartArray("services");

                foreach (var (key, value) in report.Entries)
                {
                    writer.WriteStartObject();
                    writer.WriteString("name", key);
                    writer.WriteString("serviceStatus", value.Status.ToString());
                    writer.WriteEndObject();
                }

                writer.WriteEndArray();
            }

            writer.WriteEndObject();

            return Task.CompletedTask;
        }
    }
}