using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesAuto.Api.Repositories;
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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BenhNhanKhamController : ControllerBase
    {
        private readonly IBenhNhansRepo benhNhansRepository;

        public BenhNhanKhamController(IBenhNhansRepo benhNhansRepository)
        {
            this.benhNhansRepository = benhNhansRepository;
        }
        
        [HttpPost]
        [Route("searchBN")]
        public async Task<IActionResult> GetBenhNhanKhamList(BenhNhanSM BenhNhan)
        {
            var MaBenhVienNguon = "O";
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
            var result = await benhNhansRepository.GetBenhNhanKhamList(BenhNhan, MaBenhVienNguon);
            return Ok(result);
        }

        [HttpPost]
        [Route("searchBNPage")]
        public async Task<IActionResult> GetBenhNhanKhamListPage(BenhNhanSM BenhNhan)
        {
            var MaBenhVienNguon = "O";
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
            var result = await benhNhansRepository.GetBenhNhanKhamListPage(BenhNhan, MaBenhVienNguon);
            return Ok(result);
        }

        [HttpPost]
        [Route("searchBV")]
        public async Task<IActionResult> GetBenhVienKhamList(BenhNhanSM BenhNhan)
        {
            var MaBenhVienNguon = "O";
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
            var result = await benhNhansRepository.GetBenhVienKhamList(BenhNhan, MaBenhVienNguon);

            return Ok(result);
        }



    }
}
