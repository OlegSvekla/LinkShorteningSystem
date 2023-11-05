using LinkShorteningSystem.HttpClients;
using LinkShorteningSystem.Infrastructure.Data;
using LinkShorteningSystem.Web.HttpClients;
using Microsoft.EntityFrameworkCore;

namespace LinkShorteningSystem.Web.Extensions
{
    public static class HttpClientConfigurationWeb
    {
        public static void Configuration(
            IConfiguration configuration,
            IServiceCollection services)
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

        }
    }
}
