using LinkShorteningSystem.Web.Models;

namespace LinkShorteningSystem.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        Task<bool> Register(RegisterViewModel model);
        Task<bool> Login(LoginViewModel model);
    }
}