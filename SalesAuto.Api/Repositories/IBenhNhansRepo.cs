using SalesAuto.Models.SearchModel;
using SalesAuto.Models.ViewModel;
using SalesAuto.Models.ViewModel.SeekWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public interface IBenhNhansRepo
    {
        Task<PageList<BenhNhanKhamVM>> GetBenhNhanKhamListPage(BenhNhanSM benhNhanSM, string MaBenhVien="O");
        Task<IEnumerable<BenhNhanKhamVM>> GetBenhNhanKhamList(BenhNhanSM benhNhanSM, string MaBenhVien="O");
        Task<IEnumerable<BenhVienKhamVM>> GetBenhVienKhamList(BenhNhanSM benhNhanSM, string MaBenhVien="O");
        
    }
}
