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
    public class CustomerService : ICustomerService
    {
        private readonly Context _context;

        public CustomerService(Context context)
        {
            _context = context;
        }
        public async Task<ApiResult<string>> OnboardNew(KYCDto model)
        {
            try
            {
                if(model == null)
                {
                    return new ApiResult<string> { HasError = true, Message = "Request cannot be empty" };
                }
                model = Helper.TrimStringProps(model);

                // validate email
                if (!Helper.IsValidEmail(model.Email))
                {
                    return new ApiResult<string> { HasError = true, Message = "Email is invalid" };
                }

                // ensure that neither firstname nor lastname is empty
                if (string.IsNullOrEmpty(model.Firstname) || string.IsNullOrEmpty(model.Lastname))
                    return new ApiResult<string> { HasError = true, Message = "Firstname and Lastname are required" };

                // ensure that phone number is unique
                if (_context.Customers.Any(x => x.PhoneNo == model.PhoneNo))
                    return new ApiResult<string> { HasError = true, Message = "The phone number is already linked to an account" };

                var userExist = _context.Customers.Any(x => x.Email.ToLower() == model.Email.ToLower());
                string acct_num = "";
                if (!userExist)
                {
                    // generate acct number
                    var accounts = _context.Accounts.ToList();
                    if(accounts.Count < 1 )
                    {
                        acct_num = Helper.GenerateAccountNumber();
                    }
                    else
                    {
                        string last_acct_num = accounts.OrderByDescending(x => x.AcctId).FirstOrDefault().AccountNumber;
                        acct_num = Helper.GenerateAccountNumber(last_acct_num);
                    }

                    var app_user = new Customer
                    {
                        DateCreated = DateTime.Now,
                        Email = model.Email,
                        Firstname = model.Firstname,
                        Lastname = model.Lastname,
                        Image = model.Image,
                        PhoneNo = model.PhoneNo,
                        Account = new List<Account>()
                        {
                            new Account()
                            {
                               AccountBalance = 0,
                               AccountNumber = acct_num
                            }
                        }
                    };
                    _context.Customers.Add(app_user);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return new ApiResult<string> { HasError = true, Message = "Customer already exists!" };
                }
                return new ApiResult<string>
                {
                    HasError = false,
                    Message = $"Customer Creation was successful. Account number is {acct_num}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResult<string> { HasError = true, Message = $"{ex.Message}" };
            }
        }

    }
}
