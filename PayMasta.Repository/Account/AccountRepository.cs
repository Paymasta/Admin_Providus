using Dapper;
using PayMasta.Entity.OtpInfo;
using PayMasta.Entity.UserMaster;
using PayMasta.Entity.UserSession;
using PayMasta.Entity.VirtualAccountDetail;
using PayMasta.Utilities;
using PayMasta.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Account
{
    public class AccountRepository : IAccountRepository
    {
        private string connectionString;

        public AccountRepository()
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

        public async Task<UserMaster> Login(LoginRequest request)
        {
            using (var dbConnection = Connection)
            {
                string query = @"
                            SELECT TOP 1 [Id]
                                  ,[Guid]
                                  ,[FirstName]
                                  ,[LastName]
                                  ,[Email]
                                  ,[ProfileImage]
                                  ,[IsEmailVerified]
                                  ,[IsPhoneVerified]
                                  ,[IsVerified]
                                  ,[IsGuestUser]
                                  ,[IsActive],[Status]
                                  ,[IsDeleted],[CountryCode],[PhoneNumber],[IsProfileCompleted],[UserType],[Gender]
                            FROM [dbo].[UserMaster]
                            WHERE (Email=@email OR PhoneNumber=@email) AND Password=@password                          
                            ORDER BY Id DESC
";
                return (await dbConnection.QueryAsync<UserMaster>(query,
                    new
                    {
                        email = request.Email,
                        password = request.Password,

                    })).FirstOrDefault();
            }
        }

        public async Task<UserSession> GetSessionByDeviceId(long userId, string deviceId)
        {
            string query = @"SELECT [Id]
                                  ,[Guid]
                                  ,[UserId]
                                  ,[DeviceId]
                                  ,[DeviceType]
                                  ,[DeviceToken]
                                  ,[SessionKey]
                                  ,[SessionTimeout]
                                  ,[IsActive]
                                  ,[IsDeleted]
                                  ,[CreatedAt]
                                  ,[UpdatedAt]
                                  ,[CreatedBy]
                                  ,[UpdatedBy],[JwtToken]
                              FROM [dbo].[UserSession]
                            WHERE UserId=@userId AND DeviceId=@deviceId AND IsActive=1 AND IsDeleted=0";

            using (var dbConnection = Connection)
            {
                return (await dbConnection.QueryAsync<UserSession>(query,
                    new
                    {
                        userId = @userId,
                        deviceId = deviceId
                    })).FirstOrDefault();
            }

        }

        public async Task<UserSession> GetSessionByUserId(long userId)
        {
            string query = @"SELECT TOP 1 [Id]
                                  ,[Guid]
                                  ,[UserId]
                                  ,[DeviceId]
                                  ,[DeviceType]
                                  ,[DeviceToken]
                                  ,[SessionKey]
                                  ,[SessionTimeout]
                                  ,[IsActive]
                                  ,[IsDeleted]
                                  ,[CreatedAt]
                                  ,[UpdatedAt]
                                  ,[CreatedBy]
                                  ,[UpdatedBy],[JwtToken]
                              FROM [dbo].[UserSession]
                            WHERE UserId=@userId AND IsActive=1 AND IsDeleted=0 ORDER BY Id DESC";

            using (var dbConnection = Connection)
            {
                return (await dbConnection.QueryAsync<UserSession>(query,
                    new
                    {
                        userId = @userId,
                    })).FirstOrDefault();
            }

        }

        public async Task<int> CreateSession(UserSession userSessionEntity)
        {
            using (var dbConnection = Connection)
            {
                string query = @"INSERT INTO [dbo].[UserSession]
                                   ([UserId]
                                   ,[DeviceId]
                                   ,[DeviceType]
                                   ,[DeviceToken]
                                   ,[SessionKey]
                                   ,[SessionTimeout]
                                   ,[IsActive]
                                   ,[IsDeleted]
                                   ,[CreatedAt]
                                   ,[UpdatedAt],[JwtToken]
                                  )
                             VALUES
                                   (@UserId
                                   ,@DeviceId
                                   ,@DeviceType
                                   ,@DeviceToken
                                   ,@SessionKey
                                   ,@SessionTimeout
                                   ,@IsActive
                                   ,@IsDeleted
                                   ,@CreatedAt
                                   ,@UpdatedAt,@JwtToken
                                  )
                                ";
                return (await dbConnection.ExecuteAsync(query, userSessionEntity)); ;
            }
        }

        public async Task<int> UpdateSession(UserSession session)
        {
            using (var dbConnection = Connection)
            {
                string query = @" UPDATE [dbo].[UserSession]
                               SET [DeviceId] = @DeviceId
                                  ,[DeviceType] = @DeviceType
                                  ,[DeviceToken] = @DeviceToken
                                  ,[SessionKey] = @SessionKey
                                  ,[SessionTimeout] = @SessionTimeout
                                  ,[IsActive] = @IsActive
                                  ,[IsDeleted] = @IsDeleted
                                  ,[UpdatedAt] = @UpdatedAt
                                  ,[UpdatedBy] = @UpdatedBy
                             WHERE Id=@id;
                                ";
                return (await dbConnection.ExecuteAsync(query, session)); ;
            }
        }

        public async Task<UserMaster> GetUserByEmailOrPhone(int type, string emailorPhone)
        {
            string query = @"SELECT [Id],[Guid]
                                  ,[Email]
                                  ,[FirstName]
                                  ,[LastName]
                                  ,[CountryCode]
                                  ,[PhoneNumber],[UserType]
                             FROM [dbo].[UserMaster]
                            WHERE IsActive=1 AND IsDeleted=0 AND ( Email=@emailorPhone OR  PhoneNumber=@emailorPhone)
                                 ";

            using (var dbConnection = Connection)
            {
                return (await dbConnection.QueryAsync<UserMaster>(query,
                    new
                    {
                        type = type,
                        emailorPhone = emailorPhone
                    })).FirstOrDefault();
            }
        }


        public async Task<int> InsertOtpInfo(OtpInfo otpInfoEntity)
        {
            using (var dbConnection = Connection)
            {
                string query = @"INSERT INTO [dbo].[OtpInfo]
                                   ([UserId]
                                   ,[CountryCode]
                                   ,[PhoneNumber]
                                   ,[Email]
                                   ,[OtpCode]
                                   ,[Type]
                                   ,[IsActive]
                                   ,[IsDeleted]
                                   ,[CreatedAt]
                                   ,[UpdatedAt]
                                   ,[CreatedBy]
                                   ,[UpdatedBy])
                             VALUES
                                   (@UserId
                                   ,@CountryCode
                                   ,@PhoneNumber
                                   ,@Email
                                   ,@OtpCode
                                   ,@type
                                   ,@IsActive
                                   ,@IsDeleted
                                   ,@CreatedAt
                                   ,@UpdatedAt
                                   ,@CreatedBy
                                   ,@UpdatedBy);   
                                    SELECT OtpCode from OtpInfo where UserId=@UserId
                                ";
                return (await dbConnection.ExecuteAsync(query, otpInfoEntity));
                //SELECT OtpCode from OtpInfo WHERE UserId=@UserId)
            }
        }

        public async Task<UserMaster> GetUserByGuid(Guid guid, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                  ,[Guid]                                  
                                  ,ISNULL([FirstName],'') [FirstName]
								  ,ISNULL([MiddleName],'') [MiddleName]
                                  ,ISNULL([LastName],'')   [LastName]                          
                                  ,[Email]
								  ,ISNULL([NinNo],'')[NinNo]
	                              ,[DateOfBirth]
								  ,ISNULL([Gender] ,'')[Gender]
								  ,ISNULL([State] ,'')[State]
								  ,ISNULL([City],'')[City]
								  ,ISNULL([Address] ,'')[Address]
								  ,ISNULL([PostalCode],'')[PostalCode]
								  ,ISNULL([EmployerName] ,'')[EmployerName]
								  ,ISNULL([EmployerId] ,0)[EmployerId]
							   	  ,ISNULL([StaffId],'')[StaffId]
                                  ,[Password]
                                  ,ISNULL([ProfileImage],'')[ProfileImage]
                                  ,[CountryCode]
                                  ,[PhoneNumber]                                                           
                                  ,[WalletBalance]                                 
                                  ,[IsEmailVerified]
                                  ,[IsPhoneVerified]
                                  ,[IsVerified]                                 
                                  ,[IsGuestUser]
                                  ,[Status]
                                  ,[IsActive]
                                  ,[IsDeleted]
                                  ,[CreatedAt]
                                  ,[UpdatedAt]
                                  ,[CreatedBy]
                                  ,[UpdatedBy],[UserType],[IsvertualAccountCreated],[IsProfileCompleted],[CountryName]
                              FROM [dbo].[UserMaster]
                            WHERE Guid=@guid";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UserMaster>(query,
                        new
                        {
                            guid = guid
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UserMaster>(query,
                        new
                        {
                            guid = guid
                        })).FirstOrDefault();
            }
        }

        public async Task<int> UpdateUserPassword(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET 
                                  [Password] = @Password
                                  
                             WHERE Id=@id
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, userEntity));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, userEntity));
            }
        }

        public async Task<UserMaster> GetUserByEmailOrPhone(string emailorPhone)
        {
            string query = @"SELECT [Id],[Guid]
                                  ,[Email]
                                  ,[FirstName]
                                  ,[LastName]
                                  ,[CountryCode]
                                  ,[PhoneNumber],[Password]
                             FROM [dbo].[UserMaster]
                            WHERE IsActive=1 AND IsDeleted=0 AND (Email=@emailorPhone OR PhoneNumber=@emailorPhone)
                                 ";

            using (var dbConnection = Connection)
            {
                return (await dbConnection.QueryAsync<UserMaster>(query,
                    new
                    {
                        emailorPhone = emailorPhone
                    })).FirstOrDefault();
            }
        }
        public async Task<OtpInfo> GetOtpInfoByUserId(long userId)
        {
            string query = @"SELECT TOP 1 [Id]
                                  ,[OtpCode]
                              FROM [dbo].[OtpInfo]
                            WHERE UserId=@userId
                            ORDER BY Id DESC
                            ";

            using (var dbConnection = Connection)
            {
                return (await dbConnection.QueryAsync<OtpInfo>(query,
                    new
                    {
                        userId = userId
                    })).FirstOrDefault();
            }

        }

        public async Task<UserMaster> GetUserByMobile(string mobile, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                  ,[Guid]                                  
                                  ,[FirstName] 
								  ,[MiddleName]
                                  ,[LastName]                                
                                  ,[Email]
								  ,[NinNo]
	                              ,[DateOfBirth]
								  ,[Gender] 
								  ,[State] 
								  ,[City]
								  ,[Address] 
								  ,[PostalCode]
								  ,[EmployerName] 
								  ,[EmployerId] 
							   	  ,[StaffId] 
                                  ,[Password]
                                  ,[ProfileImage]
                                  ,[CountryCode]
                                  ,[PhoneNumber]                                                           
                                  ,[WalletBalance]                                 
                                  ,[IsEmailVerified]
                                  ,[IsPhoneVerified]
                                  ,[IsVerified]                                 
                                  ,[IsGuestUser]
                                  ,[Status]
                                  ,[IsActive]
                                  ,[IsDeleted]
                                  ,[CreatedAt]
                                  ,[UpdatedAt]
                                  ,[CreatedBy]
                                  ,[UpdatedBy]
                              FROM [dbo].[UserMaster]
                            WHERE PhoneNumber=@PhoneNumber";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UserMaster>(query,
                        new
                        {
                            PhoneNumber = mobile
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UserMaster>(query,
                        new
                        {
                            PhoneNumber = mobile
                        })).FirstOrDefault();
            }
        }

        public async Task<OtpInfo> GetOtpInfoByUserId(string mobile, string otp)
        {
            string query = @"SELECT TOP 1 [Id]
                                  ,[OtpCode]
                              FROM [dbo].[OtpInfo]
                            WHERE PhoneNumber=@PhoneNumber and OtpCode=@OtpCode
                            ORDER BY Id DESC
                            ";

            using (var dbConnection = Connection)
            {
                return (await dbConnection.QueryAsync<OtpInfo>(query,
                    new
                    {
                        PhoneNumber = mobile,
                        OtpCode = otp
                    })).FirstOrDefault();
            }

        }

        public async Task<int> VerifyUserPhoneNumber(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET 
                                  [IsPhoneVerified] = @IsPhoneVerified
                                  
                             WHERE Id=@id
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, userEntity));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, userEntity));
            }
        }


        public async Task<int> UpdateAdminProfile(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            using (var dbConnection = Connection)
            {
                string query = @" UPDATE [dbo].[UserMaster]
                                           SET [FirstName] = @FirstName
                                              ,[MiddleName] = @MiddleName
                                              ,[LastName] = @LastName
                                              ,[UpdatedAt] = @UpdatedAt
                                              ,[UpdatedBy] = @UpdatedBy
     
                                         WHERE Id=@Id;
                                ";
                return (await dbConnection.ExecuteAsync(query, userEntity)); ;
            }
        }

        public async Task<int> UploadProfileImage(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET 
                                  [ProfileImage] = @ProfileImage
                                  
                             WHERE Id=@id
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, userEntity));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, userEntity));
            }
        }

        public async Task<int> DeleteSession(long userId)
        {
            using (var dbConnection = Connection)
            {
                string query = @"DELETE FROM UserSession WHERE UserId=@UserId;";
                return (await dbConnection.ExecuteAsync(query, new { UserId = userId })); ;
            }
        }

        public async Task<VirtualAccountDetail> GetVirtualAccountDetailByUserId(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[VirtualAccountId]
                                      ,[ProfileID]
                                      ,[Pin]
                                      ,[deviceNotificationToken]
                                      ,[PhoneNumber]
                                      ,[Gender]
                                      ,[DateOfBirth]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                      ,[Address]
                                      ,[Bvn]
                                      ,[AccountName]
                                      ,[AccountNumber]
                                      ,[CurrentBalance]
                                      ,[JsonData]
                                      ,[UserId]
                                      ,[AuthToken]
                                      ,[AuthJson]
                                  FROM [dbo].[VirtualAccountDetail]
                                  Where UserId=@UserId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<VirtualAccountDetail>(query, new
                    {
                        UserId = userId
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<VirtualAccountDetail>(query, new
                {
                    UserId = userId
                })).FirstOrDefault();
            }
        }
        public async Task<int> UpdateVirtualAccountDetailByUserId(VirtualAccountDetail virtualAccountDetail, IDbConnection exdbConnection = null)
        {
            string query = @"UPDATE [dbo].[VirtualAccountDetail]
                                           SET
                                              [AuthToken] = @AuthToken
                                              ,[AuthJson] = @AuthJson
                                              ,[Pin] = @Pin
                                              ,[ProfileID] = @ProfileID
                                              ,[Address] = @Address
                                              ,[Bvn] = @Bvn
                                         WHERE Id=@Id";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, virtualAccountDetail));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, virtualAccountDetail));
            }
        }
        public async Task<int> InsertVirtualAccountDetail(VirtualAccountDetail virtualAccountDetail, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[VirtualAccountDetail]
                                               ([VirtualAccountId]
                                               ,[ProfileID]
                                               ,[Pin]
                                               ,[deviceNotificationToken]
                                               ,[PhoneNumber]
                                               ,[Gender]
                                               ,[DateOfBirth]
                                               ,[IsActive]
                                               ,[IsDeleted]
                                               ,[CreatedAt]
                                               ,[UpdatedAt]
                                               ,[Address]
                                               ,[Bvn]
                                               ,[AccountName]
                                               ,[AccountNumber]
                                               ,[CurrentBalance]
                                               ,[JsonData]
                                               ,[UserId]
                                               ,[AuthToken]
                                               ,[AuthJson])
                                         VALUES
                                               (@VirtualAccountId
                                               ,@ProfileID
                                               ,@Pin
                                               ,@deviceNotificationToken
                                               ,@PhoneNumber
                                               ,@Gender
                                               ,@DateOfBirth
                                               ,@IsActive
                                               ,@IsDeleted
                                               ,@CreatedAt
                                               ,@UpdatedAt
                                               ,@Address
                                               ,@Bvn
                                               ,@AccountName
                                               ,@AccountNumber
                                               ,@CurrentBalance
                                               ,@JsonData
                                               ,@UserId
                                               ,@AuthToken
                                               ,@AuthJson)";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, virtualAccountDetail));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, virtualAccountDetail));
            }
        }


    }
}
