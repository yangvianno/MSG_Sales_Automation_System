using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public interface IABRLoadFileDanhGiaNhanVien
    {
        public Task<List<ABRDanhGiaNhanVien>> loadFile(Stream input);
        Task<List<ABRNgayCong>> LoadFileNgayCong(Stream input);
    }
}
