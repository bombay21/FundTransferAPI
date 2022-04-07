using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundTransfer.Core.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AcctId { get; set; }

        public int CustId { get; set; }

        public Customer Customer { get; set; }

        [Column(TypeName = "VARCHAR(10)")]
        public string AccountNumber { get; set; }
        
        public double AccountBalance { get; set; }

        public List<Transaction> Transaction { get; set; }
    }
}
