using Serilog;

namespace LinkShorteningSystem.WebApi.ServicesConfigurationApi
{
    public static class LogsConfigurationApi
    {
        public static void Configuration(
            IConfiguration configuration,
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