using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SalesAuto.Wasm.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blazored.Modal;
using Radzen;
using Blazorade.Msal.Configuration;

namespace SalesAuto.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");                                    
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.Configuration["BackendApiUrl"])
            });

            builder.Services.AddTransient<ILeadApiClient, LeadApiClient>();
            builder.Services.AddTransient<IBenhNhanClient, BenhNhanClient>();
            builder.Services.AddTransient<IBenhVienClient, BenhVienClient>();
            builder.Services.AddTransient<IRemovedLeadClient, RemovedLeadClient>();
            builder.Services.AddTransient<IKPIMonthlyClient, KPIMonthlyClient>();
            builder.Services.AddTransient<IChiTieuSoLuongClient, ChiTieuSoLuongClient>();
            builder.Services.AddTransient<IABRClient, ABRClient>();
            builder.Services.AddTransient<ICommonUI,CommonUI>();
            builder.Services.AddTransient<IExportFile, ExportFile>();
            builder.Services.AddTransient<IABRLoadFileDanhGiaNhanVien, ABRLoadFileDanhGiaNhanVien>();
            builder.Services.AddTransient<IDailyReportsClient, DailyReportsClient>();
            builder.Services.AddTransient <IHenKhamClient,HenKhamClient > ();
            builder.Services.AddTransient<IHisClient, HisClient>();
            builder.Services.AddTransient<IReportExcelClient, ReportExcelClient>();



            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddBlazoredToast();
            builder.Services.AddBlazoredModal();
            // thêm Radzen
            builder.Services.AddScoped<DialogService>();            
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<TooltipService>();
            builder.Services.AddScoped<ContextMenuService>();

            builder.Services.AddBlazoradeMsal((sp, options) =>
            {
                var appSettings = sp.GetService<IConfiguration>().GetSection("myapp");
                options.ClientId = appSettings.GetValue<string>("clientId");
                options.TenantId = appSettings.GetValue<string>("tenantId");
                options.DefaultScopes = new string[] { "api://b5469b8c-b814-4ee9-9652-9c4f405e0fd9/API.Access", "profile" };
                options.InteractiveLoginMode = InteractiveLoginMode.Popup;                
                options.TokenCacheScope = TokenCacheScope.Persistent;
            });

            await builder.Build().RunAsync();
        }
    }
}
