using Microsoft.EntityFrameworkCore;
using SalesAuto.Models;
using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesAuto.Models.SearchModel;
using System.IO;
using OfficeOpenXml;
using System.Globalization;
using SalesAuto.Models.ViewModel;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using DataAccessLibrary;
using DB;

namespace SalesAuto.Api.Repositories
{
    public class LeadsRepo : ILeadsRepo
    {
        private readonly SalesAutoDbContext _context;
        private readonly IMySqlDataAccess mySqlDataAccess;
        private readonly ISqlDataAccess sqlDataAccess;

        public LeadsRepo(SalesAutoDbContext context, IMySqlDataAccess mySqlDataAccess, ISqlDataAccess sqlDataAccess)
        {   
            this.mySqlDataAccess = mySqlDataAccess;
            this.sqlDataAccess = sqlDataAccess;
            _context = context;
        }
        public async Task<Lead> CreatLead(Lead lead)
        {
            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();
            return lead;
        }

        public async Task SaveLaed(Lead lead)
        {
            var found = await _context.Leads.FirstOrDefaultAsync(x => x.STT == lead.STT);
            if (found == null) 
            {
                _context.Leads.Add(lead);
                await _context.SaveChangesAsync();
            }   
        }

        public async Task SaveLaeds(List<Lead> leads, string MaBenhVien="O")
        {
            //Lay STT lon nhat trong file            
            var query = await _context.Leads.AsQueryable().Where(x=> x.MaBenhVien==MaBenhVien).OrderByDescending(x=>x.STT).FirstOrDefaultAsync();            
            foreach (Lead lead in leads)
            {
                if (query==null || lead.STT > query.STT)
                {
                    var found = await _context.Leads.FirstOrDefaultAsync(x => x.STT == lead.STT && x.MaBenhVien ==lead.MaBenhVien );
                    if (found == null)
                    {
                        _context.Leads.Add(lead);

                    }
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task SaveLaedsSQL(List<Lead> leads, string MaBenhVien = "O")
        {
            //Lay STT lon nhat trong file                        
            foreach (Lead lead in leads)
            {
                try
                {
                    var found = await sqlDataAccess.loadData<Lead, dynamic>("select * from leads where STT=" + lead.STT, new { });
                    if (found == null || found.Count<=0)
                    {
                        string sql = @"INSERT INTO [dbo].[Leads]
                                        ([STT]
                                        ,[TenKhachHang]
                                        ,[SoPhu]
                                        ,[Phone]
                                        ,[Ngay]
                                        ,[Nguon]
                                        ,[TinhThanh]
                                        ,[NgayImport]
                                        ,[file]
                                        ,[MaBenhVien]
                                        )
                                    VALUES
                                        ("+ lead.STT + @"
                                        ,N'" + lead.TenKhachHang + @"'
                                        ,N'" + lead.SoPhu + @"'
                                        ,N'" + lead.Phone+ @"'
                                        ,N'" + lead.Ngay.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                                        ,N'" + lead.Nguon+ @"'
                                        ,N'" + lead.TinhThanh + @"'
                                        ,GETDATE()
                                        ,''
                                        ,'" + MaBenhVien + @"'
                                       )";
                        await sqlDataAccess.execNonQuery(sql);
                    }
                }
                catch
                {

                }
            }
            
        }

        public async Task SaveAppointment(Appointment appointment)
        {
            var found = await _context.Appointments.FirstOrDefaultAsync(x => x.MaLichKham == appointment.MaLichKham);
            if (found == null)
            {
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveAppointments(List<Appointment> Appointments)
        {
            foreach (Appointment appointment in Appointments)
            {
                var found = await _context.Appointments.FirstOrDefaultAsync(x => x.MaLichKham == appointment.MaLichKham && x.NgayTaoLich==appointment.NgayTaoLich && x.MaBenhVien == appointment.MaBenhVien);
                if (found == null)
                {
                    _context.Appointments.Add(appointment);

                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task SaveAppointmentsSQL(List<Appointment> Appointments)
        {
            foreach (Appointment appointment in Appointments)
            {
                try
                {
                    string sql = "select * from Appointments where MaLichKham='" + appointment.MaLichKham + "' and NgayTaoLich='" + appointment.NgayTaoLich.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    var found = await sqlDataAccess.loadData<Appointment, dynamic>(sql, new { });
                    if (found == null || found.Count <= 0)
                    {
                        sql = @"insert into Appointments 
                            (
                                    [STT]
                                  ,[MaLichKham]
                                  ,[TenKhachHang]
                                  ,[DienThoai]
                                  ,[TrangThai]
                                  ,[NgayTaoLich]
                                  ,[NgayDatLichKham]
                                  ,[Loai]
                                  ,[DienThoaiVien]
                                  ,[TenChiNhanh]
                            )
                            values
                            (
                                  " + appointment.STT + @"
                                  ,N'" + appointment.MaLichKham + @"'
                                  ,N'" + appointment.TenKhachHang + @"'
                                  ,'" + appointment.DienThoai + @"'
                                  ,N'" + appointment.TrangThai + @"'
                                  ,'" + appointment.NgayTaoLich.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                                  ,'" + appointment.NgayDatLichKham.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                                  ,N'" + appointment.Loai + @"'
                                  ,N'" + appointment.DienThoaiVien + @"'
                                  ,N'" + appointment.TenChiNhanh + @"'
                            )";
                        await sqlDataAccess.execNonQuery(sql);
                    }
                }
                catch
                {

                }
            }            
        }

        public async Task<Lead> DeleteLead(int ID)
        {
            var lead = _context.Leads.Find(ID);
            if (lead!= null)
            {
                _context.Leads.Remove(lead);
                await _context.SaveChangesAsync();
            }
            return lead;
        }

        public async Task<Lead> GetLeadByID(int ID)
        {
            return await _context.Leads.FindAsync(ID);
        }

        public async Task<IEnumerable<Lead>> GetAllLeadList()
        {
            return await _context.Leads.ToListAsync();
        }

        public async Task<Lead> UpdateLead(int ID, Lead lead)
        {
            _context.Leads.Update(lead);
            await _context.SaveChangesAsync();
            return lead;
        }

        public async Task<IEnumerable<LeadVM>> GetLeadList(LeadSM leadSM, string MaBenhVien = "O")
        {
            
            string sql = "exec proc_getLeadVM '" + leadSM.TuNgay.ToString("yyyy-MM-dd") + "', '" + leadSM.DenNgay.ToString("yyyy-MM-dd 23:59:59") + "','" + MaBenhVien + "'";            
            var a = await sqlDataAccess.loadData<LeadVM, dynamic>(sql, new { });
            var query = a.AsQueryable(); 
            if (!string.IsNullOrEmpty(leadSM.TinhThanh))
            {
                query = query.Where(x => x.TinhThanh.Contains(leadSM.TinhThanh));
            }

            if (!string.IsNullOrEmpty(leadSM.Nguon))
            {
                query = query.Where(x => x.Nguon.Contains(leadSM.Nguon));
            }            
            return query.ToList();
        }

        public async Task SaveLeadFileToDataBase(string file, string MaBenhVien = "O")
        {   
            if (File.Exists(file))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                try
                {                    
                    using (var pck = new OfficeOpenXml.ExcelPackage())
                    {
                        using (var stream = System.IO.File.OpenRead(file))
                        {
                            pck.Load(stream);
                        }
                        var ws = pck.Workbook.Worksheets.First();
                        if (ws.Cells["A4"].Text == "#")
                        {
                            // Luu danh sach hen kham
                            int startRow = 5;
                            var ListAppointments = new List<Appointment>();
                            var cultureInfo = new CultureInfo("fr-FR");
                            cultureInfo.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
                            cultureInfo.DateTimeFormat.DateSeparator = "-";
                            for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                            {
                                int stt = 0;
                                string MaLichKham = "";
                                string TenKhachHang = "";
                                string DienThoai = "";
                                string TrangThai = "";                                
                                DateTime NgayTaoLich = DateTime.Now.Date;
                                DateTime NgayDatLichKham = DateTime.Now.Date;
                                string Loai = "";
                                string DienThoaiVien = "";
                                string TenChiNhanh = "";
                                try
                                {

                                    var wsRow = ws.Cells[rowNum, 1, rowNum, 13];
                                    if (wsRow[rowNum, 1].Text != "")
                                    {
                                        stt = int.Parse(wsRow[rowNum, 1].Text);
                                        MaLichKham = wsRow[rowNum, 2].Text;
                                        if(MaLichKham.Trim()=="")
                                        {
                                            MaLichKham = MaBenhVien + stt.ToString();
                                        }
                                        string str = wsRow[rowNum, 4].Text;
                                        str = str.Trim();
                                        DienThoai = Regex.Replace(str, "[^0-9]", "");
                                        TenKhachHang = wsRow[rowNum, 3].Text;
                                        TenKhachHang = TenKhachHang.Trim();                                        
                                        TrangThai = "";                                        
                                        Loai = wsRow[rowNum, 8].Text;
                                        //NgayTaoLich = (wsRow[rowNum, 6].Text == "" ? DateTime.Now.Date : (DateTime)(wsRow[rowNum, 6].Value));
                                        NgayTaoLich = (wsRow[rowNum, 6].Text==""?DateTime.Now.Date: DateTime.Parse(wsRow[rowNum, 6].Text, cultureInfo).Date);
                                        //NgayDatLichKham = (wsRow[rowNum, 7].Text == "" ? DateTime.Now.Date : (DateTime)(wsRow[rowNum, 7].Value));
                                        NgayDatLichKham = (wsRow[rowNum, 7].Text == "" ? DateTime.Now.Date : DateTime.Parse(wsRow[rowNum, 7].Text, cultureInfo).Date);
                                         if (TenKhachHang != "" && MaLichKham != "" && DienThoai!="")
                                        {

                                            ListAppointments.Add(new Appointment
                                            {
                                                STT = stt,
                                                MaLichKham = MaLichKham,
                                                TenKhachHang = TenKhachHang,
                                                DienThoai = DienThoai,
                                                TrangThai = TrangThai,
                                                NgayTaoLich = NgayTaoLich,
                                                NgayDatLichKham = NgayDatLichKham,
                                                Loai = Loai,
                                                DienThoaiVien = DienThoaiVien,
                                                TenChiNhanh = TenChiNhanh,
                                                MaBenhVien = MaBenhVien
                                            }); ;

                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }
                            await SaveAppointments(ListAppointments);

                        }
                        else
                        {
                            //Luu Danh Sach Lead
                            var leadLonNhat = await _context.Leads.AsQueryable().Where(x=>x.MaBenhVien==MaBenhVien).OrderByDescending(x => x.STT).FirstOrDefaultAsync();
                            int startRow = 3;
                            var ListLeads = new List<Lead>();
                            var cultureInfo = new CultureInfo("fr-FR");
                            for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                            {
                                int stt = 0;
                                string TenKhachHang = "";
                                string SoPhu = "";
                                string Phone = "";
                                string Nguon = "";
                                string TinhThanh = "";
                                DateTime Ngay = DateTime.Now.Date;
                                try
                                {

                                    var wsRow = ws.Cells[rowNum, 1, rowNum, 7];
                                    if (wsRow[rowNum, 1].Text != "")
                                    {
                                        stt = int.Parse(wsRow[rowNum, 1].Text);
                                        TenKhachHang = wsRow[rowNum, 2].Text;
                                        SoPhu = wsRow[rowNum, 3].Text;
                                        Phone = wsRow[rowNum, 4].Text;
                                        Nguon = wsRow[rowNum, 6].Text;
                                        TinhThanh = wsRow[rowNum, 7].Text;
                                        Ngay = (wsRow[rowNum, 5].Text == "" ? DateTime.Now.Date : DateTime.Parse(wsRow[rowNum, 5].Text, cultureInfo));
                                        //Ngay = (wsRow[rowNum, 5].Text == "" ? DateTime.Now.Date : (DateTime)(wsRow[rowNum, 5].Value));
                                        if (leadLonNhat ==  null || stt > leadLonNhat.STT)
                                        {
                                            if (TenKhachHang != "" && Phone != "")
                                            {

                                                ListLeads.Add(new Lead
                                                {
                                                    STT = stt,
                                                    TenKhachHang = TenKhachHang,
                                                    SoPhu = SoPhu,
                                                    Phone = Phone,
                                                    Nguon = Nguon,
                                                    TinhThanh = TinhThanh,
                                                    Ngay = Ngay,
                                                    file = file,
                                                    MaBenhVien=MaBenhVien
                                                }); ;

                                            }
                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }
                            await SaveLaeds(ListLeads, MaBenhVien);
                        }
                    }
                }
                catch
                {

                }

            }

        }

        public async Task SaveLeadFileToDataBaseSQL(string file)
        {
            if (File.Exists(file))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                try
                {
                    using (var pck = new OfficeOpenXml.ExcelPackage())
                    {
                        using (var stream = System.IO.File.OpenRead(file))
                        {
                            pck.Load(stream);
                        }
                        var ws = pck.Workbook.Worksheets.First();
                        if (ws.Cells["A4"].Text == "#")
                        {
                            // Luu danh sach hen kham
                            int startRow = 5;
                            var ListAppointments = new List<Appointment>();
                            var cultureInfo = new CultureInfo("fr-FR");
                            cultureInfo.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
                            cultureInfo.DateTimeFormat.DateSeparator = "-";
                            for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                            {
                                int stt = 0;
                                string MaLichKham = "";
                                string TenKhachHang = "";
                                string DienThoai = "";
                                string TrangThai = "";
                                DateTime NgayTaoLich = DateTime.Now.Date;
                                DateTime NgayDatLichKham = DateTime.Now.Date;
                                string Loai = "";
                                string DienThoaiVien = "";
                                string TenChiNhanh = "";
                                try
                                {

                                    var wsRow = ws.Cells[rowNum, 1, rowNum, 13];
                                    if (wsRow[rowNum, 1].Text != "")
                                    {
                                        stt = int.Parse(wsRow[rowNum, 1].Text);
                                        MaLichKham = wsRow[rowNum, 2].Text;
                                        string str = wsRow[rowNum, 3].Text;
                                        str = str.Trim();
                                        DienThoai = Regex.Replace(str, "[^0-9]", "");
                                        TenKhachHang = str.Replace(DienThoai, "");
                                        TenKhachHang = TenKhachHang.Trim();
                                        TrangThai = wsRow[rowNum, 5].Text;
                                        Loai = wsRow[rowNum, 8].Text;
                                        DienThoaiVien = wsRow[rowNum, 10].Text;
                                        TenChiNhanh = wsRow[rowNum, 11].Text;
                                        NgayTaoLich = (wsRow[rowNum, 6].Text == "" ? DateTime.Now.Date : DateTime.Parse(wsRow[rowNum, 6].Text).Date);
                                        NgayDatLichKham = (wsRow[rowNum, 7].Text == "" ? DateTime.Now.Date : DateTime.Parse(wsRow[rowNum, 7].Text).Date);
                                        if (TenKhachHang != "" && MaLichKham != "" && DienThoai != "")
                                        {

                                            ListAppointments.Add(new Appointment
                                            {
                                                STT = stt,
                                                MaLichKham = MaLichKham,
                                                TenKhachHang = TenKhachHang,
                                                DienThoai = DienThoai,
                                                TrangThai = TrangThai,
                                                NgayTaoLich = NgayTaoLich,
                                                NgayDatLichKham = NgayDatLichKham,
                                                Loai = Loai,
                                                DienThoaiVien = DienThoaiVien,
                                                TenChiNhanh = TenChiNhanh
                                            }); ;

                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }
                            await SaveAppointmentsSQL(ListAppointments);

                        }
                        else
                        {
                            //Luu Danh Sach Lead
                            string sql = "select TOP 1 * from Leads order by STT DESC";
                            int STT = 1;
                            var leadLonNhat = await sqlDataAccess.loadData<Lead,dynamic>(sql,new { });
                            if (leadLonNhat!=null && leadLonNhat.Count>0)
                            {
                                STT = leadLonNhat[0].STT;
                            }
                            int startRow = 3;
                            var ListLeads = new List<Lead>();
                            var cultureInfo = new CultureInfo("fr-FR");
                            for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                            {
                                int stt = 0;
                                string TenKhachHang = "";
                                string SoPhu = "";
                                string Phone = "";
                                string Nguon = "";
                                string TinhThanh = "";
                                DateTime Ngay = DateTime.Now.Date;
                                try
                                {

                                    var wsRow = ws.Cells[rowNum, 1, rowNum, 7];
                                    if (wsRow[rowNum, 1].Text != "")
                                    {
                                        stt = int.Parse(wsRow[rowNum, 1].Text);
                                        TenKhachHang = wsRow[rowNum, 2].Text;
                                        SoPhu = wsRow[rowNum, 3].Text;
                                        Phone = wsRow[rowNum, 4].Text;
                                        Nguon = wsRow[rowNum, 6].Text;
                                        TinhThanh = wsRow[rowNum, 7].Text;
                                        Ngay = (wsRow[rowNum, 5].Text == "" ? DateTime.Now.Date : DateTime.Parse(wsRow[rowNum, 5].Text, cultureInfo));
                                        if (stt > STT)
                                        {
                                            if (TenKhachHang != "" && Phone != "")
                                            {

                                                ListLeads.Add(new Lead
                                                {
                                                    STT = stt,
                                                    TenKhachHang = TenKhachHang,
                                                    SoPhu = SoPhu,
                                                    Phone = Phone,
                                                    Nguon = Nguon,
                                                    TinhThanh = TinhThanh,
                                                    Ngay = Ngay,
                                                    file = file
                                                }); ;

                                            }
                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }
                            await SaveLaedsSQL(ListLeads);
                        }
                    }
                }
                catch
                {

                }

            }

        }


        public async Task LoadLichKhamTuCRM(bool full)
        {
            string sql = @"select id_order as STT
                               , id_store_order as  MaLichKham
                               , customer_name as TenKhachHang
                               , pes_order.phone as DienThoai
                               , pes_order_status.`name` TrangThai
                               , order_date as NgayTaoLich
                               , delivery_date as NgayDatLichKham
                               , pes_type.`name` Loai
                               , `user`.full_name as DienThoaiVien
                               , pes_store.name TenChiNhanh
                        from pes_order
                        INNER JOIN pes_store on pes_order.id_store = pes_store.id_store
                        left join `user` on pes_order.id_user = `user`.id
                        left join pes_order_status on pes_order.id_order_status= pes_order_status.id_order_status
                        left join pes_type on pes_order.id_order_type = pes_type.id_type
                        ";
            if (!full)
            {  
                var NgayLapCuoi = await _context.Appointments.AsQueryable().OrderByDescending(x => x.NgayTaoLich).FirstOrDefaultAsync();

                if (NgayLapCuoi != null)
                {
                    sql += " where order_date>='" + NgayLapCuoi.NgayTaoLich.ToString("yyyy-MM-dd") + "'";
                }
            }            
            var list = await mySqlDataAccess.loadData<Appointment, dynamic>(sql, new { });
            await SaveAppointments(list);
        }

        public async Task LoadLichKhamTuCRMSQL(bool full)
        {
            string sql = @"select id_order as STT
                               , id_store_order as  MaLichKham
                               , customer_name as TenKhachHang
                               , pes_order.phone as DienThoai
                               , pes_order_status.`name` TrangThai
                               , order_date as NgayTaoLich
                               , delivery_date as NgayDatLichKham
                               , pes_type.`name` Loai
                               , `user`.full_name as DienThoaiVien
                               , pes_store.name TenChiNhanh
                        from pes_order
                        INNER JOIN pes_store on pes_order.id_store = pes_store.id_store
                        left join `user` on pes_order.id_user = `user`.id
                        left join pes_order_status on pes_order.id_order_status= pes_order_status.id_order_status
                        left join pes_type on pes_order.id_order_type = pes_type.id_type
                        ";
            if (!full)
            {

                string sqltam = "select TOP 1 * from Appointments order by NgayTaoLich DESC";
                var NgayLapCuoi = await sqlDataAccess.loadData<Appointment, dynamic>(sqltam, new { });
                //var NgayLapCuoi = await _context.Appointments.AsQueryable().OrderByDescending(x => x.NgayTaoLich).FirstOrDefaultAsync();


                if (NgayLapCuoi != null && NgayLapCuoi.Count > 0)
                {
                    sql += " where order_date>='" + NgayLapCuoi[0].NgayTaoLich.ToString("yyyy-MM-dd") + "'";
                }
            }
            var list = await mySqlDataAccess.loadData<Appointment, dynamic>(sql, new { });

            await SaveAppointmentsSQL(list);
        }

        [Obsolete]
        public async Task LoadLeadsFromGoogle()
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "MSGData.xlsx";
            LoadLeadFromGoogle.DownFileNew(fileName);
            await SaveLeadFileToDataBase(fileName);
        }

        [Obsolete]
        public async Task LoadLeadsFromGoogleSQL()
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "MSGData.xlsx";
            LoadLeadFromGoogle.DownFileNew(fileName);
            await SaveLeadFileToDataBaseSQL(fileName);
        }
    }
}
