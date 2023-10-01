using Dapper;
using PayMasta.Entity.BankDetail;
using PayMasta.Entity.UserMaster;
using PayMasta.Entity.UserSession;
using PayMasta.Utilities;
using PayMasta.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private string connectionString;

        public UserRepository()
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
        public async Task<List<EmployeesListViewModel>> GetEmployeesList(int pendingemp,int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"SELECT   COUNT(Id) OVER() as TotalCount
                                          ,ROW_NUMBER() OVER(ORDER BY Id DESC) AS RowNumber
										  ,[Id] UserId
                                          ,[Guid] UserGuid
                                          ,ISnull([FirstName],'N/A')[FirstName]
                                          ,ISnull([LastName],'')[LastName]
                                          ,[Email]
                                          ,[CountryCode]
                                          ,[PhoneNumber]
                                          ,ISnull([StaffId],'N/A')
                                           ,CASE WHEN Status=1 and IsActive=1 AND IsProfileCompleted=1 THEN 'Active' 
										  WHEN Status=0 and IsActive=1 AND IsProfileCompleted=1 THEN 'Inactive' 
										  WHEN Status=1 and IsActive=1  AND IsDeleted=0 AND IsProfileCompleted=0 THEN 'Pending' ELSE 'NA' END  [Status]
                                          ,[IsActive]
                                          ,[IsDeleted]
										  --,ISnull([EmployerName],'N/A')[EmployerName]
                                          ,CASE WHEN EmployerName IS NULL THEN 'N/A' WHEN EmployerName='' THEN 'N/A' ELSE  EmployerName END EmployerName
										  ,[CreatedAt]
                                          ,CASE WHEN IsD2CUser=1 THEN 'D2CUser' WHEN IsD2CUser=0 OR IsD2CUser IS NULL THEN 'NOT D2C User' END [IsD2CUser]
                                      FROM [dbo].[UserMaster]
                                      where IsActive=1 and IsDeleted=0 AND (UserType=4)
									   AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
									  AND (
                                @searchText='' 
                                OR FirstName LIKE('%'+@searchText+'%') OR LastName LIKE('%'+@searchText+'%'))
								 AND (
												     (@status IS NULL OR @status<0) OR (Status=@status)
											        )
                                 AND (
												     (@Pendingstatus IS NULL OR @Pendingstatus<0) OR (IsProfileCompleted=@Pendingstatus)
											        )
                            ORDER BY Id DESC 
                            OFFSET @pageSize * (@pageNumber - 1) ROWS 
                            FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            status = status,
                            Pendingstatus =pendingemp
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            status = status
                        })).ToList();
            }
        }

        public async Task<List<EmployeesListViewModel>> GetEmployeesListForNotification(int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"SELECT   COUNT(Id) OVER() as TotalCount
                                          ,ROW_NUMBER() OVER(ORDER BY Id DESC) AS RowNumber
										  ,[Id] UserId
                                          ,[Guid] UserGuid
                                          ,ISnull([FirstName],'N/A')[FirstName]
                                          ,ISnull([LastName],'')[LastName]
                                          ,[Email]
                                          ,[CountryCode]
                                          ,[PhoneNumber]
                                          ,ISnull([StaffId],'N/A')
                                          ,CASE WHEN Status=1 and IsActive=1 AND FirstName IS NOT NULL THEN 'Active' WHEN Status=0 and IsActive=1 AND FirstName IS NOT NULL THEN 'Inactive' WHEN Status=1 and IsActive=1  AND IsDeleted=0 AND FirstName IS NULL THEN 'Pending' ELSE 'NA' END  [Status]
                                          ,[IsActive]
                                          ,[IsDeleted]
										  --,ISnull([EmployerName],'N/A')[EmployerName]
                                          ,CASE WHEN EmployerName IS NULL THEN 'NA' WHEN EmployerName='' THEN 'NA' ELSE  EmployerName END EmployerName
										  ,[CreatedAt]
                                      FROM [dbo].[UserMaster]
                                      where IsActive=1 and IsDeleted=0 AND (UserType=4 OR UserType=3)
									   AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
									  AND  (
                               @searchText='' 
                                OR FirstName LIKE('%'+@searchText+'%') OR LastName LIKE('%'+@searchText+'%')  OR EmployerName LIKE('%'+@searchText+'%') OR Email LIKE('%'+@searchText+'%') 
								OR PhoneNumber LIKE ('%'+@searchText+'%')
								
								)
								 AND (
												     (@status IS NULL OR @status<0) OR (Status=@status)
											        )
                            ORDER BY Id DESC 
                            OFFSET @pageSize * (@pageNumber - 1) ROWS 
                            FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            status = status,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            status = status
                        })).ToList();
            }
        }
        public async Task<List<EmployeesListViewModel>> GetEmployeesListForTransactions(int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"SELECT   COUNT(UM.Id) OVER() as TotalCount
                                          ,ROW_NUMBER() OVER(ORDER BY UM.Id DESC) AS RowNumber
										  ,UM.[Id] UserId
                                          ,UM.[Guid] UserGuid
                                          ,ISnull(UM.[FirstName],'N/A')[FirstName]
                                          ,ISnull(UM.[LastName],'')[LastName]
                                          ,UM.[Email]
                                          ,UM.[CountryCode]
                                          ,UM.[PhoneNumber]
                                          ,ISnull(UM.[StaffId],'N/A')
                                          ,CASE WHEN UM.Status=1 and UM.IsActive=1 AND UM.FirstName IS NOT NULL THEN 'Active' WHEN UM.Status=0 and UM.IsActive=1 AND UM.FirstName IS NOT NULL THEN 'Inactive' WHEN UM.Status=1 and UM.IsActive=1  AND UM.IsDeleted=0 AND UM.FirstName IS NULL THEN 'Pending' ELSE 'NA' END  [Status]
                                          ,UM.[IsActive]
                                          ,UM.[IsDeleted]
										  --,ISnull([EmployerName],'N/A')[EmployerName]
                                          ,CASE WHEN UM.EmployerName IS NULL THEN 'NA' WHEN UM.EmployerName='' THEN 'NA' ELSE  UM.EmployerName END EmployerName
										  ,UM.[CreatedAt]
                                      FROM [dbo].[UserMaster] UM
                                      --INNER JOIN wallettransaction WT ON WT.SenderId=UM.Id
                                      where UM.IsActive=1 and UM.IsDeleted=0 AND (UM.UserType=4)  AND  (SELECT count(*) AS totalTransation from WalletTransaction WHERE WalletTransaction.SenderId=UM.Id)>0
									   AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,UM.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
									  AND (
                                @searchText='' 
                                OR UM.FirstName LIKE('%'+@searchText+'%') OR UM.LastName LIKE('%'+@searchText+'%'))
								 AND (
												     (@status IS NULL OR @status<0) OR (Status=@status)
											        )
                            ORDER BY UM.Id DESC 
                            OFFSET @pageSize * (@pageNumber - 1) ROWS 
                            FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            status = status,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            status = status
                        })).ToList();
            }
        }
        public async Task<EmployeeDetail> GetEmployeeDetailByGuid(Guid userGuid, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT   TOP 1
				                            UM.[Id] UserId
                                            ,UM.[Guid] UserGuid
                                            ,UM.[FirstName]
                                            ,UM.[LastName]
                                            ,UM.[Email]
                                            ,UM.[CountryCode]
                                            ,UM.[PhoneNumber]
				                            ,UM.[DateOfBirth]
				                            ,UPPER(LEFT(UM.[Gender],1))+LOWER(SUBSTRING(UM.[Gender],2,LEN(UM.[Gender]))) [Gender]--UM.[Gender]
				                            ,UM.[Address]
				                            ,ISNULL(UM.[CountryName],'NA')CountryName
				                            ,UM.[State]
				                            ,UM.[City]
				                            ,ISnull(UM.[PostalCode],'NA')[PostalCode]
				                            ,CASE WHEN UM.[EmployerName] IS NULL THEN 'NA' WHEN UM.[EmployerName]='' THEN 'NA' ELSE  UM.[EmployerName] END EmployerName --ISnull(UM.[EmployerName],'NA')[EmployerName]
				                            ,ISnull(UM.[StaffId],'NA')[StaffId]
				                            ,UM.NetPayMonthly NetPay
				                            ,UM.GrossPayMonthly GrossPay
                                            ,CASE WHEN UM.Status=1 and UM.IsActive=1 THEN 'Active' WHEN UM.Status=0 and UM.IsActive=1 THEN 'Inactive' ELSE 'NA' END  [Status]
                                            ,UM.[IsActive]
                                            ,UM.[IsDeleted]
				                            ,ISNULL(BD.[BankName],'NA') [BankName]
				                            ,ISNULL(BD.AccountNumber,'NA')AccountNumber
				                            ,ISNULL(BD.BVN,'NA')BVN
				                            ,ISNULL(BD.BankAccountHolderName,'NA')BankAccountHolderName
                                            ,UM.ProfileImage
                                        FROM [dbo].[UserMaster] UM 
			                            LEFT JOIN BankDetail BD on BD.UserId=UM.Id 
                                        where UM.Guid=@Guid and UM.IsActive=1 and UM.IsDeleted=0;
								";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeeDetail>(query,
                        new
                        {
                            Guid = userGuid,
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeeDetail>(query,
                        new
                        {
                            Guid = userGuid,

                        })).FirstOrDefault();
            }
        }

        public async Task<GetAdminDetailResponse> GetAdminDetailByGuid(Guid userGuid, IDbConnection exdbConnection = null)
        {
            string query = @"select  um.Id
                                    ,um.FirstName
                                    ,um.LastName
                                    ,um.Address
                                    ,um.CountryCode
                                    ,um.PhoneNumber
                                    ,um.Gender
                                    ,um.UserType
                                    from UserMaster um
                                    where um.Guid=@Guid and um.IsActive=1 and um.IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetAdminDetailResponse>(query,
                        new
                        {
                            Guid = userGuid
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetAdminDetailResponse>(query,
                        new
                        {
                            Guid = userGuid
                        })).FirstOrDefault();
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

        public async Task<GetEmployerDetailResponse> GetEmployerDetailByGuid(Guid userGuid, IDbConnection exdbConnection = null)
        {
            string query = @"select  ed.Id
                                    ,um.FirstName
                                    ,um.LastName
                                    ,um.Address
                                    ,um.CountryCode
                                    ,um.PhoneNumber
                                    ,um.Gender
                                    ,um.UserType
                                    ,ed.OrganisationName
                                    from EmployerDetail ed
                                    inner join UserMaster um on um.Id=ed.UserId
                                    where um.Guid=@Guid and um.IsActive=1 and um.IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetEmployerDetailResponse>(query,
                        new
                        {
                            Guid = userGuid
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetEmployerDetailResponse>(query,
                        new
                        {
                            Guid = userGuid
                        })).FirstOrDefault();
            }
        }
        public async Task<int> UpdateUser(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET [Status]=@status
                                  ,[IsActive] = @IsActive
                                  ,[IsDeleted] = @IsDeleted
                                  ,[UpdatedAt] = @UpdatedAt
                                  ,[UpdatedBy] = @UpdatedBy
								 
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

        public long GetUserIdByGuid(Guid guid, IDbConnection exdbConnection = null)
        {
            string query = @"
                            SELECT [Id]
                            FROM [dbo].[UserMaster]
                            WHERE Guid=@guid";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<long>(query,
                        new
                        {
                            guid = guid
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (exdbConnection.Query<long>(query,
                        new
                        {
                            guid = guid
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
                            WHERE UserId=@userId  AND IsActive=1 AND IsDeleted=0 ORDER BY Id DESC";

            using (var dbConnection = Connection)
            {
                return (await dbConnection.QueryAsync<UserSession>(query,
                    new
                    {
                        userId = @userId,

                    })).FirstOrDefault();
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

        public async Task<List<EmployeesListViewModel>> GetEmployeesListForCsv(int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT   COUNT(UM.Id) OVER() as TotalCount
                                          ,ROW_NUMBER() OVER(ORDER BY UM.Id DESC) AS RowNumber
										  ,UM.[Id] UserId
                                          ,UM.[Guid] UserGuid
                                          ,ISnull(UM.[FirstName],'N/A')[FirstName]
                                          ,ISnull(UM.[LastName],'')[LastName]
                                          ,UM.[Email]
                                          ,UM.[CountryCode]
                                          ,UM.[PhoneNumber]
                                          ,ISnull(UM.[StaffId],'N/A')
                                          ,CASE WHEN UM.Status=1 and UM.IsActive=1 AND UM.FirstName IS NOT NULL THEN 'Active' WHEN UM.Status=0 and UM.IsActive=1 AND UM.FirstName IS NOT NULL THEN 'Inactive' WHEN UM.Status=1 and UM.IsActive=1  AND UM.IsDeleted=0 AND UM.FirstName IS NULL THEN 'Pending' ELSE 'NA' END  [Status]
                                          ,UM.[IsActive]
                                          ,UM.[IsDeleted]
										  --,ISnull([EmployerName],'N/A')[EmployerName]
                                          ,CASE WHEN UM.EmployerName IS NULL THEN 'NA' WHEN UM.EmployerName='' THEN 'NA' ELSE  UM.EmployerName END EmployerName
										  ,UM.[CreatedAt]
                                      FROM [dbo].[UserMaster] UM
                                      INNER JOIN wallettransaction WT ON WT.SenderId=UM.Id
                                      where UM.IsActive=1 and UM.IsDeleted=0 AND (UM.UserType=4)
									   AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,UM.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
								 AND (
												     (@status IS NULL OR @status<0) OR (Status=@status)
											        )
                            ORDER BY UM.Id DESC ;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,

                            status = status,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,

                            status = status
                        })).ToList();
            }
        }

        public async Task<List<EmployeesListViewModel>> GetEmployeesListForCsv(int pendingemp,int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT   COUNT(UM.Id) OVER() as TotalCount
                                          ,ROW_NUMBER() OVER(ORDER BY UM.Id DESC) AS RowNumber
										  ,UM.[Id] UserId
                                          ,UM.[Guid] UserGuid
                                          ,ISnull(UM.[FirstName],'N/A')[FirstName]
                                          ,ISnull(UM.[LastName],'')[LastName]
                                          ,UM.[Email]
                                          ,UM.[CountryCode]
                                          ,UM.[PhoneNumber]
                                          ,ISnull(UM.[StaffId],'N/A')
                                          ,CASE WHEN UM.Status=1 and UM.IsActive=1 AND UM.FirstName IS NOT NULL THEN 'Active' WHEN UM.Status=0 and UM.IsActive=1 AND UM.FirstName IS NOT NULL THEN 'Inactive' WHEN UM.Status=1 and UM.IsActive=1  AND UM.IsDeleted=0 AND UM.FirstName IS NULL THEN 'Pending' ELSE 'NA' END  [Status]
                                          ,UM.[IsActive]
                                          ,UM.[IsDeleted]
										  --,ISnull([EmployerName],'N/A')[EmployerName]
                                          ,CASE WHEN UM.EmployerName IS NULL THEN 'NA' WHEN UM.EmployerName='' THEN 'NA' ELSE  UM.EmployerName END EmployerName
										  ,UM.[CreatedAt]
                                      FROM [dbo].[UserMaster] UM
                                     -- INNER JOIN wallettransaction WT ON WT.SenderId=UM.Id
                                      where UM.IsActive=1 and UM.IsDeleted=0 AND (UM.UserType=4)
									   AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,UM.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
								 AND (
												     (@status IS NULL OR @status<0) OR (UM.Status=@status)
											        )
                                 AND (
												     (@Pendingstatus IS NULL OR @Pendingstatus<0) OR (UM.IsProfileCompleted=@Pendingstatus)
											        )
                            ORDER BY UM.Id DESC ;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,
                            status = status,
                            Pendingstatus=pendingemp
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,

                            status = status
                        })).ToList();
            }
        }

        public async Task<List<EmployeesListViewModel>> GetEmployeesListByEmployerId(long employerId, IDbConnection exdbConnection = null)
        {
         
            string query = @"SELECT   COUNT(Id) OVER() as TotalCount
                                          ,ROW_NUMBER() OVER(ORDER BY Id DESC) AS RowNumber
										  ,[Id] UserId
                                          ,[Guid] UserGuid
                                          ,[FirstName]
                                          ,[LastName]
                                          ,[Email]
                                          ,[CountryCode]
                                          ,[PhoneNumber]
                                          ,[StaffId]
                                          ,CASE WHEN Status=1 and IsActive=1 THEN 'Active' WHEN Status=0 and IsActive=1 THEN 'Inactive' ELSE 'NA' END  [Status]
                                          ,[IsActive],[Status] StatusId
                                          ,[IsDeleted]
                                      FROM [dbo].[UserMaster]
                                      where EmployerId=@EmployerId and IsActive=1 and IsDeleted=0 AND IsEmployerRegister=1
									  
                            ORDER BY Id DESC;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            EmployerId = employerId,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            EmployerId = employerId,
                        })).ToList();
            }
        }

        public async Task<List<BankDetail>> GetBankDetailByUserId(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[UserId]
                                      ,[BankName]
                                      ,[AccountNumber]
                                      ,[BVN]
                                      ,[BankAccountHolderName]
                                      ,[CustomerId]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                  FROM [dbo].[BankDetail]
                                  WHERE UserId=@UserId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<BankDetail>(query,
                        new
                        {
                            UserId = userId
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<BankDetail>(query,
                        new
                        {
                            UserId = userId
                        })).ToList();
            }
        }

        public async Task<int> DeleteBankByBankDetailId(BankDetail bankDetail, IDbConnection exdbConnection = null)
        {
            string query = @"
                           Update BankDetail set IsActive=0,IsDeleted=1,UpdatedAt=@UpdatedAt where Id=@Id
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, bankDetail));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, bankDetail));
            }
        }
    }
}
