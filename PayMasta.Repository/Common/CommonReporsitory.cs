using Dapper;
using PayMasta.Entity.ErrorLog;
using PayMasta.Entity.UserMaster;
using PayMasta.Entity.UserSession;
using PayMasta.Utilities;
using PayMasta.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Common
{
    public class CommonReporsitory : ICommonReporsitory
    {
        private string connectionString;

        public CommonReporsitory()
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
        public async Task<RandomInvoiceNumber> GetInvoiceNumber(IDbConnection dbConnection)
        {
            string query = @"usp_GetInvoiceNumber";
            // DynamicParameters parameter = new DynamicParameters();
            return (await dbConnection.QueryAsync<RandomInvoiceNumber>(query,
                      commandType: CommandType.StoredProcedure)).FirstOrDefault();
        }
        public UserSession GetUserSessionByToken(string token, IDbConnection exdbConnection = null)
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
                                      ,[UpdatedBy]
                                      ,[JwtToken]
                                  FROM [dbo].[UserSession]
                                  WHERE JwtToken=@JwtToken AND IsActive=1 AND IsDeleted=0 ORDER by Id Desc";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<UserSession>(query, new { JwtToken = token })).FirstOrDefault();
                }
            }
            else
            {
                return (exdbConnection.Query<UserSession>(query, new { JwtToken = token })).FirstOrDefault();
            }
        }

        public UserMaster GetUserByIdForSession(long userId, IDbConnection exdbConnection = null)
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
                                  ,[IslinkToOkra]
                              FROM [dbo].[UserMaster]
                            WHERE Id=@Id";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<UserMaster>(query,
                        new
                        {
                            Id = userId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (exdbConnection.Query<UserMaster>(query,
                        new
                        {
                            Id = userId
                        })).FirstOrDefault();
            }
        }

        public async Task<int> InsertProvidusBankResponse(ErrorLog errorLog, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[ErrorLog]
                                               ([ErrorMessage]
                                               ,[ClassName]
                                               ,[MethodName]
                                               ,[JsonData]
                                               ,[CreatedDate])
                                         VALUES
                                               (@ErrorMessage
                                               ,@ClassName
                                               ,@MethodName
                                               ,@JsonData
                                               ,@CreatedDate);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, errorLog));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, errorLog));
            }
        }
    }
}
