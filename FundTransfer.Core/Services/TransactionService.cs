using FundTransfer.Core.DTOs;
using FundTransfer.Core.Entities;
using FundTransfer.Core.Interfaces;
using FundTransfer.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundTransfer.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly Context _context;
        private readonly IPaymentService _paymentService;
        private IConfiguration _config;

        public TransactionService(Context context, IPaymentService paymentService, IConfiguration _configuration)
        {
            _config = _configuration;
            _context = context;
            _paymentService = paymentService;
        }

        public async Task<ApiResult<PaystackInitializeResponse>> InitializeTransfer(FundTransferDto model)
        {
            try
            {
                if (model == null)
                {
                    return new ApiResult<PaystackInitializeResponse> { HasError = true, Message = "Request cannot be empty" };
                }
                model = Helper.TrimStringProps(model);

                if (model.Amount < 1)
                {
                    return new ApiResult<PaystackInitializeResponse> { HasError = true, Message = "Amount must be greater than 0" };
                }

                if (string.IsNullOrEmpty(model.AccountNumber))
                    return new ApiResult<PaystackInitializeResponse> { HasError = true, Message = "Account number is required" };

                var account = _context.Accounts.Where(x => x.AccountNumber == model.AccountNumber).Include(x => x.Customer).FirstOrDefault();
                if (account == null)
                {
                    return new ApiResult<PaystackInitializeResponse> { HasError = true, Message = "Customer's account not found" };
                }

                if (model.TranType == "CR")
                {
                    account.AccountBalance += model.Amount;
                }
                else if (model.TranType == "DR")
                {
                    if (account.AccountBalance < model.Amount)
                    {
                        return new ApiResult<PaystackInitializeResponse> { HasError = true, Message = "Insufficient funds" };
                    }

                    // call payment gateway
                    PaymentDto dto = new()
                    {
                        email = $"{_config.GetSection("ApiSettings")["BusinessEmail"]}",
                        amount = (Convert.ToDouble(model.Amount) * 100).ToString()
                    };

                    PaystackInitializeResponse response = await _paymentService.PostRequest<PaystackInitializeResponse, PaymentDto>(dto, $"{_config.GetSection("ApiSettings")["InitializeUri"]}");
                    if (response.status)
                    {
                        // redirect to checkout page
                        // and verify transaction
                        // but I would proceed to balance the books from here

                        //debit customer's account
                        account.AccountBalance -= model.Amount;

                        var trans = new Transaction
                        {
                            TranDate = DateTime.Now,
                            TranAmount = model.Amount,
                            TranType = model.TranType,
                            AcctId = account.AcctId
                        };
                        await _context.Transactions.AddAsync(trans);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return new ApiResult<PaystackInitializeResponse>
                        {
                            HasError = true,
                            Message = "Transfer could not be initialized"
                        };
                    }
                }
                else
                {
                    return new ApiResult<PaystackInitializeResponse> { HasError = true, Message = "Invalid transaction type" };
                }



                return new ApiResult<PaystackInitializeResponse>
                {
                    HasError = false,
                    Message = $"{model.TranType}: Transfer of {model.Amount} was successful"
                };
            }
            catch (Exception ex)
            {
                return new ApiResult<PaystackInitializeResponse> { HasError = true, Message = $"{ex.Message}" };
            }
        }


        //    verify trx from paystack and save transaction details
    }
}
