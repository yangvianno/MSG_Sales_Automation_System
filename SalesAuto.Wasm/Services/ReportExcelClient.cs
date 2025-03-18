using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public class ReportExcelClient : IReportExcelClient
    {
        private readonly HttpClient httpClient;

        public ReportExcelClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        #region Xuất File cho OP
        public async Task<IEnumerable<IDictionary<string, object>>> GetDanhSachLead(DateTime TuNgay, DateTime DenNgay)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["TuNgay"] = TuNgay.ToString("yyyy-MM-dd 00:00:00"),
                ["DenNgay"] = DenNgay.ToString("yyyy-MM-dd 23:00:00"),
            };
            string url = QueryHelpers.AddQueryString("api/ReportExcel/GetDanhSachLead", queryStringParam);
            var respone = await httpClient.GetAsync(url);

            if (respone.IsSuccessStatusCode)
            {
                var content = await respone.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<IDictionary<string, object>>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return result;
            }
            return null;
        }
        public async Task<IEnumerable<IDictionary<string, object>>> GetDanhSachBook(DateTime TuNgay, DateTime DenNgay)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["TuNgay"] = TuNgay.ToString("yyyy-MM-dd 00:00:00"),
                ["DenNgay"] = DenNgay.ToString("yyyy-MM-dd 23:00:00"),
            };
            string url = QueryHelpers.AddQueryString("api/ReportExcel/GetDanhSachBook", queryStringParam);
            var respone = await httpClient.GetAsync(url);

            if (respone.IsSuccessStatusCode)
            {
                var content = await respone.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<IDictionary<string, object>>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return result;
            }
            return null;
        }
        public async Task<IEnumerable<IDictionary<string, object>>> GetDanhSachKham(DateTime TuNgay, DateTime DenNgay)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["TuNgay"] = TuNgay.ToString("yyyy-MM-dd 00:00:00"),
                ["DenNgay"] = DenNgay.ToString("yyyy-MM-dd 23:00:00"),
            };
            string url = QueryHelpers.AddQueryString("api/ReportExcel/GetDanhSachKham", queryStringParam);
            var respone = await httpClient.GetAsync(url);

            if (respone.IsSuccessStatusCode)
            {
                var content = await respone.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<IDictionary<string, object>>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return result;
            }
            return null;
        }
        public async Task<IEnumerable<IDictionary<string, object>>> GetDanhSachPhauThuat(DateTime TuNgay, DateTime DenNgay)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["TuNgay"] = TuNgay.ToString("yyyy-MM-dd 00:00:00"),
                ["DenNgay"] = DenNgay.ToString("yyyy-MM-dd 23:00:00"),
            };
            string url = QueryHelpers.AddQueryString("api/ReportExcel/GetDanhSachPhauThuat", queryStringParam);
            var respone = await httpClient.GetAsync(url);

            if (respone.IsSuccessStatusCode)
            {
                var content = await respone.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<IDictionary<string, object>>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return result;
            }
            return null;
        }

        public async Task<IEnumerable<IDictionary<string, object>>> GetQuaTrinhKham(DateTime TuNgay, DateTime DenNgay)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["TuNgay"] = TuNgay.ToString("yyyy-MM-dd 00:00:00"),
                ["DenNgay"] = DenNgay.ToString("yyyy-MM-dd 23:00:00"),
            };
            string url = QueryHelpers.AddQueryString("api/ReportExcel/GetQuaTrinhKham", queryStringParam);
            var respone = await httpClient.GetAsync(url);

            if (respone.IsSuccessStatusCode)
            {
                var content = await respone.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<IDictionary<string, object>>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return result;
            }
            return null;
        }
        public async Task<IEnumerable<IDictionary<string, object>>> GetQuaTrinhPhauThuat(DateTime TuNgay, DateTime DenNgay)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["TuNgay"] = TuNgay.ToString("yyyy-MM-dd 00:00:00"),
                ["DenNgay"] = DenNgay.ToString("yyyy-MM-dd 23:00:00"),
            };
            string url = QueryHelpers.AddQueryString("api/ReportExcel/GetQuaTrinhPhauThuat", queryStringParam);
            var respone = await httpClient.GetAsync(url);

            if (respone.IsSuccessStatusCode)
            {
                var content = await respone.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<IDictionary<string, object>>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return result;
            }
            return null;
        }
        #endregion
    }
}
