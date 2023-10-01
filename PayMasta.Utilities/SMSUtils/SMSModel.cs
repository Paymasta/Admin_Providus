using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Utilities.SMSUtils
{
    public class SMSModel
    {
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }
}
