using SalesAuto.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public interface IChiTieuSoLuongRepo
    {
        Task SaveChiTieuLasikFileToDataBase(string file);
        Task<List<ChiTieuSoLuong>> GetChiTieuSoLuong(int MaLoaiChiTieu, int Nam, string MaBenhVien);
        Task<List<LoaiChiTieu>> GetAllLoaiThiTieu();
        Task<ChiTieuSoLuong>  Save(ChiTieuSoLuong chiTieuSoLuong);
    }
}