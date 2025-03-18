using Microsoft.AspNetCore.WebUtilities;
using SalesAuto.Models.Entities.HisDoiTuong;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public class HisClient : IHisClient
    {
        private HttpClient httpClient;

        public HisClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        #region Đối tượng
        public async Task<List<BangGiaTheoDoiTuong>> GetBangGiaTheoDoiTuong(Guid ID_LoaiDoiTuong)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["ID_LoaiDoiTuong"] = ID_LoaiDoiTuong.ToString(),
            };
            string url = QueryHelpers.AddQueryString($"/api/His/GetBangGiaTheoDoiTuong", queryStringParam);
            var responce = await httpClient.GetAsync(url);
            if (responce.IsSuccessStatusCode)
            {
                var content = await responce.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonSerializer.Deserialize<List<BangGiaTheoDoiTuong>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
                return result;
            }

            return null;
        }
        public async Task<List<LoaiDoiTuong>> GetDanhSachDoiTuong()
        {
            var responce = await httpClient.GetAsync($"/api/His/GetDanhSachDoiTuong");
            if (responce.IsSuccessStatusCode)
            {            
                var content = await responce.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonSerializer.Deserialize<List<LoaiDoiTuong>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
                return result;
            }
            return null;
        }

        public async Task<BangGiaTheoDoiTuong> SaveBangGiaTheoDoiTuong(BangGiaTheoDoiTuong item)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/His/SaveBangGiaTheoDoiTuong", item);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<BangGiaTheoDoiTuong>();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<BangGiaTheoDoiTuong> DeleteBangGiaTheoDoiTuong(BangGiaTheoDoiTuong item)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/His/DeleteBangGiaTheoDoiTuong", item);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<BangGiaTheoDoiTuong>();
                return result;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
