using SalesAuto.Models.ViewModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public class DailyReportsClient : IDailyReportsClient
    {
        private readonly HttpClient httpClient;

        public DailyReportsClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> GetDailyReportMat()
        {
            var response = await httpClient.PostAsJsonAsync($"/api/DailyReport/GetDailyReportMat", new { });
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
        public async Task<string> GetDailyReportMatTuan(TuanVM tuan)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/DailyReport/GetDailyReportMatTuan", tuan);
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
        public async Task<string> GetDailyReportBenhVienString()
        {
            var response = await httpClient.PostAsJsonAsync($"/api/DailyReport/GetDailyReportBenhVienString", new { });
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
        public async Task<string> GetDailyReportMatSum()
        {
            var response = await httpClient.PostAsJsonAsync($"/api/DailyReport/GetDailyReportMatSum", new { });
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

        public async Task<string> GetDailyReportMatSumTuan(TuanVM tuan )
        {
            var response = await httpClient.PostAsJsonAsync($"/api/DailyReport/GetDailyReportMatSumTuan", tuan);
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

    }
}
