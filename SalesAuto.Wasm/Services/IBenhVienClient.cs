using SalesAuto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public interface IBenhVienClient
    {
        Task<List<BenhVienVM>> GetAll();
        Task<List<BenhVienVM>> GetBenhVienForUser();
        Task<List<BenhVienVM>> GetBenhVienByEmail();
        Task<string> getBenhVienDangLamViec();
        Task SetBenhVienDangLamViec(string MaBenhVien);
    }
    
}