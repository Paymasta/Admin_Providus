using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity.SupportMaster
{
    public class SupportMaster:BaseEntity
    {
        public long UserId { get; set; }

        public string TicketNumber { get; set; }
        public string Title { get; set; }
        public string DescriptionText { get; set; }
        public int Status { get; set; }
        
    }
}
