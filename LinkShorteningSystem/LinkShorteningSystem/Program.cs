using LinkShorteningSystem;
using LinkShorteningSystem.HttpClients;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient<ILinkShorteningSystemHttpClient, LinkShorteningSystemHttpClient>()
    .ConfigureHttpClient(cfg =>
    {
        // TODO: MUST BE LOADED FROM CONFIGURATION
        cfg.BaseAddress = new Uri("http://localhost:5176");
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
