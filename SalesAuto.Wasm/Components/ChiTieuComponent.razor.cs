using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SalesAuto.Models.Entities;
using SalesAuto.Wasm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Components
{
    public partial class ChiTieuComponent
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        [Inject] IChiTieuSoLuongClient chiTieuSoLuongClient { get; set; }
        [Inject] IToastService toastService { get; set; }
        
        private int nam = 0;
        private List<ChiTieuSoLuong> listChiTieu;
        private List<LoaiChiTieu> listLoaiChiTieu;
        private int m_MaLoaiChiTieu;
        private bool isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            nam = DateTime.Now.Year;
            await loadLoaiChiTieu();
        }
        private async Task loadLoaiChiTieu()
        {
            isLoading = true;
            await InvokeAsync(StateHasChanged);
            var user = (await authenticationStateTask).User;            
            listLoaiChiTieu = await chiTieuSoLuongClient.GetLoaiChiTieu();

            if(user.IsInRole("Sale"))
            {
                listChiTieu = listChiTieu.FindAll(x => x.MaLoaiChiTieu >= 8 && x.MaLoaiChiTieu >= 13);
            }
            if (listLoaiChiTieu != null && listLoaiChiTieu.Count > 0)
            {  
                m_MaLoaiChiTieu = listChiTieu.FirstOrDefault().MaLoaiChiTieu;
            }
            isLoading = false;
            await InvokeAsync(StateHasChanged);
        }
        private async Task loadList()
        {
            isLoading = true;
            listChiTieu = await chiTieuSoLuongClient.GetChiTieuSoLuong(m_MaLoaiChiTieu,nam);
            isLoading = false;
        }
        private async Task SaveList()
        {
            toastService.ShowInfo("Đang lưu chỉ tiêu ...");
            foreach (var item in listChiTieu)
            {
                await chiTieuSoLuongClient.Save(item);
            }
            toastService.ShowSuccess("Lưu chỉ tiêu thành công");
        }
        async Task OnChange(object value, string name)
        {
            var str = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            if (str != null)
            {
                m_MaLoaiChiTieu = int.Parse(str.ToString());
                await loadList();
            }
            Console.WriteLine($"{name} value changed to {str}");
        }
    }
}
