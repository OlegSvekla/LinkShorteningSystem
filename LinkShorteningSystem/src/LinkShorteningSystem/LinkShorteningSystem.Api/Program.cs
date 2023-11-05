using LibraryAPI.ServicesConfiguration;
using LinkShorteningSystem.Api.Extensions;
using LinkShorteningSystem.Web.Extensions;
using LinkShorteningSystem.WebApi.ServicesConfigurationApi;

var builder = WebApplication.CreateBuilder(args);

LogsConfigurationApi.Configuration(builder.Configuration, builder.Logging);
SwaggerConfigurationApi.Configuration(builder.Services);
DbConfigurationApi.Configuration(builder.Configuration, builder.Services);
ConfigurationServicesApi.AddCoreServices(builder.Services, builder.Configuration);

var app = builder.Build();

await app.RunDbContextMigrationsApi();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();