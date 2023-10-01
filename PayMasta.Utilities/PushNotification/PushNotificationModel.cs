using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Utilities.PushNotification
{
    public class PushModel
    {
        public string DeviceToken { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int BadgeCounter { get; set; }
        public DateTime DateTime { get; set; }
        public Guid DataGuid { get; set; }
        public Guid SenderGuid { get; set; }
        public Guid RecieverGuid { get; set; }
        public int Tag { get; set; }
        public long GroupId { get; set; }
    }
}
