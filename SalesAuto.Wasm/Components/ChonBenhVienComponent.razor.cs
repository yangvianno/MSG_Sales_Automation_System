using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesAuto.Models.ViewModel;
using SalesAuto.Wasm.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Components
{
    public partial class ChonBenhVienComponent
    {
        [Inject] IBenhVienClient benhVienClient { get; set; }
        [Inject] private IJSRuntime jsRuntime { get; set; }
        private List<BenhVienVM> DanhSachBenhVien;
        private string BenhVienDangLamViec = "";
        protected override async Task OnInitializedAsync()
        {
            DanhSachBenhVien = await benhVienClient.GetBenhVienForUser();
            BenhVienDangLamViec = await benhVienClient.getBenhVienDangLamViec(); 
            if (BenhVienDangLamViec == null)
            {
                if (DanhSachBenhVien.Count>0)
                {
                    BenhVienDangLamViec = DanhSachBenhVien[0].MaBenhVien;
                    await benhVienClient.SetBenhVienDangLamViec(BenhVienDangLamViec);
                }
            }
        }

        async Task OnChange(object value, string name)
        {
            var str = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            if(str != null)
            { 
                BenhVienDangLamViec = str.ToString();
                await benhVienClient.SetBenhVienDangLamViec(BenhVienDangLamViec);
                await InvokeAsync(StateHasChanged);
                await jsRuntime.InvokeAsync<object>("reloadPage");                
            }   
            
            Console.WriteLine($"{name} value changed to {str}");
        }
    }
}
