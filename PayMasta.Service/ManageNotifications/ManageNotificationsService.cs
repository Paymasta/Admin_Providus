using PayMasta.Entity.Notifications;
using PayMasta.Repository.Account;
using PayMasta.Repository.ManageNotifications;
using PayMasta.Repository.User;
using PayMasta.Utilities;
using PayMasta.Utilities.PushNotification;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.ManageNotificationsVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ManageNotifications
{
    public class ManageNotificationsService : IManageNotificationsService
    {


        //private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IManageNotificationsRepository _manageNotificationsRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPushNotification _pushNotification;
        public ManageNotificationsService()
        {
            _manageNotificationsRepository = new ManageNotificationsRepository();
            _userRepository = new UserRepository();
            _accountRepository = new AccountRepository();
            _pushNotification = new PushNotification();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }
        public async Task<SendNotificationsResponse> SendNotification(SendNotificationsRequest request)
        {
            var res = new SendNotificationsResponse();
            int result = 0;
            try
            {
                var adminData = await _userRepository.GetUserByGuid(request.AdminGuid);

                var ids = request.UserIds.Split(',');
                foreach (var id in ids)
                {
                    var req = new Notifications
                    {
                        AlterMessage = request.NotificatiomMessage,
                        DeviceToken = string.Empty,
                        DeviceType = 1,
                        CreatedAt = DateTime.Now,
                        CreatedBy = adminData.Id,
                        IsActive = true,
                        IsDeleted = false,
                        IsDelivered = true,
                        IsRead = false,
                        NotificationType = 1,
                        SenderId = adminData.Id,
                        ReceiverId = Convert.ToInt32(id),
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = adminData.Id,
                        NotificationJson = string.Empty,
                    };
                    result = await _manageNotificationsRepository.InsertNotification(req);
                    var userSessionData = await _accountRepository.GetSessionByUserId(Convert.ToInt32(id));
                    if (userSessionData != null && userSessionData.DeviceType != 3)
                    {
                        var notiReq = new PushModel
                        {
                            DeviceToken = userSessionData.DeviceToken,
                            Title = "PayMasta",
                            Message = request.NotificatiomMessage
                        };

                        var noti1 = _pushNotification.SendPush(notiReq);
                    }

                }
                if (result > 0)
                {
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.NOTIFICATIONSENT;
                }
                else
                {
                    res.IsSuccess = true;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                }


            }
            catch (Exception ex)
            {

            }
            return res;
        }
    }
}
