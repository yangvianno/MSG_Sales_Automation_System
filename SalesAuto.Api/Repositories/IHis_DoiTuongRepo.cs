using SalesAuto.Models.Entities.HisDoiTuong;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public interface IHis_DoiTuongRepo
    {
        Task<BangGiaTheoDoiTuong> DeleteBangGiaTheoDoiTuong(string MaBenhVien, BangGiaTheoDoiTuong item);
        Task<List<BangGiaTheoDoiTuong>> GetBangGiaTheoDoiTuong(string MaBenhVien, Guid ID_LoaiDoiTuong);
        Task<List<LoaiDoiTuong>> GetDanhSachDoiTuong(string MaBenhVien);
        Task<BangGiaTheoDoiTuong> SaveBangGiaTheoDoiTuong(string MaBenhVien, BangGiaTheoDoiTuong item);
    }
}