using LinkShorteningSystem;
using LinkShorteningSystem.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

LogsConfigurationWeb.Configuration(builder.Configuration, builder.Logging);
DbConfigurationWeb.Configuration(builder.Configuration, builder.Services);
ServicesConfigurationWeb.Configuration(builder.Services);
AuthConfigurationWeb.Configuration(builder.Services, builder.Configuration);
HttpClientConfigurationWeb.Configuration(builder.Configuration, builder.Services);

var app = builder.Build();

await app.RunDbContextMigrationsWeb();

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