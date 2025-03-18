using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesAuto.Models;
using SalesAuto.Wasm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Pages
{
    public partial class Login
    {
        [Inject] IAuthService AuthService { get; set; }
        [Inject] NavigationManager navigationManager { get; set; }
        [Inject] private IJSRuntime jsRuntime { get; set; }

        private LoginRequest loginRequest = new LoginRequest();
        private bool showError;
        private string errorMessage = "";
        private async Task HandleLogin()
        {
            showError = false;
            var result = await AuthService.Login(loginRequest);
            if (result.Successful)
            {
                await jsRuntime.InvokeAsync<object>("reloadPage");
                navigationManager.NavigateTo("/");                
            }
            else
            {
                showError = true;
                errorMessage = result.Error;
            }
        }
    }

}
