namespace LinkShorteningSystem.ServicesConfigurationWeb
{
    //public static class JwtTokenExtension
    //{
    //    internal static IServiceCollection Configuration(
    //        this IServiceCollection services,
    //        IConfiguration configuration)
    //    {
    //        var jwtSettings = configuration.GetSection("JwtSettings");
    //        var secretKey = jwtSettings["SecretKey"];
    //        var issuer = jwtSettings["Issuer"];
    //        var audience = jwtSettings["Audience"];
    //        var accessTokenExpirationMinutes = Convert.ToInt32(jwtSettings["AccessTokenExpirationMinutes"]);

    //        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //            .AddJwtBearer(options =>
    //            {
    //                options.TokenValidationParameters = new TokenValidationParameters
    //                {
    //                    ValidateIssuer = true,
    //                    ValidateAudience = true,
    //                    ValidateIssuerSigningKey = true,
    //                    ValidIssuer = issuer,
    //                    ValidAudience = audience,
    //                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    //                };
    //            });

    //        services.Configure<JwtSettings>(jwtSettings);
    //        services.AddScoped<JwtTokenService>();

    //        return services;
    //    }
    //}
}
