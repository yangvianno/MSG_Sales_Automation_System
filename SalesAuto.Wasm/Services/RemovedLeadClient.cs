using Microsoft.AspNetCore.Components;
using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public class RemovedLeadClient : IRemovedLeadClient
    {
        private readonly HttpClient httpClient;

        public RemovedLeadClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<RemovedLead>> GetList(int Nam)
        {
            var response = await httpClient.GetFromJsonAsync<List<RemovedLead>>($"/api/removedLead?nam=" + Nam);
            return response;
        }

        public async Task<bool> Save(RemovedLead removedLead)
        {
            var response = await httpClient.PostAsJsonAsync("/api/removedLead", removedLead);
            return response.IsSuccessStatusCode;
        }
    }
}
