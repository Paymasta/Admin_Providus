using Dapper;
using PayMasta.Entity.BankDetail;
using PayMasta.Entity.Earning;
using PayMasta.Entity.ErrorLog;
using PayMasta.Entity.ExpressWallet;
using PayMasta.Entity.TransactionLog;
using PayMasta.Entity.VirtualAccountDetail;
using PayMasta.Utilities;
using PayMasta.ViewModel.EWAVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Withdrawals
{
    public class WithdrawalsRepository : IWithdrawalsRepository
    {
        private string connectionString;

        public WithdrawalsRepository()
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

        public async Task<List<AccessAmountViewModel>> GetEmployeesEwaRequestList(int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"SELECT  COUNT(AAR.Id) OVER() as TotalCount
                                                ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
                                                ,UM.Id UserId
												,UM.Guid UserGuid
												,UM.FirstName
												,UM.LastName
												,UM.EmployerName
												,UM.EmployerId
                                                ,UM.Email
                                                ,UM.PhoneNumber
                                                ,UM.CountryCode
                                                ,CASE WHEN AAR.Status=0  THEN 'Pending' WHEN AAR.Status=1 THEN 'Approved' WHEN AAR.Status=2 THEN 'Pending' WHEN AAR.Status=3 THEN 'Rejected' WHEN AAR.Status=6 THEN 'Hold' ELSE 'NA' END  [Status]
                                                ,CASE WHEN AAR.AdminStatus=0 OR AAR.AdminStatus IS NULL  THEN 'Pending' WHEN AAR.AdminStatus=1 THEN 'Approved' WHEN AAR.AdminStatus=2 THEN 'Pending' WHEN AAR.AdminStatus=3 THEN 'Rejected' WHEN AAR.AdminStatus=6 THEN 'Hold' ELSE 'NA' END  [AdminStatus]
                                                ,UM.[IsActive]
                                                ,UM.[IsDeleted]
                                                ,AAR.Status StatusId
                                                ,AAR.AdminStatus AdminStatusId
                                                ,AAR.TotalAmountWithCommission AccessAmount
												,AAR.Id AccessAmountId
												,AAR.Guid AccessAmountGuid
                                                ,AAR.CreatedAt
                                                ,ISNULL(ED.IsEwaApprovalAccess,0) IsEwaApprovalAccess
                                                ,CASE WHEN UM.IsD2CUser=1 THEN 'D2CUser' WHEN UM.IsD2CUser=0 OR UM.IsD2CUser IS NULL THEN 'NOT D2C User' END [IsD2CUser]
                                                FROM UserMaster UM 
                                                INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
                                                INNER JOIN EmployerDetail ED ON ED.Id=UM.EmployerId
                                                 where UM.IsActive=1 and UM.IsDeleted=0
												  AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,AAR.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
		                                                AND (
                                                @searchText='' 
                                                OR UM.FirstName LIKE('%'+@searchText+'%') OR UM.Email LIKE('%'+@searchText+'%'))
												 AND (
												     (@status IS NULL OR @status<0) OR (AAR.Status=@status)
											        )
                                                ORDER BY AAR.Id DESC 
                                                OFFSET @pageSize * (@pageNumber - 1) ROWS 
                                                FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<AccessAmountViewModel>(query,
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
                return (await exdbConnection.QueryAsync<AccessAmountViewModel>(query,
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

        public async Task<AccessAmountViewDetail> GetEmployeesEwaRequestDetail(long UserId, long AccessAmountId, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT  COUNT(AAR.Id) OVER() as TotalCount
                                                ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
                                                ,UM.Id UserId
												,UM.Guid UserGuid
												,UM.FirstName
												,UM.LastName
												,UM.EmployerName
												,UM.EmployerId
                                                ,UM.Email
                                                ,UM.PhoneNumber
                                                ,UM.CountryCode
                                                ,CASE WHEN AAR.Status=0  THEN 'Pending' WHEN AAR.Status=1 THEN 'Approved'
												WHEN AAR.Status=2 THEN 'Pending' WHEN AAR.Status=3 THEN 'Rejected' 
												WHEN AAR.Status=6 THEN 'Hold' ELSE 'NA' END  [Status]
                                                ,UM.[IsActive]
                                                ,UM.[IsDeleted]
                                                ,AAR.Status StatusId
                                                ,AAR.TotalAmountWithCommission AccessAmount
											    ,EM.EarnedAmount
												,14 [Hours]
												,UM.StaffId
												,CAST(EM.UsableAmount as decimal(12,2)) AvailableAmount
												,AAR.Id AccessAmountId
												,AAR.Guid AccessAmountGuid
                                                ,AAR.CreatedAt
                                                ,ISNULL(UM.ProfileImage,'') ProfileImage
												,ISNULL(AAR.AdminStatus,0)AdminStatus
                                                FROM UserMaster UM 
                                                INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
												INNER JOIN EarningMaster EM ON EM.UserId=UM.Id
                                                 where UM.IsActive=1 and UM.IsDeleted=0 AND UM.Id=@UserId AND AAR.Id=@AccessAmountId;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<AccessAmountViewDetail>(query,
                        new
                        {
                            UserId = UserId,
                            AccessAmountId = AccessAmountId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<AccessAmountViewDetail>(query,
                        new
                        {
                            UserId = UserId,
                            AccessAmountId = AccessAmountId
                        })).FirstOrDefault();
            }
        }

        public async Task<AccessAmountViewDetail> GetEmployeesEwaRequestDetail1(long UserId, long AccessAmountId, IDbConnection exdbConnection = null)
        {

            string query = @"
;WITH tblEarning AS(
SELECT TOP 1 EarnedAmount,UsableAmount,UserId from EarningMaster WHERE UserId=@UserId AND  MONTH(CreatedAt)= MONTH(GETDATE()) AND Year(CreatedAt)= Year(GETDATE())  ORDER BY 1 DESC
)
SELECT  COUNT(AAR.Id) OVER() as TotalCount
                                                ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
                                                ,UM.Id UserId
												,UM.Guid UserGuid
												,UM.FirstName
												,UM.LastName
												,UM.EmployerName
												,UM.EmployerId
                                                ,UM.Email
                                                ,UM.PhoneNumber
                                                ,UM.CountryCode
                                                ,CASE WHEN AAR.Status=0  THEN 'Pending' WHEN AAR.Status=1 THEN 'Approved'
												WHEN AAR.Status=2 THEN 'Pending' WHEN AAR.Status=3 THEN 'Rejected' 
												WHEN AAR.Status=6 THEN 'Hold' ELSE 'NA' END  [Status]
                                                ,UM.[IsActive]
                                                ,UM.[IsDeleted]
                                                ,AAR.Status StatusId
                                                ,AAR.TotalAmountWithCommission AccessAmount
											    ,EM.EarnedAmount
												,14 [Hours]
												,UM.StaffId
												,CAST(EM.UsableAmount as decimal(12,2)) AvailableAmount
												,AAR.Id AccessAmountId
												,AAR.Guid AccessAmountGuid
                                                ,AAR.CreatedAt
                                                ,ISNULL(UM.ProfileImage,'') ProfileImage
												,ISNULL(AAR.AdminStatus,0)AdminStatus
                                                FROM UserMaster UM 
                                                INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
												INNER JOIN tblEarning EM ON EM.UserId=UM.Id 
                                                 where UM.IsActive=1 and UM.IsDeleted=0 AND UM.Id=@UserId AND AAR.Id=@AccessAmountId;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<AccessAmountViewDetail>(query,
                        new
                        {
                            UserId = UserId,
                            AccessAmountId = AccessAmountId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<AccessAmountViewDetail>(query,
                        new
                        {
                            UserId = UserId,
                            AccessAmountId = AccessAmountId
                        })).FirstOrDefault();
            }
        }

        public async Task<List<AccessAmountViewModel>> GetEmployeesEwaRequestListForCsv(int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT  COUNT(AAR.Id) OVER() as TotalCount
                                                ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
                                                ,UM.Id UserId
												,UM.Guid UserGuid
												,UM.FirstName
												,UM.LastName
												,UM.EmployerName
												,UM.EmployerId
                                                ,UM.Email
                                                ,UM.PhoneNumber
                                                ,UM.CountryCode
                                                ,CASE WHEN AAR.Status=0  THEN 'Pending' WHEN AAR.Status=1 THEN 'Approved' WHEN AAR.Status=2 THEN 'Pending' WHEN AAR.Status=3 THEN 'Rejected' WHEN AAR.Status=6 THEN 'Hold' ELSE 'NA' END  [Status]
                                                ,UM.[IsActive]
                                                ,UM.[IsDeleted]
                                                ,AAR.Status StatusId
                                                ,AAR.AccessAmount
												,AAR.Id AccessAmountId
												,AAR.Guid AccessAmountGuid
                                                ,AAR.CreatedAt
                                                FROM UserMaster UM 
                                                INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
                                                 where UM.IsActive=1 and UM.IsDeleted=0
												  AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,AAR.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
		                                               
												 AND (
												     (@status IS NULL OR @status<0) OR (AAR.Status=@status)
											        )
                                                ORDER BY AAR.Id DESC 
                                               ;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<AccessAmountViewModel>(query,
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
                return (await exdbConnection.QueryAsync<AccessAmountViewModel>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,
                            status = status,
                        })).ToList();
            }
        }

        public async Task<PayMasta.Entity.AccessAmountRequest.AccessAmountRequest> GetEmployeesEwaRequestDetailByUserId(long UserId, long AccessAmountId, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT [Id]
                                  ,[Guid]
                                  ,[UserId]
                                  ,[AccessAmount]
                                  ,[AccessedPercentage]
                                  ,[AvailableAmount]
                                  ,[PayCycle]
                                  ,[IsActive]
                                  ,[IsDeleted]
                                  ,[CreatedAt]
                                  ,[UpdatedAt]
                                  ,[CreatedBy]
                                  ,[UpdatedBy]
                                  ,[Status]
                                  ,[AdminStatus],[TotalAmountWithCommission]
                              FROM [dbo].[AccessAmountRequest]
                              WHERE UserId=@UserId AND Id=@AccessId;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<PayMasta.Entity.AccessAmountRequest.AccessAmountRequest>(query,
                        new
                        {
                            UserId = UserId,
                            AccessId = AccessAmountId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<PayMasta.Entity.AccessAmountRequest.AccessAmountRequest>(query,
                        new
                        {
                            UserId = UserId,
                            AccessId = AccessAmountId
                        })).FirstOrDefault();
            }
        }
        public async Task<BankDetail> GetBankDetailByUserId(long UserId, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT TOP 1 [Id]
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
                                          ,[BankCode]
                                          ,[ImageUrl]
                                          ,[IsSalaryAccount]
                                          ,[IsPayMastaCardApplied]
                                      FROM [dbo].[BankDetail]
                                      WHERE UserId=@UserId AND IsSalaryAccount=1
                                      ORDER BY Id Desc;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<BankDetail>(query,
                        new
                        {
                            UserId = UserId,
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<BankDetail>(query,
                        new
                        {
                            UserId = UserId,
                        })).FirstOrDefault();
            }
        }

        public async Task<int> UpdateEwaStatus(PayMasta.Entity.AccessAmountRequest.AccessAmountRequest accessAmountRequest, IDbConnection exdbConnection = null)
        {
            string query = @" UPDATE [dbo].[AccessAmountRequest]
                                           SET 
                                               [UpdatedAt] = @UpdatedAt
                                              ,[UpdatedBy] = @UpdatedBy
                                              ,[Status] = @Status
                                              ,[AdminStatus] = @AdminStatus
                                         WHERE Id=@Id;
                                ";

            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, accessAmountRequest));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, accessAmountRequest));
            }
        }

        public async Task<AccessdAmountPercentageResponse> GetAccessAmountPercentage(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT 
                                   UserId
                                   ,SUM([AccessedPercentage])[AccessedPercentage]
                              FROM [dbo].[AccessAmountRequest]
                              WHERE UserId=@UserId  AND ( MONTH(CreatedAt)=Month(GETDATE()) and year(CreatedAt)=year(GETDATE()))
                              group by UserId;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<AccessdAmountPercentageResponse>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<AccessdAmountPercentageResponse>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
            }
        }
        public async Task<EarningMasterResponse> GetEarnings(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT ISNull(em.[Id],0) Id
                                      ,em.[Guid]
                                      ,um.[Id] UserId
                                      ,ISNULL(em.[EarnedAmount],0)EarnedAmount
                                      ,ISNULL(em.[AccessedAmount],0) AccessedAmount
                                      ,ISNULL(em.[UsableAmount],0) AvailableAmount
                                      ,em.[PayCycle]
                                      ,em.[IsActive]
                                      ,em.[IsDeleted]
                                      ,em.[CreatedAt]
                                      ,em.[UpdatedAt]
                                      ,em.[CreatedBy]
                                      ,em.[UpdatedBy],ed.OrganisationName
									 ,CASE WHEN ed.StartDate IS NULL THEN 'NA' ELSE CONVERT(nvarchar(50), ed.StartDate)+' '+datename(MONTH,GETDATE()) END PayCycleFrom 
									 , CASE WHEN ed.EndDate IS NULL THEN 'NA' ELSE CONVERT(nvarchar(50), ed.EndDate)+' '+datename(MONTH,GETDATE()) END PayCycleTo 
									 --,FORMAT(ed.PayCycleTo, 'dd MMM') as PayCycleTo
                                  FROM  usermaster um
								  INNER join [dbo].[EarningMaster] em on em.UserId=um.id
								  INNER join EmployerDetail ed on ed.Id=um.EmployerId
								  WHERE um.id=@UserId AND ( MONTH(em.CreatedAt)=Month(GETDATE()) and year(em.CreatedAt)=year(GETDATE()))";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EarningMasterResponse>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EarningMasterResponse>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
            }
        }
        public async Task<EarningMaster> GetEarningByEarningId(long userId, long id, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                              ,[Guid]
                                              ,[UserId]
                                              ,[EarnedAmount]
                                              ,[AccessedAmount]
                                              ,[AvailableAmount]
                                              ,[PayCycle]
                                              ,[IsActive]
                                              ,[IsDeleted]
                                              ,[CreatedAt]
                                              ,[UpdatedAt]
                                              ,[CreatedBy]
                                              ,[UpdatedBy]
                                              ,[EarningMonth]
                                              ,[EarningYear],[UsableAmount]
                                          FROM [dbo].[EarningMaster]
                                          WHERE Id=@Id AND UserId=@UserId AND ( MONTH(CreatedAt)=Month(GETDATE()) and year(CreatedAt)=year(GETDATE()))";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EarningMaster>(query,
                        new
                        {
                            Id = id,
                            UserId = userId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EarningMaster>(query,
                        new
                        {
                            Id = id,
                            UserId = userId
                        })).FirstOrDefault();
            }
        }

        public async Task<int> UpdateEwaEarning(EarningMaster earningMaster, IDbConnection exdbConnection = null)
        {
            string query = @"UPDATE [dbo].[EarningMaster]
                                       SET [AccessedAmount] = @AccessedAmount
                                          ,[AvailableAmount] = @AvailableAmount
                                          ,[UpdatedAt] = @UpdatedAt
                                          ,[UpdatedBy] = @UpdatedBy
                                          ,[UsableAmount]=@UsableAmount
                                     WHERE Id=@Id;
                                ";

            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, earningMaster));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, earningMaster));
            }
        }

        public async Task<int> InsertTransactionLog(TransactionLog transactionLog, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[TransactionLog]
                                                ([LogType]
                                               ,[CreditAccount]
                                               ,[DebitAccount]
                                               ,[TransactionAmount]
                                               ,[TransactionReference]
                                               ,[TransactionName]
                                               ,[LogJson]
                                               ,[Detail]
                                               ,[LogDate])
                                         VALUES
                                               (@LogType
                                               ,@CreditAccount
                                               ,@DebitAccount
                                               ,@TransactionAmount
                                               ,@TransactionReference
                                               ,@TransactionName
                                               ,@LogJson
                                               ,@Detail
                                               ,@LogDate);";

            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, transactionLog));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, transactionLog));
            }
        }

        public async Task<VirtualAccountDetail> GetVirtualAccountDetailByUserId(long UserId, IDbConnection exdbConnection = null)
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
                                  WHERE UserId=@UserId;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<VirtualAccountDetail>(query,
                        new
                        {
                            UserId = UserId,
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<VirtualAccountDetail>(query,
                        new
                        {
                            UserId = UserId,
                        })).FirstOrDefault();
            }
        }

        public async Task<ExpressVirtualAccountDetail> GetExpressVirtualAccountDetailByUserId(long UserId, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT *
                                  FROM [dbo].[ExpressVirtualAccountDetail]
                                  WHERE UserId=@UserId;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<ExpressVirtualAccountDetail>(query,
                        new
                        {
                            UserId = UserId,
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<ExpressVirtualAccountDetail>(query,
                        new
                        {
                            UserId = UserId,
                        })).FirstOrDefault();
            }
        }
    }
}
