using LinkShorteningSystem.Areas.Identity.JwtConfig.Models;
using LinkShorteningSystem.Areas.Identity.JwtConfig.Services;
using LinkShorteningSystem.HttpClients;
using LinkShorteningSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace LinkShorteningSystem
{
    public static class ConfigurationServicesWeb
    {
        internal static IServiceCollection ConfigureServices(
            this IServiceCollection services,
            IConfiguration configuration,
            ILoggingBuilder logging)
        {

            logging.ClearProviders();
            logging.AddSerilog(
                new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger());

            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var accessTokenExpirationMinutes = Convert.ToInt32(jwtSettings["AccessTokenExpirationMinutes"]);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                });

            var configSection = configuration.GetRequiredSection(HostConfig.CONFIG_NAME);
            services.Configure<HostConfig>(configSection);
            var baseLinkConfig = configSection.Get<HostConfig>();

            services.AddHttpClient<ILinkShorteningSystemHttpClient, LinkShorteningSystemHttpClient>()
                .ConfigureHttpClient(cfg =>
                {
                    cfg.BaseAddress = new Uri(baseLinkConfig.ApiBase);
                    cfg.Timeout = TimeSpan.FromSeconds(30);
                });

            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<JwtSettings>(jwtSettings);
            services.AddScoped<JwtTokenService>();
            services.AddControllersWithViews();
            services.AddRazorPages();

            return services;
        }
    }
}