using LinkShorteningSystem.BL.ImplementServices;
using LinkShorteningSystem.Domain.Interfaces.Repositories;
using LinkShorteningSystem.Domain.Interfaces.Services;
using LinkShorteningSystem.Infrastructure.Data;
using LinkShorteningSystem.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;

namespace LinkShorteningSystem.Web.Extensions
{
    public static class ServicesConfigurationWeb
    {
        public static void Configuration(
            IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserRefreshTokenRepository, UserRefreshTokenRepository>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IAccountService, AccountService>();

            services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>();
        }
    }
}
