using Microsoft.AspNetCore.Components;
using SalesAuto.Models.Entities;
using SalesAuto.Wasm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Components
{
    public partial class KPIMonthlyComponent
    {
        [Inject] IKPIMonthlyClient kPIMonthlyClient { get; set; }
        private int nam = 0;
        private List<KPIMonthly> listKPIMonthly;
        protected override async Task OnInitializedAsync()
        {
            nam = DateTime.Now.Year;
            await loadList();
        }

        private async Task loadList()
        {
            listKPIMonthly = await kPIMonthlyClient.GetList(nam);
        }
        private async Task SaveList()
        {
            foreach (var item in listKPIMonthly)
            {
                await kPIMonthlyClient.Save(item);
            }
        }
    }
}
