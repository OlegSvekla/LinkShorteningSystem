using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LinkShorteningSystem.Web.Extensions
{
    public static class AuthConfigurationWeb
    {
        public static void Configuration(
            IServiceCollection services,
            IConfiguration configuration)
        {
            var tokens = configuration.GetSection("Tokens");
            var secretKey = tokens["SecretKey"];
            var issuer = configuration["Tokens:Issuer"];
            var audience = configuration["Tokens:Audience"];
            var accessTokenExpirationMinutes = Convert.ToInt32(tokens["AccessTokenExpirationMinutes"]);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/Register";
            });

            //    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //.AddCookie(options => //CookieAuthenticationOptions
            //{
            //    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
            //});
        }
    }
}
