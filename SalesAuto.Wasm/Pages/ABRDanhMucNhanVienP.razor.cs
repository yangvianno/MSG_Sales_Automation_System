using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using SalesAuto.Models.Entities;
using SalesAuto.Models.ViewModel;
using SalesAuto.Wasm.Components;
using SalesAuto.Wasm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Pages
{
    public partial class ABRDanhMucNhanVienP
    {
        [Inject] private IABRClient aBRClient { get; set; }
        [Inject] ICommonUI commonUI { get; set; }
        [Inject] IToastService toastService { get; set; }
        [Inject] DialogService dialogService { get; set; }
        [Inject] IExportFile exportFile { get; set; }
        [Inject] IJSRuntime jsRuntime { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }

        RadzenDataGrid<ABRNhanVien> danhMucGrid;

        private List<ABRNhanVien> listChinh;
        private List<AbrPool> listPool;
        private List<ABRDanhMuc> listABRDanhMuc;
        private List<BenhVienVM> BenhVienVMs;

        bool ThemMoi = false;

        protected override async Task OnInitializedAsync()
        {
            BenhVienVMs = await aBRClient.GetBenhVien();
            listPool = await aBRClient.GetDanhSachPool();
            listABRDanhMuc = await aBRClient.GetDanhMucABR();            
            await LoadDanhMuc();
            
            
        }

        async Task LoadDanhMuc()
        {
            commonUI.BusyDialog(dialogService, "Loading...");
            try
            {
                await InvokeAsync(StateHasChanged);
                listChinh = await aBRClient.LayDanhSachABRNhanVien();
                foreach (var item in listChinh)
                {
                    item.PoolDuocHuong = await aBRClient.GetNhanVienHuongPool(item.ID);
                    if (item.PoolDuocHuong != null)
                    {
                        item.CacPoolHuong = "";
                        foreach (var p in item.PoolDuocHuong)
                        {
                            item.CacPoolHuong += p.TenPool + ",";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            dialogService.Close();  
        }

        void InsertRow()
        {
            toastService.ShowInfo("Thêm danh muc mói");
            danhMucGrid.InsertRow(new ABRNhanVien());
            ThemMoi = true;
        }
        async Task OnUpdateRow(ABRNhanVien item)
        {
        }

        void OnCreateRow(ABRNhanVien item)
        {
        }

        async Task ChonABRPool(ABRNhanVien item)
        {
            var parameters = new ModalParameters();
            parameters.Add("NhanVien", item);
            parameters.Add("listPool", listPool);
            var result = Modal.Show<ABRNhanVienPoolDuocHuong>("Chọn Pool được hưởng", parameters);            
            await result.Result;
            item.PoolDuocHuong = await aBRClient.GetNhanVienHuongPool(item.ID);
            await danhMucGrid.UpdateRow(item);
        }
        async Task ABRTinhRieng(ABRNhanVien item)
        {
            var parameters = new ModalParameters();
            parameters.Add("IDNhanVien", item.ID);            
            var result = Modal.Show<ABRDanhMucNhanVienCom>("Mức hưởng ARB theo nhân viên " + item.TenNhanVien, parameters);
            await result.Result;
            item.PoolDuocHuong = await aBRClient.GetNhanVienHuongPool(item.ID);
            await danhMucGrid.UpdateRow(item);
        }

        async Task ChonABRLuonDuocHuong(ABRNhanVien item)
        {
            var parameters = new ModalParameters();
            parameters.Add("NhanVien", item);
            parameters.Add("listABRDanhMuc", listABRDanhMuc);
            item.ABRLuonDuocHuong = await aBRClient.GetNhanVienABRLuonDuocHuong(item.ID);
            var result = Modal.Show<ABRNhanVienABRLuonDuocHuong>("Chọn Pool được hưởng", parameters);
            await result.Result;
            item.ABRLuonDuocHuong = await aBRClient.GetNhanVienABRLuonDuocHuong(item.ID);
            await danhMucGrid.UpdateRow(item);
        }

        ABRNhanVien OldRow = new ABRNhanVien();

        void EditRow(ABRNhanVien item)
        {
            danhMucGrid.EditRow(item);
            ThemMoi = false;
            GanGiaTri(OldRow, item);
        }

        void GanGiaTri(ABRNhanVien OldRow, ABRNhanVien aBRDanhMuc)
        {
            OldRow.MaNhanVien = aBRDanhMuc.MaNhanVien;
            OldRow.TenNhanVien = aBRDanhMuc.TenNhanVien;
            OldRow.ChucDanh = aBRDanhMuc.ChucDanh;
            OldRow.TinhTrucTiep = aBRDanhMuc.TinhTrucTiep;
            OldRow.HuongTrucTiep = aBRDanhMuc.HuongTrucTiep;
            OldRow.HuongGianTiep = aBRDanhMuc.HuongGianTiep;
            OldRow.PhongBan = aBRDanhMuc.PhongBan;
            OldRow.ThuocPool = aBRDanhMuc.ThuocPool;
            OldRow.HeSoGianTiep = aBRDanhMuc.HeSoGianTiep;
            OldRow.MaBenhVien = aBRDanhMuc.MaBenhVien;
        }

        async Task SaveRow(ABRNhanVien item)
        {
            bool result = await aBRClient.SaveABRNhanVien(item);
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
                    //await LoadDanhMuc();
                    toastService.ShowSuccess("Lưu thành công");
                    await danhMucGrid.UpdateRow(item);    
                    ThemMoi = false;
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    toastService.ShowError("Lỗi");
                }

            }
        }

        void CancelEdit(ABRNhanVien item)
        {
            danhMucGrid.CancelEditRow(item);
            if (!ThemMoi)
            {
                GanGiaTri(item, OldRow);
            }
        }

        async Task DeleteRow(ABRNhanVien item)
        {
            bool Result = (bool)await dialogService.Confirm("Bạn thực sự muốn xóa?", "Xóa", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No"});
            if (Result)
            {
                bool ketqua = await aBRClient.DeleteABRNhanVien(item.ID);
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
        private async Task Export()
        {
            var pkg = await exportFile.SaveFile(listChinh);
            var fileBytes = pkg.GetAsByteArray();
            pkg.Dispose();
            var fileName = $"BaoCao.xlsx";
            await jsRuntime.InvokeAsync<object>("saveAsFile", fileName, Convert.ToBase64String(fileBytes));
        }
    }
}
