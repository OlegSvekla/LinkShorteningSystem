using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using LinkShorteningSystem.Web.Models;
using LinkShorteningSystem.Domain.Entities;
using LinkShorteningSystem.Domain.Interfaces.Services;
using LinkShorteningSystem.Infrastructure.Data;
using FluentNHibernate.Mapping;

namespace LinkShorteningSystem.Web.Controllers
{
    public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly IEmailSender _emailSender;
		private readonly ILogger _logger;
		private readonly IConfiguration _config;
		private readonly IdentityDbContext _context;
		private readonly IPasswordHasher _passwordHasher;
		private readonly ITokenService _tokenService;
        private readonly IAccountService _accountService;

		public AccountController(
			UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager,
			IEmailSender emailSender,
			ILogger<AccountController> logger,
			IConfiguration config,
            IdentityDbContext context,
			IPasswordHasher passwordHasher,
			ITokenService tokenService,
            IAccountService accountService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
			_logger = logger;
			_config = config;
			_context = context;
			_passwordHasher = passwordHasher;
			_tokenService = tokenService;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var success = await _accountService.Register(model);

            return success == false ? NotFound("This user is already exist") : RedirectToAction("Index", "Web");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
            var success = await _accountService.Login(model);

            return success == false ? BadRequest("This user is not found exist") : RedirectToAction("Index", "Web");
        }
	}
}