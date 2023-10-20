using LinkShorteningSystem.HttpClients;
using LinkShorteningSystem.Infrastructure;
using LinkShorteningSystem.Infrastructure.Data;
using LinkShorteningSystem.Infrastructure.Identity;
using LinkShorteningSystem.ServicesConfigurationWeb;
using LinkShorteningSystem.WebApi.ApplicationBuilderExtension;
using LinkShorteningSystem.WebApi.ServicesConfigurationWeb;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

LogsConfiguration.Configuration(builder.Configuration, builder.Logging);
DependenciesWeb.ConfigureServices(builder.Configuration, builder.Services);
//JwtTokenExtension.Configuration(builder.Services, builder.Configuration);
ConfigurationServicesWeb.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

app.AppExtensionWeb();

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
        defaults: new { controller = "Web", action = "RedirectLink" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Web}/{action=Index}/{id?}");
});

app.Run();