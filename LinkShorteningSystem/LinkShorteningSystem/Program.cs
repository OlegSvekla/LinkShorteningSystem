using LinkShorteningSystem;
using LinkShorteningSystem.HttpClients;
using LinkShorteningSystem.Infrastructure;
using LinkShorteningSystem.Infrastructure.Data;
using LinkShorteningSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

DependenciesWeb.ConfigureServices(builder.Configuration, builder.Services);
ConfigurationServicesWeb.AddCoreServices(builder.Services, builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var identityContext = scopedProvider.GetRequiredService<AppIdentityDbContext>();
        if (identityContext.Database.IsSqlServer())
        {
            identityContext.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        var logger = scopedProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while adding migrations to the Database");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
    endpoints.MapControllerRoute(
        name: "redirect",
        pattern: "{shortenedLink}",
        defaults: new { controller = "Link", action = "RedirectLink" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Link}/{action=Index}/{id?}");
});

app.Run();