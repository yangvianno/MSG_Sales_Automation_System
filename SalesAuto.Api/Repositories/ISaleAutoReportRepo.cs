using OfficeOpenXml;
using SalesAuto.Models;
using SalesAuto.Models.Entities;
using SalesAuto.Models.SearchModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public interface ISaleAutoReportRepo
    {
        public Task<ExcelPackage> createBenhNhanReport(BenhNhanSM benhNhanSM, string MaBenhVien="O");
        public Task<ExcelPackage> createCPAReport(int nam, int thang=0, string MaBenhVien = "O");
        public Task<ExcelPackage> createLeadFollowReport(int nam, int thang = 0, string MaBenhVien = "O");
        public Task<ExcelPackage> createKPIReport(int thang, int nam, DateTime Ngay, bool GuiTheoNgay = false);
        public Task<ExcelPackage> createLeadsChannelReport(int thang, int nam, bool GuiTheoTuan=false, string MaBenhVien="O");
        public Task<ExcelPackage> TaoBaoCaoTuan(int tuan, int nam);
        public Task<ExcelPackage> TaoBaoCaoThang(int thang, int nam, DateTime Ngay, bool TinhTheoNgay=false);
        public Task<ExcelPackage> createFollowupPatientsReport(int tuan, int nam, string MaBenhVien, int Thang = 0);
        public Task<string> TaoDuLieuCPA(int nam, int thang, bool TheoNgay = false);
        public Task<string> TaoDuLieuLeadFollow(int nam, int thang, bool TheoNgay = false);        
        public Task<string> LuuThangGuiMail(int nam, int thang);
        public Task<bool> CheckDaGuiMailTuan(int tuanBC, int namBC);
        public Task<bool> CheckDaGuiMailFollowupPatientTuanBenhVien(int tuanBC, int namBC, string MaBenhVien);
        public Task LuuThongTinGuiMailTuan(int tuanBC, int namBC, KetQuaGuiMail thanhCong);
        public Task LuuThongTinGuiMailFollowupPatientTuanBenhVien(int tuanBC, int namBC, KetQuaGuiMail thanhCong, string MaBenhVien);
        public Task<bool> CheckDaGuiMailThang(int thangBC, int namBC);
        public Task<bool> CheckDaGuiMailCPAAndCall(int thangBC, int namBC, string MaBenhVien = "O");
        public Task<bool> CheckDaGuiMailThangTheoTuan(int tuanBC, int namBC);
        public Task<bool> CheckDaGuiMailCPAAndCallTheoTuan(int tuanBC, int namBC, string MaBenhVien = "O");
        public Task LuuThongTinGuiMailThangTheoTuan(int tuanBC, int namBC, KetQuaGuiMail thanhCong);
        public Task LuuThongTinGuiMailThang(int thangBC, int namBC, KetQuaGuiMail thanhCong);
        public Task LuuThongTinGuiMailCPATheoTuan(int tuanBC, int namBC, KetQuaGuiMail thanhCong, string MaBenhVien = "O");
        public Task LuuThongTinGuiMailCPA(int thangBC, int namBC, KetQuaGuiMail thanhCong, string MaBenhVien = "O");        
        public Task<List<ThongTinGuiMailBenhVien>> LayThongTinGuiMailWeeklyBenhVien(string MaBenhVien="");
        public Task<List<ThongTinGuiMailBenhVien>> LayThongTinGuiMailCPAAndCallBenhVien(string MaBenhVien);
        public Task<ThongTinGuiMailBenhVien> LayThongTinGuiMailDailyTuan(string MaBenhVien ="O", LoaiDailyReportTuan LoaiReport = LoaiDailyReportTuan.Mat);
        public Task<ThongTinGuiMailBenhVien> LayThongTinGuiMailDailyBenhVien(string MaBenhVien);
        public Task LuuThongTinGuiMailDailyTuan(LoaiDailyReportTuan loaiDailyReportTuan, int tuan, int nam, KetQuaGuiMail loi);
        public Task LuuThongTinGuiMailDailyBenhVien(string MaBenhVien);
        public Task<bool> CheckDaGuiThongTinGuiMailDailyTuan(LoaiDailyReportTuan loaiDailyReportTuan , int tuan, int nam);
        public Task<bool> CheckDaGuiMailDailyBenhVien(string MaBenhVien);
        Task<DataTable> GetBookingTable(DateTime tuNgay, DateTime denNgay, string maBenhVien);
        Task<string> GetTenVietTatBenhVien(string MaBenhVien);
        Task<ThongTinGuiMailBenhVien> LayThongTinGuiMailWeekly(string MaBenhVien = "O");
        Task<ThongTinGuiMailBenhVien> LayThongTinGuiMailMonthly(string MaBenhVien = "O");
        Task LuuThongTinGuiMailFollowupPatientThangBenhVien(int Thang, int namBC, KetQuaGuiMail thanhCong, string MaBenhVien);
        Task<bool> CheckDaGuiMailFollowupPatientThangBenhVien(int thangBC, int namBC, string MaBenhVien);
        Task<ExcelPackage> GetChiTietLeadsChuaBook(DateTime TuNngay, DateTime DenNgay, string maBenhVien = "O");
        public Task<bool> CheckDaGuiMailChiTietLeadsChuaBook(int tuanBC, int namBC);
        Task<ThongTinGuiMailBenhVien> LayThongTinGuiMailChiTietLeadsChuaBook(string MaBenhVien = "O");
        Task LuuThongTinGuiMailChiTietLeadsChuaBook(int Tuan, int Nam, KetQuaGuiMail thanhCong, string MaBenhVien = "O");
        Task<ExcelPackage> createFollowupPatientsReportGroup(int tuan, int nam, int Thang = 0);
        Task<IEnumerable<IDictionary<string, object>>> GetDanhSachLead(DateTime TuNgay, DateTime DenNgay);
        Task<IEnumerable<IDictionary<string, object>>> GetDanhSachBook(DateTime TuNgay, DateTime DenNgay);
        Task<IEnumerable<IDictionary<string, object>>> GetDanhSachKham(DateTime TuNgay, DateTime DenNgay);
        Task<IEnumerable<IDictionary<string, object>>> GetDanhSachPhauThuat(DateTime TuNgay, DateTime DenNgay);
        Task<IEnumerable<IDictionary<string, object>>> GetQuaTrinhKham(DateTime TuNgay, DateTime DenNgay);
        Task<IEnumerable<IDictionary<string, object>>> GetQuaTrinhPhauThuat(DateTime TuNgay, DateTime DenNgay);
    }
}
