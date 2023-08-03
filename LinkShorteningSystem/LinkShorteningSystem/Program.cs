using LinkShorteningSystem;
using LinkShorteningSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
LinkShorteningSystem.Infrastructure.Dependencies.ConfigureServices(builder.Configuration, builder.Services);

//IoC
builder.Services.AddCoreServices();

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
        await CatalogContextSeed.SeedAsync(catalogContext, app.Logger);

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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Link}/{action=Index}/{id?}");

app.Run();
