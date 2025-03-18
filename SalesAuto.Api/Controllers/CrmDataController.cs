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
    public class CrmDataController : ControllerBase
    {
        private readonly ILeadsRepo leadsRepository;
        public CrmDataController(ILeadsRepo leadsRepository)
        {
            this.leadsRepository = leadsRepository;
        }

        [HttpGet]
        [Route("LichKham")]
        public async Task<IActionResult> GetAll(bool full)
        {
            try
            {
                await leadsRepository.LoadLichKhamTuCRM(full);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
            
        }

        [HttpGet]
        [Route("LeadfromGoogle")]
        public async Task<IActionResult> GetleadsFromGoole(bool full)
        {
            try
            {
                await leadsRepository.LoadLeadsFromGoogle();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
