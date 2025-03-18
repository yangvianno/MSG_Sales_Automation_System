using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public interface IReportExcelClient
    {
        Task<IEnumerable<IDictionary<string, object>>> GetDanhSachBook(DateTime TuNgay, DateTime DenNgay);

        Task<IEnumerable<IDictionary<string, object>>> GetDanhSachLead(DateTime TuNgay, DateTime DenNgay);

        Task<IEnumerable<IDictionary<string, object>>> GetDanhSachKham(DateTime TuNgay, DateTime DenNgay);
        Task<IEnumerable<IDictionary<string, object>>> GetDanhSachPhauThuat(DateTime TuNgay, DateTime DenNgay);
        Task<IEnumerable<IDictionary<string, object>>> GetQuaTrinhKham(DateTime TuNgay, DateTime DenNgay);
        Task<IEnumerable<IDictionary<string, object>>> GetQuaTrinhPhauThuat(DateTime TuNgay, DateTime DenNgay);

    }
}