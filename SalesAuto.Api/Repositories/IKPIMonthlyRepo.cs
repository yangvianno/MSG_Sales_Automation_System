using SalesAuto.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public interface IKPIMonthlyRepo
    {
        Task<List<KPIMonthly>> GetList(int nam, string MaBenhVien="O");
        Task<KPIMonthly> Save(KPIMonthly kPIMonthly);
    }
}