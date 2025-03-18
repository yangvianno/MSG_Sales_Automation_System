using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public interface IDailyReportRepo
    {
        public Task<string> GetDailyReportMatString();
        public Task<string> GetDailyReportMatString(DateTime TuNgay, DateTime DenNgay);
        public Task<string> GetDailyReportDaKhoaString();
        Task<string> GetDailyReportBenhVienString(string MaBenhVien);        
        Task<string> GetDailyReportMatSumString();
        Task<string> GetDailyReportMatSumString(DateTime TuNgay, DateTime DenNgay);

        Task CreateExcel(ExcelWorksheet Sheet, DataTable table, bool TinhTong = false, bool TangGiamTuan = false, bool TangGiamNam = false, bool InDauDong = true,int startDong=0, int startCot=0);
        Task<DataTable> GetDailyReportMatTable(DateTime TuNgay, DateTime DenNgay);
        Task<DataTable> GetReportMatTable();
        Task<DataTable> GetDailyReportMatSumTable();
        Task<DataTable> GetDailyReportMatSum(DateTime TuNgay, DateTime DenNgay);
        Task<DataTable> GetDailyReportBenhVienMatSumTable();
        Task<DataTable> GetDailyReportBenhVienMatSumTable(DateTime TuNgay, DateTime DenNgay);
        Task<string> GetDailyReportDaKhoaSumString();
        Task<string> GetDailyReportDaKhoaSumString(DateTime TuNgay, DateTime DenNgay);
        Task<string> GetDailyReportDaKhoaString(DateTime TuNgay, DateTime DenNgay);
        Task<DataTable> GetDailyReportDaKhoaSumTable();
        Task<DataTable> GetReportDaKhoaTable();      
        Task<DataTable> GetDailyReportDaKhoaTable(DateTime TuNgay, DateTime DenNgay);
        Task<DataTable> GetDailyReportBenhVienDaKhoaSumTable();
    }
}
