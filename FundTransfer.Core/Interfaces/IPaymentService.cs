using FundTransfer.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundTransfer.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<K> PostRequest<K, T>(T model, string url);
        Task<T> GetRequest<T>(string url);
    }
}
