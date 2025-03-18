using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SalesAuto.Models;
using SalesAuto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorageService;
        private readonly AuthenticationStateProvider auuthenticationStateProvider;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider auuthenticationStateProvider, ILocalStorageService localStorageService)
        {
            this.httpClient = httpClient;
            this.localStorageService = localStorageService;
            this.auuthenticationStateProvider = auuthenticationStateProvider;            
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            
            var result = await httpClient.PostAsJsonAsync("/api/login", loginRequest);
            var content = await result.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(content,
               new JsonSerializerOptions()
               {
                   PropertyNameCaseInsensitive = true
               });
            if (!result.IsSuccessStatusCode)
            {
                return loginResponse;
            }
            await localStorageService.SetItemAsync("authToken", loginResponse.Token);
            await localStorageService.SetItemAsync("authTokenExpiry", loginResponse.Expiry);
            await localStorageService.SetItemAsync("userId", loginResponse.UserId);
            await localStorageService.SetItemAsync("userType", "local");

            ((ApiAuthenticationStateProvider)auuthenticationStateProvider).MarkUserAsAuthenticated(loginRequest.UserName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResponse.Token);            
            return loginResponse;
        }

        public async Task<LoginResponse> SetLoginMsal(LoginResponse loginResponse, string UserName,string msalRoles, Dictionary<string, object> claims)
        {            
            await localStorageService.SetItemAsync("authToken", loginResponse.Token);
            await localStorageService.SetItemAsync("authTokenExpiry", loginResponse.Expiry);
            await localStorageService.SetItemAsync("userId", loginResponse.UserId);
            await localStorageService.SetItemAsync("userType", "msal");
            await localStorageService.SetItemAsync("msalRoles", msalRoles);
            await localStorageService.SetItemAsync("msalclaim", claims);

            ((ApiAuthenticationStateProvider)auuthenticationStateProvider).MarkUserAsAuthenticated(UserName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResponse.Token);
            return loginResponse;
        }

        public async Task Logout()
        {              
            await localStorageService.RemoveItemAsync("authToken");
            await localStorageService.RemoveItemAsync("authTokenExpiry");
            await localStorageService.RemoveItemAsync("userId");
            await localStorageService.RemoveItemAsync("userType");
            ((ApiAuthenticationStateProvider)auuthenticationStateProvider).MarkUserAsLoggedOut();            
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    
        public async Task<int> ChangePass(ChangePassVM item)
        {
            item.UserID = await localStorageService.GetItemAsync<Guid>("userId");
            var response = await httpClient.PostAsJsonAsync("/api/login/ChangePass", item);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<int>();
                return result;
            }
            else
            {
                return 0;
            }
        }

        public async Task<Guid> GetUserID()
        {
            var userId = await localStorageService.GetItemAsync<Guid>("userId");
            return userId;
        }
    }
}
