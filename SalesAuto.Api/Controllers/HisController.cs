using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesAuto.Api.Repositories;
using SalesAuto.Models.Entities.HisDoiTuong;
using System;
using System.Threading.Tasks;

namespace SalesAuto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HisController : ControllerBase
    {
        private readonly IHis_DoiTuongRepo his_DoiTuong;

        public HisController(IHis_DoiTuongRepo his_DoiTuong)
        {
            this.his_DoiTuong = his_DoiTuong;
        }
        [HttpGet]
        [Route("GetDanhSachDoiTuong")]
        public async Task<IActionResult> GetDanhSachDoiTuong(string MaBenhVien = "")
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
                    return BadRequest("Vui lòng chọn Bệnh viện!");
                }
            }
            var result = await his_DoiTuong.GetDanhSachDoiTuong(MaBenhVienNguon);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetBangGiaTheoDoiTuong")]       
        public async Task<IActionResult> GetBangGiaTheoDoiTuong(Guid ID_LoaiDoiTuong, string MaBenhVien = "")
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
                    return BadRequest("Vui lòng chọn Bệnh viện!");
                }
            }
            var result = await his_DoiTuong.GetBangGiaTheoDoiTuong(MaBenhVienNguon, ID_LoaiDoiTuong);
            return Ok(result);
        }

        [HttpPost]
        [Route("SaveBangGiaTheoDoiTuong")]
        public async Task<IActionResult> SaveBangGiaTheoDoiTuon(BangGiaTheoDoiTuong item, string MaBenhVien = "")
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
                    return BadRequest("Vui lòng chọn Bệnh viện!");
                }
            }
            var result = await his_DoiTuong.SaveBangGiaTheoDoiTuong(MaBenhVienNguon, item);
            return Ok(result);
        }

        [HttpPost]
        [Route("DeleteBangGiaTheoDoiTuong")]
        public async Task<IActionResult> DeleteBangGiaTheoDoiTuon(BangGiaTheoDoiTuong item, string MaBenhVien = "")
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
                    return BadRequest("Vui lòng chọn Bệnh viện!");
                }
            }
            var result = await his_DoiTuong.DeleteBangGiaTheoDoiTuong(MaBenhVienNguon, item);
            return Ok(result);
        }
    }
}
