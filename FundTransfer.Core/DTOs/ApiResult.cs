using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundTransfer.Core.DTOs
{
    public class ApiResult<T>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
