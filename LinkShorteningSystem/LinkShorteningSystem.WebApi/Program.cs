using LibraryAPI.ServicesConfiguration;
using LinkShorteningSystem.Infrastructure.Data;
using LinkShorteningSystem.Infrastructure.Dependencies;
using LinkShorteningSystem.WebApi.ApplicationBuilderExtensionApi;
using LinkShorteningSystem.WebApi.ServicesConfigurationApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

LogsConfiguration.Configuration(builder.Configuration, builder.Logging);
SwaggerConfiguration.Configuration(builder.Services);
DependenciesApi.ConfigureServices(builder.Configuration, builder.Services);
ConfigurationServicesApi.AddCoreServices(builder.Services, builder.Configuration);

var app = builder.Build();

app.AppExtenssionApi();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();