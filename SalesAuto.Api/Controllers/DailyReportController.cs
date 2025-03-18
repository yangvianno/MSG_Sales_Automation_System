using Microsoft.AspNetCore.Mvc;
using SalesAuto.Api.Repositories;
using SalesAuto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyReportController : ControllerBase
    {
        private readonly ISaleAutoReportRepo reportRepository;
        private readonly IDailyReportRepo dailyReportRepository;
        private readonly IMailRepo mailRepo;

        public DailyReportController(ISaleAutoReportRepo reportRepository, IDailyReportRepo dailyReportRepository, IMailRepo mailRepo)
        {
            this.reportRepository = reportRepository;
            this.dailyReportRepository = dailyReportRepository;
            this.mailRepo = mailRepo;
        }


        [HttpPost]        
        [Route("GetDailyReportMat")]
        public async Task<IActionResult> GetDailyReportMat()
        {
            var result = await dailyReportRepository.GetDailyReportMatString();            
            return Ok(result);
        }

        [HttpPost]
        [Route("GetDailyReportMatTuan")]
        public async Task<IActionResult> GetDailyReportMatTuan(TuanVM tuan)
        {
            var result = await dailyReportRepository.GetDailyReportMatString(tuan.TuNgay, tuan.DenNgay);
            return Ok(result);
        }


        [HttpPost]
        [Route("GetDailyReportBenhVienString")]
        public async Task<IActionResult> GetDailyReportBenhVienString(string MaBenhVien="")
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
            var result = await dailyReportRepository.GetDailyReportBenhVienString(MaBenhVienNguon);
            return Ok(result);
        }

        [HttpPost]
        [Route("GetDailyReportMatSum")]
        public async Task<IActionResult> GetDailyReportMatSum()
        {
            var result = await dailyReportRepository.GetDailyReportMatSumString();            
            return Ok(result);
        }

        [HttpPost]
        [Route("GetDailyReportMatSumTuan")]
        public async Task<IActionResult> GetDailyReportMatSum(TuanVM tuan)
        {
            var result = await dailyReportRepository.GetDailyReportMatSumString(tuan.TuNgay, tuan.DenNgay);
            return Ok(result);
        }

        [HttpPost]
        [Route("GetDailyReportDaKhoa")]
        public async Task<IActionResult> GetDailyReportDaKhoa()
        {
            var result = await dailyReportRepository.GetDailyReportDaKhoaString();
            return Ok(result);
        }

        [HttpPost]
        [Route("GetDailyReportDaKhoaTuan")]
        public async Task<IActionResult> GetDailyReportDaKhoaTuan(TuanVM tuan)
        {
            var result = await dailyReportRepository.GetDailyReportDaKhoaString(tuan.TuNgay, tuan.DenNgay);
            return Ok(result);
        }


        [HttpPost]
        [Route("GetDailyReportDaKhoaSum")]
        public async Task<IActionResult> GetDailyReportDaKhoaSum()
        {
            var result = await dailyReportRepository.GetDailyReportMatSumString();
            return Ok(result);
        }

        [HttpPost]
        [Route("GetDailyReportDaKhoaSumTuan")]
        public async Task<IActionResult> GetDailyReportDaKhoaSum(TuanVM tuan)
        {
            var result = await dailyReportRepository.GetDailyReportMatSumString(tuan.TuNgay, tuan.DenNgay);
            return Ok(result);
        }

        [HttpPost]
        [Route("SendMailDaiLyTuanMat")]
        public async Task<IActionResult> SendMailDaiLyTuanMat(bool resend = false)
        {
            try
            {
                await mailRepo.SendMailDaiLyTuanMat(reportRepository, dailyReportRepository, resend);
                return Ok("Da gui mail!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SendMailDaiLyTuanDaKhoa")]
        public async Task<IActionResult> SendMailDaiLyTuanDaKhoa(bool resend = false)
        {
            try
            {
                await mailRepo.SendMailDaiLyTuanDaKhoa(reportRepository, dailyReportRepository, resend);
                return Ok("Da gui mail!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SendMailDaiLyBenhVien")]
        public async Task<IActionResult> SendMailDaiLyBenhVien(string MaBenhVien,bool resend = false)
        {
            try
            {
                await mailRepo.SendMailDaiLyBenhVien(reportRepository, dailyReportRepository, MaBenhVien, resend);
                return Ok("Da gui mail!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
