using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Utilities.SMSUtils
{
    public interface ISMSUtils
    {
        Task<bool> SendSms(SMSModel model);
    }
}
