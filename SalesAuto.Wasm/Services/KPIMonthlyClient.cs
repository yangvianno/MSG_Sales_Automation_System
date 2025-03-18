using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public class KPIMonthlyClient : IKPIMonthlyClient
    {
        private readonly HttpClient httpClient;

        public KPIMonthlyClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<KPIMonthly>> GetList(int Nam)
        {
            var response = await httpClient.GetFromJsonAsync<List<KPIMonthly>>("/api/KPIMonthly?nam=" + Nam);
            return response;
        }

        public async Task<bool> Save(KPIMonthly kPIMonthly)
        {
            var response = await httpClient.PostAsJsonAsync("/api/kPIMonthly", kPIMonthly);
            return response.IsSuccessStatusCode;
        }
    }
}
