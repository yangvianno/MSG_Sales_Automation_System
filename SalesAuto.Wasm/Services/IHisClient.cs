using SalesAuto.Models.Entities.HisDoiTuong;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public interface IHisClient
    {
        Task<BangGiaTheoDoiTuong> DeleteBangGiaTheoDoiTuong(BangGiaTheoDoiTuong item);
        Task<List<BangGiaTheoDoiTuong>> GetBangGiaTheoDoiTuong(Guid ID_LoaiDoiTuong);
        Task<List<LoaiDoiTuong>> GetDanhSachDoiTuong();
        Task<BangGiaTheoDoiTuong> SaveBangGiaTheoDoiTuong(BangGiaTheoDoiTuong item);
    }
}