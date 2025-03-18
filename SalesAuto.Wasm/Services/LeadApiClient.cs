using SalesAuto.Models.Entities;
using SalesAuto.Wasm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using SalesAuto.Models;
using SalesAuto.Models.ViewModel;
using SalesAuto.Models.SearchModel;
using System.Text.Json;

namespace SalesAuto.Wasm.Services
{
    public class LeadApiClient: ILeadApiClient
    {

        public HttpClient _httpClient;

        public LeadApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<LeadVM>>GetAllLeadList()
        {
            var result = await _httpClient.GetFromJsonAsync<List<LeadVM>>("/api/leads");
            return result;
        }
        public async Task<List<LeadVM>> GetLeadList(LeadSM leadSM)
        {          
            var response = await _httpClient.PostAsJsonAsync($"/api/leads/search", leadSM);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<LeadVM>>();
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
