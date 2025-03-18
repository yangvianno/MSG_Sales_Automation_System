using SalesAuto.Models.SearchModel;
using SalesAuto.Models.ViewModel;
using SalesAuto.Models.ViewModel.SeekWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public class BenhNhanClient : IBenhNhanClient
    {
        private readonly HttpClient httpClient;

        public BenhNhanClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<BenhNhanKhamVM>> GetBenhNhanKhamList(BenhNhanSM benhNhanSM)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/benhNhanKham/searchBN", benhNhanSM);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<BenhNhanKhamVM>>();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<PageList<BenhNhanKhamVM>> GetBenhNhanKhamListPage(BenhNhanSM benhNhanSM)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/benhNhanKham/searchBNPage", benhNhanSM);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PageList<BenhNhanKhamVM>>();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<BenhVienKhamVM>> GetBenhVienKhamList(BenhNhanSM benhNhanSM)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/benhNhanKham/searchBV", benhNhanSM);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<BenhVienKhamVM>>();
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
