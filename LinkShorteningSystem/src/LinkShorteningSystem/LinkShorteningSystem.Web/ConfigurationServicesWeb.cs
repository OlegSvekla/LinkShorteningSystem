using LinkShorteningSystem.BL.ImplementServices;
using LinkShorteningSystem.Domain.Interfaces.Repositories;
using LinkShorteningSystem.Domain.Interfaces.Services;
using LinkShorteningSystem.HttpClients;
using LinkShorteningSystem.Infrastructure.Data;
using LinkShorteningSystem.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace LinkShorteningSystem
{
    public static class ConfigurationServicesWeb
    {
        internal static IServiceCollection ConfigureServices(
            this IServiceCollection services,
            IConfiguration configuration,
            ILoggingBuilder logging)
        {
            services.AddTransient<IAccountService, AccountService>();
            logging.ClearProviders();
            logging.AddSerilog(
                new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger());

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
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserRefreshTokenRepository, UserRefreshTokenRepository>();

            services.AddSingleton<IConfiguration>(provider => configuration);
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();


            var tokens = configuration.GetSection("Tokens");
            var secretKey = tokens["SecretKey"];
            var issuer = configuration["Tokens:Issuer"];
            var audience = configuration["Tokens:Audience"];
            var accessTokenExpirationMinutes = Convert.ToInt32(tokens["AccessTokenExpirationMinutes"]);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => //CookieAuthenticationOptions
        {
            options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
        });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero, //the default for this setting is 5 minutes
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

            return services;
        }
    }
}