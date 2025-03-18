using SalesAuto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public interface IBenhViensRepo
    {
        Task<IEnumerable<BenhVienVM>> GetAllBenhVienList();
        Task<IEnumerable<BenhVienVM>> GetBenhVienByEmail(string Email);
        Task<IEnumerable<BenhVienVM>> GetBenhVienByMaBenhVien(string MaBenhVien);
        Task<IEnumerable<BenhVienVM>> GetBenhVienByTenVietTat(string TenVietTat);
        Task<IEnumerable<BenhVienVM>> GetBenhVienByUserID(Guid userID);
    }
}
