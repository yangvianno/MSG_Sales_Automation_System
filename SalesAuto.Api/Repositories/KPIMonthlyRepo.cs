using Microsoft.EntityFrameworkCore;
using DB;
using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public class KPIMonthlyRepo : IKPIMonthlyRepo
    {
        private readonly SalesAutoDbContext context;

        public KPIMonthlyRepo(SalesAutoDbContext context)
        {
            this.context = context;
        }

        public async Task<KPIMonthly> Save(KPIMonthly kPIMonthly)
        {
            var found = await context.KPIMonthlys.FirstOrDefaultAsync(x => x.Thang == kPIMonthly.Thang && x.Nam == kPIMonthly.Nam);
            if (found == null)
            {
                context.KPIMonthlys.Add(kPIMonthly);
            }
            else
            {
                found.ActualDigitalCost = kPIMonthly.ActualDigitalCost;
                found.ActualBranding = kPIMonthly.ActualBranding;
                found.BudgetDigitalCost = kPIMonthly.BudgetDigitalCost;
                found.MaBenhVien = kPIMonthly.MaBenhVien;
            }
            await context.SaveChangesAsync();
            return kPIMonthly;
        }

        public async Task<List<KPIMonthly>> GetList(int nam, string MaBenhVien="O")
        {
            var query = context.KPIMonthlys.Where(x => x.Nam == nam && x.MaBenhVien==MaBenhVien).AsQueryable();
            query = query.OrderBy(x => x.Nam).ThenBy(x => x.Thang);

            return await query.ToListAsync();
        }
    }
}
