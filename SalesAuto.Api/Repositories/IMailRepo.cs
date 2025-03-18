using SalesAuto.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public interface IMailRepo
    {
        Task SendEmailAsync(MailRequest mailRequest, List<string> ImagePaths = null, string SendTo = "", string CCto="");
        public Task SendMailBaoCaoThang(ISaleAutoReportRepo reportRepository, bool resend = false, bool TinhToiNgay = false);
        public Task SendMailBaoCaoTuan(ISaleAutoReportRepo reportRepository, bool resend=false, string Tomail="", int Tuan = 0, int Nam = 0);
        public Task SendMailBaoCaoFollowupPatientTuanBenhVien(ISaleAutoReportRepo reportRepository, bool resend = false);
        public Task SendMailBaoCaoCPAAndCallBenhVien(ISaleAutoReportRepo reportRepository,string MaBenhVien, bool TinhToiNgay = false, bool resend = false);
        public Task SendMailCPAAndCall(ISaleAutoReportRepo reportRepository, bool resend = false, bool TinhToiNgay = false, string MaBenhVien = "O");
        public Task SendMailCPAAndCallBV(ISaleAutoReportRepo reportRepository, bool resend = false, bool GuiTheoTuan = false, string MaBenhVien = "O", string SendTo = "", string CCTo = "");
        public Task SendMailDaiLyTuanMat(ISaleAutoReportRepo reportRepository, IDailyReportRepo dailyReportRepository, bool resend = false);
        public Task SendMailDaiLyTuanDaKhoa(ISaleAutoReportRepo reportRepository, IDailyReportRepo dailyReportRepository, bool resend = false);
        public Task SendMailDaiLyBenhVien(ISaleAutoReportRepo reportRepository, IDailyReportRepo dailyReportRepository, string MaBenhVien, bool resend = false);
        Task SendMailBaoCaoFollowupPatientThangBenhVien(ISaleAutoReportRepo reportRepository, bool resend = false);
        Task SendMailChiTietLeadsChuaBook(ISaleAutoReportRepo reportRepository, bool resend = false);
        Task SendMailBaoCaoFollowupPatientTuanGroup(ISaleAutoReportRepo reportRepository, bool resend = false, string SendTo="");
        Task SendMailBaoCaoFollowupPatientThangGroup(ISaleAutoReportRepo reportRepository, bool resend = false, string SendTo = "");
    }
}