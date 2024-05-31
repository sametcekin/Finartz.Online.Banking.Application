using Application.Abstractions;
using Domain.AppSettings;
using Domain.Entities;
using Domain.SharedCore;
using Infrastructure.CrossCuttingConcerns;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<AuditableEntitiesInterceptor>();
        services.AddDbContext<AppDbContext>((sp, opt) =>
        {
            var interceptor = sp.GetService<AuditableEntitiesInterceptor>() ?? throw new ArgumentNullException();
            opt.UseInMemoryDatabase("FinartzDB").AddInterceptors(interceptor);
        });
        services.AddScoped<IRepository<Account>, Repository<AppDbContext, Account>>();
        services.AddScoped<IRepository<User>, Repository<AppDbContext, User>>();


        services.Configure<TokenSettings>(opt => configuration.GetSection(nameof(TokenSettings)).Bind(opt));
        var tokenOptions = configuration.GetSection(nameof(TokenSettings)).Get<TokenSettings>();
        services.AddTransient<ITokenProvider, JwtTokenProvider>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Secret)),

                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
        });



    }
}
