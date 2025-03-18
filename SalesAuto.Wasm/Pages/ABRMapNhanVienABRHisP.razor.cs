using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using SalesAuto.Models.Entities;
using SalesAuto.Models.ViewModel;
using SalesAuto.Wasm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Pages
{
    public partial class ABRMapNhanVienABRHisP
    {
        [Inject] private IABRClient aBRClient { get; set; }
        [Inject] IToastService toastService { get; set; }
        [Inject] DialogService dialogService { get; set; }
        RadzenDataGrid<ABRMapNhanVienABRHISVM> danhMucGrid;

        private List<ABRMapNhanVienABRHISVM> listChinh;
        private List<AbrNhanVienHisVM> listNhanVienHIS;
        private List<ABRNhanVien> listNhanVienABR;

        bool ThemMoi = false;

        protected override async Task OnInitializedAsync()
        {
            listNhanVienHIS = await  aBRClient.LayDanhSachNhanVienHIS();
            listNhanVienABR = await aBRClient.LayDanhSachABRNhanVien();
            await LoadDanhMuc();
        }

        async Task LoadDanhMuc()
        {
            listChinh = await aBRClient.GetDanhSachMapNhanVienABRHIS();
        }

        void InsertRow()
        {
            toastService.ShowInfo("Thêm danh muc mói");
            danhMucGrid.InsertRow(new ABRMapNhanVienABRHISVM());
            ThemMoi = true;
        }
        async Task OnUpdateRow(ABRMapNhanVienABRHISVM item)
        {
        }
        

        void OnCreateRow(ABRMapNhanVienABRHISVM item)
        {
        }

        ABRMapNhanVienABRHISVM OldRow = new ABRMapNhanVienABRHISVM();

        void EditRow(ABRMapNhanVienABRHISVM item)
        {
            danhMucGrid.EditRow(item);
            ThemMoi = false;
            GanGiaTri(OldRow, item);
        }

        void GanGiaTri(ABRMapNhanVienABRHISVM OldRow, ABRMapNhanVienABRHISVM aBRDanhMuc)
        {   
            OldRow.ID = aBRDanhMuc.ID;
            OldRow.MaNhanVienHIS = aBRDanhMuc.MaNhanVienHIS;
            OldRow.ChucDanhHIS = aBRDanhMuc.ChucDanhHIS;
            OldRow.TenNhanVienHIS = aBRDanhMuc.TenNhanVienHIS;
            OldRow.IDNhanVienABR = aBRDanhMuc.IDNhanVienABR;
            OldRow.MaNhanVienABR = aBRDanhMuc.MaNhanVienABR;
            OldRow.TenNhanVienABR = aBRDanhMuc.TenNhanVienABR;
            OldRow.ChucDanhABR = aBRDanhMuc.ChucDanhABR;
        }

        async Task SaveRow(ABRMapNhanVienABRHISVM item)
        {
            bool result = await aBRClient.SaveMapNhanVienABRHIS(item);
            if (ThemMoi)
            {
                if (result)
                {
                    await LoadDanhMuc();
                    toastService.ShowSuccess("Lưu thành công");
                    await danhMucGrid.LastPage();
                    ThemMoi = false;
                }
                else
                {
                    toastService.ShowError("Lỗi");
                }
            }
            else
            {
                if (result)
                {                       
                    toastService.ShowSuccess("Lưu thành công");
                    var found = listNhanVienABR.Find(x => x.ID == item.IDNhanVienABR);
                    if (found != null)
                    {
                        item.TenNhanVienABR = found.TenNhanVien;
                        item.ChucDanhABR = found.ChucDanh;
                        item.MaNhanVienABR = found.MaNhanVien;
                    }
                    ThemMoi = false;
                    await danhMucGrid.UpdateRow(item);
                }
                else
                {
                    toastService.ShowError("Lỗi");
                }

            }
        }

        void CancelEdit(ABRMapNhanVienABRHISVM item)
        {
            danhMucGrid.CancelEditRow(item);
            if (!ThemMoi)
            {
                GanGiaTri(item, OldRow);
            }
        }

        async Task DeleteRow(ABRMapNhanVienABRHISVM item)
        {
            bool Result = (bool)await dialogService.Confirm("Bạn thực sự muốn xóa?", "Xóa", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (Result)
            {
                bool ketqua = await aBRClient.DeleteMapNhanVienABRHIS(item.ID);
                if (ketqua)
                {
                    toastService.ShowSuccess("Xóa thành công");
                    listChinh.Remove(item);
                    await danhMucGrid.Reload();
                }
                else
                {
                    toastService.ShowError("Xóa bị lỗi!");
                }
            }

        }
    }
}
