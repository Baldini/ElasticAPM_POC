using Elastic.Apm.SerilogEnricher;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using System;

namespace API2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseSerilog((context, configuration) =>
                    {
                        configuration.Enrich.WithElasticApmCorrelationInfo();
                        configuration.WriteTo.Console(outputTemplate: "[{ElasticApmTraceId} {ElasticApmTransactionId} {Message:lj} {NewLine}{Exception}");
                        configuration.WriteTo.Elasticsearch(
                            new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))
                            {
                                AutoRegisterTemplate = true,
                                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                                CustomFormatter = new ElasticsearchJsonFormatter(),
                                BufferBaseFilename = "./logs/log",
                            });
                    });
                });
    }
}
