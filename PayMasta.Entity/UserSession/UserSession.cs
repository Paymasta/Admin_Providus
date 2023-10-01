using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity.UserSession
{
    public class UserSession : BaseEntity
    {
        public long UserId { get; set; }
        public string DeviceId { get; set; }
        public int DeviceType { get; set; }
        public string DeviceToken { get; set; }
        public string SessionKey { get; set; }
        public DateTime SessionTimeout { get; set; }
        public string JwtToken { get; set; }
    }
}
