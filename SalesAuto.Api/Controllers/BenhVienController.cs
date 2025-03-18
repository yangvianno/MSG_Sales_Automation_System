using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesAuto.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BenhVienController : ControllerBase
    {
        private readonly IBenhViensRepo _benhViensRepository;

        public BenhVienController(IBenhViensRepo benhViensRepository)
        {
            this._benhViensRepository = benhViensRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var benhViens = await _benhViensRepository.GetAllBenhVienList();
            return Ok(benhViens);
        }
        [HttpPost]
        public async Task<IActionResult> GetBenhVienByUid(Guid userId )
        {
            var benhViens = await _benhViensRepository.GetBenhVienByUserID(userId);
            return Ok(benhViens);
        }
        [HttpPost]
        [Route("GetBenhVienByEmail")]
        public async Task<IActionResult> GetBenhVienByEmail(string Email)
        {
            var benhViens = await _benhViensRepository.GetBenhVienByEmail(Email);
            return Ok(benhViens);
        }

        
    }

}
