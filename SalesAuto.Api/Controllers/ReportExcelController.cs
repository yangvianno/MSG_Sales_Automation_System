using HelperLib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using SalesAuto.Api.Repositories;
using SalesAuto.Models;
using SalesAuto.Models.Entities;
using SalesAuto.Models.SearchModel;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportExcelController : ControllerBase
    {
        private readonly IBenhNhansRepo benhNhanRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ISaleAutoReportRepo reportRepository;
        private readonly IMailRepo mailRepo;
        private const string _contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ReportExcelController(IBenhNhansRepo benhNhanRepository, IWebHostEnvironment webHostEnvironment, ISaleAutoReportRepo reportRepository, IMailRepo mailRepo)
        {
            this.benhNhanRepository = benhNhanRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.reportRepository = reportRepository;
            this.mailRepo = mailRepo;
        }

        [HttpPost]
        public async Task<IActionResult> DownLoadExcelFile(BenhNhanSM benhNhanSM)
        {
            byte[] bytes;
            var MaBenhVienNguon = "O";
            try
            {
                if (Request != null)
                {
                    var HeaderMaBenhVienNguon = Request.Headers["MaBenhVienNguon"];
                    if (HeaderMaBenhVienNguon.Count >= 0)
                    {
                        MaBenhVienNguon = HeaderMaBenhVienNguon[0].ToString();
                    }
                }
            }
            catch
            {
            }
            var pkg = await reportRepository.createBenhNhanReport(benhNhanSM);
            bytes = pkg.GetAsByteArray();
            pkg.Dispose();
            return File(bytes, _contentType, "BenhNhan.xlsx");
        }

        [HttpPost]
        [Route("CPA")]
        public async Task<IActionResult> DownLoadCPAExcelFile(int nam, int thang = 0, string MaBenhVien = "")
        {
            var MaBenhVienNguon = "O";
            if (MaBenhVien != "")
            {
                MaBenhVienNguon = MaBenhVien;
            }
            else
            {

                try
                {
                    if (Request != null)
                    {
                        var HeaderMaBenhVienNguon = Request.Headers["MaBenhVienNguon"];
                        if (HeaderMaBenhVienNguon.Count >= 0)
                        {
                            MaBenhVienNguon = HeaderMaBenhVienNguon[0].ToString();
                        }
                    }
                }
                catch
                {
                }
            }

            byte[] bytes;
            var pkg = await reportRepository.createCPAReport(nam, thang, MaBenhVienNguon);
            bytes = pkg.GetAsByteArray();
            pkg.Dispose();
            return File(bytes, _contentType, thang + "" + nam + "CPAReport.xlsx");
        }

        [HttpPost]
        [Route("SendMailCPA")]
        public async Task<IActionResult> SendMailCPAExcelFile(int nam, int thang = 0)
        {
            var repo = reportRepository;
            var pkg = await repo.createCPAReport(nam, thang);
            FileInfo fi = new FileInfo(thang + nam + "CPAReport.xlsx");
            await pkg.SaveAsAsync(fi);
            MailRequest request = new MailRequest();
            request.Attachments.Add(fi.FullName);
            request.Subject = "Marketting CPA report " + thang + "-" + nam;
            request.Body = "Kính gửi anh/chị báo cáo CPA tháng " + thang + " năm " + nam;
            try
            {
                await mailRepo.SendEmailAsync(request);
                await repo.LuuThangGuiMail(thang, nam);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("TaoDuLieuCPA")]
        public async Task<IActionResult> TaoDuLiauCPAE(int nam, int thang = 0)
        {
            var repo = reportRepository;
            var result = await repo.TaoDuLieuCPA(nam, thang);
            try
            {
                if (result == "Thành công")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("leadfollow")]
        public async Task<IActionResult> DownLoadLeadFollowExcelFile(int nam, int thang = 0, string MaBenhVien = "")
        {

            byte[] bytes;
            var MaBenhVienNguon = "O";
            if (MaBenhVien != "")
            {
                MaBenhVienNguon = MaBenhVien;
            }
            else
            {

                try
                {
                    if (Request != null)
                    {
                        var HeaderMaBenhVienNguon = Request.Headers["MaBenhVienNguon"];
                        if (HeaderMaBenhVienNguon.Count >= 0)
                        {
                            MaBenhVienNguon = HeaderMaBenhVienNguon[0].ToString();
                        }
                    }
                }
                catch
                {
                }
            }
            var repo = reportRepository;
            var pkg = await repo.createLeadFollowReport(nam, thang, MaBenhVienNguon);
            bytes = pkg.GetAsByteArray();
            pkg.Dispose();
            return File(bytes, _contentType, thang + "" + nam + "LeadFollow.xlsx");

        }

        [HttpPost]
        [Route("SendMailleadfollow")]
        public async Task<IActionResult> SendMailLeadFollowExcelFile(int nam, int thang = 0)
        {
            var repo = reportRepository;
            var pkg = await repo.createLeadFollowReport(nam, thang);
            FileInfo fi = new FileInfo(thang + nam + "LeadFollow.xlsx");
            await pkg.SaveAsAsync(fi);
            MailRequest request = new MailRequest();
            request.Attachments.Add(fi.FullName);
            request.Subject = "Marketting lead follow report " + thang + "-" + nam;
            request.Body = "Kính gửi anh/chị báo cáo lead follow tháng " + thang + " năm " + nam;
            try
            {
                await mailRepo.SendEmailAsync(request);
                await repo.LuuThangGuiMail(thang, nam);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("TaoDuLieuleadfollow")]
        public async Task<IActionResult> TaoDuLieuLeadFollow(int nam, int thang = 0)
        {
            var repo = reportRepository;
            var result = await repo.TaoDuLieuLeadFollow(nam, thang);
            try
            {
                if (result == "Thành công")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("kpi")]
        public async Task<IActionResult> DownLoadkpiExcelFile(int thang, int nam)
        {
            byte[] bytes;
            var repo = reportRepository;
            var pkg = await repo.createKPIReport(thang, nam, DateTime.Now);
            bytes = pkg.GetAsByteArray();
            pkg.Dispose();
            return File(bytes, _contentType, thang + "" + nam + "KPIReport.xlsx");
        }

        [HttpPost]
        [Route("SendMailkpi")]
        public async Task<IActionResult> SendMailkpiExcelFile(int thang, int nam)
        {
            var repo = reportRepository;
            var pkg = await repo.createKPIReport(thang, nam, DateTime.Now);
            FileInfo fi = new FileInfo(thang + nam + "KPIReports.xlsx");
            await pkg.SaveAsAsync(fi);
            MailRequest request = new MailRequest();
            request.Attachments.Add(fi.FullName);
            request.Subject = "Marketting KPI report " + thang + "-" + nam;
            request.Body = "Kính gửi anh/chị báo cáo KPI tháng " + thang + " năm " + nam;
            try
            {
                await mailRepo.SendEmailAsync(request);
                await repo.LuuThangGuiMail(thang, nam);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("LeadsChannel")]
        public async Task<IActionResult> DownLoadLeadsChannelExcelFile(int thang, int nam, string MaBenhVien = "")
        {
            byte[] bytes;
            var MaBenhVienNguon = "O";
            if (MaBenhVien != "")
            {
                MaBenhVienNguon = MaBenhVien;
            }
            else
            {

                try
                {
                    if (Request != null)
                    {
                        var HeaderMaBenhVienNguon = Request.Headers["MaBenhVienNguon"];
                        if (HeaderMaBenhVienNguon.Count >= 0)
                        {
                            MaBenhVienNguon = HeaderMaBenhVienNguon[0].ToString();
                        }
                    }
                }
                catch
                {
                }
            }
            var repo = reportRepository;
            var pkg = await repo.createLeadsChannelReport(thang, nam, false, MaBenhVienNguon);
            bytes = pkg.GetAsByteArray();
            pkg.Dispose();
            return File(bytes, _contentType, thang + "" + nam + "LeadsChannelReport.xlsx");
        }

        [HttpPost]
        [Route("SendMailLeadsChannel")]
        public async Task<IActionResult> SendMailLeadsChannelExcelFile(int thang, int nam)
        {
            var repo = reportRepository;
            var pkg = await repo.createLeadsChannelReport(thang, nam);
            FileInfo fi = new FileInfo(thang + "" + nam + "LeadsChannelReports.xlsx");
            await pkg.SaveAsAsync(fi);
            MailRequest request = new MailRequest();
            request.Attachments.Add(fi.FullName);
            request.Subject = "Marketting KPI report " + thang + "-" + nam;
            request.Body = "Kính gửi anh/chị báo cáo KPI tháng " + thang + " năm " + nam;
            try
            {
                await mailRepo.SendEmailAsync(request);
                await repo.LuuThangGuiMail(thang, nam);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("DownloadAll")]
        public async Task<IActionResult> DownLoadAll(int nam, int thang = 0, string MaBenhVien = "")
        {
            byte[] bytes;
            var MaBenhVienNguon = "O";
            if (MaBenhVien != "")
            {
                MaBenhVienNguon = MaBenhVien;
            }
            else
            {

                try
                {
                    if (Request != null)
                    {
                        var HeaderMaBenhVienNguon = Request.Headers["MaBenhVienNguon"];
                        if (HeaderMaBenhVienNguon.Count >= 0)
                        {
                            MaBenhVienNguon = HeaderMaBenhVienNguon[0].ToString();
                        }
                    }
                }
                catch
                {
                }
            }
            if (MaBenhVienNguon == "O")
            {
                var repo = reportRepository;
                var pkg = await repo.createCPAReport(nam, thang);
                var SheetCPA = pkg.Workbook.Worksheets[0];
                pkg = await repo.createLeadFollowReport(nam, thang);
                var sheetLead = pkg.Workbook.Worksheets[0];
                pkg = await repo.createLeadsChannelReport(thang, nam);
                var sheetLeadsChannel = pkg.Workbook.Worksheets[0];
                pkg = await repo.createKPIReport(thang, nam, DateTime.Now);
                pkg.Workbook.Worksheets.Add("CPA", SheetCPA);
                pkg.Workbook.Worksheets.Add("Leads Following", sheetLead);
                pkg.Workbook.Worksheets.Add("Leads Channel", sheetLeadsChannel);
                bytes = pkg.GetAsByteArray();
                pkg.Dispose();
                return File(bytes, _contentType, MaBenhVien + thang + "" + nam + "CPA_Call_Center_Report.xlsx");
            }
            else
            {
                var pkgLead = await reportRepository.createLeadFollowReport(nam, thang, MaBenhVienNguon);
                var sheetLead = pkgLead.Workbook.Worksheets[0];
                var pkgLeadChanel = await reportRepository.createLeadsChannelReport(thang, nam, false, MaBenhVienNguon);
                var sheetLeadsChannel = pkgLeadChanel.Workbook.Worksheets[0];
                var pkg = await reportRepository.createCPAReport(nam, thang, MaBenhVienNguon); ;
                //pkg.Workbook.Worksheets.Add("CPA", SheetCPA);
                pkg.Workbook.Worksheets.Add("Leads Following", sheetLead);
                pkgLead.Dispose();
                pkg.Workbook.Worksheets.Add("Leads Channel", sheetLeadsChannel);
                pkgLeadChanel.Dispose();
                bytes = pkg.GetAsByteArray();
                pkg.Dispose();
                return File(bytes, _contentType, MaBenhVien + "" + thang + "" + nam + "CPA_Call_Center_Report.xlsx");
            }
        }
        [HttpPost]
        [Route("SendMailAll")]
        public async Task<IActionResult> SendMailAll(int nam, int thang = 0, string MaBenhVien = "")
        {
            var MaBenhVienNguon = "O";
            if (MaBenhVien != "")
            {
                MaBenhVienNguon = MaBenhVien;
            }
            else
            {

                try
                {
                    if (Request != null)
                    {
                        var HeaderMaBenhVienNguon = Request.Headers["MaBenhVienNguon"];
                        if (HeaderMaBenhVienNguon.Count >= 0)
                        {
                            MaBenhVienNguon = HeaderMaBenhVienNguon[0].ToString();
                        }
                    }
                }
                catch
                {
                }
            }
            if (MaBenhVienNguon == "O")
            {
                var repo = reportRepository;
                var pkg = await repo.createCPAReport(nam, thang);
                var SheetCPA = pkg.Workbook.Worksheets[0];
                pkg = await repo.createLeadFollowReport(nam, thang);
                var sheetLead = pkg.Workbook.Worksheets[0];
                FileInfo filead = new FileInfo("LeadFollowReport.xlsx");
                await pkg.SaveAsAsync(filead);
                Workbook workbooklead = new Workbook();
                FileInfo fiLead = new FileInfo(string.Format("Sheet2.png"));
                workbooklead.LoadFromFile(filead.FullName);
                {
                    Worksheet sheet2 = workbooklead.Worksheets[0];
                    Image[] imgssheet2 = workbooklead.SaveChartAsImage(sheet2);
                    if (imgssheet2.Length > 0)
                    {
                        Bitmap bit1 = ImageHelper.CombineBitmap(imgssheet2);
                        bit1.Save(fiLead.Name, ImageFormat.Png);
                    }
                }
                workbooklead.Dispose();

                pkg = await repo.createLeadsChannelReport(thang, nam);
                var sheetLeadsChannel = pkg.Workbook.Worksheets[0];
                FileInfo fiChanel = new FileInfo("LeadChanelReport.xlsx");
                await pkg.SaveAsAsync(fiChanel);
                Workbook workbookChanel = new Workbook();
                workbookChanel.LoadFromFile(fiChanel.FullName);
                FileInfo fiChanelImage = new FileInfo(string.Format("Sheet3.png"));
                {
                    Image[] imgssheet3 = workbookChanel.SaveChartAsImage(workbookChanel.Worksheets[0]);
                    if (imgssheet3.Length > 0)
                    {
                        Bitmap bit1 = ImageHelper.CombineBitmap(imgssheet3);
                        bit1.Save(fiChanelImage.Name, ImageFormat.Png);
                    }
                }
                workbookChanel.Dispose();

                pkg = await repo.createKPIReport(thang, nam, DateTime.Now);
                pkg.Workbook.Worksheets.Add("CPA", SheetCPA);
                pkg.Workbook.Worksheets.Add("Leads Following", sheetLead);
                pkg.Workbook.Worksheets.Add("Leads Channel", sheetLeadsChannel);
                FileInfo fi = new FileInfo(thang + "" + nam + "CPA_Call_Center_Report.xlsx");
                await pkg.SaveAsAsync(fi);
                // xuat hinh
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(fi.FullName);
                Worksheet sheet = workbook.Worksheets[0];
                List<string> imagepaths = new List<string>();
                Image[] imgs = workbook.SaveChartAsImage(sheet);
                for (int i = 0; i < imgs.Length; i++)
                {
                    FileInfo f = new FileInfo(string.Format("imgTuan-{0}.png", i));
                    imgs[i].Save(f.Name, ImageFormat.Png);
                    imagepaths.Add(f.FullName);
                }
                imagepaths.Add(fiLead.FullName);
                imagepaths.Add(fiChanelImage.FullName);
                workbook.Dispose();

                MailRequest request = new MailRequest();
                request.Attachments.Add(fi.FullName);
                request.IsBodyHtml = true;
                request.Subject = "CPA & Call Center Report " + thang + "-" + nam;
                request.Body = "Kính gửi anh chị báo cáo tháng " + thang + " năm " + nam;
                try
                {
                    await mailRepo.SendEmailAsync(request, imagepaths);
                    return Ok();
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }
            }
            else
            {
                try
                {
                    var ThongTinGuiMail = await reportRepository.LayThongTinGuiMailCPAAndCallBenhVien(MaBenhVien);
                    foreach (var item in ThongTinGuiMail)
                    {
                        if (item.MaBenhVien == MaBenhVienNguon)
                        {
                            await mailRepo.SendMailCPAAndCallBV(reportRepository, true, false, item.MaBenhVien, item.SendTo, item.CCTo);
                        }
                    }
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost]
        [Route("SendMailBaoCaoTuan")]
        public async Task<IActionResult> SendMailBaoCaoTuan(int tuan, int nam, bool resend = false,string ToMail="")
        {
            try
            {
                await mailRepo.SendMailBaoCaoTuan(reportRepository, resend, ToMail,tuan,nam);
                return Ok("Da gui mail!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("SendMailBaoCaoThang")]
        public async Task<IActionResult> SendMailBaoCaoThang(bool resend = false, bool TinhTheoNgay = false)
        {
            try
            {
                await mailRepo.SendMailBaoCaoThang(reportRepository, resend, TinhTheoNgay);
                return Ok("Da gui mail!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("SendMailBaoCaoTuanBenhVien")]
        public async Task<IActionResult> SendMailBaoCaoTuanBenhVien(bool resend = false)
        {
            try
            {
                await mailRepo.SendMailBaoCaoFollowupPatientTuanBenhVien(reportRepository, resend);
                return Ok("Da gui mail!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("BaoCaoTuan")]
        public async Task<IActionResult> BaoCaoTuan(int tuan, int nam = 0)
        {
            byte[] bytes;
            var repo = reportRepository;
            var pkg = await repo.TaoBaoCaoTuan(tuan, nam);
            bytes = pkg.GetAsByteArray();
            pkg.Dispose();
            return File(bytes, _contentType, tuan + "" + nam + "WeeklyReport.xlsx");
            //byte[] bytes;
            //var pkg = await reportRepository.createCPAReport(nam, thang, MaBenhVienNguon);
            //bytes = pkg.GetAsByteArray();
            //pkg.Dispose();
            //return File(bytes, _contentType, thang + "" + nam + "CPAReport.xlsx");

        }
        [HttpPost]
        [Route("BaoCaoThang")]
        public async Task<IActionResult> BaoCaoThang(int thang, int nam = 0, bool TinhTheoNgay = false)
        {
            byte[] bytes;
            var repo = reportRepository;
            DateTime Ngay = new DateTime(nam, thang, 1);
            var tuan = HelperLib.DateTimeHelp.LayTuan(DateTime.Now);
            if (TinhTheoNgay)
            {
                Ngay = tuan.TuNgay.AddDays(-1);
            }
            int namBC = Ngay.Year;
            int thangBC = Ngay.Month;

            var pkg = await repo.TaoBaoCaoThang(thangBC, namBC, Ngay, TinhTheoNgay);
            bytes = pkg.GetAsByteArray();
            pkg.Dispose();
            return File(bytes, _contentType, thang + "" + nam + "MonthlyReport.xlsx");
        }

        [HttpPost]
        [Route("FollowupPatients")]
        public async Task<IActionResult> FollowupPatients(int tuan, int nam, string MaBenhVien, int Thang = 0)
        {
            byte[] bytes;
            var repo = reportRepository;
            var pkg = await repo.createFollowupPatientsReport(tuan, nam, MaBenhVien, Thang);
            bytes = pkg.GetAsByteArray();
            pkg.Dispose();
            return File(bytes, _contentType, tuan + "" + nam + "FollowupPatients.xlsx");
        }

        [HttpPost]
        [Route("FollowupPatientsGroup")]
        public async Task<IActionResult> FollowupPatientsGroup(int tuan, int nam, int Thang = 0)
        {
            byte[] bytes;
            var repo = reportRepository;
            var pkg = await repo.createFollowupPatientsReportGroup(tuan, nam, Thang);
            bytes = pkg.GetAsByteArray();
            pkg.Dispose();
            return File(bytes, _contentType, tuan + "" + nam + "FollowupPatients.xlsx");
        }
        [HttpPost]
        [Route("SendMailBaoCaoFollowupPatientTuanGroup")]
        public async Task<IActionResult> SendMailBaoCaoFollowupPatientTuanGroup(bool resend=false, string SendTo="")
        {
            try
            {
                var repo = mailRepo;
                await repo.SendMailBaoCaoFollowupPatientTuanGroup(reportRepository,resend,SendTo);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        [Route("SendMailChiTietLeadsChuaBook")]
        public async Task<IActionResult> SendMailChiTietLeadsChuaBook(bool resend)
        {
            try
            {
                await mailRepo.SendMailChiTietLeadsChuaBook(reportRepository, resend);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Gửi mail thành công!");
        }

        [HttpGet]
        public async Task<IActionResult> DownLoadExcel()
        {
            var fileInfo = this.webHostEnvironment.ContentRootFileProvider.GetFileInfo("REPORT_BenhNhan.xlsx");
            var pictureBytes = await System.IO.File.ReadAllBytesAsync(fileInfo.PhysicalPath);
            return File(pictureBytes, _contentType);
        }

        [HttpGet]
        [Route("GetDanhSachLead")]
        public async Task<IActionResult> GetDanhSachLead(DateTime TuNgay, DateTime DenNgay)
        {
            try
            {
                var result = await reportRepository.GetDanhSachLead(TuNgay, DenNgay);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDanhSachBook")]
        public async Task<IActionResult> GetDanhSachBook(DateTime TuNgay, DateTime DenNgay)
        {
            try
            {
                var result = await reportRepository.GetDanhSachBook(TuNgay, DenNgay);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDanhSachKham")]
        public async Task<IActionResult> GetDanhSachKham(DateTime TuNgay, DateTime DenNgay)
        {
            try
            {
                var result = await reportRepository.GetDanhSachKham(TuNgay, DenNgay);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDanhSachPhauThuat")]
        public async Task<IActionResult> GetDanhSachPhauThuat(DateTime TuNgay, DateTime DenNgay)
        {
            try
            {
                var result = await reportRepository.GetDanhSachPhauThuat(TuNgay, DenNgay);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetQuaTrinhKham")]
        public async Task<IActionResult> GetQuaTrinhKham(DateTime TuNgay, DateTime DenNgay)
        {
            try
            {
                var result = await reportRepository.GetQuaTrinhKham(TuNgay, DenNgay);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetQuaTrinhPhauThuat")]
        public async Task<IActionResult> GetQuaTrinhPhauThuat(DateTime TuNgay, DateTime DenNgay)
        {
            try
            {
                var result = await reportRepository.GetQuaTrinhPhauThuat(TuNgay, DenNgay);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
