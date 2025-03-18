using SalesAuto.Models;
using SalesAuto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public interface IAuthService
    {
        Task<int> ChangePass(ChangePassVM item);
        Task<Guid> GetUserID();
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task Logout();
        Task<LoginResponse> SetLoginMsal(LoginResponse loginResponse, string UserName, string msalRoles, Dictionary<string, object> claims);
    }
}
