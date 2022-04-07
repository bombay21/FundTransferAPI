using FundTransfer.Core.DTOs;
using FundTransfer.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundTransfer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> TopUp([FromBody] AccountDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _accountService.TopUp(model);
            return Ok(response);
        }
    }
}
