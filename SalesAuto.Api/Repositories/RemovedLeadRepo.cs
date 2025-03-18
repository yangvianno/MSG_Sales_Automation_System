using Microsoft.EntityFrameworkCore;
using DB;
using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public class RemovedLeadRepo : IRemovedLeadRepo
    {
        private readonly SalesAutoDbContext context;

        public RemovedLeadRepo(SalesAutoDbContext context)
        {
            this.context = context;
        }

        public async Task<RemovedLead> Save(RemovedLead removedLead)
        {
            var found = await context.RemovedLeads.FirstOrDefaultAsync(x => x.Thang == removedLead.Thang && x.Nam == removedLead.Nam);
            if (found == null)
            {
                context.RemovedLeads.Add(removedLead);
            }
            else
            {
                found.SoLuong = removedLead.SoLuong;
            }
            await context.SaveChangesAsync();
            return removedLead;
        }

        public async Task<List<RemovedLead>> GetList(int nam, string MaBenhVien = "O")
        {
            var query = context.RemovedLeads.Where(x => x.Nam == nam && x.MaBenhVien ==MaBenhVien ).AsQueryable();
            query = query.OrderBy(x => x.Nam).ThenBy(x => x.Thang);
            bool CoThayDoi = false;
            for(int i=1;i<=12;i++)
            {
                if ((await query.Where(x=>x.Thang==i).FirstOrDefaultAsync())==null)
                {
                    context.RemovedLeads.Add(
                        new RemovedLead
                        {
                            Nam = nam,
                            Thang = i,
                            SoLuong=0,
                            MaBenhVien=MaBenhVien
                        }
                        ); 
                    CoThayDoi = true;
                }
            }
            if (CoThayDoi)
            {
                await context.SaveChangesAsync();
                query = context.RemovedLeads.Where(x => x.Nam == nam && x.MaBenhVien == MaBenhVien).AsQueryable();
                query = query.OrderBy(x => x.Nam).ThenBy(x => x.Thang);
            }
            return await query.ToListAsync();
        }

    }
}
