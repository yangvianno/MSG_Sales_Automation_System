using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using SalesAuto.Models;
using SalesAuto.Models.Entities;
using SalesAuto.Models.SearchModel;
using SalesAuto.Models.ViewModel;
using SalesAuto.Wasm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Pages
{
    public partial class ABRChuaXacDinhThucHienP
    {
        [Inject] private IABRClient aBRClient { get; set; }
        [Inject] IToastService toastService { get; set; }
        [Inject] ICommonUI commonUI { get; set; }
        [Inject] DialogService dialogService { get; set; }
        [Inject] IExportFile exportFile { get; set; }
        [Inject] IJSRuntime jsRuntime { get; set; }

        public DateTime TuNgay;
        public DateTime DenNgay;
        private string NhomCongViec = "Phẫu thuật";
        private List<string> listNhomCongViec;
        private List<ABRNhanVienThucHienVM> listChinh;
        private List<ABRNhanVien> listABRNhanVien;
        private List<ABRDanhMuc> listABRDanhMuc;
        private List<ABRThucHienCuoiVM> listThucHienCuoi;
        private Dictionary<ABRLoaiTinhTrangTimKiem, string> listTinhTrang;
        private ABRLoaiTinhTrangTimKiem TinhTrang;
        private bool NhanVienKhacHis=false;
        private bool DaXetDuyet = false;

        RadzenDataGrid<ABRNhanVienThucHienVM> danhMucGrid;        


        protected override async Task OnInitializedAsync()
        {
            TuNgay = DateTime.Now;
            DenNgay = DateTime.Now;
            listNhomCongViec = new List<string>();
            listNhomCongViec = await aBRClient.GetNhomCongViecThongKe();
            if (listNhomCongViec.IndexOf("All") < 0)
            {
                listNhomCongViec.Insert(0, "All");
            }            
            await LoadListABRNhanVien();            
            await LoadlistABRDanhMuc();
            await LoadListThucHienCuoi();
            listTinhTrang = new Dictionary<ABRLoaiTinhTrangTimKiem, string>();
            listTinhTrang.Add(ABRLoaiTinhTrangTimKiem.ToanBo,"Toàn bộ");
            listTinhTrang.Add(ABRLoaiTinhTrangTimKiem.DaLuu,"Đã lưu");
            listTinhTrang.Add(ABRLoaiTinhTrangTimKiem.ChuaLuu, "Chưa lưu");
            TinhTrang = ABRLoaiTinhTrangTimKiem.ChuaLuu;
            await LoadDanhSach();
        }

        private async Task LoadListThucHienCuoi()
        {
            listThucHienCuoi = await aBRClient.GetThucHienCuoi();
        }

        private async Task LoadlistABRDanhMuc()
        {
            commonUI.BusyDialog(dialogService, "Loading...");
            try
            {
                listABRDanhMuc = await aBRClient.GetDanhMucABR();                
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            dialogService.Close();
        }

        private async Task LoadListABRNhanVien()
        {
            listABRNhanVien = await aBRClient.LayDanhSachABRNhanVien();
        }

        private async Task LoadDanhSach()
        {
            commonUI.BusyDialog(dialogService, "Loading...");
            try
            {
                NhanVienThucHienSM nhanVienThucHienSM = new NhanVienThucHienSM()
                {
                    TuNgay = TuNgay,
                    DenNgay = DenNgay,
                    NhomCongViecThongKe = NhomCongViec,
                    TinhTrang = TinhTrang,
                    NhanVienThucHienKhacHis = NhanVienKhacHis
                };
                listChinh = await aBRClient.GetNhanVienThucHien(nhanVienThucHienSM);
                foreach (var item in listChinh)
                {
                    if (item.IDABRDanhMuc == null || item.IDABRDanhMuc == 0)
                    {

                    }
                    else
                    {
                        var a = listABRDanhMuc.Find(x => x.ID == item.IDABRDanhMuc);
                        if (a != null)
                        {
                            item.TenCongViecABR = a.TenCongViec;
                        }
                        else
                        {
                            item.TenCongViecABR = "Chưa xác định!";
                        }
                    }
                }
                DaXetDuyet = false;
                DateTime NgayTam = TuNgay;
                int Thang = TuNgay.Month;
                int Nam = TuNgay.Year;
                while (!DaXetDuyet && (Nam * 12 + Thang) <= (DenNgay.Year * 12 + DenNgay.Month))
                {
                    DaXetDuyet = await aBRClient.CheckDaXetDuyetTheoNgay(TuNgay);
                    if (!DaXetDuyet)
                    {
                        DaXetDuyet = await aBRClient.CheckDaXetDuyetTheoNgay(DenNgay);
                    }
                    NgayTam = NgayTam.AddMonths(1);
                    Thang = NgayTam.Month;
                    Nam = NgayTam.Year;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowInfo("Load bị lỗi+" + ex.Message);
            }

            dialogService.Close();
        }
        void InsertRow()
        {
            toastService.ShowInfo("Thêm danh muc mói");
            danhMucGrid.InsertRow(new ABRNhanVienThucHienVM());
            ThemMoi = true;
        }
        async Task OnUpdateRow(ABRNhanVienThucHienVM item)
        {
        }

        void OnCreateRow(ABRNhanVienThucHienVM item)
        {
        }

        ABRNhanVienThucHienVM OldRow = new ABRNhanVienThucHienVM();
        private bool ThemMoi;

        void EditRow(ABRNhanVienThucHienVM item)
        {
            danhMucGrid.EditRow(item);
            ThemMoi = false;
            GanGiaTri(OldRow, item);
            if(String.IsNullOrEmpty(item.TenNhanVienABR))
            {
                if (string.IsNullOrEmpty(item.TenNhanVienHIS))
                {
                    // Lấy người cuối cùng thực hiện 
                    if (listThucHienCuoi != null)
                    {
                        var found = listThucHienCuoi.Find(x => x.IDMapCongViecABRHIS == item.IDMapABRHIS);
                        if (found != null)
                        {
                            item.TenNhanVienABR = found.TenNhanVien;
                            item.MaNhanVienABR = found.IDABRNhanVien;
                        }
                    }
                }
                else
                {
                    item.TenNhanVienABR = item.TenNhanVienHIS;
                    item.MaNhanVienABR = item.MaNhanVienHIS;
                }
                
            }
            if(item.IDABRDanhMuc == null || item.IDABRDanhMuc == 0)
            {
                var a = listABRDanhMuc.Find(x => x.TenCongViec == item.TenCongViecABR);
                if (a!=null)
                {
                    item.IDABRDanhMuc = a.ID;
                }
            }
            
        }

        private async Task CopyRow(ABRNhanVienThucHienVM item)
        {
            commonUI.BusyDialog(dialogService, "Đang lưu...");
            try
            {
                int currentPage = danhMucGrid.CurrentPage;
                await danhMucGrid.LastPage();
                int SoTrang = danhMucGrid.CurrentPage;
                for (int i = 0; i <= SoTrang; i++)
                {
                    await danhMucGrid.GoToPage(i);
                    foreach (var localitem in danhMucGrid.PagedView)
                    {

                        if ((localitem.IDABRDanhMuc == null || localitem.IDABRDanhMuc == 0)
                            && (string.IsNullOrEmpty(localitem.TenCongViecABR)))
                        {
                            localitem.IDABRDanhMuc = item.IDABRDanhMuc;
                            localitem.TenCongViecABR = item.TenCongViecABR;
                            await danhMucGrid.UpdateRow(localitem);
                        }
                        if (string.IsNullOrEmpty(localitem.TenNhanVienHIS) && string.IsNullOrEmpty(localitem.TenNhanVienABR))
                        {
                            localitem.MaNhanVienHIS = item.MaNhanVienABR;
                            localitem.TenNhanVienHIS = item.TenNhanVienABR;
                            await danhMucGrid.UpdateRow(localitem);
                        }
                    }
                }
                await danhMucGrid.GoToPage(currentPage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            dialogService.Close();
        }

        void GanGiaTri(ABRNhanVienThucHienVM oldRow, ABRNhanVienThucHienVM newRow)
        {
            oldRow.ID_DSCV = newRow.ID_DSCV;
            oldRow.NgayThu = newRow.NgayThu;
            oldRow.MaBenhAn = newRow.MaBenhAn;
            oldRow.HOTENBN = newRow.HOTENBN;
            oldRow.NhomCongViecThongKe = newRow.NhomCongViecThongKe;
            oldRow.TenCongViecHIS = newRow.TenCongViecHIS;
            oldRow.NhomABR = newRow.NhomABR;
            oldRow.TenCongViecABR = newRow.TenCongViecABR;
            oldRow.MaNhanVienHIS = newRow.MaNhanVienHIS;
            oldRow.TenNhanVienHIS = newRow.TenNhanVienHIS;
            oldRow.IDMapABRHIS = newRow.IDMapABRHIS;
            oldRow.MaNhanVienABR = newRow.MaNhanVienABR;
            oldRow.TenNhanVienABR = newRow.TenNhanVienABR;
            oldRow.IDABRDanhMuc = newRow.IDABRDanhMuc;
        }

        async Task SaveRow(ABRNhanVienThucHienVM item)
        {            
            if (item.MaNhanVienABR.HasValue && item.IDABRDanhMuc.HasValue && item.TenNhanVienABR != "")
            {
                ABRNhanVienThucHien aBRNhanVienThucHien = new ABRNhanVienThucHien()
                {
                    ID_DSCV = item.ID_DSCV,
                    IDABRNhanVien = (Guid)item.MaNhanVienABR,
                    IDMapDanhMucABRHIS= item.IDMapABRHIS,
                    IDABRDanhMuc = (int)item.IDABRDanhMuc,
                    DoanhThuTinhABR = item.DoanhThuTinhABR,
                    SoLuong = item.SoLuong
                };
                bool result = await aBRClient.SaveABRNhanVienThucHien(aBRNhanVienThucHien);                
                if (result)
                {
                    toastService.ClearSuccessToasts();
                    toastService.ShowSuccess("Lưu " + item.HOTENBN + " thành công ");
                    
                    var a = listABRNhanVien.Find(x => x.ID == item.MaNhanVienABR);
                    if (a!=null)
                    {
                        item.TenNhanVienABR = a.TenNhanVien;
                    }
                    
                    var dm = listABRDanhMuc.Find(x => x.ID == item.IDABRDanhMuc);
                    if (dm != null)
                    {
                        item.TenCongViecABR = dm.TenCongViec;
                    }                    
                    await danhMucGrid.UpdateRow(item);
                }
                else
                {
                    toastService.ShowError("Lỗi");

                }                
            }
            else if (item.MaNhanVienHIS.HasValue && !String.IsNullOrEmpty(item.TenCongViecABR))
            {
                var a = listABRDanhMuc.Find(X => X.TenCongViec == item.TenCongViecABR);
                if (a != null)
                {
                    ABRNhanVienThucHien aBRNhanVienThucHien = new ABRNhanVienThucHien()
                    {
                        ID_DSCV = item.ID_DSCV,
                        IDABRNhanVien = (Guid)item.MaNhanVienHIS,
                        IDMapDanhMucABRHIS = item.IDMapABRHIS,
                        IDABRDanhMuc = (int)a.ID
                    };

                    bool result = await aBRClient.SaveABRNhanVienThucHien(aBRNhanVienThucHien);
                    if (result)
                    {
                        toastService.ClearSuccessToasts();
                        toastService.ShowSuccess("Lưu " + item.HOTENBN + " thành công");
                        item.MaNhanVienABR = (Guid)item.MaNhanVienHIS;
                        item.IDABRDanhMuc = (int)a.ID;
                        item.TenNhanVienABR = item.TenNhanVienHIS;
                        await danhMucGrid.UpdateRow(item);
                    }
                    else
                    {
                        toastService.ShowError("Lỗi");
                    }
                }
                else
                {
                    toastService.ShowWarning("Vui lòng chọn nhân viên và dịc vụ ABR");
                }
            }
            else
            {
                toastService.ShowWarning("Vui lòng chọn nhân viên và dịc vụ ABR");
            }
        }

        void CancelEdit(ABRNhanVienThucHienVM item)
        {
            danhMucGrid.CancelEditRow(item);
            if (!ThemMoi)
            {
                GanGiaTri(item, OldRow);
            }
        }

        async Task DeleteRow(ABRNhanVienThucHienVM item, bool Confirm=false)
        {
            if (item.MaNhanVienABR.HasValue && item.IDABRDanhMuc.HasValue && item.TenNhanVienABR != "")
            {
                bool Result = true;
                if (Confirm)
                {
                    Result = (bool)await dialogService.Confirm("Bạn thực sự muốn xóa?", "Xóa", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
                }
                if (Result)
                {
                
                    ABRNhanVienThucHien aBRNhanVienThucHien = new ABRNhanVienThucHien()
                    {
                        ID_DSCV = item.ID_DSCV,
                        IDMapDanhMucABRHIS = item.IDMapABRHIS,
                        IDABRNhanVien = (Guid)item.MaNhanVienABR,
                        IDABRDanhMuc = (int)item.IDABRDanhMuc
                    };
                    bool ketqua = await aBRClient.DeleteABRNhanVienThucHien(aBRNhanVienThucHien);
                    if (ketqua)
                    {
                        toastService.ClearInfoToasts();
                        toastService.ShowSuccess("Xóa thành công");                        
                        item.MaNhanVienABR = null;
                        item.TenNhanVienABR = null;
                        item.IDABRDanhMuc = null;
                        await danhMucGrid.UpdateRow(item);
                    }
                    else
                    {
                        toastService.ShowError("Xóa bị lỗi!");
                    }
                }
            }
            else
            {
                toastService.ShowSuccess("Dịch vụ này chưa lưu nên không cần xóa!");
            }

        }

        public async Task LuuToanTrang()
        {
            //int start = danhMucGrid.CurrentPage * danhMucGrid.PageSize;
            //int end = ((danhMucGrid.CurrentPage+1) * danhMucGrid.PageSize<= danhMucGrid.Count? (danhMucGrid.CurrentPage + 1) * danhMucGrid.PageSize: danhMucGrid.Count);            
            commonUI.BusyDialog(dialogService,"Đang lưu...");
            try
            {
                foreach (var item in danhMucGrid.PagedView)
                {
                    await SaveRow(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            dialogService.Close();
        }

        public async Task LuuToanBo()
        {            
            bool Result = (bool)await dialogService.Confirm("Bạn thực sự lưu toàn bộ?", "Lưu", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (Result)
            {
                await danhMucGrid.LastPage();
                int SoTrang = danhMucGrid.CurrentPage;
                for (int i=0;i<= SoTrang; i++)
                {
                    await danhMucGrid.GoToPage(i);
                    await LuuToanTrang();
                }
            }
        }

        public async Task CopyMoiThucHienToanBo()
        {
            commonUI.BusyDialog(dialogService, "Đang load...");
            try
            {
                int currentPage = danhMucGrid.CurrentPage;
                await danhMucGrid.LastPage();
                int SoTrang = danhMucGrid.CurrentPage;
                for (int i = 0; i <= SoTrang; i++)
                {
                    await danhMucGrid.GoToPage(i);
                    foreach (var localitem in danhMucGrid.PagedView)
                    {

                        if (string.IsNullOrEmpty(localitem.TenNhanVienHIS) && string.IsNullOrEmpty(localitem.TenNhanVienABR))
                        {
                            var found = listThucHienCuoi.Find(x => x.IDMapCongViecABRHIS == localitem.IDMapABRHIS);
                            if (found != null)
                            {
                                localitem.MaNhanVienHIS = found.IDABRNhanVien;
                                localitem.TenNhanVienHIS = found.TenNhanVien;
                                await danhMucGrid.UpdateRow(localitem);
                            }
                        }
                    }
                }
                await danhMucGrid.GoToPage(currentPage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            dialogService.Close();
        }

        public async Task XoaTrang()
        {            
            commonUI.BusyDialog(dialogService, "Đang lưu...");
            try
            {
                foreach (var item in danhMucGrid.PagedView)
                {
                    await DeleteRow(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            dialogService.Close();
        }
        public async Task XoaToanBo()
        {
            bool Result = (bool)await dialogService.Confirm("Bạn thực sự muốn xóa toàn bộ?", "Lưu", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (Result)
            {
                await danhMucGrid.LastPage();
                int SoTrang = danhMucGrid.CurrentPage;
                for (int i = 0; i <= SoTrang; i++)
                {
                    await danhMucGrid.GoToPage(i);
                    await XoaTrang();
                }
            }
        }

        void LoadData(LoadDataArgs args, ABRNhanVienThucHienVM item)
        {            
            args.Filter = "BBB";
            InvokeAsync(StateHasChanged);
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
