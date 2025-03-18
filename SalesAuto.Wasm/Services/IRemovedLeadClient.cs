using SalesAuto.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public interface IRemovedLeadClient
    {
        Task<List<RemovedLead>> GetList(int Nam);
        Task<bool> Save(RemovedLead removedLead);
    }
}