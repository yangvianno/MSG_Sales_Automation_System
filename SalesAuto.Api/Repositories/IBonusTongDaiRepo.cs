using OfficeOpenXml;
using SalesAuto.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public interface IBonusTongDaiRepo
    {
        Task<ExcelPackage> createBonusExcel(int thang, int nam, string MaBenhVien = "O");
        Task<ExcelPackage> createBonusExcelChiTiet(int thang, int nam, string MaBenhVien = "O");
        Task<List<KPIVM>> GetKIPNhanVien(int Thang, int Nam, string MaBenhVien = "O");
        Task<List<KPIVM>> GetKIPPhauThuat(int Thang, int Nam, string MaBenhVien = "O");
        Task<List<KPIVM>> GetTarget(int Thang, int Nam, string MaBenhVien = "O");
    }
}