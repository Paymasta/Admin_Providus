using PayMasta.Entity.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.ManageNotifications
{
    public interface IManageNotificationsRepository
    {
        Task<int> InsertNotification(Notifications notifications, IDbConnection exdbConnection = null);
    }
}
