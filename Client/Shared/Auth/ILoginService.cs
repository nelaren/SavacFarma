using SavacFarma.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SavacFarma.Client.Shared.Auth
{
    public interface ILoginService
    {
        Task HandlingTokenExpiration();
        Task Login(UserToken token);
        Task Logout();
    }
}
