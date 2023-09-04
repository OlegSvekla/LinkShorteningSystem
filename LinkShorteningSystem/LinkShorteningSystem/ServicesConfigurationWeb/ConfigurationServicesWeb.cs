﻿using LinkShorteningSystem.Areas.Identity.JwtConfig.Models;
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

namespace LinkShorteningSystem.ServicesConfigurationWeb
{
    public static class ConfigurationServicesWeb
    {
        internal static IServiceCollection ConfigureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
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

            services.AddControllersWithViews();
            services.AddRazorPages();

            return services;
        }
    }
}