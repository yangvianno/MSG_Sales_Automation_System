using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesAuto.Api.Repositories;
using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KPIMonthlyController : ControllerBase
    {
        private readonly IKPIMonthlyRepo kPIMonthlyRepository;

        public KPIMonthlyController(IKPIMonthlyRepo kPIMonthlyRepository)
        {
            this.kPIMonthlyRepository = kPIMonthlyRepository;
        }
        [HttpGet]
        public async Task<IActionResult> LoadList(int nam, string MaBenhVien="")
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
            var result = await kPIMonthlyRepository.GetList(nam, MaBenhVienNguon);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Save(KPIMonthly kPIMonthly, string MaBenhVien="")
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
            kPIMonthly.MaBenhVien = MaBenhVienNguon;
            var result = await kPIMonthlyRepository.Save(kPIMonthly);
            return Ok(result);
        }
    }
}
