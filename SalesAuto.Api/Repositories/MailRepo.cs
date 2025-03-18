using DataAccessLibrary;
using HelperLib;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using SalesAuto.Models;
using SalesAuto.Models.Entities;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace SalesAuto.Api.Repositories
{
    public class MailRepo : IMailRepo
    {
        private readonly MailSettings mailSettings;
        public MailRepo(IOptions<MailSettings> mailSettings)
        {
            this.mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest, List<string> ImagePaths=null, string SendTo = "", string CCto = "")
        {
            MailMessage msg = new();
            foreach (var str in mailRequest.Attachments)
            {
                Attachment item = new Attachment(str);
                msg.Attachments.Add(item);
            }
            string strSendTo = mailSettings.SendTo;
            string strCCTo = mailSettings.CCTo;
            if (SendTo!="")
            {
                strSendTo = SendTo;
                strCCTo = CCto;
            }

            if (strSendTo != null)
            {
                foreach (string str in strSendTo.Split(','))
                {
                    if (str.Trim() != "")
                    {
                        msg.To.Add(new MailAddress(str));
                    }
                }
            }
            if (strCCTo != null)
            {
                foreach (string str in strCCTo.Split(','))
                {
                    if (str.Trim() != "")
                    {
                        msg.CC.Add(new MailAddress(str));
                    }
                }
            }
            msg.From = new MailAddress(mailSettings.Mail, mailSettings.DisplayName);
            msg.Subject = mailRequest.Subject;
            if(mailRequest.IsBodyHtml)
            {
                msg.AlternateViews.Add(Mail_Body(ImagePaths, mailRequest.Body));
            }
            else
            {
                msg.Body = mailRequest.Body + "<br/> Thanks and best regards<br/>  ";
            }
            msg.IsBodyHtml = true;
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(mailSettings.Mail, mailSettings.Password);
            client.Port = mailSettings.Port; // You can use Port 25 if 587 is blocked (mine is!)
            client.Host = mailSettings.Host;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            try
            {
                await client.SendMailAsync(msg);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static AlternateView Mail_Body(List<string> pathImages, string body)
        {
            
            if (pathImages != null)
            {
                string str = @"  
                    <table>  
                        <tr>  
                            <td> " + body + @"</td>                          
                        </tr>  
                ";
                List<LinkedResource> Imgs = new List<LinkedResource>();
                int i = 1;
                foreach (string path in pathImages)
                {                    
                    if (File.Exists(path))
                    {
                        LinkedResource Img = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                        Img.ContentId = "MyImage"+i;                        
                        str += @"
                            <tr>
                                <td style="+"\"text - align:center\""+@">
                                        <img src="+ "\"cid:MyImage"+i+"\" id = 'img"+i+@"' alt = '' />
                                </td>
                            </tr>";
                        Imgs.Add(Img);
                    }
                    i++;
                }
                str += @"
                <tr>
                            <td>
                                Thanks and best regards
                            </td>
                        </tr>
                    </table>";
                AlternateView AV = AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
                foreach (var item in Imgs)
                {
                    AV.LinkedResources.Add(item);
                }
                return AV;
            }
            else
            {
                string str = @"  
                <table>  
                    <tr>  
                        <td> " + body + @"
                        </td>  
                    </tr>  
                    <tr>
                            <td>
                                Thanks and best regards
                            </td>
                        </tr>
                </table>  
                ";
                AlternateView AV = AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
                return AV;
            }
            
        }

        public async Task SendMailBaoCaoThang(ISaleAutoReportRepo reportRepository, bool resend = false, bool GuiTheoTuan=false)
        {
            DateTime Ngay = DateTime.Now.AddMonths(-1);
            DateTime NgayBaoCaoTheoThang = DateTime.Now;
            var tuan = HelperLib.DateTimeHelp.LayTuan(DateTime.Now);
            if (GuiTheoTuan)
            {                
                NgayBaoCaoTheoThang = tuan.TuNgay.AddDays(-1);
                Ngay = NgayBaoCaoTheoThang;
            }
            int namBC = Ngay.Year;
            int thangBC = Ngay.Month;

            var repo = reportRepository;
            var DaGuiMail = (await repo.CheckDaGuiMailThang(thangBC, namBC));
            if (GuiTheoTuan)
            {
                DaGuiMail = (await repo.CheckDaGuiMailThangTheoTuan(tuan.Tuan, tuan.Nam));
            }

            if (resend || !DaGuiMail)
            {
                var pkg = await repo.TaoBaoCaoThang(thangBC, namBC, NgayBaoCaoTheoThang, GuiTheoTuan);
                string fileName = namBC + "_" + thangBC + "_MonthlyReports.xlsx";
                if (GuiTheoTuan)
                {
                    fileName = namBC + "_" + thangBC + "_" + NgayBaoCaoTheoThang.ToString("MMddyyyy") + "_MonthlyReports.xlsx";
                }
                int stt = 0;
                while (File.Exists((stt==0?"":""+stt)+fileName))
                {                    
                    stt++;
                }
                fileName = (stt == 0 ? "" : "" + stt) + fileName;
                FileInfo fi = new(fileName);
                await pkg.SaveAsAsync(fi);
                pkg.Dispose();

                Workbook workbook = new();
                workbook.LoadFromFile(fi.FullName);
                Worksheet sheet = workbook.Worksheets[0];
                //Image[] imgs = workbook.SaveChartAsImage(sheet);
                //"ChartGroupExamSur"
                FileInfo fiImage = new(namBC + thangBC + "MonthlyReports.png");
                List<string> images = new();
                sheet.Zoom = 100;
                sheet.PageSetup.TopMargin = 0;
                sheet.PageSetup.LeftMargin = 0;
                sheet.PageSetup.BottomMargin = 0;
                sheet.PageSetup.RightMargin = 0;
                sheet.PageSetup.VResolution = 2400;
                sheet.PageSetup.HResolution = 2400;
                sheet.SaveToImage(fiImage.FullName, ImageFormat.Png);
                images.Add(fiImage.FullName);

                MailRequest request = new();
                request.Attachments.Add(fi.FullName);
                request.IsBodyHtml = true;
                
                request.Subject = "Marketting monthly report " + thangBC + "/" + namBC ;
                if (GuiTheoTuan)
                {
                    request.Subject += " (" + NgayBaoCaoTheoThang.ToString("MM/dd/yyyy") + ")";
                }
                request.Body = "Kính gửi anh chị báo cáo tháng " + thangBC + "/" + namBC;

                if (GuiTheoTuan)
                {
                    request.Body += " tới ngày " + NgayBaoCaoTheoThang.ToString("dd/MM/yyyy");
                }
                var ThongTin = await repo.LayThongTinGuiMailMonthly();
                try
                {
                    await SendEmailAsync(request, images,ThongTin.SendTo, ThongTin.CCTo);
                    if (GuiTheoTuan)
                    {
                        await repo.LuuThongTinGuiMailThangTheoTuan(tuan.Tuan, tuan.Nam, KetQuaGuiMail.ThanhCong);
                    }
                    else
                    {
                        await repo.LuuThongTinGuiMailThang(thangBC, namBC, KetQuaGuiMail.ThanhCong);
                    }
                }
                catch (Exception)
                {
                    if (GuiTheoTuan)
                    {
                        await repo.LuuThongTinGuiMailThangTheoTuan(tuan.Tuan, tuan.Nam, KetQuaGuiMail.Loi);
                    }
                    else
                    {
                        await repo.LuuThongTinGuiMailThang(thangBC, namBC, KetQuaGuiMail.Loi);
                    }

                }
                workbook.Dispose();
            }
        }

        public async Task SendMailCPAAndCall(ISaleAutoReportRepo reportRepository, bool resend = false, bool GuiTheoTuan = false, string MaBenhVien = "O")
        {
            if (MaBenhVien!="O")
            {
                var ThongTinGuiMail = await reportRepository.LayThongTinGuiMailCPAAndCallBenhVien(MaBenhVien);
                foreach (var item in ThongTinGuiMail)
                {
                    if (item.MaBenhVien == MaBenhVien)
                    {
                        await SendMailCPAAndCallBV(reportRepository, resend, GuiTheoTuan, MaBenhVien,item.SendTo, item.CCTo);
                    }
                }
                return;
            }
            DateTime Ngay = DateTime.Now.AddMonths(-1);
            DateTime NgayBaoCaoTheoThang = DateTime.Now;
            var tuan = HelperLib.DateTimeHelp.LayTuan(DateTime.Now);
            if (GuiTheoTuan)
            {
                NgayBaoCaoTheoThang = tuan.TuNgay.AddDays(-1);
                Ngay = NgayBaoCaoTheoThang;
            }
            int namBC = Ngay.Year;
            int thangBC = Ngay.Month;

            var repo = reportRepository;
            var DaGuiMail = (await repo.CheckDaGuiMailCPAAndCall(thangBC, namBC, MaBenhVien));
            if (GuiTheoTuan)
            {
                DaGuiMail = (await repo.CheckDaGuiMailCPAAndCallTheoTuan(tuan.Tuan, tuan.Nam, MaBenhVien));
            }

            if (resend || !DaGuiMail)
            {    
                
                await repo.TaoDuLieuCPA(namBC, thangBC, true);
                await repo.TaoDuLieuLeadFollow(namBC, thangBC, true);
                var pkg = await repo.createCPAReport(namBC, thangBC);
                var SheetCPA = pkg.Workbook.Worksheets[0];
                pkg = await repo.createLeadFollowReport(namBC, thangBC);
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

                pkg = await repo.createLeadsChannelReport(thangBC, namBC,GuiTheoTuan);
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
                if (GuiTheoTuan)
                {
                    pkg = await repo.createKPIReport(thangBC, namBC,tuan.TuNgay.AddMinutes(-1),true);
                }
                else
                {
                    pkg = await repo.createKPIReport(thangBC, namBC,DateTime.Now);
                }
                
                pkg.Workbook.Worksheets.Add("CPA", SheetCPA);
                pkg.Workbook.Worksheets.Add("Leads Following", sheetLead);
                pkg.Workbook.Worksheets.Add("Leads Channel", sheetLeadsChannel);
                FileInfo fi = new FileInfo(thangBC + "" +namBC + "CPA_Call_Center_Report.xlsx");
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
                request.Subject = "CPA & Call Center Report " + thangBC + "-" + namBC + " ("+ Ngay.ToString("dd/MM/yyyy") +")";
                request.Body = "Kính gửi anh chị báo cáo tháng " + thangBC + " năm " + namBC + " (" + Ngay.ToString("dd/MM/yyyy") + ")";
                var thongtin = (await reportRepository.LayThongTinGuiMailCPAAndCallBenhVien(MaBenhVien)).FirstOrDefault();                
                try
                {
                    if (thongtin != null)
                    {
                        await SendEmailAsync(request, imagepaths, thongtin.SendTo, thongtin.CCTo);
                    }
                    else
                    {
                        await SendEmailAsync(request, imagepaths);
                    }
                    if (GuiTheoTuan)
                    {
                        await repo.LuuThongTinGuiMailCPATheoTuan(tuan.Tuan, tuan.Nam, KetQuaGuiMail.ThanhCong, MaBenhVien);
                    }
                    else
                    {
                        await repo.LuuThongTinGuiMailCPA(thangBC, namBC, KetQuaGuiMail.ThanhCong, MaBenhVien);
                    }
                }
                catch (Exception ex)
                {
                    if (GuiTheoTuan)
                    {
                        await repo.LuuThongTinGuiMailCPATheoTuan(tuan.Tuan, tuan.Nam, KetQuaGuiMail.Loi, MaBenhVien);
                    }
                    else
                    {
                        await repo.LuuThongTinGuiMailCPA(thangBC, namBC, KetQuaGuiMail.Loi, MaBenhVien);
                    }
                }
                workbook.Dispose();
            }
        }

        public async Task SendMailCPAAndCallBV(ISaleAutoReportRepo reportRepository, bool resend = false, bool GuiTheoTuan = false, string MaBenhVien = "O", string SendTo="", string CCTo="")
        {   
            DateTime Ngay = DateTime.Now.AddMonths(-1);
            DateTime NgayBaoCaoTheoThang = DateTime.Now;
            var tuan = HelperLib.DateTimeHelp.LayTuan(DateTime.Now);
            if (GuiTheoTuan)
            {
                NgayBaoCaoTheoThang = tuan.TuNgay.AddDays(-1);
                Ngay = NgayBaoCaoTheoThang;
            }
            int namBC = Ngay.Year;
            int thangBC = Ngay.Month;

            var repo = reportRepository;
            var DaGuiMail = (await repo.CheckDaGuiMailCPAAndCall(thangBC, namBC, MaBenhVien));
            if (GuiTheoTuan)
            {
                DaGuiMail = (await repo.CheckDaGuiMailCPAAndCallTheoTuan(tuan.Tuan, tuan.Nam, MaBenhVien));
            }

            if (resend || !DaGuiMail)
            {
                if (MaBenhVien != "O")
                {
                    await repo.TaoDuLieuCPA(namBC, thangBC, true);
                    await repo.TaoDuLieuLeadFollow(namBC, thangBC, true);
                }               
                var pkg = await repo.createLeadFollowReport(namBC, (GuiTheoTuan ? 0 : thangBC), MaBenhVien);                
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

                pkg = await repo.createLeadsChannelReport(thangBC, namBC, GuiTheoTuan, MaBenhVien);
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
                var pkgLead = await repo.createLeadFollowReport(namBC, (GuiTheoTuan ? 0 : thangBC), MaBenhVien);
                var sheetLead = pkgLead.Workbook.Worksheets[0];
                var pkgLeadChanel = await repo.createLeadsChannelReport(thangBC, namBC, GuiTheoTuan, MaBenhVien);
                var sheetLeadsChannel = pkgLeadChanel.Workbook.Worksheets[0];

                pkg = await repo.createCPAReport(namBC, (GuiTheoTuan ? 0 : thangBC), MaBenhVien); ;
                //pkg.Workbook.Worksheets.Add("CPA", SheetCPA);
                pkg.Workbook.Worksheets.Add("Leads Following", sheetLead);
                pkgLead.Dispose();
                pkg.Workbook.Worksheets.Add("Leads Channel", sheetLeadsChannel);
                pkgLeadChanel.Dispose();
                var sheetBooking=pkg.Workbook.Worksheets.Add("booking HO");
                if (GuiTheoTuan)
                {
                    sheetBooking.Cells[1, 1].LoadFromDataTable(await reportRepository.GetBookingTable(tuan.TuNgay, tuan.DenNgay, MaBenhVien), true);
                }
                else
                {
                    DateTime TuNgay = new DateTime(namBC, thangBC, 1);
                    DateTime DenNgay = TuNgay.AddMonths(1).AddMinutes(-1);
                    sheetBooking.Cells[1, 1].LoadFromDataTable(await reportRepository.GetBookingTable(TuNgay, DenNgay, MaBenhVien), true);
                }                

                FileInfo fi = new FileInfo(thangBC + "" + namBC + "CPA_Call_Center_Report.xlsx");
                await pkg.SaveAsAsync(fi);
                pkg.Dispose();
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
                if (MaBenhVien != "O")
                {
                    string TenVietTat = await repo.GetTenVietTatBenhVien(MaBenhVien);
                    request.Subject =(TenVietTat!=""? TenVietTat + " - ":"")  + "CPA & Call Center Report " + thangBC + "-" + namBC + (GuiTheoTuan ? " (" + Ngay.ToString("dd/MM/yyyy") + ")" : "");
                }
                else
                {
                    request.Subject = "CPA & Call Center Report " + thangBC + "-" + namBC + (GuiTheoTuan ? " (" + Ngay.ToString("dd/MM/yyyy") + ")" : "");
                }

                request.Body = "Kính gửi anh chị báo cáo tháng " + thangBC + " năm " + namBC + (GuiTheoTuan ? " (" + Ngay.ToString("dd/MM/yyyy") + ")":"");                
                try
                {
                    if (SendTo != "")
                    {
                        await SendEmailAsync(request, imagepaths, SendTo, CCTo);
                    }
                    else
                    {
                        await SendEmailAsync(request, imagepaths);
                    }
                    if (GuiTheoTuan)
                    {
                        await repo.LuuThongTinGuiMailCPATheoTuan(tuan.Tuan, tuan.Nam, KetQuaGuiMail.ThanhCong, MaBenhVien);
                    }
                    else
                    {
                        await repo.LuuThongTinGuiMailCPA(thangBC, namBC, KetQuaGuiMail.ThanhCong, MaBenhVien);
                    }
                }
                catch (Exception ex)
                {
                    if (GuiTheoTuan)
                    {
                        await repo.LuuThongTinGuiMailCPATheoTuan(tuan.Tuan, tuan.Nam, KetQuaGuiMail.Loi, MaBenhVien);
                    }
                    else
                    {
                        await repo.LuuThongTinGuiMailCPA(thangBC, namBC, KetQuaGuiMail.Loi, MaBenhVien);
                    }
                }
                workbook.Dispose();
            }
        }

        public async Task SendMailBaoCaoTuan(ISaleAutoReportRepo reportRepository, bool resend = false, string Tomail = "", int Tuan=0, int Nam=0)
        {
            
            var tuan = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7));
            //var tuan = DateTimeHelp.LayTuan(new DateTime(2022,1,3).AddDays(-7));

            int namBC = tuan.TuNgay.Year;
            int tuanBC = tuan.Tuan;
            if (Tuan!=0)
            {
                namBC = Nam;
                tuanBC = Tuan;
            }
            var repo = reportRepository;
            if ( resend || !(await repo.CheckDaGuiMailTuan(tuanBC, namBC)))
            {
                var pkg = await repo.TaoBaoCaoTuan(tuanBC, namBC);

                string fileName = namBC + "" + tuanBC + "WeeklyReports.xlsx";
                int stt = 0;
                while (File.Exists(fileName))
                {
                    fileName = stt +"_"+ fileName;
                }
                FileInfo fi = new(fileName);
                await pkg.SaveAsAsync(fi);
                pkg.Dispose();

                Workbook workbook = new Workbook();                
                workbook.LoadFromFile(fi.FullName);
                Worksheet sheet = workbook.Worksheets[0];
                
                List<string> imagepaths = new List<string>();                
                Image[] imgs = workbook.SaveChartAsImage(sheet);
                string ChartNameSends = "Chart1;Chart6;Chart7;Chart9;";
                for (int i = 0; i < imgs.Length; i++)                    
                {
                    if (ChartNameSends.IndexOf(sheet.Charts[i].Name) >= 0)
                    {
                        FileInfo f = new FileInfo(string.Format("imgTuan-{0}.png", i));
                        imgs[i].Save(f.Name, ImageFormat.Png);
                        imagepaths.Add(f.FullName);
                    }
                }
                Bitmap bit1 = ImageHelper.CombineBitmap(new string[2]{imagepaths[0], imagepaths[1]});
                FileInfo f1 = new FileInfo("HinhBaoCaoTuan1.png");
                bit1.Save(f1.Name, ImageFormat.Png);
                bit1 = ImageHelper.CombineBitmap(new string[2] { imagepaths[2], imagepaths[3] });
                FileInfo f2 = new FileInfo("HinhBaoCaoTuan2.png");
                bit1.Save(f2.Name, ImageFormat.Png);
                imagepaths.Clear();
                imagepaths.Add(f1.FullName);
                imagepaths.Add(f2.FullName);
                workbook.Dispose();
                MailRequest request = new MailRequest();
                request.Attachments.Add(fi.FullName);
                request.IsBodyHtml = true;
                request.Subject = "Lasik Workstream W" + tuan.Tuan + "/" + namBC + "(" + tuan.TuNgay.ToString("dd/MM/yyyy") + "-" + tuan.DenNgay.ToString("dd/MM/yyyy") + ")";
                request.Body = "Kính gửi anh chị báo cáo tuần thứ " + tuan.Tuan + " năm " + namBC + " từ ngày " + tuan.TuNgay.ToString("dd/MM/yyyy") + " đến ngày " + tuan.DenNgay.ToString("dd/MM/yyyy");
                var ThongTin = await repo.LayThongTinGuiMailWeekly();
                try
                {
                    if (Tomail=="")
                    {
                        await SendEmailAsync(request, imagepaths, ThongTin.SendTo, ThongTin.CCTo);
                        await repo.LuuThongTinGuiMailTuan(tuanBC, namBC, KetQuaGuiMail.ThanhCong);
                    }
                    else
                    {
                        await SendEmailAsync(request, imagepaths, Tomail, "");
                    }
                    
                    
                }
                catch (Exception)
                {
                    await repo.LuuThongTinGuiMailTuan(tuanBC, namBC, KetQuaGuiMail.Loi);                    
                }
            }
        }

        public async Task SendMailBaoCaoFollowupPatientTuanGroup(ISaleAutoReportRepo reportRepository, bool resend = false, string SendTo ="")
        {
            var tuan = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7));
            int namBC = tuan.TuNgay.Year;
            int tuanBC = tuan.Tuan;
            var repo = reportRepository;

            var ThongTinGuiMail = await reportRepository.LayThongTinGuiMailWeeklyBenhVien("O");
            foreach (var item in ThongTinGuiMail)
            {
                if (resend || !(await repo.CheckDaGuiMailFollowupPatientTuanBenhVien(tuanBC, namBC, item.MaBenhVien)))
                {
                    var pkg = await repo.createFollowupPatientsReportGroup(tuanBC, namBC);
                    string fileName = item.MaBenhVien+namBC + "" + tuanBC + "WeeklyReports.xlsx";
                    int stt = 0;
                    while (File.Exists(fileName))
                    {
                        fileName = stt + "_" + fileName;
                    }
                    FileInfo fi = new(fileName);
                    await pkg.SaveAsAsync(fi);
                    pkg.Dispose();
                    Workbook workbook = new();
                    workbook.LoadFromFile(fi.FullName);
                    Worksheet sheet = workbook.Worksheets[0];
                    //Image[] imgs = workbook.SaveChartAsImage(sheet);
                    //"ChartGroupExamSur"
                    FileInfo fiImage = new(tuanBC +""+ namBC+ item.MaBenhVien + "WeeklyReports.png");
                    List<string> images = new();
                    sheet.Zoom = 100;
                    sheet.PageSetup.TopMargin = 0;
                    sheet.PageSetup.LeftMargin = 0;
                    sheet.PageSetup.BottomMargin = 0;
                    sheet.PageSetup.RightMargin = 0;
                    sheet.PageSetup.VResolution = 2400;
                    sheet.PageSetup.HResolution = 2400;
                    try
                    { 
                        sheet.ToImage(1,1,32,11).Save(fiImage.FullName, ImageFormat.Png);                    
                        //sheet.SaveToImage(fiImage.FullName, ImageFormat.Png);
                        images.Add(fiImage.FullName);
                    }
                    catch (Exception ex)
                    {
                        LogHelp.Write(ex.Message);
                    }
                    
                    MailRequest request = new MailRequest();
                    request.Attachments.Add(fi.FullName);
                    request.IsBodyHtml = true;
                    request.Subject = item.GuiTu + " - Lasik Workstream to date" + tuan.DenNgay.ToString("dd/MM/yyyy");
                    request.Body = "Kính gửi anh chị báo cáo đến ngày " + tuan.DenNgay.ToString("dd/MM/yyyy");                    
                    try
                    {
                        if (SendTo == "")
                        {
                            await SendEmailAsync(request, images, item.SendTo, item.CCTo);
                            await repo.LuuThongTinGuiMailFollowupPatientTuanBenhVien(tuanBC, namBC, KetQuaGuiMail.ThanhCong, item.MaBenhVien);
                        }
                        else
                        {
                            await SendEmailAsync(request, images, SendTo, "");
                        }
                    }
                    catch (Exception)
                    {
                        await repo.LuuThongTinGuiMailFollowupPatientTuanBenhVien(tuanBC, namBC, KetQuaGuiMail.Loi, item.MaBenhVien);
                    }
                    workbook.Dispose();
                }
            }
        }
        public async Task SendMailBaoCaoFollowupPatientTuanBenhVien(ISaleAutoReportRepo reportRepository, bool resend = false)
        {
            var tuan = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7));
            int namBC = tuan.TuNgay.Year;
            int tuanBC = tuan.Tuan;
            var repo = reportRepository;

            var ThongTinGuiMail = await reportRepository.LayThongTinGuiMailWeeklyBenhVien();

            foreach (var item in ThongTinGuiMail)
            {
                if (item.MaBenhVien != "O")
                {
                    if (resend || !(await repo.CheckDaGuiMailFollowupPatientTuanBenhVien(tuanBC, namBC, item.MaBenhVien)))
                    {
                        try
                        {
                            var pkg = await repo.createFollowupPatientsReport(tuanBC, namBC, item.MaBenhVien);
                            string fileName = item.MaBenhVien + namBC + "" + tuanBC + "WeeklyReports.xlsx";
                            int stt = 0;
                            while (File.Exists(fileName))
                            {
                                fileName = stt + "_" + fileName;
                            }
                            FileInfo fi = new(fileName);
                            await pkg.SaveAsAsync(fi);
                            pkg.Dispose();
                            Workbook workbook = new();
                            workbook.LoadFromFile(fi.FullName);
                            Worksheet sheet = workbook.Worksheets[0];
                            //Image[] imgs = workbook.SaveChartAsImage(sheet);
                            //"ChartGroupExamSur"
                            FileInfo fiImage = new(tuanBC + "" + namBC + item.MaBenhVien + "WeeklyReports.png");
                            List<string> images = new();
                            sheet.Zoom = 100;
                            sheet.PageSetup.TopMargin = 0;
                            sheet.PageSetup.LeftMargin = 0;
                            sheet.PageSetup.BottomMargin = 0;
                            sheet.PageSetup.RightMargin = 0;
                            sheet.PageSetup.VResolution = 2400;
                            sheet.PageSetup.HResolution = 2400;
                            try
                            {
                                sheet.ToImage(1, 1, 32, 11).Save(fiImage.FullName, ImageFormat.Png);
                                //sheet.SaveToImage(fiImage.FullName, ImageFormat.Png);
                                images.Add(fiImage.FullName);
                            }
                            catch (Exception ex)
                            {
                                LogHelp.Write(ex.Message);
                            }
                            MailRequest request = new MailRequest();
                            request.Attachments.Add(fi.FullName);
                            request.IsBodyHtml = true;
                            request.Subject = item.GuiTu + " - Lasik Workstream to date" + tuan.DenNgay.ToString("dd/MM/yyyy");
                            request.Body = "Kính gửi anh chị báo cáo đến ngày " + tuan.DenNgay.ToString("dd/MM/yyyy");
                            try
                            {
                                await SendEmailAsync(request, images, item.SendTo, item.CCTo);
                                await repo.LuuThongTinGuiMailFollowupPatientTuanBenhVien(tuanBC, namBC, KetQuaGuiMail.ThanhCong, item.MaBenhVien);
                            }
                            catch (Exception)
                            {
                                await repo.LuuThongTinGuiMailFollowupPatientTuanBenhVien(tuanBC, namBC, KetQuaGuiMail.Loi, item.MaBenhVien);
                            }
                        }
                        catch (Exception ex)
                        {
                            await repo.LuuThongTinGuiMailFollowupPatientTuanBenhVien(tuanBC, namBC, KetQuaGuiMail.Loi, item.MaBenhVien);
                        }
                    }
                }
            }
        }
        public async Task SendMailBaoCaoFollowupPatientThangBenhVien(ISaleAutoReportRepo reportRepository, bool resend = false)
        {            
            int ThangBc = DateTime.Now.AddMonths(-1).Month;
            int namBC = DateTime.Now.AddMonths(-1).Year;            
            var repo = reportRepository;

            var ThongTinGuiMail = await reportRepository.LayThongTinGuiMailWeeklyBenhVien();
            foreach (var item in ThongTinGuiMail)
            {
                if (item.MaBenhVien != "O")
                {
                    if (resend || !(await repo.CheckDaGuiMailFollowupPatientThangBenhVien(ThangBc, namBC, item.MaBenhVien)))
                    {
                        var pkg = await repo.createFollowupPatientsReport(1, namBC, item.MaBenhVien, ThangBc);
                        string fileName = item.MaBenhVien + namBC + "" + ThangBc + "MonthlyReports.xlsx";
                        int stt = 0;
                        while (File.Exists(fileName))
                        {
                            fileName = stt + "_" + fileName;
                        }
                        FileInfo fi = new(fileName);
                        await pkg.SaveAsAsync(fi);
                        pkg.Dispose();
                        Workbook workbook = new();
                        workbook.LoadFromFile(fi.FullName);
                        Worksheet sheet = workbook.Worksheets[0];
                        //Image[] imgs = workbook.SaveChartAsImage(sheet);
                        //"ChartGroupExamSur"
                        FileInfo fiImage = new(ThangBc + "" + namBC + item.MaBenhVien + "MonthlyWeeklyReports.png");
                        List<string> images = new();
                        for (int i = 15; i <= 57; i++)
                        {
                            sheet.HideColumn(i);
                        }
                        sheet.SaveToImage(fiImage.FullName, ImageFormat.Png);
                        images.Add(fiImage.FullName);
                        for (int i = 15; i <= 57; i++)
                        {
                            sheet.ShowColumn(i);
                        }
                        MailRequest request = new MailRequest();
                        request.Attachments.Add(fi.FullName);
                        request.IsBodyHtml = true;
                        request.Subject = item.GuiTu + " - Marketting monthly report " + ThangBc + "-" + namBC;
                        request.Body = "Kính gửi anh chị báo cáo tháng " + ThangBc + " năm " + namBC;
                        try
                        {
                            await SendEmailAsync(request, images, item.SendTo, item.CCTo);
                            await repo.LuuThongTinGuiMailFollowupPatientThangBenhVien(ThangBc, namBC, KetQuaGuiMail.ThanhCong, item.MaBenhVien);
                        }
                        catch (Exception)
                        {
                            await repo.LuuThongTinGuiMailFollowupPatientThangBenhVien(ThangBc, namBC, KetQuaGuiMail.Loi, item.MaBenhVien);
                        }
                        workbook.Dispose();
                    }
                }
            }
        }

        [Obsolete]
        public async Task SendMailBaoCaoFollowupPatientThangGroup(ISaleAutoReportRepo reportRepository, bool resend = false, string SendTo = "")
        {
            int ThangBc = DateTime.Now.AddMonths(-1).Month;
            int namBC = DateTime.Now.AddMonths(-1).Year;
            var repo = reportRepository;

            var ThongTinGuiMail = await reportRepository.LayThongTinGuiMailWeeklyBenhVien("O");
            foreach (var item in ThongTinGuiMail)
            {
                if (item.MaBenhVien== "O")
                {
                    if (resend || !(await repo.CheckDaGuiMailFollowupPatientThangBenhVien(ThangBc, namBC, item.MaBenhVien)))
                    {
                        var pkg = await repo.createFollowupPatientsReportGroup(1, namBC, ThangBc);
                        string fileName = item.MaBenhVien + namBC + "" + ThangBc + "MonthlyReports.xlsx";
                        int stt = 0;
                        while (File.Exists(fileName))
                        {
                            fileName = stt + "_" + fileName;
                        }
                        FileInfo fi = new(fileName);
                        await pkg.SaveAsAsync(fi);
                        pkg.Dispose();
                        Workbook workbook = new();
                        workbook.LoadFromFile(fi.FullName);
                        Worksheet sheet = workbook.Worksheets[0];
                        //Image[] imgs = workbook.SaveChartAsImage(sheet);
                        //"ChartGroupExamSur"
                        FileInfo fiImage = new(ThangBc + "" + namBC + item.MaBenhVien + "MonthlyWeeklyReports.png");
                        List<string> images = new();
                        sheet.Zoom = 100;
                        sheet.PageSetup.TopMargin = 0;
                        sheet.PageSetup.LeftMargin = 0;
                        sheet.PageSetup.BottomMargin = 0;
                        sheet.PageSetup.RightMargin = 0;
                        sheet.PageSetup.VResolution = 2400;
                        sheet.PageSetup.HResolution = 2400;
                        try
                        { 
                            sheet.ToImage(1, 1, 32, 12).Save(fiImage.FullName, ImageFormat.Png);                        
                            //sheet.SaveToImage(fiImage.FullName, ImageFormat.Png);
                            images.Add(fiImage.FullName);
                        }
                        catch
                        {

                        }
                        MailRequest request = new MailRequest();
                        request.Attachments.Add(fi.FullName);
                        request.IsBodyHtml = true;
                        request.Subject = item.GuiTu + " - Marketting monthly report " + ThangBc + "-" + namBC;
                        request.Body = "Kính gửi anh chị báo cáo tháng " + ThangBc + " năm " + namBC;
                        try
                        {
                            if (SendTo == "")
                            {
                                await SendEmailAsync(request, images, item.SendTo, item.CCTo);
                                await repo.LuuThongTinGuiMailFollowupPatientThangBenhVien(ThangBc, namBC, KetQuaGuiMail.ThanhCong, item.MaBenhVien);
                            }
                            else
                            {
                                await SendEmailAsync(request, images, SendTo, "");
                            }
                        }
                        catch (Exception)
                        {
                            await repo.LuuThongTinGuiMailFollowupPatientThangBenhVien(ThangBc, namBC, KetQuaGuiMail.Loi, item.MaBenhVien);
                        }
                        workbook.Dispose();
                    }
                }
            }
        }
        public async Task SendMailBaoCaoCPAAndCallBenhVien(ISaleAutoReportRepo reportRepository,string MaBenhVien, bool TinhToiNgay = false, bool resend = false)
        {
            
            var ThongTinGuiMail = await reportRepository.LayThongTinGuiMailCPAAndCallBenhVien(MaBenhVien);
            foreach (var item in ThongTinGuiMail)
            {
                await SendMailCPAAndCallBV(reportRepository, resend, TinhToiNgay, item.MaBenhVien, item.SendTo, item.CCTo);
            }
        }

        public async Task SendMailChiTietLeadsChuaBook(ISaleAutoReportRepo reportRepository, bool resend = false)
        {
            var tuan = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7));
            //var tuan = DateTimeHelp.LayTuan(new DateTime(2022,1,3).AddDays(-7));

            int namBC = tuan.TuNgay.Year;
            int tuanBC = tuan.Tuan;            
            
            if (resend || !(await reportRepository.CheckDaGuiMailChiTietLeadsChuaBook(tuanBC, namBC)))
            {
                var pkg = await reportRepository.GetChiTietLeadsChuaBook(tuan.TuNgay.AddDays(-7), tuan.DenNgay, "O");

                string fileName = namBC + "" + tuanBC + "LeadsChuaBookReports.xlsx";
                int stt = 0;
                while (File.Exists(fileName))
                {
                    fileName = stt + "_" + fileName;
                }
                FileInfo fi = new(fileName);
                await pkg.SaveAsAsync(fi);
                pkg.Dispose();

                Workbook workbook = new Workbook();
                workbook.LoadFromFile(fi.FullName);
                Worksheet sheet = workbook.Worksheets[0];

                List<string> imagepaths = new List<string>();
                FileInfo f1 = new FileInfo(namBC + "" + tuanBC + "Leads.png");
                sheet.Zoom = 100;
                sheet.PageSetup.TopMargin = 0;
                sheet.PageSetup.LeftMargin = 0;
                sheet.PageSetup.BottomMargin = 0;
                sheet.PageSetup.RightMargin = 0;
                sheet.PageSetup.VResolution = 2400;
                sheet.PageSetup.HResolution = 2400;
                sheet.SaveToImage(f1.FullName);                
                imagepaths.Add(f1.FullName);
                workbook.Dispose();
                MailRequest request = new MailRequest();
                request.Attachments.Add(fi.FullName);
                request.IsBodyHtml = true;
                request.Subject = "Leads follow từ ngày " + tuan.TuNgay.AddDays(-7).ToString("dd/MM/yyyy") + "-" + tuan.DenNgay.ToString("dd/MM/yyyy");
                request.Body = "Kính gửi anh/chị báo cáo theo dõi leads từ ngày " + tuan.TuNgay.AddDays(-7).ToString("dd/MM/yyyy") + " đến ngày " + tuan.DenNgay.ToString("dd/MM/yyyy");
                var ThongTin = await reportRepository.LayThongTinGuiMailChiTietLeadsChuaBook();
                try
                {
                    await SendEmailAsync(request, imagepaths, ThongTin.SendTo, ThongTin.CCTo);
                    await reportRepository.LuuThongTinGuiMailChiTietLeadsChuaBook(tuanBC, namBC, KetQuaGuiMail.ThanhCong);
                }
                catch (Exception)
                {
                    await reportRepository.LuuThongTinGuiMailChiTietLeadsChuaBook(tuanBC, namBC, KetQuaGuiMail.Loi);
                }
            }
        }

        #region Gửi mail daily

        [Obsolete]
        public async Task SendMailDaiLyTuanMat(ISaleAutoReportRepo reportRepository, IDailyReportRepo dailyReportRepository, bool resend = false)
        {
            var tuan = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7));
            try
            {
                if (! (await reportRepository.CheckDaGuiThongTinGuiMailDailyTuan(LoaiDailyReportTuan.Mat, tuan.Tuan, tuan.Nam)) || resend)
                {
                    var ThongTinGuiMail = await reportRepository.LayThongTinGuiMailDailyTuan();                    
                    var TuanTruoc = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-14));
                    MailRequest request = new MailRequest();
                    request.IsBodyHtml = true;
                    request.Subject = "Daily reports các bv mắt tuần " + tuan.Tuan + "/" + tuan.Nam + " từ ngày " + tuan.TuNgay.ToString("dd/MM/yyyy") + " đến ngày " + tuan.DenNgay.ToString("dd/MM/yyyy");
                    request.Body = "Kính gửi anh/chị báo cáo bv mắt tuần " + tuan.Tuan + " năm " + tuan.Nam + ", từ ngày " + tuan.TuNgay.ToString("dd/MM/yyyy") + " đến ngày " + tuan.DenNgay.ToString("dd/MM/yyyy");
                    string BaoCao = (await dailyReportRepository.GetDailyReportMatSumString());

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage pkg = new ExcelPackage();
                    var Sheet = pkg.Workbook.Worksheets.Add("Summary");
                    DataTable table = await dailyReportRepository.GetDailyReportMatSumTable();                    
                    await dailyReportRepository.CreateExcel(Sheet, table, false, true, true);
                    table = await dailyReportRepository.GetDailyReportBenhVienMatSumTable();
                    await dailyReportRepository.CreateExcel(Sheet, table, false, false, false, false, 0,11);
                    

                    Sheet = pkg.Workbook.Worksheets.Add("Tuần này");
                    table = await dailyReportRepository.GetReportMatTable();
                    await dailyReportRepository.CreateExcel(Sheet, table, false, true, true);                    

                    Sheet = pkg.Workbook.Worksheets.Add("Tuần trước");
                    table = await dailyReportRepository.GetDailyReportMatTable(TuanTruoc.TuNgay, TuanTruoc.DenNgay);
                    await dailyReportRepository.CreateExcel(Sheet, table, false);

                    Sheet = pkg.Workbook.Worksheets.Add("YTD");
                    table = await dailyReportRepository.GetDailyReportMatTable(new DateTime(tuan.DenNgay.Year, 1, 1), tuan.DenNgay);
                    await dailyReportRepository.CreateExcel(Sheet, table, false);

                    Sheet = pkg.Workbook.Worksheets.Add("LY");
                    table = await dailyReportRepository.GetDailyReportMatTable(new DateTime(tuan.DenNgay.Year - 1, 1, 1), tuan.DenNgay.AddYears(-1));
                    await dailyReportRepository.CreateExcel(Sheet, table, false);

                    FileInfo file = new FileInfo("EyesHosReport.xlsx");
                    await pkg.SaveAsAsync(file);
                    pkg.Dispose();

                    //string BaoCaoTuanTruoc = (await dailyReportRepository.GetDailyReportMatString(TuanTruoc.TuNgay,TuanTruoc.DenNgay));
                    //string DailyNam = (await dailyReportRepository.GetDailyReportMatString(new DateTime(tuan.DenNgay.Year,1,1), tuan.DenNgay));
                    //string DailyNamTruoc = (await dailyReportRepository.GetDailyReportMatString(new DateTime(tuan.DenNgay.Year-1, 1, 1), tuan.DenNgay.AddYears(-1)));
                    //FileInfo fiTuan = new FileInfo(tuan.Tuan + "-" + tuan.Nam + "DailyTuan.xlsx");
                    //FileInfo fiTuanTruoc = new FileInfo(tuan.Tuan + "-" + tuan.Nam + "DailyTuantruoc.xlsx");
                    //FileInfo fiNam = new FileInfo(tuan.Tuan + "-" + tuan.Nam + "DailyNam.xlsx");
                    //FileInfo fiNamTruoc = new FileInfo(tuan.Tuan + "-" + tuan.Nam + "DailyNamTruoc.xlsx");

                    //await File.WriteAllTextAsync (fiTuan.FullName, BaoCao,System.Text.Encoding.UTF8);
                    //await File.WriteAllTextAsync(fiTuanTruoc.FullName, BaoCaoTuanTruoc, System.Text.Encoding.UTF8);
                    //await File.WriteAllTextAsync(fiNam.FullName, DailyNam, System.Text.Encoding.UTF8);
                    //await File.WriteAllTextAsync(fiNamTruoc.FullName, DailyNamTruoc, System.Text.Encoding.UTF8);
                    Workbook workbook = new();
                    workbook.LoadFromFile(file.FullName);
                    Worksheet sheet = workbook.Worksheets[0];
                    //Image[] imgs = workbook.SaveChartAsImage(sheet);
                    //"ChartGroupExamSur"
                    FileInfo fiImage = new(TuanTruoc.Tuan + "" + TuanTruoc.Nam + "WeeklEyeReports.png");
                    List<string> images = new();
                    sheet.Columns[10].ColumnWidth = 2;                    
                    sheet.Zoom = 100;
                    sheet.PageSetup.TopMargin = 0;
                    sheet.PageSetup.LeftMargin = 0;
                    sheet.PageSetup.BottomMargin = 0;
                    sheet.PageSetup.RightMargin = 0;
                    sheet.PageSetup.VResolution = 2400;
                    sheet.PageSetup.HResolution = 2400;
                    sheet.SaveToImage(fiImage.FullName, ImageFormat.Png);    
                    sheet.Zoom = 100;
                    images.Add(fiImage.FullName);

                    //BaoCao = "<p>" + BaoCao + "</p>";
                    //request.Body += BaoCao;

                    //request.Attachments.Add(fiTuan.FullName);
                    //request.Attachments.Add(fiTuanTruoc.FullName);
                    //request.Attachments.Add(fiNam.FullName);
                    //request.Attachments.Add(fiNamTruoc.FullName);
                    request.Attachments.Add(file.FullName);
                    await SendEmailAsync(request, images, ThongTinGuiMail.SendTo, ThongTinGuiMail.CCTo);
                    await reportRepository.LuuThongTinGuiMailDailyTuan(LoaiDailyReportTuan.Mat, tuan.Tuan, tuan.Nam, KetQuaGuiMail.ThanhCong);
                }
            }
            catch (Exception ex)
            {
                await reportRepository.LuuThongTinGuiMailDailyTuan(LoaiDailyReportTuan.Mat, tuan.Tuan, tuan.Nam, KetQuaGuiMail.Loi);
            }
        }

        public async Task SendMailDaiLyTuanDaKhoa(ISaleAutoReportRepo reportRepository, IDailyReportRepo dailyReportRepository, bool resend = false)
        {
            var tuan = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7));
            try
            {
                if (!(await reportRepository.CheckDaGuiThongTinGuiMailDailyTuan(LoaiDailyReportTuan.DaKhoa, tuan.Tuan, tuan.Nam)) || resend)
                {
                    var ThongTinGuiMail = await reportRepository.LayThongTinGuiMailDailyTuan("O",LoaiDailyReportTuan.DaKhoa);
                    var TuanTruoc = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-14));
                    MailRequest request = new MailRequest();
                    request.IsBodyHtml = true;
                    request.Subject = "Daily reports các bv đa khoa tuần " + tuan.Tuan + "/" + tuan.Nam + " từ ngày " + tuan.TuNgay.ToString("dd/MM/yyyy") + " đến ngày " + tuan.DenNgay.ToString("dd/MM/yyyy");
                    request.Body = "Kính gửi anh/chị báo cáo bv đa khoa tuần " + tuan.Tuan + " năm " + tuan.Nam + ", từ ngày " + tuan.TuNgay.ToString("dd/MM/yyyy") + " đến ngày " + tuan.DenNgay.ToString("dd/MM/yyyy");
                    
                    string BaoCao = (await dailyReportRepository.GetDailyReportDaKhoaSumString());

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage pkg = new ExcelPackage();

                    var Sheet = pkg.Workbook.Worksheets.Add("Summary");
                    DataTable table = null;
                    table = await dailyReportRepository.GetDailyReportDaKhoaSumTable();
                    await dailyReportRepository.CreateExcel(Sheet, table, false, true, true);
                    table = await dailyReportRepository.GetDailyReportBenhVienDaKhoaSumTable();
                    await dailyReportRepository.CreateExcel(Sheet, table, false, false, false, false, 0, 11);


                    Sheet = pkg.Workbook.Worksheets.Add("Tuần này");
                    table = await dailyReportRepository.GetReportDaKhoaTable();
                    await dailyReportRepository.CreateExcel(Sheet, table, false, true, true);

                    Sheet = pkg.Workbook.Worksheets.Add("Tuần trước");
                    table = await dailyReportRepository.GetDailyReportDaKhoaTable(TuanTruoc.TuNgay, TuanTruoc.DenNgay);
                    await dailyReportRepository.CreateExcel(Sheet, table, false);

                    Sheet = pkg.Workbook.Worksheets.Add("YTD");
                    table = await dailyReportRepository.GetDailyReportDaKhoaTable(new DateTime(tuan.DenNgay.Year, 1, 1), tuan.DenNgay);
                    await dailyReportRepository.CreateExcel(Sheet, table, false);

                    Sheet = pkg.Workbook.Worksheets.Add("LY");
                    table = await dailyReportRepository.GetDailyReportDaKhoaTable(new DateTime(tuan.DenNgay.Year - 1, 1, 1), tuan.DenNgay.AddYears(-1));
                    await dailyReportRepository.CreateExcel(Sheet, table, false);

                    FileInfo file = new FileInfo("GenaralHosReport.xlsx");
                    await pkg.SaveAsAsync(file);
                    pkg.Dispose();

                    //string BaoCaoTuanTruoc = (await dailyReportRepository.GetDailyReportMatString(TuanTruoc.TuNgay,TuanTruoc.DenNgay));
                    //string DailyNam = (await dailyReportRepository.GetDailyReportMatString(new DateTime(tuan.DenNgay.Year,1,1), tuan.DenNgay));
                    //string DailyNamTruoc = (await dailyReportRepository.GetDailyReportMatString(new DateTime(tuan.DenNgay.Year-1, 1, 1), tuan.DenNgay.AddYears(-1)));
                    //FileInfo fiTuan = new FileInfo(tuan.Tuan + "-" + tuan.Nam + "DailyTuan.xlsx");
                    //FileInfo fiTuanTruoc = new FileInfo(tuan.Tuan + "-" + tuan.Nam + "DailyTuantruoc.xlsx");
                    //FileInfo fiNam = new FileInfo(tuan.Tuan + "-" + tuan.Nam + "DailyNam.xlsx");
                    //FileInfo fiNamTruoc = new FileInfo(tuan.Tuan + "-" + tuan.Nam + "DailyNamTruoc.xlsx");

                    //await File.WriteAllTextAsync (fiTuan.FullName, BaoCao,System.Text.Encoding.UTF8);
                    //await File.WriteAllTextAsync(fiTuanTruoc.FullName, BaoCaoTuanTruoc, System.Text.Encoding.UTF8);
                    //await File.WriteAllTextAsync(fiNam.FullName, DailyNam, System.Text.Encoding.UTF8);
                    //await File.WriteAllTextAsync(fiNamTruoc.FullName, DailyNamTruoc, System.Text.Encoding.UTF8);
                    Workbook workbook = new();
                    workbook.LoadFromFile(file.FullName);
                    Worksheet sheet = workbook.Worksheets[0];
                    //Image[] imgs = workbook.SaveChartAsImage(sheet);
                    //"ChartGroupExamSur"
                    FileInfo fiImage = new(TuanTruoc.Tuan + "" + TuanTruoc.Nam + "GeneralEyeReports.png");
                    List<string> images = new();
                    sheet.Columns[10].ColumnWidth = 2;
                    sheet.Zoom = 100;
                    sheet.PageSetup.TopMargin = 0;
                    sheet.PageSetup.LeftMargin = 0;
                    sheet.PageSetup.BottomMargin = 0;
                    sheet.PageSetup.RightMargin = 0;
                    sheet.PageSetup.VResolution = 2400;
                    sheet.PageSetup.HResolution = 2400;
                    sheet.SaveToImage(fiImage.FullName, ImageFormat.Png);
                    sheet.Zoom = 100;
                    images.Add(fiImage.FullName);

                    //BaoCao = "<p>" + BaoCao + "</p>";
                    //request.Body += BaoCao;

                    //request.Attachments.Add(fiTuan.FullName);
                    //request.Attachments.Add(fiTuanTruoc.FullName);
                    //request.Attachments.Add(fiNam.FullName);
                    //request.Attachments.Add(fiNamTruoc.FullName);
                    request.Attachments.Add(file.FullName);
                    await SendEmailAsync(request, images, ThongTinGuiMail.SendTo, ThongTinGuiMail.CCTo);
                    await reportRepository.LuuThongTinGuiMailDailyTuan(LoaiDailyReportTuan.DaKhoa, tuan.Tuan, tuan.Nam, KetQuaGuiMail.ThanhCong);
                }
            }
            catch (Exception ex)
            {
                await reportRepository.LuuThongTinGuiMailDailyTuan(LoaiDailyReportTuan.Mat, tuan.Tuan, tuan.Nam, KetQuaGuiMail.Loi);
            }
        }

        public async Task SendMailDaiLyBenhVien(ISaleAutoReportRepo reportRepository, IDailyReportRepo dailyReportRepository,string MaBenhVien, bool resend = false)
        {
            
            try
            {
                if (!(await reportRepository.CheckDaGuiMailDailyBenhVien(MaBenhVien)) || resend)
                {
                    var ThongTinGuiMail = await reportRepository.LayThongTinGuiMailDailyBenhVien(MaBenhVien);
                    MailRequest request = new MailRequest();
                    request.IsBodyHtml = true;
                    request.Subject = ThongTinGuiMail.GuiTu + " - Báo cáo daily report ngày " + DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
                    request.Body = "Kính gửi anh/chị báo cáo daily ngày " + DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
                    string BaoCao = (await dailyReportRepository.GetDailyReportBenhVienString(MaBenhVien));
                    if (BaoCao=="")
                    {
                        LogHelp.Write("Send mail " + MaBenhVien + " lỗi: Không có số liệu");
                        return;
                    }
                    FileInfo fi = new FileInfo("DailyReport"+MaBenhVien+".xlsx");
                    await File.WriteAllTextAsync (fi.FullName, BaoCao, System.Text.Encoding.UTF8);                                        
                    BaoCao = "<p>" + BaoCao + "</p>";
                    request.Body += BaoCao;                    
                    //Save the document to file
                    request.Attachments.Add(fi.FullName);
                    await SendEmailAsync(request, null, ThongTinGuiMail.SendTo, ThongTinGuiMail.CCTo);
                    await reportRepository.LuuThongTinGuiMailDailyBenhVien(MaBenhVien);
                }
            }
            catch (Exception ex)
            {
                LogHelp.Write("Send mail " + MaBenhVien +" lỗi:" + ex.Message);
            }
        }
       
        #endregion
    }
}

