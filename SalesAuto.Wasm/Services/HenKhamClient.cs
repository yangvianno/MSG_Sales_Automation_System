using Microsoft.AspNetCore.WebUtilities;
using SalesAuto.Models.Entities.CRM;
using SalesAuto.Models.Entities.HenKham;
using SalesAuto.Models.ViewModel.HenKham;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public class HenKhamClient : IHenKhamClient
    {
        private readonly HttpClient httpClient;

        public HenKhamClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<BenhNhanHenKham>> GetDanhSachHenKham(DateTime TuNgay, DateTime DenNgay, bool LayTheoNgayHen = true, bool BacSyHen = true, bool HoSoLasik = true, bool BenhChuaHen = true)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["TuNgay"] = TuNgay.ToString("yyyy-MM-dd 00:00:00"),
                ["DenNgay"] = DenNgay.ToString("yyyy-MM-dd 23:00:00"),
                ["LayTheoNgayHen"] = LayTheoNgayHen.ToString(),
                ["BacSyHen"] = BacSyHen.ToString(),
                ["HoSoLasik"] = HoSoLasik.ToString(),
                ["BenhChuaHen"] = BenhChuaHen.ToString()
            };
            string url = QueryHelpers.AddQueryString("api/HenKham/GetDanhSachHenKham", queryStringParam);
            var result = await httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var orderRe = JsonSerializer.Deserialize<List<BenhNhanHenKham>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return orderRe;
            }
            return null;
        }
        public async Task<List<BenhNhanHenKham>> GetHenKhamThucHienCuoi()
        {            
            string url = "api/HenKham/GetHenKhamThucHienCuoi";
            var result = await httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var orderRe = JsonSerializer.Deserialize<List<BenhNhanHenKham>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return orderRe;
            }
            return null;
        }
        
        public async Task<BenhNhanHenKham> GetHenKham(Guid ID)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["ID"] = ID.ToString()
            };
            string url = QueryHelpers.AddQueryString("api/HenKham/GetHenKham", queryStringParam);
            var result = await httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var orderRe = JsonSerializer.Deserialize<BenhNhanHenKham>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return orderRe;
            }
            return null;
        }
        public async Task<Guid?> AddHenKhamFromHis(BenhNhanHenKham benhHenKham)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/HenKham/SaveHenKham", benhHenKham);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Guid?>();
                return result;
            }
            else
            {
                return null;
            }
        }
        public async Task<string> PushHenKhamToCRM(BenhNhanHenKham benhHenKham)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["ID"] = benhHenKham.ID.ToString()
            };
            string url = QueryHelpers.AddQueryString("api/HenKham/PushHenKhamToCRM", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, benhHenKham.ID);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();               
                return result;
            }
            else
            {
                return "";
            }
        }
        public async Task<string> UpdateTinhTrangHenKhamToCRM(HKLayDanhSachCapNhatTinhTrang hKLayDanhSachCapNhatTinhTrang)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["CRM_id_order"] = hKLayDanhSachCapNhatTinhTrang.CRM_id_order.ToString()
            };
            string url = QueryHelpers.AddQueryString("api/HenKham/UpdateTinhTrangHenKhamToCRM", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, hKLayDanhSachCapNhatTinhTrang);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                return "";
            }
        }
        public async Task<List<Product>> GetDanhSachCRMProduct()
        {
            var url = $"/api/HenKham/GetDanhSachCRMProduct";
            var result = await httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var orderRe = JsonSerializer.Deserialize<List<Product>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return orderRe;
            }
            return null;
        }
        public async Task<List<Order_status>> GetDanhSachCRMOrder_status()
        {
            var url = $"/api/HenKham/GetDanhSachCRMOrder_status";
            var result = await httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var orderRe = JsonSerializer.Deserialize<List<Order_status>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return orderRe;
            }
            return null;
        }

        public async Task<List<HKBenhChuyenKhoa>> GetDanhSachBenhChuyenKhoa()
        {
            var url = $"/api/HenKham/GetDanhSachBenhChuyenKhoa";
            var result = await httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var orderRe = JsonSerializer.Deserialize<List<HKBenhChuyenKhoa>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return orderRe;
            }
            return null;
        }

        public async Task<List<HKLayDanhSachCapNhatTinhTrang>> GetDanhSachCapNhatTinhTrang(DateTime TuNgay, DateTime DenNgay)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["TuNgay"] = TuNgay.ToString("yyyy-MM-dd 00:00:00"),
                ["DenNgay"] = DenNgay.ToString("yyyy-MM-dd 23:00:00"),
            };
            string url = QueryHelpers.AddQueryString("api/HenKham/GetDanhSachCapNhatTinhTrang", queryStringParam);
            var result = await httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var orderRe = JsonSerializer.Deserialize<List<HKLayDanhSachCapNhatTinhTrang>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return orderRe;
            }
            return null;
        }

        #region Mẫu cập nhật tình trạng
        public async Task<HKMauCapNhatTinhTrang> SaveMauCapNhatTinhTrang(HKMauCapNhatTinhTrang mau)
        {
            string url = "api/HenKham/SaveMauCapNhatTinhTrang";
            var result = await httpClient.PostAsJsonAsync(url, mau);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var orderRe = JsonSerializer.Deserialize<HKMauCapNhatTinhTrang>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                return orderRe;
            }
            else
            {
                return null;
            }
        }

        
        public async Task<bool> DeleteMauCapNhatTinhTrang(Guid id)
        {
            string url = "api/HenKham/DeleteMauCapNhatTinhTrang";
            var response = await httpClient.PostAsJsonAsync(url, id);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                if (result.ToLower() == "true")
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<HKMauCapNhatTinhTrang>> GetDanhSachMauCapNhatTinhTrang()
        {
            var url = $"/api/HenKham/GetDanhSachMauCapNhatTinhTrang";
            var result = await httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var orderRe = JsonSerializer.Deserialize<List<HKMauCapNhatTinhTrang>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return orderRe;
            }
            return null;
        }
        #endregion // Mẫu cập nhật tình trạng

        #region Mẫu hẹn khán theo toa
        public async Task<HKMauHenKhamTheoToa> SaveMauHenKhamTheoToa(HKMauHenKhamTheoToa mau)
        {
            string url = "api/HenKham/SaveMauHenKhamTheoToa";
            var result = await httpClient.PostAsJsonAsync(url, mau);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var orderRe = JsonSerializer.Deserialize<HKMauHenKhamTheoToa>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                return orderRe;
            }
            else
            {
                return null;
            }
        }


        public async Task<bool> DeleteMauHenKhamTheoToa(Guid id)
        {
            string url = "api/HenKham/DeleteMauHenKhamTheoToa";
            var response = await httpClient.PostAsJsonAsync(url, id);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                if (result.ToLower() == "true")
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<HKMauHenKhamTheoToa>> GetDanhSachMauHenKhamTheoToa()
        {
            var url = $"/api/HenKham/GetDanhSachMauHenKhamTheoToa";
            var result = await httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var orderRe = JsonSerializer.Deserialize<List<HKMauHenKhamTheoToa>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return orderRe;
            }
            return null;
        }
        #endregion // Mẫu cập nhật tình trạng
    }
}
