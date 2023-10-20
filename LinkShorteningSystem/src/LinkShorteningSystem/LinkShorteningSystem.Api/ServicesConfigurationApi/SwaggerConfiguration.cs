using Microsoft.OpenApi.Models;
using System.Reflection;

namespace LibraryAPI.ServicesConfiguration
{
    public static class SwaggerConfiguration
    {
        public static void Configuration(
            IServiceCollection services)
        {
            services.AddSwaggerGen();           
        }
    }
}