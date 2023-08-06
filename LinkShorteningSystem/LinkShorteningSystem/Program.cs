using LinkShorteningSystem;
using LinkShorteningSystem.HttpClients;
using LinkShorteningSystem.Infrastructure;
using LinkShorteningSystem.Infrastructure.Data;
using LinkShorteningSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Dependencies.ConfigureServices(builder.Configuration, builder.Services);
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
Configuration.AddCoreServices(builder.Services, builder.Configuration);

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddDefaultUI()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();

var configSection = builder.Configuration.GetRequiredSection(HostConfig.CONFIG_NAME);
builder.Services.Configure<HostConfig>(configSection);
var baseUrlConfig = configSection.Get<HostConfig>();

builder.Services.AddHttpClient<ILinkShorteningSystemHttpClient, LinkShorteningSystemHttpClient>()
    .ConfigureHttpClient(cfg =>
    {
        cfg.BaseAddress = new Uri(baseUrlConfig.ApiBase);
        cfg.Timeout = TimeSpan.FromSeconds(30);
    });

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
        app.Logger.LogError(ex, "An error occured addition migrations to Database");
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
        name: "default",
        pattern: "{controller=Link}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "redirect",
        pattern: "{shortenedUrl}",
        defaults: new { controller = "Link", action = "RedirectLink" });
});


app.Run();