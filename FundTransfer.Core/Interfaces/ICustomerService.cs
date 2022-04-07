using FundTransfer.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundTransfer.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<ApiResult<string>> OnboardNew(KYCDto model);
    }
}
