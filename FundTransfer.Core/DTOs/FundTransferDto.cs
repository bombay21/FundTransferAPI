using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundTransfer.Core.DTOs
{
    public class FundTransferDto : AccountDto
    {
        public string TranType { get; set; }
    }
}
