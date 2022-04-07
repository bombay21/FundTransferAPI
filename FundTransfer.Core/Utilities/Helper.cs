using FundTransfer.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FundTransfer.Core.Utilities
{
    public static class Helper
    {
        public static K TrimStringProps<K>(this K model)
        {
            try
            {
                var stringProperties = model.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

                foreach (var stringProperty in stringProperties)
                {
                    string currentValue = (string)stringProperty.GetValue(model, null);
                    if (currentValue != null)
                        stringProperty.SetValue(model, currentValue.Trim(), null);
                }
                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();

                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static string GenerateAccountNumber()
        {
            return "2000000001";
        }

        public static string GenerateAccountNumber(string last)
        {
            return $"{Convert.ToInt64(last) + 1}";
        }
    }
}
