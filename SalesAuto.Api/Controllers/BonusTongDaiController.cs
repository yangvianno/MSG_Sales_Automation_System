using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesAuto.Api.Repositories;
using System.Threading.Tasks;

namespace SalesAuto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BonusTongDaiController : ControllerBase
    {
        private readonly IBonusTongDaiRepo bonusTongDaiRep;
        private const string _contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public BonusTongDaiController(IBonusTongDaiRepo bonusTongDaiRep)
        {
            this.bonusTongDaiRep = bonusTongDaiRep;
        }

        [HttpPost]
        [Route("GetBonusExcel")]
        public async Task<IActionResult> GetBonusExcel(int Thang, int Nam, string MaBenhVien = "")
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
            byte[] bytes;
            var pkg = await bonusTongDaiRep.createBonusExcel(Thang,Nam, MaBenhVienNguon);            
            bytes = pkg.GetAsByteArray();
            pkg.Dispose();
            return File(bytes, _contentType, Thang + "" + Nam + "BonusTongDai.xlsx");
            
        }
        [HttpPost]
        [Route("GetBonusExcelChiTiet")]
        public async Task<IActionResult> GetBonusExcelChiTiet(int Thang, int Nam, string MaBenhVien = "")
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
            byte[] bytes;
            var pkg = await bonusTongDaiRep.createBonusExcelChiTiet(Thang, Nam, MaBenhVienNguon);
            bytes = pkg.GetAsByteArray();
            pkg.Dispose();
            return File(bytes, _contentType, Thang + "" + Nam + "BonusTongDai.xlsx");

        }


    }
}
