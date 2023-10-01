using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity.Notifications
{
    public class Notifications : BaseEntity
    {
        public long ReceiverId { get; set; }
        public long SenderId { get; set; }
        public string AlterMessage { get; set; }
        public string NotificationJson { get; set; }
        public int NotificationType { get; set; }
        public string DeviceToken { get; set; }
        public int DeviceType { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsRead { get; set; }

    }
}
