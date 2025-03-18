using Microsoft.AspNetCore.WebUtilities;
using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public class ChiTieuSoLuongClient:IChiTieuSoLuongClient
    {
        private readonly HttpClient httpClient;

        public ChiTieuSoLuongClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<LoaiChiTieu>> GetLoaiChiTieu()
        {
            var response = await httpClient.GetFromJsonAsync<List<LoaiChiTieu>>("/api/ChiTieu/LoaiChiTieu");
            response.RemoveAll(x => x == null);
            return response;
        }

        public async Task<List<ChiTieuSoLuong>> GetChiTieuSoLuong(int MaLoaiChiTieu,int nam)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["MaLoaiChiTieu"] = MaLoaiChiTieu.ToString(),
                ["Nam"] = nam.ToString(),
            };
            string url = QueryHelpers.AddQueryString("/api/ChiTieu/ChiTieuSoLuong", queryStringParam);
            var response = await httpClient.GetFromJsonAsync<List<ChiTieuSoLuong>>(url);
            return response;
        }

        public async Task<bool> Save(ChiTieuSoLuong chiTieuSoLuong)
        {            
            var response = await httpClient.PostAsJsonAsync("/api/ChiTieu", chiTieuSoLuong);
            return response.IsSuccessStatusCode;
        }
    }
}
