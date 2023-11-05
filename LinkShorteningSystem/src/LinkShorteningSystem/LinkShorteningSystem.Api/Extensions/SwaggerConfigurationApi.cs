using Microsoft.OpenApi.Models;
using System.Reflection;

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