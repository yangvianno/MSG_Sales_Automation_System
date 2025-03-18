using SalesAuto.Models;
using SalesAuto.Models.Entities;
using SalesAuto.Models.SearchModel;
using SalesAuto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public interface ILeadsRepo
    {
        Task<IEnumerable<Lead>> GetAllLeadList();
        Task<Lead> CreatLead(Lead lead);
        Task<Lead> UpdateLead(int ID, Lead lead);
        Task<Lead> DeleteLead(int ID);
        Task<Lead> GetLeadByID(int ID);
        Task<IEnumerable<LeadVM>> GetLeadList(LeadSM leadSM, string MaBenhVien="O");
        Task SaveLaed(Lead lead);
        Task SaveLaeds(List<Lead> leads, string MaBenhVien = "O");
        public Task SaveLeadFileToDataBase(string file, string MaBenhVien = "O");
        public Task LoadLichKhamTuCRM(bool full);
        public Task LoadLeadsFromGoogle();


    }
}
