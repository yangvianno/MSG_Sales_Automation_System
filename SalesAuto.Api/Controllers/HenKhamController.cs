using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesAuto.Api.Repositories;
using SalesAuto.Models.Entities.HenKham;
using SalesAuto.Models.ViewModel.HenKham;
using System;
using System.Threading.Tasks;

namespace SalesAuto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HenKhamController : ControllerBase
    {
        private readonly IHenKhamRepo henKhamRepo;

        public HenKhamController(IHenKhamRepo henKhamRepo)
        {
            this.henKhamRepo = henKhamRepo;
        }

        [HttpGet]
        [Route("GetDanhSachHenKham")]
        public async Task<IActionResult> GetDanhSachHenKham(DateTime TuNgay, DateTime DenNgay, bool LayTheoNgayHen=true, bool BacSyHen=true, bool HoSoLasik = true, bool BenhChuaHen = true, string MaBenhVien="")
        {
            try
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
                var result = await henKhamRepo.GetDanhSachHenKham(MaBenhVienNguon, TuNgay, DenNgay, LayTheoNgayHen, BacSyHen, HoSoLasik, BenhChuaHen);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetHenKham")]
        public async Task<IActionResult> GetHenKham(Guid ID, string MaBenhVien = "")
        {
            try
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
                var result = await henKhamRepo.GetHenKham(MaBenhVienNguon, ID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetHenKhamThucHienCuoi")]
        public async Task<IActionResult> GetHenKhamThucHienCuoi(string MaBenhVien = "")
        {
            try
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
                var result = await henKhamRepo.GetHenKhamThucHienCuoi(MaBenhVienNguon);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        

        [HttpGet]
        [Route("GetDanhSachCRMOrder_status")]
        public async Task<IActionResult> GetDanhSachCRMOrder_status(string MaBenhVien = "")
        {
            try
            {
                var result = await henKhamRepo.GetDanhSachCRMOrder_status();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDanhSachCRMProduct")]
        public async Task<IActionResult> GetDanhSachCRMProduct(string MaBenhVien = "")
        {
            try
            {
                var result = await henKhamRepo.GetDanhSachCRMProduct();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetDanhSachBenhChuyenKhoa")]
        public async Task<IActionResult> GetDanhSachBenhChuyenKhoa(string MaBenhVien = "")
        {
            try
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
                var result = await henKhamRepo.GetDanhSachBenhChuyenKhoa(MaBenhVienNguon);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SaveHenKham")]
        public async Task<IActionResult> SaveHenKham(BenhNhanHenKham benhNhanHenKham, string MaBenhVien = "")
        {
            try
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
                var result = await henKhamRepo.AddHenKhamFromHis(MaBenhVienNguon, benhNhanHenKham);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("PushHenKhamToCRM")]
        public async Task<IActionResult> PushHenKhamToCRM(Guid ID, string MaBenhVien = "")
        {
            try
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
                var result = await henKhamRepo.PushHenKhamToCRM(MaBenhVienNguon,ID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateTinhTrangHenKhamToCRM")]
        public async Task<IActionResult> UpdateTinhTrangHenKhamToCRM(HKLayDanhSachCapNhatTinhTrang hKLayDanhSachCapNhatTinhTrang , string MaBenhVien = "")
        {
            try
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
                var result = await henKhamRepo.UpdateTinhTrangHenKhamToCRM(MaBenhVienNguon, hKLayDanhSachCapNhatTinhTrang);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDanhSachCapNhatTinhTrang")]
        public async Task<IActionResult> GetDanhSachCapNhatTinhTrang(DateTime TuNgay, DateTime DenNgay, string MaBenhVien = "")
        {
            try
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
                var result = await henKhamRepo.GetDanhSachCapNhatTinhTrang(MaBenhVienNguon, TuNgay, DenNgay);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region MauCapNhatTinhTrang
        [HttpGet]
        [Route("GetDanhSachMauCapNhatTinhTrang")]
        public async Task<IActionResult> GetDanhSachMauCapNhatTinhTrang(string MaBenhVien = "")
        {
            try
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
                var result = await henKhamRepo.GetDanhSachMauCapNhatTinhTrang(MaBenhVienNguon);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SaveMauCapNhatTinhTrang")]
        public async Task<IActionResult> SaveMauCapNhatTinhTrang(HKMauCapNhatTinhTrang mau,string MaBenhVien = "")
        {
            try
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
                var result = await henKhamRepo.SaveMauCapNhatTinhTrang(MaBenhVienNguon, mau);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("DeleteMauCapNhatTinhTrang")]
        public async Task<IActionResult> DeleteMauCapNhatTinhTrang(Guid IDmau, string MaBenhVien = "")
        {
            try
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
                var result = await henKhamRepo.DeleteMauCapNhatTinhTrang(MaBenhVienNguon, IDmau);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion //MauCapNhatTinhTrang

        #region MauHenKhamTheoToa
        [HttpGet]
        [Route("GetDanhSachMauHenKhamTheoToa")]
        public async Task<IActionResult> GetDanhSachMauHenKhamTheoToa(string MaBenhVien = "")
        {
            try
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
                var result = await henKhamRepo.GetDanhSachMauHenKhamTheoToa(MaBenhVienNguon);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SaveMauHenKhamTheoToa")]
        public async Task<IActionResult> SaveMauHenKhamTheoToa(HKMauHenKhamTheoToa mau, string MaBenhVien = "")
        {
            try
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
                var result = await henKhamRepo.SaveMauHenKhamTheoToa(MaBenhVienNguon, mau);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("DeleteMauHenKhamTheoToa")]
        public async Task<IActionResult> DeleteMauHenKhamTheoToa(Guid IDmau, string MaBenhVien = "")
        {
            try
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
                var result = await henKhamRepo.DeleteMauHenKhamTheoToa(MaBenhVienNguon, IDmau);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion //MauCapNhatTinhTrang

        #region Bệnh đến khám

        [HttpPost]
        [Route("PushBenhDenKhamToCRM")]
        public async Task<IActionResult> PushBenhDenKhamToCRM(string MaBenhVien = "")
        {
            try
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
                var result = await henKhamRepo.PushBenhDenKhamToCRM(MaBenhVienNguon);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion // Bệnh đến khám
    }


}
