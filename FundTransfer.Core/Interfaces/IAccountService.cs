using FundTransfer.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundTransfer.Core.Interfaces
{
    public interface IAccountService
    {
        Task<ApiResult<string>> TopUp(AccountDto model);
    }
}
