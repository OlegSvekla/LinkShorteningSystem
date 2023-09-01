using LinkShorteningSystem;
using LinkShorteningSystem.BL.ImplementServices;
using LinkShorteningSystem.Domain.Interfaces.Repositories;
using LinkShorteningSystem.Domain.Interfaces.Services;
using LinkShorteningSystem.Infrastructure.Data;
using LinkShorteningSystem.Infrastructure.Dependencies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

DependenciesApi.ConfigureServices(builder.Configuration, builder.Services);
ConfigurationServicesApi.AddCoreServices(builder.Services, builder.Configuration, builder.Logging);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var catalogContext = scopedProvider.GetRequiredService<CatalogContext>();
        if (catalogContext.Database.IsSqlServer())
        {
            catalogContext.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        var logger = scopedProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while adding migrations to Database");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();