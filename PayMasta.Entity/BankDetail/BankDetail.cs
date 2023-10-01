using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity.BankDetail
{
    public class BankDetail : BaseEntity
    {
        public long UserId { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string AccountNumber { get; set; }
        public string BVN { get; set; }
        public string BankAccountHolderName { get; set; }
        public string CustomerId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsSalaryAccount { get; set; }
    }
}
