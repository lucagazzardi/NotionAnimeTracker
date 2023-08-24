using Business_AnimeToNotion.Integrations.Internal;
using Business_AnimeToNotion.Integrations.MAL;
using Data_AnimeToNotion.Context;
using Data_AnimeToNotion.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()    
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    {
        s.AddDbContext<AnimeShowContext>(
            options => options.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnectionString"))
        );
        s.AddSingleton<IInternal_Integration, Internal_Integration>();
        s.AddSingleton<IMAL_Integration, MAL_Integration>();
        s.AddSingleton<IAnimeShowRepository, AnimeShowRepository>();
        s.AddSingleton<ISyncToNotionRepository, SyncToNotionRepository>();
        s.Configure<LoggerFilterOptions>(options =>
        {
            // The Application Insights SDK adds a default logging filter that instructs ILogger to capture only Warning and more severe logs. Application Insights requires an explicit override.
            // Log levels can also be configured using appsettings.json. For more information, see https://learn.microsoft.com/en-us/azure/azure-monitor/app/worker-service#ilogger-logs
            LoggerFilterRule toRemove = options.Rules.FirstOrDefault(rule => rule.ProviderName
                == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");

            if (toRemove is not null)
            {
                options.Rules.Remove(toRemove);
            }

            options.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
        });
    })
    .Build();

await host.RunAsync();
