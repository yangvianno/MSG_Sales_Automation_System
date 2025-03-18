using Microsoft.AspNetCore.Http;
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
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _userRepository;

        public UsersController(IUserRepo userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetUserList();
            return Ok(users);
        }
        [HttpPost]
        public async Task<IActionResult> Save(UserVM userVM)
        {
            await _userRepository.Save(userVM);
            return Ok();
        }

    }
}
