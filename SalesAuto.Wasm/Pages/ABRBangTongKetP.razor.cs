using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using SalesAuto.Models;
using SalesAuto.Models.Entities;
using SalesAuto.Models.SearchModel;
using SalesAuto.Wasm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Pages
{
    public partial class ABRBangTongKetP
    {
        [Inject] private IABRClient ABRClient { get; set; }
        [Inject] IToastService ToastService { get; set; }
        [Inject] ICommonUI CommonUI { get; set; }
        [Inject] DialogService DialogService { get; set; }
        [Inject] IExportFile ExportFile { get; set; }
        [Inject] IJSRuntime JsRuntime { get; set; }
        public DateTime TuNgay;
        public DateTime DenNgay;
        private string NhomCongViec = "Phẫu thuật";        
        private List<string> listNhomCongViec;
        private IEnumerable<IDictionary<string, object>> listChinh;
        public IDictionary<string, object> colinfor;
        private Dictionary<ABRLoaiBaoCaoTongHop, string> listLoaiBaoCao;
        private ABRLoaiBaoCaoTongHop LoaiBaoCao;

        protected override async Task OnInitializedAsync()
        {
            TuNgay = DateTime.Now;
            DenNgay = DateTime.Now;
            listNhomCongViec = new List<string>();
            listNhomCongViec = await ABRClient.GetNhomCongViecThongKe();            
            listNhomCongViec.Insert(0, "All");
            listLoaiBaoCao = new Dictionary<ABRLoaiBaoCaoTongHop, string>();
            listLoaiBaoCao.Add(ABRLoaiBaoCaoTongHop.ToanBo, "Toàn bộ");
            listLoaiBaoCao.Add(ABRLoaiBaoCaoTongHop.TheoVND, "Theo VNĐ");
            listLoaiBaoCao.Add(ABRLoaiBaoCaoTongHop.ThePhanTram, "Theo phần trăm");
            LoaiBaoCao = ABRLoaiBaoCaoTongHop.ToanBo;
            await LoadDanhSach();
        }

        private async Task LoadDanhSach()
        {
            CommonUI.BusyDialog(DialogService, "Loading...");
            try
            {
                colinfor = null;
                NhanVienThucHienSM nhanVienThucHienSM = new NhanVienThucHienSM()
                {
                    TuNgay = TuNgay,
                    DenNgay = DenNgay,
                    NhomCongViecThongKe = NhomCongViec,
                    LoaiBaoTongHop = LoaiBaoCao
                };
                listChinh = await ABRClient.GetBaoDaThucHien(nhanVienThucHienSM);
                if (listChinh.Count() > 0)
                {
                    colinfor = listChinh.First();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            DialogService.Close();
        }

        private async Task Export()
        {
            var pkg = await ExportFile.SaveFile(listChinh);
            var fileBytes = pkg.GetAsByteArray();
            pkg.Dispose();
            var fileName = $"BaoCao.xlsx";
            await JsRuntime.InvokeAsync<object>("saveAsFile", fileName, Convert.ToBase64String(fileBytes));
        }
    }
}
