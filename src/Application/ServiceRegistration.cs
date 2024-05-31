using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class ServiceRegistration
{
    public static void AddApplicationServiceRegistration(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}
