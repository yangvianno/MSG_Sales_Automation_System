using Blazored.LocalStorage;
using Microsoft.AspNetCore.WebUtilities;
using SalesAuto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public class BenhVienClient : IBenhVienClient
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;

        public BenhVienClient(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }
        public async Task<List<BenhVienVM>> GetAll()
        {
            return await httpClient.GetFromJsonAsync<List<BenhVienVM>>("/api/benhvien");
        }

        public async Task<List<BenhVienVM>> GetBenhVienForUser()
        {            
            string userType = await localStorage.GetItemAsync<string>("userType");                        
            if (userType == "msal")
            {
                return await GetBenhVienByEmail();
            }
            else
            {
                var userId = await localStorage.GetItemAsync<Guid>("userId");
                var queryStringParam = new Dictionary<string, string>
                {
                    ["userId"] = userId.ToString()
                };
                string url = QueryHelpers.AddQueryString("/api/benhvien", queryStringParam);
                var response = await httpClient.PostAsJsonAsync(url, userId);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<BenhVienVM>>();
                    return result;
                }
                else
                {
                    return null;
                }
            }
                
        }
        public async Task<List<BenhVienVM>> GetBenhVienByEmail()
        {            
            var cls = await localStorage.GetItemAsync<Dictionary<string, object>>("msalclaim");
            string Email = "";
            if(cls!= null)
            {
                if (cls.ContainsKey("preferred_username"))
                {
                    Email = cls["preferred_username"].ToString();
                }

            }
            var queryStringParam = new Dictionary<string, string>
            {
                ["Email"] = Email.ToString()
            };
            string url = QueryHelpers.AddQueryString("/api/benhvien/GetBenhVienByEmail", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, Email);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<BenhVienVM>>();
                return result;
            }
            else
            {
                return null;
            }

        }

        public async Task<string> getBenhVienDangLamViec()
        {
            string MaBenhVien = await localStorage.GetItemAsync<string>("BenhVienDangLamViec");
            if (MaBenhVien != "")
            {
                httpClient.DefaultRequestHeaders.Remove("MaBenhVienNguon");
                httpClient.DefaultRequestHeaders.Add("MaBenhVienNguon", MaBenhVien);
            }
            return await localStorage.GetItemAsync<string>("BenhVienDangLamViec");
            
        }

        public async Task SetBenhVienDangLamViec(string MaBenhVien)
        {
            await localStorage.SetItemAsync("BenhVienDangLamViec",MaBenhVien);
            httpClient.DefaultRequestHeaders.Remove("MaBenhVienNguon");
            httpClient.DefaultRequestHeaders.Add("MaBenhVienNguon", MaBenhVien);
        }

    }
}
