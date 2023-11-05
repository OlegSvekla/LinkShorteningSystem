using LinkShorteningSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        Task<bool> Register(RegisterViewModel model);
        Task<bool> Login(LoginViewModel model);
    }
}
