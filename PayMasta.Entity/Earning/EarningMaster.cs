using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity.Earning
{
    public class EarningMaster : BaseEntity
    {
        public long UserId { get; set; }
        public decimal EarnedAmount { get; set; }
        public decimal AccessedAmount { get; set; }
        public decimal AvailableAmount { get; set; }
        public DateTime PayCycle { get; set; }
        public decimal UsableAmount { get; set; }
    }
}
