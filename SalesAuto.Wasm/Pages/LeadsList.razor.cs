using Microsoft.AspNetCore.Components;
using SalesAuto.Models.SearchModel;
using SalesAuto.Models.ViewModel;
using SalesAuto.Wasm.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Pages
{
    public partial class LeadsList
    {
        [Inject] private ILeadApiClient leadApiClient { get; set; }
        private List<LeadVM> Leads;

        private LeadSM leadSM = new();
        protected override async Task OnInitializedAsync()
        {
            if (leadSM != null)
            {
                leadSM.TuNgay = DateTime.Now.Date;
                leadSM.DenNgay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                leadSM.TinhThanh = "";
                leadSM.Nguon = "";
            }
            //Leads = await leadApiClient.GetAllLeadList();
            Leads = await leadApiClient.GetLeadList(leadSM);
        }

        private async Task SearchLead()
        {
            Leads = await leadApiClient.GetLeadList(leadSM);
        }

    }
}
