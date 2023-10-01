using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity.AccessAmountRequest
{
    public class AccessAmountRequest : BaseEntity
    {
        public long UserId { get; set; }
        public decimal AccessAmount { get; set; }
        public decimal AccessedPercentage { get; set; }
        public decimal AvailableAmount { get; set; }
        public DateTime PayCycle { get; set; }
        public int Status { get; set; }
        public int AdminStatus { get; set; }
        public decimal TotalAmountWithCommission { get; set; }
    }
}
