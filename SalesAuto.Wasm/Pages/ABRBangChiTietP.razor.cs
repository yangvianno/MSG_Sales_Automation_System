using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using SalesAuto.Models.SearchModel;
using SalesAuto.Wasm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Pages
{
    public partial class ABRBangChiTietP
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
        private IEnumerable<IDictionary<string, object>> listChinh;
        private int count;
        

        protected override async Task OnInitializedAsync()
        {
            TuNgay = DateTime.Now;
            DenNgay = DateTime.Now;
            listNhomCongViec = new List<string>();
            listNhomCongViec = await aBRClient.GetNhomCongViecThongKe();
            
            listNhomCongViec.Insert(0, "All");
            await LoadDanhSach();
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
                    NhomCongViecThongKe = NhomCongViec
                };
                var result = await aBRClient.GetBaoCaoChiTietDaThucHien(nhanVienThucHienSM);
                listChinh = result;
                count = result.Count();
                //listChinh = ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            dialogService.Close();            
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
