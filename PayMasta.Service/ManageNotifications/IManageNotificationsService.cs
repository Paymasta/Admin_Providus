using PayMasta.ViewModel.ManageNotificationsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ManageNotifications
{
    public interface IManageNotificationsService
    {
        Task<SendNotificationsResponse> SendNotification(SendNotificationsRequest request);
    }
}
