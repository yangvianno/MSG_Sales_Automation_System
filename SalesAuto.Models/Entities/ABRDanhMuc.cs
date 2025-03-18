using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class ABRDanhMuc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int STT { get; set; }
        public string Code { get; set; }
        public int? MaNhomABR { get; set; }
        public string TenCongViec { get; set; }
        public int MucHuongVND { get; set; }
        public decimal MucHuongPhanTram { get; set; }
        public bool TinhTheoPoolThucHien { get; set; }
        public int LoaiHanhDong { get; set; }
        public bool TinhTheoBenhAn { get; set; }
        public bool ChuongTrinhRieng { get; set; }
        public decimal? TyLeGianTiep { get; set; }
        public bool? TinhTheoDoanhThu { get; set; }
        public decimal? HuongToiDa { get; set; }
        public List<ABRHuongBacThang> HuongBacThangs { get; set; }
    }
    public class ABRDanhMucXetDuyet:ABRDanhMuc
    {
        public string MaBenhVien { get; set; }
        public Guid IDXetDuyet { get; set; }
        public string? GhiChu { get; set; }
        public TinhTrangXetDuyet TinhTrang { get; set; } // 0 chưa xét duyệt 1 đã xét duyệt 2 từ chối
        public TrangThai Loai { get; set; } // 0 giá trị cũ 1 giá trị mới
        public DateTime NgayTao { get; set; }
        public DateTime? NgayXetDuyet { get; set; }
        public int? IDDanhMuc { get; set; }
        public string? TenBenhVien { get; set; }
    }

    public enum TrangThai
    {
        Cu,
        Moi
    }
}
