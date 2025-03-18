using SalesAuto.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public interface IRemovedLeadRepo
    {
        Task<List<RemovedLead>> GetList(int nam, string MaBenhVien = "O");        
        Task<RemovedLead> Save(RemovedLead removedLead);
    }
}