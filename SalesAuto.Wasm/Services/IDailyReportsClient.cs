using SalesAuto.Models.ViewModel;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public interface IDailyReportsClient
    {
        Task<string> GetDailyReportBenhVienString();
        Task<string> GetDailyReportMat();
        Task<string> GetDailyReportMatSum();
        Task<string> GetDailyReportMatSumTuan(TuanVM tuan);
        Task<string> GetDailyReportMatTuan(TuanVM tuan);
    }
}