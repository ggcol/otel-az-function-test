using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddOpenTelemetry();
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .ConfigureLogging(logging =>
    {
        //hacking MS default behaviours :)!
        logging.Services.Configure<LoggerFilterOptions>(options =>
        {
            var defaltRule = options.Rules.FirstOrDefault(rule =>
                rule.ProviderName.Equals(
                    "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider"));
            if (defaltRule is not null)
            {
                options.Rules.Remove(defaltRule);
            }
        });
    })
    .Build();

host.Run();