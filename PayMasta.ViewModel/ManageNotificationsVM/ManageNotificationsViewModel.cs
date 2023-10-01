using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.ManageNotificationsVM
{
    public class SendNotificationsRequest
    {
        public Guid AdminGuid { get; set; }
        public string NotificatiomMessage { get; set; }
        public string UserIds { get; set; }
    }

    public class SendNotificationsResponse
    {
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }
}
