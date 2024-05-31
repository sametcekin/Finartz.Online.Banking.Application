using API.Controllers.V1;
using Application;
using Domain.AppSettings;
using Domain.Exceptions;
using Infrastructure;
using MassTransit;
using Polly;
using Polly.CircuitBreaker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<BusSettings>(options => builder.Configuration.GetSection(nameof(BusSettings)).Bind(options));

var busSettings = builder.Configuration.GetSection(nameof(BusSettings)).Get<BusSettings>()!;

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
    {
        busFactoryConfigurator.Host("rabbitmq", hostConfigurator => { });
    });
});
builder.Services.AddSingleton(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<ILogger<AccountController>>();
    static AsyncCircuitBreakerPolicy CreatePolicy(
            int exceptionsAllowedBeforeBreaking,
            TimeSpan durationOfBreak,
            ILogger logger)
    {
        return Policy
            .Handle<BusinessRuleException>()
            .Or<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking,
                durationOfBreak,
                onBreak: (exception, breakDelay) =>
                {
                    logger.LogError($"Circuit breaker opened for {breakDelay.TotalSeconds} seconds due to: {exception.Message}");
                },
                onReset: () =>
                {
                    logger.LogInformation("Circuit breaker reset.");
                },
                onHalfOpen: () =>
                {
                    logger.LogInformation("Circuit breaker is half-open. Next call is a trial.");
                }
            );
    }

    return CreatePolicy(1, TimeSpan.FromSeconds(30), logger);
});

builder.Services.AddApplicationServiceRegistration();
builder.Services.AddInfrastructureServiceRegistration(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
