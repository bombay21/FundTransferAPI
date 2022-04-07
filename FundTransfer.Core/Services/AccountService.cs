using FundTransfer.Core.DTOs;
using FundTransfer.Core.Entities;
using FundTransfer.Core.Interfaces;
using FundTransfer.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundTransfer.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly Context _context;

        public AccountService(Context context)
        {
            _context = context;
        }

        public async Task<ApiResult<string>> TopUp(AccountDto model)
        {
            try
            {
                if (model == null)
                {
                    return new ApiResult<string> { HasError = true, Message = "Request cannot be empty" };
                }
                model = Helper.TrimStringProps(model);

                if (model.Amount < 1)
                {
                    return new ApiResult<string> { HasError = true, Message = "Amount must be greater than 0" };
                }

                if (string.IsNullOrEmpty(model.AccountNumber))
                    return new ApiResult<string> { HasError = true, Message = "Customer's account number is required" };

                var account = _context.Accounts.FirstOrDefault(x => x.AccountNumber == model.AccountNumber);
                if (account == null)
                {
                    return new ApiResult<string> { HasError = true, Message = "Customer's account not found" };
                }
                else
                {
                    account.AccountBalance += model.Amount;
                    var trans = new Transaction
                    {
                        TranDate = DateTime.Now,
                        TranAmount = model.Amount,
                        TranType = "CR",
                        AcctId = account.AcctId
                    };
                    _context.Transactions.Add(trans);
                }

                await _context.SaveChangesAsync();

                return new ApiResult<string>
                {
                    HasError = false,
                    Message = $"Your account has been successfully toped up with {model.Amount}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResult<string> { HasError = true, Message = $"{ex.Message}" };
            }
        }
    }
}
