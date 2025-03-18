using DataAccessLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesAuto.Api.Repositories;
using SalesAuto.Models;
using SalesAuto.Models.Entities;
using SalesAuto.Models.SearchModel;
using SalesAuto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ABRController : ControllerBase
    {
        private readonly IABRRepo ABRRepository;
        private const string _contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ABRController(IABRRepo ABRRepository)
        {
            this.ABRRepository = ABRRepository;
        }

        [HttpGet]
        [Route("GetDanhMucABR")]
        public async Task<IActionResult> GetDanhMucABR(string MaBenhVien="")
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
            var result = await ABRRepository.LayDanhMucABR(MaBenhVienNguon);
            return Ok(result);
        }

        #region Xét duyệt Chính sách ABR
        [HttpGet]
        [Route("GetDanhMucXetDuyet")]
        public async Task<IActionResult> GetDanhMucXetDuyet(string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.ABRGetDanhMucXetDuyet(MaBenhVienNguon);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        
        [HttpGet]
        [Route("GetDanhMucXetDuyetMaster")]
        public async Task<IActionResult> GetDanhMucXetDuyetMaster()
        {
            
            try
            {
                var result = await ABRRepository.ABRGetDanhMucXetDuyetMaster();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("SaveTinhTrangXetDuyetMaster")]
        public async Task<IActionResult> SaveTinhTrangXetDuyetMaster(ABRDanhMucXetDuyet aBRDanhMucXetDuyet)
        {

            try
            {
                var result = await ABRRepository.SaveTinhTrangXetDuyetMaster(aBRDanhMucXetDuyet);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        #endregion
        [HttpGet]
        [Route("GetNhomCongViecThongKe")]
        public async Task<IActionResult> GetNhomCongViecThongKe(string MaBenhVien = "")
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
            var result = await ABRRepository.GetNhomCongViecThongKe(MaBenhVienNguon);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetNhomABR")]
        public async Task<IActionResult> GetNhomABR(string MaBenhVien = "")
        {   
            var result = await ABRRepository.LayNhomABR();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetDanhMucCongViecHis")]
        public async Task<IActionResult> GetDanhMucCongViecHis(string MaBenhVien = "")
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
            var result = await ABRRepository.LayDanhMucCongViecHis(MaBenhVienNguon);
            return Ok(result);
        }


        [HttpPost]
        [Route("SaveDanhMucABR")]
        public async Task<IActionResult> SaveDanhMucABR(ABRDanhMuc aBRDanhMuc, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.SaveDanhMucABR(MaBenhVienNguon, aBRDanhMuc);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(false);
        }
        [HttpPost]
        [Route("DeleteDanhMucABR")]
        public async Task<IActionResult> DeleteDanhMucABR(int iD, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.DeleteDanhMucABR(MaBenhVienNguon, iD);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(false);
        }

        #region MasterData
        [HttpPost]
        [Route("SaveDanhMucABRMasterData")]
        public async Task<IActionResult> SaveDanhMucABRMasterData(ABRDanhMuc aBRDanhMuc)
        {
            
            try
            {
                var result = await ABRRepository.SaveDanhMucABRMasterData(aBRDanhMuc);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(false);
        }
        [HttpPost]
        [Route("DeleteDanhMucABRMasterData")]
        public async Task<IActionResult> DeleteDanhMucABRMasterData(int iD)
        {
            
            try
            {
                var result = await ABRRepository.DeleteDanhMucABRMasterData(iD);
                return Ok(result);
            }
            catch
            {

            }
            return Ok(false);
        }
        [HttpGet]
        [Route("GetDanhMucABRMasterData")]
        public async Task<IActionResult> GetDanhMucABRMasterData()
        {            
            var result = await ABRRepository.GetDanhMucABRMasterData();
            return Ok(result);
        }
        #endregion

        #region MapCongViecABRHIS
        [HttpGet]
        [Route("getDanhSachMapCongViecABRHIS")]
        public async Task<IActionResult> getDanhSachMapCongViecABRHIS(string MaBenhVien = "")
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
            var result = await ABRRepository.LayDanhSachMapCongViecABRHIS(MaBenhVienNguon);
            return Ok(result);
        }


        [HttpPost]
        [Route("saveMapCongViecABRHIS")]
        public async Task<IActionResult> saveMapCongViecABRHIS(ABRMapCongViecABRHIS item, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.SaveMapCongViecABRHIS(MaBenhVienNguon, item);
                return Ok(result);
            }
            catch
            {

            }
            return Ok(false);
        }
        [HttpPost]
        [Route("deleteMapCongViecABRHIS")]
        public async Task<IActionResult> DeleteMapCongViecABRHIS(int iD, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.DeleteMapCongViecABRHIS(MaBenhVienNguon, iD);
                return Ok(result);
            }
            catch
            {

            }
            return Ok(false);
        }
        #endregion

        #region ABR Nhân vien

        [HttpGet]
        [Route("GetDanhSachABRNhanVien")]
        public async Task<IActionResult> LayDanhSachABRNhanVien(string maBenhVien="")
        {
            var MaBenhVienNguon = "O";
            if (maBenhVien != "")
            {
                MaBenhVienNguon = maBenhVien;
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
            var result = await ABRRepository.LayDanhSachABRNhanVien(MaBenhVienNguon);
            return Ok(result);
        }

        [HttpPost]
        [Route("SaveABRNhanVien")]
        public async Task<IActionResult> SaveABRNhanVien(ABRNhanVien item, string maBenhVien="")
        {
            var MaBenhVienNguon = "O";
            if (maBenhVien != "")
            {
                MaBenhVienNguon = maBenhVien;
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
            try
            {
                var result = await ABRRepository.SaveABRNhanVien(MaBenhVienNguon, item);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpPost]
        [Route("AddNhanVienHuongPool")]
        public async Task<IActionResult> AddNhanVienHuongPool(Guid IDABRNhanVien, Guid IDABRPool, string maBenhVien = "")
        {
            var MaBenhVienNguon = "O";
            if (maBenhVien != "")
            {
                MaBenhVienNguon = maBenhVien;
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
            var result = await ABRRepository.AddNhanVienHuongPool(MaBenhVienNguon,IDABRNhanVien, IDABRPool );
            return Ok(result);
        }

        [HttpPost]
        [Route("DeleteNhanVienHuongPool")]
        public async Task<IActionResult> DeleteNhanVienHuongPool(Guid IDABRNhanVien, Guid IDABRPool, string maBenhVien = "")
        {
            var MaBenhVienNguon = "O";
            if (maBenhVien != "")
            {
                MaBenhVienNguon = maBenhVien;
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
            var result = await ABRRepository.DeleteNhanVienHuongPool(MaBenhVienNguon, IDABRNhanVien, IDABRPool);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetNhanVienHuongPool")]
        public async Task<IActionResult> GetNhanVienHuongPool(Guid IDABRNhanVien, string maBenhVien = "")
        {
            var MaBenhVienNguon = "O";
            if (maBenhVien != "")
            {
                MaBenhVienNguon = maBenhVien;
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
            var result = await ABRRepository.GetNhanVienHuongPool(MaBenhVienNguon, IDABRNhanVien);
            return Ok(result);
        }

        [HttpPost]
        [Route("AddNhanVienABRLuonDuocHuong")]
        public async Task<IActionResult> AddNhanVienABRLuonDuocHuong(Guid IDABRNhanVien, int IDABRDanhMuc, string maBenhVien = "")
        {
            var MaBenhVienNguon = "O";
            if (maBenhVien != "")
            {
                MaBenhVienNguon = maBenhVien;
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
            var result = await ABRRepository.AddNhanVienABRLuonDuocHuong(MaBenhVienNguon, IDABRNhanVien, IDABRDanhMuc);
            return Ok(result);
        }

        [HttpPost]
        [Route("DeleteNhanVienABRLuonDuocHuong")]
        public async Task<IActionResult> DeleteNhanVienABRLuonDuocHuong(Guid IDABRNhanVien, int IDABRDanhMuc, string maBenhVien = "")
        {
            var MaBenhVienNguon = "O";
            if (maBenhVien != "")
            {
                MaBenhVienNguon = maBenhVien;
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
            var result = await ABRRepository.DeleteNhanVienABRLuonDuocHuong(MaBenhVienNguon, IDABRNhanVien, IDABRDanhMuc);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetNhanVienABRLuonDuocHuong")]
        public async Task<IActionResult> GetNhanVienABRLuonDuocHuong(Guid IDABRNhanVien, string maBenhVien = "")
        {
            var MaBenhVienNguon = "O";
            if (maBenhVien != "")
            {
                MaBenhVienNguon = maBenhVien;
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
            var result = await ABRRepository.GetNhanVienABRLuonDuocHuong(MaBenhVienNguon, IDABRNhanVien);
            return Ok(result);
        }
        #region HuongBacThang

        [HttpPost]
        [Route("SaveHuongBacThang")]
        public async Task<IActionResult> SaveHuongBacThang(ABRHuongBacThang aBRHuongBacThang, string maBenhVien = "")
        {
            try
            {
                var MaBenhVienNguon = "O";
                if (maBenhVien != "")
                {
                    MaBenhVienNguon = maBenhVien;
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
                var result = await ABRRepository.SaveHuongBacThang(MaBenhVienNguon, aBRHuongBacThang);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("DeleteHuongBacThang")]
        public async Task<IActionResult> DeleteHuongBacThang(Guid ID, string maBenhVien = "")
        {
            try
            {
                var MaBenhVienNguon = "O";
                if (maBenhVien != "")
                {
                    MaBenhVienNguon = maBenhVien;
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
                var result = await ABRRepository.DeleteHuongBacThang(MaBenhVienNguon, ID);
                return Ok(result);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDanhSachHuongBacThang")]
        public async Task<IActionResult> GetDanhSachHuongBacThang(int IDABRDanhMuc, string maBenhVien = "")
        {
            try
            {
                var MaBenhVienNguon = "O";
                if (maBenhVien != "")
                {
                    MaBenhVienNguon = maBenhVien;
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
                var result = await ABRRepository.GetDanhSachHuongBacThang(MaBenhVienNguon, IDABRDanhMuc);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion //HuongBacThang
        [HttpPost]
        [Route("DeleteABRNhanVien")]
        public async Task<IActionResult> DeleteABRNhanVien(Guid iD, string maBenhVien="")
        {
            var MaBenhVienNguon = "O";
            if (maBenhVien != "")
            {
                MaBenhVienNguon = maBenhVien;
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
            var result = await ABRRepository.DeleteABRNhanVien(MaBenhVienNguon, iD);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetDanhSachMapNhanVienABRHIS")]
        public async Task<IActionResult> LayDanhSachMapNhanVienABRHIS(string maBenhVien="")
        {
            var MaBenhVienNguon = "O";
            if (maBenhVien != "")
            {
                MaBenhVienNguon = maBenhVien;
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
            var result = await ABRRepository.LayDanhSachMapNhanVienABRHIS(MaBenhVienNguon);
            return Ok(result);
        }
        [HttpPost]
        [Route("SaveMapNhanVienABRHIS")]
        public async Task<IActionResult> SaveMapNhanVienABRHIS(ABRMapNhanVienABRHIS item, string maBenhVien="")
        {
            var MaBenhVienNguon = "O";
            if (maBenhVien != "")
            {
                MaBenhVienNguon = maBenhVien;
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
            var result = await ABRRepository.SaveMapNhanVienABRHIS(MaBenhVienNguon, item);
            return Ok(result);
        }

        [HttpPost]
        [Route("DeleteMapNhanVienABRHIS")]
        public async Task<IActionResult> DeleteMapNhanVienABRHIS(Guid iD, string maBenhVien="")
        {
            var MaBenhVienNguon = "O";
            if (maBenhVien != "")
            {
                MaBenhVienNguon = maBenhVien;
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
            var result = await ABRRepository.DeleteMapNhanVienABRHIS(MaBenhVienNguon, iD);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetDanhSachNhanVienHIS")]
        public async Task<IActionResult> LayDanhSachNhanVienHIS(string maBenhVien="")
        {
            var MaBenhVienNguon = "O";
            if (maBenhVien != "")
            {
                MaBenhVienNguon = maBenhVien;
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
            var result = await ABRRepository.LayDanhSachNhanVienHIS(MaBenhVienNguon);
            return Ok(result);
        }
        #endregion

        #region Nhân viên thực hiện
        [HttpPost]
        [Route("getNhanVienThucHien")]
        public async Task<IActionResult> getNhanVienThucHien(NhanVienThucHienSM nhanVienThucHienSM, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.getNhanVienThucHien(MaBenhVienNguon, nhanVienThucHienSM);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("SaveNhanVienThucHien")]
        public async Task<IActionResult> SaveNhanVienThucHien(ABRNhanVienThucHien aBRNhanVienThucHien, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.SaveNhanVienThucHien(MaBenhVienNguon, aBRNhanVienThucHien);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("DeleteNhanVienThucHien")]
        public async Task<IActionResult> DeleteNhanVienThucHien(ABRNhanVienThucHien aBRNhanVienThucHien, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.DeleteNhanVienThucHien(MaBenhVienNguon, aBRNhanVienThucHien);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        #region Báo cáo
        [HttpPost]
        [Route("GetBaoCaoChiTietDaThucHien")]
        public async Task<IActionResult> GetBaoCaoChiTietDaThucHien(NhanVienThucHienSM nhanVienThucHienSM, string MaBenhVien="")
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
            try
            {
                var result = await ABRRepository.GetBaoCaoChiTietDaThucHien(MaBenhVienNguon, nhanVienThucHienSM);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetBaoCaoDaThucHien")]
        public async Task<IActionResult> GetBaoCaoDaThucHien(NhanVienThucHienSM nhanVienThucHienSM, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.GetBaoCaoDaThucHien(MaBenhVienNguon, nhanVienThucHienSM);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost]
        [Route("DownLoadABRExcel")]
        public async Task<IActionResult> DownLoadABRExcel(int thang,int Nam, string MaBenhVien = "")
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
            var pkg = await ABRRepository.createABRExcel(thang, Nam, MaBenhVienNguon);
            bytes = pkg.GetAsByteArray();
            pkg.Dispose();
            return File(bytes, _contentType, thang + "" + Nam + "ABRReport.xlsx");
        }        

        [HttpPost]
        [Route("GetABRDieuChinh")]
        public async Task<IActionResult> GetABRDieuChinh(int thang, int Nam, string MaBenhVien = "")
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
            var result = await ABRRepository.GetABRDieuChinh(thang, Nam, MaBenhVienNguon);

            return Ok(result);
        }

        [HttpPost]
        [Route("SaveABRDieuChinh")]
        public async Task<IActionResult> SaveABRDieuChinh(ABRDieuChinh item , string MaBenhVien = "")
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
            var result = await ABRRepository.SaveABRDieuChinh(item, MaBenhVienNguon);

            return Ok(result);
        }
        [HttpPost]
        [Route("GetABRSoSanhChiTiet")]
        public async Task<IActionResult> GetABRSoSanhChiTiet(int thang, int Nam, string MaBenhVienChiNhanh = "", string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.GetABRSoSanhChiTiet(thang, Nam, MaBenhVienChiNhanh, MaBenhVienNguon);
                return Ok(result);
            }
            catch  (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost]
        [Route("GetABRSoSanhTongHop")]
        public async Task<IActionResult> GetABRSoSanhTongHop(int thang, int Nam, string MaBenhVienChiNhanh = "", string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.GetABRSoSanhTongHop(thang, Nam, MaBenhVienChiNhanh, MaBenhVienNguon);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("GetABRDoanhThuThangTongHop")]
        public async Task<IActionResult> GetABRDoanhThuThangTongHop(int thang, int Nam, string MaBenhVienChiNhanh = "", string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.GetABRDoanhThuThangTongHop(thang, Nam, MaBenhVienChiNhanh, MaBenhVienNguon);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetABRDieuChinhThangTongHop")]
        public async Task<IActionResult> GetABRDieuChinhThangTongHop(int thang, int Nam, string MaBenhVienChiNhanh = "", string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.GetABRDieuChinhThangTongHop(thang, Nam, MaBenhVienChiNhanh, MaBenhVienNguon);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("GetBangDanhGiaNhanVien")]
        public async Task<IActionResult> GetBangDanhGiaNhanVien(int thang, int nam, string MaBenhVien="")
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
            var result = await ABRRepository.GetBangDanhGiaNhanVien(MaBenhVienNguon,thang, nam);

            return Ok(result);
        }

        [HttpPost]
        [Route("GetLuongChiTietDichVu")]
        public async Task<IActionResult> GetLuongChiTietDichVu(int thang, int nam, int page=0, int NumRecords=0, string MaBenhVien = "")
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
            var result = await ABRRepository.GetLuongChiTietDichVu(MaBenhVienNguon, thang, nam, page, NumRecords);
            return Ok(result);
        }

        [HttpPost]
        [Route("GetFileLuongChiTietDichVu")]
        public async Task<IActionResult> GetFileLuongChiTietDichVu(int thang, int nam, string MaBenhVien = "")
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
            var repo = ABRRepository;
            var pkg = await repo.GetLuongChiTietDichVu(MaBenhVienNguon, thang, nam);
            bytes = pkg.GetAsByteArray();
            pkg.Dispose();
            return File(bytes, _contentType, thang + "" + nam + "ChiTietReport.xlsx");            

        }

        [HttpPost]
        [Route("GetGetLuongNhomDichVu")]
        public async Task<IActionResult> GetGetLuongNhomDichVu(int thang, int nam, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.GetGetLuongNhomDichVu(MaBenhVienNguon, thang, nam);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetNhanVienThucHienHis")]
        public async Task<IActionResult> GetNhanVienThucHienHis(NhanVienThucHienSM nhanVienThucHienSM, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.GetNhanVienThucHienHis(MaBenhVienNguon, nhanVienThucHienSM);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        #endregion

        #region Tinh ABR
        [HttpPost]
        [Route("SaveDanhGiaNhanVien")]
        public async Task<IActionResult> SaveDanhGiaNhanVien(ABRDanhGiaNhanVien aBRDanhGiaNhanVien, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.SaveDanhGiaNhanVien(MaBenhVienNguon, aBRDanhGiaNhanVien);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        [Route("DeleteBangDanhGiaNhanVien")]
        public async Task<IActionResult> DeleteBangDanhGiaNhanVien(int thang, int nam, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.DeleteBangDanhGiaNhanVien(MaBenhVienNguon, thang, nam);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("GetABRChiTietNhanVien")]
        public async Task<IActionResult> GetABRChiTietNhanVien(string MaNhanVien, int thang, int nam, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.GetABRChiTietNhanVien(MaBenhVienNguon, MaNhanVien, thang, nam);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpGet]
        [Route("GetDanhSachPool")]
        public async Task<IActionResult> GetDanhSachPool(string MaBenhVien = "")
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
            try  
            {
                var result = await ABRRepository.GetDanhSachPool(MaBenhVienNguon);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SavePool")]
        public async Task<IActionResult> SavePool(AbrPool aBRPool, string MaBenhVien="")
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
            try
            {
                var result = await ABRRepository.SavePool(MaBenhVienNguon, aBRPool);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("DeletePool")]
        public async Task<IActionResult> DeletePool(Guid Id, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.DeletePool(MaBenhVienNguon, Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("SaveXetDuyet")]
        public async Task<IActionResult> SaveXetDuyet(int thang, int nam, int muc, List<ABRSoSanhTongHopVM> listTongHop, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.SaveXetDuyet(MaBenhVienNguon,thang,nam,muc, listTongHop);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        [Route("DeleteXetDuyet")]
        public async Task<IActionResult> DeleteXetDuyet(int thang, int nam, int muc, string MaBenhVienChiNhanh="", string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.DeleteXetDuyet(MaBenhVienNguon, thang, nam, muc, MaBenhVienChiNhanh);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("CheckDaTinhSoLuongABR")]
        public async Task<IActionResult> CheckDaTinhSoLuongABR(int Thang, int Nam, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.CheckDaTinhSoLuongABR(MaBenhVienNguon,Thang, Nam);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("CheckDaUploadBangDanhGia")]
        public async Task<IActionResult> CheckDaUploadBangDanhGia(int Thang, int Nam, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.CheckDaUploadBangDanhGia(MaBenhVienNguon, Thang, Nam);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("CheckDaTinhABR")]
        public async Task<IActionResult> CheckDaTinhABR(int Thang, int Nam, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.CheckDaTinhABR(MaBenhVienNguon, Thang, Nam);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("CheckDaXetDuyet")]
        public async Task<IActionResult> CheckDaXetDuyet(int Thang, int Nam, int muc, string MaBenhVienChiNhanh, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.CheckDaXetDuyet(MaBenhVienNguon, Thang, Nam, muc, MaBenhVienChiNhanh);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("CheckDaXetDuyetTheoNgay")]
        public async Task<IActionResult> CheckDaXetDuyetTheoNgay(DateTime Ngay, string MaBenhVienChiNhanh="", string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.CheckDaXetDuyetTheoNgay(MaBenhVienNguon, Ngay, MaBenhVienChiNhanh);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetSoLanTuChoi")]
        public async Task<IActionResult> GetSoLanTuChoi(int Thang, int Nam, int Muc, string MaBenhVien="")
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
            try
            {
                var result = await ABRRepository.GetSoLanTuChoi(MaBenhVienNguon, Thang, Nam, Muc);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("TinhHuongABR")]
        public async Task<IActionResult> TinhHuongABR(int Thang, int Nam, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.TinhHuongABR(MaBenhVienNguon, Thang, Nam);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetNganSachThang")]
        public async Task<IActionResult> GetNganSachThang(int thang, int nam, string MaBenhVienChiNhanh="", string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.GetNganSachThang(MaBenhVienNguon, thang, nam, MaBenhVienChiNhanh);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SaveNganSachThang")]
        public async Task<IActionResult> SaveNganSachThang(ABRNganSachThang item, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.SaveNganSachThang(MaBenhVienNguon, item);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet]
        [Route("GetThucHienCuoi")]
        public async Task<IActionResult> GetThucHienCuoi(string MaBenhVien = "")
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
            var result = await ABRRepository.GetThucHienCuoi(MaBenhVienNguon);
            return Ok(result);
        }
        #endregion

        #region Loại Vai Trò

        [HttpGet]
        [Route("GetDanhSachLoaiVaiTro")]  
        public async Task<IActionResult> GetDanhSachLoaiVaiTro(string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.GetDanhSachLoaiVaiTro(MaBenhVienNguon);
                return Ok(result);
            }
            catch
            {

            }
            return Ok(false);
        }
        #endregion

        #region Pool theo danh mục
        [HttpPost]
        [Route("GetPoolHuongTheoDanhMuc")]
        public async Task<IActionResult> GetPoolHuongTheoDanhMuc(int idDanhMuc, string MaBenhVien="")
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
            try
            {
                var result = await ABRRepository.GetPoolHuongTheoDanhMuc(MaBenhVienNguon, idDanhMuc);
                return Ok(result);
            }
            catch
            {

            }
            return Ok(false);
        }
        [HttpPost]
        [Route("DeletePoolHuongTheoDanhMuc")]
        public async Task<IActionResult> DeletePoolHuongTheoDanhMuc(int idDanhMuc, string MaBenhVien="")
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
            try
            {
                var result = await ABRRepository.DeletePoolHuongTheoDanhMuc(MaBenhVienNguon, idDanhMuc);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SavePoolHuongTheoDanhMuc")]
        public async Task<IActionResult> SavePoolHuongTheoDanhMuc(int idDanhMuc, Guid idPool, string MaBenhVien="")
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
            try
            {
                var result = await ABRRepository.SavePoolHuongTheoDanhMuc(MaBenhVienNguon, idDanhMuc, idPool);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
        #region Tháng Năm
        [HttpGet]
        [Route("getNgayTheoThang")]
        public async Task<IActionResult> getNgayTheoThang(int Thang, int Nam, string MaBenhVien="")
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
            try
            {
                var result = await ABRRepository.getNgayTheoThang(MaBenhVienNguon,Thang, Nam);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }
        [HttpPost]
        [Route("SaveNgayTheoThang")]
        public async Task<IActionResult> SaveNgayTheoThang(ABRThangNam item, string MaBenhVien="")
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
            try
            {
                var result = await ABRRepository.SaveNgayTheoThang(MaBenhVienNguon, item);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }
        #endregion

        #region Mức tính ABR cho nhân viên
        [HttpPost]
        [Route("GetDanhSackDanhMucNhanVien")]
        public async Task<IActionResult> GetDanhSackDanhMucNhanVien(Guid IDNhanVien, string MaBenhVien="")
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
            try
            {
                var result = await ABRRepository.GetDanhSackDanhMucNhanVien(MaBenhVienNguon, IDNhanVien);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("SaveDanhMucNhanVien")]
        public async Task<IActionResult> SaveDanhMucNhanVien(ABRDanhMucNhanVienVM aBRDanhMucNhanVienVM, string MaBenhVien="")
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
            try
            {
                var result = await ABRRepository.SaveDanhMucNhanVien(MaBenhVienNguon, aBRDanhMucNhanVienVM);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("DeleteDanhMucNhanVien")]
        public async Task<IActionResult> DeleteDanhMucNhanVien(ABRDanhMucNhanVienVM aBRDanhMucNhanVienVM, string MaBenhVien = "")
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
            try
            {
                var result = await ABRRepository.DeleteDanhMucNhanVien(MaBenhVienNguon, aBRDanhMucNhanVienVM);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Xác nhận his  
        [HttpPost]
        [Route("GetXacNhanNhanVienThucHienHis")]
        public async Task<IActionResult> GetXacNhanNhanVienThucHienHis(TuNgayDenNgayOneParaSM tuNgayDenNgayOneParaSM)
        {
            var MaBenhVienNguon = "O";            
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
            try
            {
                var result = await ABRRepository.GetXacNhanNhanVienThucHienHis(MaBenhVienNguon, tuNgayDenNgayOneParaSM.Para1, tuNgayDenNgayOneParaSM.TuNgay, tuNgayDenNgayOneParaSM.DenNgay);
                return Ok(result);
            }
            catch(Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        [Route("SaveXacNhanNhanVienThucHienHis")]
        public async Task<IActionResult> SaveXacNhanNhanVienThucHienHis(ABRXacNhanNhanVienThucHienHisVM item)
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.SaveXacNhanNhanVienThucHienHis(MaBenhVienNguon, item);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        [Route("DeleteXacNhanNhanVienThucHienHis")]
        public async Task<IActionResult> DeleteXacNhanNhanVienThucHienHis(ABRXacNhanNhanVienThucHienHisVM item)
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.DeleteXacNhanNhanVienThucHienHis(MaBenhVienNguon, item);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        [Route("DeleteUserKetThucCongViecHis")]
        public async Task<IActionResult> DeleteUserKetThucCongViecHis( Guid ID)
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.DeleteUserKetThucCongViecHis(MaBenhVienNguon, ID);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        [Route("SaveUserKetThucCongViecHis")]
        public async Task<IActionResult> SaveUserKetThucCongViecHis(ABRUserKetThucCongViecHis item)
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.SaveUserKetThucCongViecHis(MaBenhVienNguon, item);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        [HttpPost]
        [Route("GetUserKetThucCongViecHis")]
        public async Task<IActionResult> GetUserKetThucCongViecHis()
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.GetUserKetThucCongViecHis(MaBenhVienNguon);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        [Route("LayUserKetThucHis")]
        public async Task<IActionResult> LayUserKetThucHis()
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.LayUserKetThucHis(MaBenhVienNguon);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        [Route("GetBaoCaoXacNhanNhanVienHis")]
        public async Task<IActionResult> GetBaoCaoXacNhanNhanVienHis(TuNgayDenNgayOneParaSM sM)
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.GetBaoCaoXacNhanNhanVienHis(MaBenhVienNguon,sM);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }

        }
        [HttpPost]
        [Route("GetBaoCaoChiTietXacNhanNhanVienHis")]
        public async Task<IActionResult> GetBaoCaoChiTietXacNhanNhanVienHis(TuNgayDenNgayOneParaSM sM)
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.GetBaoCaoChiTietXacNhanNhanVienHis(MaBenhVienNguon, sM);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }

        }

        [HttpPost]
        [Route("GetUserXacNhanNoiLamViecThucHien")]
        public async Task<IActionResult> GetUserXacNhanNoiLamViecThucHien()
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.GetUserXacNhanNoiLamViecThucHien(MaBenhVienNguon);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SaveUserXacNhanNoiLamViecThucHien")]
        public async Task<IActionResult> SaveUserXacNhanNoiLamViecThucHien(ABRUserXacNhanNoiLamViec aBRUserXacNhanNoiLamViec)
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.SaveUserXacNhanNoiLamViecThucHien(MaBenhVienNguon, aBRUserXacNhanNoiLamViec);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("DeleteUserXacNhanNoiLamViecThucHien")]
        public async Task<IActionResult> DeleteUserXacNhanNoiLamViecThucHien(ABRUserXacNhanNoiLamViec aBRUserXacNhanNoiLamViec)
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.DeleteUserXacNhanNoiLamViecThucHien(MaBenhVienNguon, aBRUserXacNhanNoiLamViec);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("GetUserXacNhanNoiLamViecChiDinh")]
        public async Task<IActionResult> GetUserXacNhanNoiLamViecChiDinh()
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.GetUserXacNhanNoiLamViecChiDinh(MaBenhVienNguon);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SaveUserXacNhanNoiLamViecChiDinh")]
        public async Task<IActionResult> SaveUserXacNhanNoiLamViecChiDinh(ABRUserXacNhanNoiLamViec aBRUserXacNhanNoiLamViec)
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.SaveUserXacNhanNoiLamViecChiDinh(MaBenhVienNguon, aBRUserXacNhanNoiLamViec);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("DeleteUserXacNhanNoiLamViecChiDinh")]
        public async Task<IActionResult> DeleteUserXacNhanNoiLamViecChiDinh(ABRUserXacNhanNoiLamViec aBRUserXacNhanNoiLamViec)
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.DeleteUserXacNhanNoiLamViecChiDinh(MaBenhVienNguon, aBRUserXacNhanNoiLamViec);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("GetNoiLamViec")]
        public async Task<IActionResult> GetNoiLamViec()
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.GetNoiLamViec(MaBenhVienNguon);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        #endregion

        #region ngày công nhân viên
        [HttpPost]
        [Route("GetNgayCong")]
        public async Task<IActionResult> GetNgayCong(int Thang, int Nam)
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.GetNgayCong(MaBenhVienNguon, Thang, Nam);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        
        [HttpPost]
        [Route("SaveNgayCong")]
        public async Task<IActionResult> SaveNgayCong(ABRNgayCong item)
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.SaveNgayCong(MaBenhVienNguon, item);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        [Route("DeleteNgayCong")]
        public async Task<IActionResult> DeleteNgayCong(ABRNgayCong item)
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.DeleteNgayCong(MaBenhVienNguon, item);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        #endregion

        #region Nhiều bệnh viện
        [HttpPost]
        [Route("GetBenhVien")]
        public async Task<IActionResult> GetBenhVien()
        {
            var MaBenhVienNguon = "O";
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
            try
            {
                var result = await ABRRepository.GetBenhVien(MaBenhVienNguon);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        #endregion
    }
}
