using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesAuto.Models.SearchModel;
using SalesAuto.Models.ViewModel;
using SalesAuto.Models.ViewModel.SeekWork;
using SalesAuto.Wasm.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Pages
{
    public partial class BenhNhanKham
    {        
        [Inject] private IBenhNhanClient benhNhanClient { get; set; }
        [Inject] private IJSRuntime jsRuntime { get; set;}
        [Inject] private HttpClient httpClient { get; set; }
        [Inject] private IBenhVienClient benhVienClient { get; set; }

        private List<BenhVienKhamVM> benhVienKhams;
        private List<BenhNhanKhamVM> benhNhanKhams;
        private List<BenhVienVM> benhVienVms;
        private PageList<BenhNhanKhamVM> pageListBenhNhanKhams;

        public MetaData metaData { get; set; } = new MetaData();

        private BenhNhanSM benhNhanSM = new();
        protected override async Task OnInitializedAsync()
        {
            if (benhNhanSM != null)
            {
                benhNhanSM.TuNgay = DateTime.Now.Date;
                benhNhanSM.DenNgay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                benhNhanSM.Loai = "";
                benhNhanSM.Nguon = "";
            }
            benhVienVms = await benhVienClient.GetAll();
            await SearchAll();
        }

        private async Task SearchAll()
        {
            benhNhanKhams = null;
            benhVienKhams = null;
            benhVienKhams = await benhNhanClient.GetBenhVienKhamList(benhNhanSM);
            await SearchBenhNhan();
        }

        private async Task SearchBenhNhan()
        {
            benhNhanKhams = null;
            pageListBenhNhanKhams = await benhNhanClient.GetBenhNhanKhamListPage(benhNhanSM);
            metaData = pageListBenhNhanKhams.MetaData;
            benhNhanKhams = pageListBenhNhanKhams.Items;
        }


        public int isDownLoadStarted { get; set; } = 1;
        public async Task DownLoadExcel()
        {
            if (await jsRuntime.InvokeAsync<bool>("confirm", $"Confirm download?"))
            {
                isDownLoadStarted = 1;                
                var postBody = new { Title = "Blazor POST Request Example" };
                var response = await httpClient.PostAsJsonAsync("api/ReportExcel", benhNhanSM);
                response.EnsureSuccessStatusCode();
                isDownLoadStarted = 2;
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var files = fileBytes.ToArray();
                var file = File.Create("benhNhan.xlsx");
                file.Write(files);
                file.Close();

                var fileName = $"BenhNhan.xlsx";                   
                await jsRuntime.InvokeAsync<object>("saveAsFile", fileName,Convert.ToBase64String(fileBytes));
            }
        }

        private async Task SelectedPage(int page)
        {
            benhNhanSM.pageNumber = page;
            await SearchBenhNhan();
        }

    }
}
