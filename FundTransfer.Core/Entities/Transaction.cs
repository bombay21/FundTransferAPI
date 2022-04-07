using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundTransfer.Core.Entities
{
    public class Transaction
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int TranId { get; set; }

		public int AcctId { get; set; }

        public Account Account { get; set; }

        [Required]
		[Column(TypeName = "VARCHAR(2)")]
		public string TranType { get; set; }

		[Required]
		public double TranAmount { get; set; }

		public DateTime TranDate { get; set; }
	}
}
