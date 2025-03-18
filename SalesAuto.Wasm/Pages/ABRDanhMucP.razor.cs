using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using SalesAuto.Models.Entities;
using SalesAuto.Wasm.Components;
using SalesAuto.Wasm.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Pages
{    

    public partial class ABRDanhMucP
    {
        [Inject] private IABRClient aBRClient { get; set; }
        [Inject] IToastService toastService { get; set; }
        [Inject] ICommonUI commonUI { get; set; }        
        [Inject] DialogService dialogService { get; set; }
        [Inject] IExportFile exportFile { get; set; }
        [Inject] IJSRuntime jsRuntime { get; set; }

        RadzenDataGrid<ABRDanhMuc> danhMucABRGrid;

        [CascadingParameter]
        public IModalService Modal { get; set; }

        private List<ABRDanhMuc> listDanhMucABR;
        private List<ABRNhom> listNhomABR;
        private List<ABRLoaiVaiTro> listABRLoaiTinh;
        bool ThemMoi = false;
        bool isloading = true;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                isloading = true;
                listNhomABR = await aBRClient.GetNhomABR();
                listABRLoaiTinh = await aBRClient.GetDanhSachLoaiVaiTro();
                await LoadDanhMucABR();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            isloading= false;

        }
        
        async Task LoadDanhMucABR()
        {
            isloading = true;
            try
            {
                listDanhMucABR = await aBRClient.GetDanhMucABR();
                await LoadHuongBacThang();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            isloading= false;
        }

        async Task LoadHuongBacThang()
        {
            if(listDanhMucABR!=null)
            {
                foreach(var item in listDanhMucABR)
                {
                    item.HuongBacThangs = await aBRClient.GetDanhSachHuongBacThang(item.ID);
                }
            }
        }

        void InsertRow()
        {
            toastService.ShowInfo("Thêm danh muc mói");            
            danhMucABRGrid.InsertRow(new ABRDanhMuc());
            ThemMoi = true;
        }

        ABRDanhMuc OldRow = new ABRDanhMuc(); 

        void EditRow(ABRDanhMuc aBRDanhMuc)
        {            
            danhMucABRGrid.EditRow(aBRDanhMuc);
            ThemMoi = false;
            GanGiaTri(OldRow, aBRDanhMuc);
        }

        void GanGiaTri (ABRDanhMuc OldRow, ABRDanhMuc aBRDanhMuc)
        {
            OldRow.Code = aBRDanhMuc.Code;
            OldRow.TenCongViec = aBRDanhMuc.TenCongViec;
            OldRow.MaNhomABR = aBRDanhMuc.MaNhomABR;
            OldRow.MucHuongVND = aBRDanhMuc.MucHuongVND;
            OldRow.MucHuongPhanTram = aBRDanhMuc.MucHuongPhanTram;
            OldRow.TinhTheoBenhAn = aBRDanhMuc.TinhTheoBenhAn;
            OldRow.ChuongTrinhRieng = aBRDanhMuc.ChuongTrinhRieng;
        }

        async Task SaveRow(ABRDanhMuc aBRDanhMuc)
        {
            commonUI.BusyDialog(dialogService,"Saving...");
            try
            {
                bool result = await aBRClient.SaveDanhMucABR(aBRDanhMuc);
                if (ThemMoi)
                {
                    if (result)
                    {
                        await LoadDanhMucABR();
                        toastService.ShowSuccess("Lưu thành công");
                        await danhMucABRGrid.LastPage();
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
                        await danhMucABRGrid.UpdateRow(aBRDanhMuc);
                        toastService.ShowSuccess("Lưu thành công");
                        ThemMoi = false;
                    }
                    else
                    {
                        toastService.ShowError("Lỗi");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            dialogService.Close();
        }

        async Task OnUpdateRow(ABRDanhMuc aBRDanhMuc)
        {         
        }

        void OnCreateRow(ABRDanhMuc aBRDanhMuc)
        {         
        }

        void CancelEdit(ABRDanhMuc aBRDanhMuc)
        {         
            danhMucABRGrid.CancelEditRow(aBRDanhMuc);
            if (!ThemMoi)
            {
                GanGiaTri(aBRDanhMuc, OldRow);
            }            
        }

        async Task DeleteRow(ABRDanhMuc aBRDanhMuc)
        {
            try
            {
                bool Result = (bool)await dialogService.Confirm("Are you sure?", "MyTitle", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
                if (Result)
                {
                    commonUI.BusyDialog(dialogService, "Delete...");
                    bool ketqua = await aBRClient.DeleteDanhMucABR(aBRDanhMuc.ID);
                    if (ketqua)
                    {
                        toastService.ShowSuccess("Lưu thành công");
                        listDanhMucABR.Remove(aBRDanhMuc);
                        await danhMucABRGrid.Reload();
                    }
                    else
                    {
                        toastService.ShowError("Lưu bị lỗi");
                    }
                    dialogService.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
        async Task HuongBacThang(ABRDanhMuc item)
        {
            var parameters = new ModalParameters();
            parameters.Add("aBRDanhMuc", item);
            var result = Modal.Show<ABRHuongBacThangCom>("Hường theo số lượng " + item.TenCongViec, parameters);
            await result.Result;            
        }
        async Task ChonPoolHuongRieng(ABRDanhMuc item)
        {
            var parameters = new ModalParameters();
            parameters.Add("aBRDanhMuc", item);
            var result = Modal.Show<ABRChonPoolHuongTheoDanhMucCom>("Hường theo số lượng " + item.TenCongViec, parameters);
            await result.Result;
        }
        private async Task Export()
        {
            var pkg = await exportFile.SaveFile(listDanhMucABR);
            var fileBytes = pkg.GetAsByteArray();
            pkg.Dispose();
            var fileName = $"BaoCao.xlsx";
            await jsRuntime.InvokeAsync<object>("saveAsFile", fileName, Convert.ToBase64String(fileBytes));
        }


    }
}
