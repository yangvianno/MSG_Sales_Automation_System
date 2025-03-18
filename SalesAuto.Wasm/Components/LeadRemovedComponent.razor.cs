using Microsoft.AspNetCore.Components;
using SalesAuto.Models.Entities;
using SalesAuto.Wasm.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Components
{
    public partial class LeadRemovedComponent
    {
        [Inject] IRemovedLeadClient RemovedLeadClient{ get; set; }
        private int nam = 0;
        private List<RemovedLead> listLeadRemove;
        protected override async Task OnInitializedAsync()
        {
            nam = DateTime.Now.Year;
            await loadList();
        }

        private  async Task loadList()
        {
            listLeadRemove = await RemovedLeadClient.GetList(nam);
        }
        private async Task SaveList()
        {
            foreach (var item in listLeadRemove)
            {
                await RemovedLeadClient.Save(item);
            }
        }
    }
}
