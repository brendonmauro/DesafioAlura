using Application;
using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.AddScoped<ISeleniumService, AluraService>();
        services.AddSingleton<string[]>(args);
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();