using LinkShorteningSystem;
using LinkShorteningSystem.HttpClients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Çàãðóçêà êîíôèãóðàöèè èç ôàéëà launchSettings.json àïè ïðîåêòà
//var config = new ConfigurationBuilder()
//    .SetBasePath(builder.Environment.ContentRootPath)
//    .AddJsonFile("../LinkShorteningSystem.MVC/src/LinkShorteningSystem/appsettings.json", optional: true, reloadOnChange: true)
//    .Build();

//var baseAddress = config["baseUrls:apiBase"];
//builder.Services.AddHttpClient<ILinkShorteningSystemHttpClient, LinkShorteningSystemHttpClient>()
//    .ConfigureHttpClient(cfg =>
//    {

//        // TODO: MUST BE LOADED FROM CONFIGURATION
//        cfg.BaseAddress = new Uri(baseAddress);
//        cfg.Timeout = TimeSpan.FromSeconds(30);
//    });

// blazor configuration
var configSection = builder.Configuration.GetRequiredSection(HostConfig.CONFIG_NAME);
builder.Services.Configure<HostConfig>(configSection);
var baseUrlConfig = configSection.Get<HostConfig>();

// Blazor Admin Required Services for Prerendering
builder.Services.AddHttpClient<ILinkShorteningSystemHttpClient, LinkShorteningSystemHttpClient>()
    .ConfigureHttpClient(cfg =>
    {

        // TODO: MUST BE LOADED FROM CONFIGURATION
        cfg.BaseAddress = new Uri(baseUrlConfig.ApiBase);
        cfg.Timeout = TimeSpan.FromSeconds(30);
    });

var app = builder.Build();

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
        pattern: "{shortenedUrl}",
        defaults: new { controller = "Link", action = "RedirectLink" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Link}/{action=Index}/{id?}");
});


app.Run();