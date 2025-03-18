using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesAuto.Api.Repositories;
using System;
using System.Threading.Tasks;

namespace SalesAuto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRMController : ControllerBase
    {
        private readonly ICRMClientRepo cRMClientRep;

        public CRMController(ICRMClientRepo cRMClientRep)
        {
            this.cRMClientRep = cRMClientRep;
        }

        [HttpGet]
        [Route("Getproduct")]
        public async Task<IActionResult> Getproduct()
        {            
            var result = await cRMClientRep.Getproduct();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetStore")]
        public async Task<IActionResult> GetStore()
        {
            var result = await cRMClientRep.GetStore();
            return Ok(result);
        }

        [HttpGet]
        [Route("Getorder")]
        public async Task<IActionResult> Getorder(DateTime TuNgay, DateTime DenNgay, int Trang = 1)
        {
            var result = await cRMClientRep.Getorder(TuNgay, DenNgay, Trang);
            return Ok(result);
        }

        [HttpGet]
        [Route("Getorder_status")]
        public async Task<IActionResult> Getorder_status()
        {
            var result = await cRMClientRep.Getorder_status();
            return Ok(result);
        }

        [HttpGet]
        [Route("Getlocation")]
        public async Task<IActionResult> Getlocation()
        {
            var result = await cRMClientRep.Getlocation();
            return Ok(result);
        }
    }
}
