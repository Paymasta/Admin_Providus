using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity.OtpInfo
{
    public class OtpInfo : BaseEntity
    {
        public long UserId { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string OtpCode { get; set; }
        public int Type { get; set; }
    }
}
