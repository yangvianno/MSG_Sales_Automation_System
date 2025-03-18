using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public interface IExportFile
    {
        Task<ExcelPackage> SaveFile(IEnumerable<IDictionary<string, object>> jsonItems);
        Task<ExcelPackage> SaveFile<T>(List<T> jsonItems);
    }
}
