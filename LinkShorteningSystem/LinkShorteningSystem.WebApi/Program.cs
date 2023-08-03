using LinkShorteningSystem.BL.ImplementServices;
using LinkShorteningSystem.Domain.Interfaces.Repositories;
using LinkShorteningSystem.Domain.Interfaces.Services;
using LinkShorteningSystem.Infrastructure;
using LinkShorteningSystem.Infrastructure.Data;
using LinkShorteningSystem.WebApi.Mapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
Dependencies.ConfigureServices(builder.Configuration, builder.Services);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<ILinkService, LinkService>();
builder.Services.AddAutoMapper(typeof(Map));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var catalogContext = scopedProvider.GetRequiredService<ApiContext>();
        if (catalogContext.Database.IsSqlServer())
        {
            catalogContext.Database.Migrate();
        }

        //var identityContext = scopedProvider.GetRequiredService<AppIdentityDbContext>();
        //if (identityContext.Database.IsSqlServer())
        //{
        //identityContext.Database.Migrate();
        //}
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occured addition migrations to Database");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
