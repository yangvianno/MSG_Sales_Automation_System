using Microsoft.AspNetCore.Mvc;
using SalesAuto.Models.Entities;
using SalesAuto.Api.Repositories;
using System.Linq;
using System.Threading.Tasks;
using SalesAuto.Models.ViewModel;
using SalesAuto.Models.SearchModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace SalesAuto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LeadsController : ControllerBase
    {
        private readonly ILeadsRepo _leadsRepository;

        public LeadsController(ILeadsRepo leadsRepository)
        {
            this._leadsRepository = leadsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {   
            
            var leads = await _leadsRepository.GetAllLeadList();
            var relsult = leads.Select(x => new LeadVM()
            {
                Id = x.Id,
                STT = x.STT,
                TenKhachHang = x.TenKhachHang,
                SoPhu = x.SoPhu,
                Phone = x.Phone,
                Ngay = x.Ngay,
                Nguon = x.Nguon,
                TinhThanh = x.TinhThanh
            }
                );
            return Ok(relsult);
        }


        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> GetLeadList(LeadSM leadSM)
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
            var leads = await _leadsRepository.GetLeadList(leadSM, MaBenhVienNguon);
            return Ok(leads);
        }
        

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var lead = await _leadsRepository.GetLeadByID(id);
            if (lead == null)
            {
                return NotFound($"{id} not found");
            }
            return Ok(new LeadVM()
            {
                Id = lead.Id,
                STT = lead.STT,
                TenKhachHang = lead.TenKhachHang,
                SoPhu = lead.SoPhu,
                Phone = lead.Phone,
                Ngay = lead.Ngay,
                Nguon = lead.Nguon,
                TinhThanh = lead.TinhThanh
            });
        }


        [HttpPost]
        public async Task<IActionResult> Create(Lead lead)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var leads = await _leadsRepository.CreatLead(lead);
            return CreatedAtAction(nameof(GetByID), new { id = lead.Id }, lead);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, Lead lead)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _lead = await _leadsRepository.GetLeadByID(id);

            if (_lead == null)
            {
                return NotFound($"{id} not found");
            }
            _lead.TenKhachHang = lead.TenKhachHang;
            var leads =  await _leadsRepository.UpdateLead(id, _lead);
            return Ok(new LeadVM()
                {
                    Id = leads.Id,
                    STT = leads.STT,
                    TenKhachHang = leads.TenKhachHang,
                    SoPhu = leads.SoPhu,
                    Phone = leads.Phone,
                    Ngay = leads.Ngay,
                    Nguon = leads.Nguon,
                    TinhThanh = leads.TinhThanh
                }
            );
        }
        
    }
}
