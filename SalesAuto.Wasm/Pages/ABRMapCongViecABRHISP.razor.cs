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
    public partial class ABRMapCongViecABRHISP
    {
        [CascadingParameter]
        public IModalService Modal { get; set; }

        [Inject] private IABRClient aBRClient { get; set; }
        [Inject] ICommonUI commonUI { get; set; }
        [Inject] IToastService toastService { get; set; }
        [Inject] DialogService dialogService { get; set; }
        [Inject] IExportFile exportFile { get; set; }
        [Inject] IJSRuntime jsRuntime { get; set; }

        RadzenDataGrid<ABRMapCongViecABRHISVM> danhMucGrid;

        private List<ABRCongViecHisVM> listConViecHis;
        private List<ABRDanhMuc> listDanhMucABR;
        private List<ABRMapCongViecABRHISVM> listChinh;
        private List<ABRNhom> listNhomABR;
        private bool HienCuaSoMoi= false;

        protected override async Task OnInitializedAsync()
        {            
            listConViecHis = await aBRClient.GetDanhMucCongViecHis();
            listDanhMucABR = await aBRClient.GetDanhMucABR();
            listNhomABR = await aBRClient.GetNhomABR();
            await LoadDanhSachMapCongViecABRHIS();
        }
        async Task LoadDanhSachMapCongViecABRHIS()
        {
            commonUI.BusyDialog(dialogService, "Loading");
            try
            {
                listChinh = await aBRClient.GetDanhSachMapCongViecABRHIS();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            dialogService.Close();
        }
        bool ThemMoi = false;
        void InsertRow()
        {
            toastService.ShowInfo("Thêm danh muc mói");
            danhMucGrid.InsertRow(new ABRMapCongViecABRHISVM() { QuyRa =1});
            ThemMoi = true;
        }

        ABRMapCongViecABRHISVM OldRow = new ABRMapCongViecABRHISVM();

        async Task EditRow(ABRMapCongViecABRHISVM aBRDanhMuc)
        {
            if (HienCuaSoMoi)
            {
                var parameters = new ModalParameters();
                parameters.Add("listConViecHis", listConViecHis);
                parameters.Add("listDanhMucABR", listDanhMucABR);
                parameters.Add("listNhomABR", listNhomABR);
                List<ABRMapCongViecABRHISVM> lst = new List<ABRMapCongViecABRHISVM> { aBRDanhMuc };
                parameters.Add("listChinh", lst);
                var result = Modal.Show<ABRMapCongViecABRHISEditCom>("Chỉnh sửa dịch vụ " + aBRDanhMuc.TenCongViec, parameters);
                await result.Result;
            }
            else
            {
                _ = danhMucGrid.EditRow(aBRDanhMuc);
                ThemMoi = false;
                await GanGiaTri(OldRow, aBRDanhMuc);
            }
        }
        async Task CopyRow(ABRMapCongViecABRHISVM aBRDanhMuc)
        {
            var newItem = new ABRMapCongViecABRHISVM();            
            newItem.IDDanhMucABR = aBRDanhMuc.IDDanhMucABR;
            newItem.MaCV = aBRDanhMuc.MaCV;
            newItem.QuyRa = 1;
            await danhMucGrid.InsertRow(newItem);            
            ThemMoi = true;
        }        

        async Task GanGiaTri(ABRMapCongViecABRHISVM OldRow, ABRMapCongViecABRHISVM row)
        {
            OldRow.Code = row.Code;
            OldRow.TenCongViec = row.TenCongViec;
            OldRow.NhomABR = row.NhomABR;
            OldRow.MucHuongVND = row.MucHuongVND;
            OldRow.MucHuongPhanTram = row.MucHuongPhanTram;
            OldRow.QuyRa = row.QuyRa;
            OldRow.DoanhThuTinhABR = row.DoanhThuTinhABR;
            OldRow.TinhTheoDoanhThu = row.TinhTheoDoanhThu;
        }
        
        async Task SaveRow(ABRMapCongViecABRHISVM row)
        {            
            ABRMapCongViecABRHIS item = new ABRMapCongViecABRHIS();
            item.ID = row.ID;
            item.MaCV = row.MaCV;
            item.IDDanhMucABR = row.IDDanhMucABR;
            item.QuyRa = row.QuyRa;
            item.DoanhThuTinhABR = row.DoanhThuTinhABR;
            item.TinhTheoDoanhThu = row.TinhTheoDoanhThu;
            item.RowGuid = row.RowGuid;
            var result = (await aBRClient.SaveMapCongViecABRHIS(item));
            if (ThemMoi)
            {
                if (result.RowGuid!=Guid.Empty)
                {
                    //await LoadDanhSachMapCongViecABRHIS();
                    row.RowGuid = result.RowGuid;
                    await UpdateInfo(row);
                    await danhMucGrid.UpdateRow(row);
                    toastService.ShowSuccess("Lưu thành công");
                    //await danhMucGrid.LastPage();
                    ThemMoi = false;
                }
                else
                {
                    toastService.ShowError("Lỗi");
                }
            }
            else
            {
                if (result.RowGuid != Guid.Empty)
                {
                    //await LoadDanhSachMapCongViecABRHIS();
                    row.RowGuid = result.RowGuid;
                    await UpdateInfo(row);
                    await danhMucGrid.UpdateRow(row);
                    toastService.ShowSuccess("Lưu thành công");                    
                    ThemMoi = false;
                }
                else
                {
                    toastService.ShowError("Lỗi");
                }

            }
        }

        private async  Task UpdateInfo(ABRMapCongViecABRHISVM row)
        {
            var congViecHis = listConViecHis.Find(x => x.MaCV == row.MaCV);
            if (congViecHis!=null)
            {
                row.TenCongViec = congViecHis.TenCongViec;
                row.NhomCongViecThongKe = congViecHis.NhomCongViecThongKe;
                row.KhoaPhauThuat = congViecHis.KhoaPhauThuat;
                row.LoaiPhauThuat = congViecHis.LoaiPhauThuat;
                row.GiaTien = congViecHis.GiaTien;
            }
            var abrCongViec = listDanhMucABR.Find(x => x.ID == row.IDDanhMucABR);
            if (abrCongViec!=null)
            {
                row.Code = abrCongViec.Code;
                row.NhomABR = listNhomABR.Find(x=>x.ID == abrCongViec.MaNhomABR)?.TenNhom;
                row.TenCongViecABR = abrCongViec.TenCongViec;
                row.MucHuongPhanTram = abrCongViec.MucHuongPhanTram;
                row.MucHuongVND = abrCongViec.MucHuongVND;
            }

        }

        async Task OnUpdateRow(ABRMapCongViecABRHISVM row)
        {
            //toastService.ShowInfo("Update Row");
        }

        void OnCreateRow(ABRMapCongViecABRHISVM row)
        {
            //toastService.ShowInfo("Ceeate Row");
        }

        async Task CancelEdit(ABRMapCongViecABRHISVM row)
        {
            //toastService.ShowInfo("Cancel Edit");
            danhMucGrid.CancelEditRow(row);
            if (!ThemMoi)
            {
                await GanGiaTri(row, OldRow);
            }
        }

        async Task DeleteRow(ABRMapCongViecABRHISVM row)
        {
            bool Result = (bool)await dialogService.Confirm("Are you sure?", "MyTitle", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (Result)
            {
                bool ketqua = await aBRClient.DeleteMapCongViecABRHIS(row.ID);
                if (ketqua)
                {
                    toastService.ShowSuccess("Xóa thành công");
                    listChinh.Remove(row);
                    await danhMucGrid.Reload();
                }
                else
                {
                    toastService.ShowError("Xóa bị lỗi");
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
