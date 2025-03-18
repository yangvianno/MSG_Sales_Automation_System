using Microsoft.AspNetCore.Mvc;
using SalesAuto.Api.Repositories;
using SalesAuto.Models.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTieuController : ControllerBase
    {
        private readonly IChiTieuSoLuongRepo chiTieuSoLuongRepository;

        public ChiTieuController(IChiTieuSoLuongRepo chiTieuSoLuongRepository
                              )
        {
            this.chiTieuSoLuongRepository = chiTieuSoLuongRepository;
        }

        [HttpGet]
        [Route("LoaiChiTieu")]
        public async Task<IActionResult> GetLoaiChiTieu()
        {
            var result = await chiTieuSoLuongRepository.GetAllLoaiThiTieu();
            return Ok(result);
        }

        [HttpGet]
        [Route("ChiTieuSoLuong")]
        public async Task<IActionResult> ChiTieuSoLuong(int Nam, int MaLoaiChiTieu, string MaBenhVien="")
        {
            var MaBenhVienNguon = LayChiMaBenhVien(MaBenhVien);
            var result = await chiTieuSoLuongRepository.GetChiTieuSoLuong(MaLoaiChiTieu, Nam, MaBenhVienNguon);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Save(ChiTieuSoLuong chiTieuSoLuong, string MaBenhVien = "")
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
            chiTieuSoLuong.MaBenhVien = MaBenhVienNguon;
            var result = await chiTieuSoLuongRepository.Save(chiTieuSoLuong);
            return Ok(result);
        }


        private string LayChiMaBenhVien(string MaBenhVien)
        {
            var MaBenhVienNguon = "O";
            if (MaBenhVien == "")
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
            return MaBenhVienNguon;
        }
    }
}
