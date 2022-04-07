using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundTransfer.Core.Entities
{
    public class Customer
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CustId { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(150)")]
        public string Firstname { get; set; }
        
        [Required]
        [Column(TypeName = "VARCHAR(150)")]
        public string Lastname { get; set; }
        
        [Required]
        [Column(TypeName = "VARCHAR(150)")]
        public string Email { get; set; }
        
        [Required]
        [Column(TypeName = "VARCHAR(30)")]
        public string PhoneNo { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string Image { get; set; }

        public DateTime DateCreated { get; set; }

        public List<Account> Account { get; set; }
    }
}
