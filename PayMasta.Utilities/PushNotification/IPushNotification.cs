using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Utilities.PushNotification
{
    public interface IPushNotification
    {
        string SendPush(PushModel model);
    }
}
