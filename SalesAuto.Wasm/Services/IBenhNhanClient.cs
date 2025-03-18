using SalesAuto.Models.SearchModel;
using SalesAuto.Models.ViewModel;
using SalesAuto.Models.ViewModel.SeekWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public interface IBenhNhanClient
    {
        Task<List<BenhNhanKhamVM>> GetBenhNhanKhamList(BenhNhanSM benhNhanSM);
        Task<PageList<BenhNhanKhamVM>> GetBenhNhanKhamListPage(BenhNhanSM benhNhanSM);
        Task<List<BenhVienKhamVM>> GetBenhVienKhamList(BenhNhanSM benhNhanSM);
    }
}
