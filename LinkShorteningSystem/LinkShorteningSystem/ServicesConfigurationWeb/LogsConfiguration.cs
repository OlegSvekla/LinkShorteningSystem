using Serilog;

namespace LinkShorteningSystem.WebApi.ServicesConfigurationWeb
{
    public static class LogsConfiguration
    {
        public static void Configuration(
            this IConfiguration configuration,
            ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.AddSerilog(
                new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger());
        }
    }
}