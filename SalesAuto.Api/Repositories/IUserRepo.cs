using SalesAuto.Models.Entities;
using SalesAuto.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public interface IUserRepo
    {
        Task<List<User>> GetUserList();
        Task Save(UserVM userVM);
    }
}