using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity.WalletService
{
    public class WalletService : BaseEntity
    {
        public string ServiceName { get; set; }
        public int SubCategoryId { get; set; }
        public string ImageUrl { get; set; }
        public string BankCode { get; set; }
        public string HttpVerbs { get; set; }
        public string RawData { get; set; }
        public string CountryCode { get; set; }
        public string BillerName { get; set; }
        public long OperatorId { get; set; }
    }
}
