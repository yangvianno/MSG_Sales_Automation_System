using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalesAuto.Models.Entities;
using SalesAuto.Models.Entities.CRM;
using SalesAuto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB
{
    public class SalesAutoDbContext : IdentityDbContext<User,Role,Guid>
    {


        public SalesAutoDbContext(DbContextOptions<SalesAutoDbContext> options) : base(options)
        {

        }        
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<CPAReportVM> CPAReportVMs { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ChiTieuSoLuong> ChiTieuSoLuongs { get; set; }
        public DbSet<RemovedLead> RemovedLeads { get; set; }
        public DbSet<KPIMonthly> KPIMonthlys { get; set; }
        public DbSet<LFUReportVM> LFUReportVMs { get; set; }
        public DbSet<ThoiGianGuiMailWeekly> ThoiGianGuiMailWeeklys { get; set; }
        public DbSet<ThoiGianGuiMailMonthly> ThoiGianGuiMailMonthlys { get; set; }
        public DbSet<ThongTinGuiMailBenhVien> ThongTinGuiMailBenhViens { get; set; }
        public DbSet<ThongTinGuiMailBenhVien> ThongTinGuiMaiCPAAndCallBenhViens { get; set; }
        public virtual DbSet<LeadVM> LeadVMs { get; set; }
        public virtual DbSet<BenhNhanKhamVM> BenhNhanKhamVMs { get; set; }
        public virtual DbSet<BenhVienVM> BenhVienVMs { get; set; }
        public virtual DbSet<BenhVienKhamVM> BenhVienKhamVMs { get; set; }

        public DbSet<Category> CRM_Categories { set; get; }
        public DbSet<District> CRM_Districts { set; get; }
        public DbSet<Product> CRM_Products { set; get; }
        public DbSet<Province> CRM_Provinces { set; get; }
        public DbSet<Store> CRM_Stores { set; get; }
        public DbSet<Ward> CRM_Wards { set; get; }
        public DbSet<SalesAuto.Models.Entities.CRM.Type> CRM_Types { set; get; }
        public DbSet<Order_status>  CRM_Order_status { set; get; }
    }
}
