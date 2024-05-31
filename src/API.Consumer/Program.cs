// See https://aka.ms/new-console-template for more information
using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Reflection;

Console.WriteLine("Hello, World!");

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{
    services.AddMassTransit(busConfigurator =>
    {
        var entryAssembly = Assembly.GetExecutingAssembly();

        busConfigurator.AddConsumers(entryAssembly);
        busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
        {
            busFactoryConfigurator.Host("rabbitmq", "/", h => { });

            busFactoryConfigurator.ConfigureEndpoints(context);
        });
    });
});
