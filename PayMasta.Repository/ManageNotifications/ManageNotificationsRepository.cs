using Dapper;
using PayMasta.Entity.Notifications;
using PayMasta.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.ManageNotifications
{
    public class ManageNotificationsRepository: IManageNotificationsRepository
    {
        private string connectionString;

        public ManageNotificationsRepository()
        {
            connectionString = AppSetting.ConnectionStrings;
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        public async Task<int> InsertNotification(Notifications notifications, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[Notifications]
                                                   ([ReceiverId]
                                                   ,[SenderId]
                                                   ,[AlterMessage]
                                                   ,[NotificationJson]
                                                   ,[NotificationType]
                                                   ,[DeviceToken]
                                                   ,[DeviceType]
                                                   ,[IsRead]
                                                   ,[IsDelivered]
                                                   ,[IsActive]
                                                   ,[IsDeleted]
                                                   ,[CreatedAt]
                                                   ,[UpdatedAt])
                                             VALUES
                                                   (@ReceiverId
                                                   ,@SenderId
                                                   ,@AlterMessage
                                                   ,@NotificationJson
                                                   ,@NotificationType
                                                   ,@DeviceToken
                                                   ,@DeviceType
                                                   ,@IsRead
                                                   ,@IsDelivered
                                                   ,@IsActive
                                                   ,@IsDeleted
                                                   ,@CreatedAt
                                                   ,@UpdatedAt)";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, notifications));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, notifications));
            }
        }
    }
}
