using SalesAuto.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public interface IKPIMonthlyClient
    {
        Task<List<KPIMonthly>> GetList(int Nam);
        Task<bool> Save(KPIMonthly kPIMonthly);
    }
}