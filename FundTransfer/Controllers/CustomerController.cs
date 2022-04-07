using FundTransfer.Core.DTOs;
using FundTransfer.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundTransfer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> OnboardNew([FromBody] KYCDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _customerService.OnboardNew(model);
            return Ok(response);
        }

    }
}
