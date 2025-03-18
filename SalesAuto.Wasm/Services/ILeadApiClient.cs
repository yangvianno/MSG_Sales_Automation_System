using SalesAuto.Models.Entities;
using SalesAuto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesAuto.Models.ViewModel;
using SalesAuto.Models.SearchModel;

namespace SalesAuto.Wasm.Services
{
    public interface ILeadApiClient
    {
        public Task<List<LeadVM>> GetAllLeadList();
        public Task<List<LeadVM>> GetLeadList(LeadSM leadSM);
    }
}
