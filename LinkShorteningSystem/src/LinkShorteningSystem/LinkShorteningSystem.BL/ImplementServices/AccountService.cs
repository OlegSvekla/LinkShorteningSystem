using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using LinkShorteningSystem.Web.Models;
using LinkShorteningSystem.Domain.Interfaces.Services;
using LinkShorteningSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Azure;
using LinkShorteningSystem.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using LinkShorteningSystem.Domain.Interfaces.Repositories;

namespace LinkShorteningSystem.BL.ImplementServices
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountService> _logger;
        private readonly IConfiguration _config;
        private readonly IdentityDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRefreshTokenRepository _userRefreshTokenRepository;

        public AccountService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender,
             ILogger<AccountService> logger,
            IConfiguration config,
            IdentityDbContext context,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor,
            IUserRefreshTokenRepository userRefreshTokenRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _config = config;
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService; ;
            _httpContextAccessor = httpContextAccessor;
            _userRefreshTokenRepository = userRefreshTokenRepository;

        }

        public async Task<bool> Register(RegisterViewModel model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };

            user.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                };

                var accessToken = _tokenService.GenerateAccessToken(claims);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddMinutes(15)
                };
                _httpContextAccessor.HttpContext.Response.Cookies.Append("access_token", accessToken, cookieOptions);

                var signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (signInResult.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> Login(LoginViewModel model)
        {
            //if (ModelState.IsValid)            
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is not null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.Email)
                    };

                    var token = _tokenService.GenerateAccessToken(claims);
                    var newRefreshToken = _tokenService.GenerateRefreshToken();

                    var userRefreshToken = await _userRefreshTokenRepository.GetOneByAsync(expression: _ => _.Username.Equals(user.Email));
                    if (userRefreshToken is not null)
                    {
                        userRefreshToken.RefreshToken = newRefreshToken;
                        await _userRefreshTokenRepository.UpdateAsync(userRefreshToken);

                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Expires = DateTime.UtcNow.AddMinutes(15)
                        };
                        _httpContextAccessor.HttpContext.Response.Cookies.Append("access_token", userRefreshToken.RefreshToken, cookieOptions);
                    }
                    else
                    {
                        userRefreshToken = new UserRefreshToken
                        {
                            Username = user.Email,
                            RefreshToken = newRefreshToken,
                            Password = model.Password
                        };

                        await _userRefreshTokenRepository.CreateAsync(userRefreshToken);

                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Expires = DateTime.UtcNow.AddMinutes(15)
                        };
                        _httpContextAccessor.HttpContext.Response.Cookies.Append("access_token", userRefreshToken.RefreshToken, cookieOptions);
                    }               
                }
                return true;
            }           
            return false;
        }
    }
}
