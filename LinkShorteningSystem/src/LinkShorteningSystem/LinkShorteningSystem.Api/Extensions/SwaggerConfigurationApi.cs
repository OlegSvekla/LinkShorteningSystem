namespace LibraryAPI.ServicesConfiguration
{
    public static class SwaggerConfigurationApi
    {
        public static void Configuration(
            IServiceCollection services)
        {
            services.AddSwaggerGen();           
        }
    }
}