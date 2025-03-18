using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    interface IChiTieuSoLuongClient
    {
        public Task<List<LoaiChiTieu>> GetLoaiChiTieu();

        public Task<List<ChiTieuSoLuong>> GetChiTieuSoLuong(int MaLoaiChiTieu, int nam);

        public Task<bool> Save(ChiTieuSoLuong chiTieuSoLuong);
    }
}
