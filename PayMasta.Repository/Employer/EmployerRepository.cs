using Dapper;
using PayMasta.Utilities;
using PayMasta.ViewModel.EmployerVM;
using PayMasta.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer
{
    public class EmployerRepository : IEmployerRepository
    {
        private string connectionString;

        public EmployerRepository()
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

        public async Task<List<EmployerResponse>> GetEmployerList(int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"SELECT  COUNT(ED.Id) OVER() as TotalCount
                                                ,ROW_NUMBER() OVER(ORDER BY ED.Id DESC) AS RowNumber
                                                ,UM.Id UserId
                                                ,UM.Email
                                                ,UM.PhoneNumber
                                                ,UM.CountryCode
                                                ,CASE WHEN UM.Status=1 and UM.IsActive=1 AND ED.OrganisationName IS NOT NULL THEN 'Active' WHEN UM.Status=0 and UM.IsActive=1 AND ED.OrganisationName IS NOT NULL THEN 'Inactive' WHEN  UM.Status=1 and UM.IsActive=1 AND ED.OrganisationName IS NULL THEN 'Pending' ELSE 'NA' END  [Status]
                                                ,UM.CreatedAt
                                                ,UM.Guid
                                                ,ISNULL(ED.OrganisationName,'N/A')OrganisationName
                                                ,UM.[IsActive]
                                                ,UM.[IsDeleted]
                                                FROM UserMaster UM 
                                                LEFT JOIN EmployerDetail ED on ED.UserId=UM.Id
                                                 where UM.IsActive=1 and UM.IsDeleted=0 AND UM.UserType=3
												  AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,UM.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
		                                                AND (
                                                @searchText='' 
                                                OR FirstName LIKE('%'+@searchText+'%') OR ED.OrganisationName LIKE('%'+@searchText+'%'))
												 AND (
												     (@status IS NULL OR @status<0) OR (UM.Status=@status)
											        )
                                                ORDER BY ED.Id DESC 
                                                OFFSET @pageSize * (@pageNumber - 1) ROWS 
                                                FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployerResponse>(query,
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
                return (await exdbConnection.QueryAsync<EmployerResponse>(query,
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

        public async Task<EmployerProfileResponse> GetEmployerProfile(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT
                                     UM.Id UserId
                                    ,UM.Guid
                                    ,ED.OrganisationName
                                    ,UM.Email
                                    ,UM.CountryCode
                                    ,UM.PhoneNumber
                                    ,UM.CountryName
                                    ,UM.State
                                    ,UM.PostalCode
                                    ,UM.Address
                                    ,UM.IsActive
                                    ,UM.IsDeleted
                                    ,UM.Status,UM.ProfileImage
                                    from UserMaster UM
                                    INNER JOIN EmployerDetail ED on ED.UserId=UM.Id
                                    WHERE UM.Id=@UserId AND UM.IsActive=1 AND UM.IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployerProfileResponse>(query,
                        new
                        {
                            UserId = userId,

                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployerProfileResponse>(query,
                        new
                        {
                            UserId = userId,
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

        public async Task<List<EmployeesListViewModel>> GetEmployeesListByEmployerId(long employerId, int pageNumber, int pageSize, string searchText, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
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
									  AND (
                                @searchText='' 
                                OR FirstName LIKE('%'+@searchText+'%') OR LastName LIKE('%'+@searchText+'%'))
                            ORDER BY Id DESC 
                            OFFSET @pageSize * (@pageNumber - 1) ROWS 
                            FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);	";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            EmployerId = employerId,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            EmployerId = employerId,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText
                        })).ToList();
            }
        }


        public async Task<List<EmployeesWithdrawls>> GetEmployeesWithdrwals(long userid, int month, int pageSize, int pageNumber, IDbConnection exdbConnection = null)
        {

            string query = @";WITH  tableearning as(
                    select top 1 UserId from EarningMaster where UserId=@UserId and MONTH(CreatedAt)=MONTH(GETDATE()) AND YEAR(CreatedAt)=YEAR(GETDATE()) ORDER by 1 desc
                    )
                    SELECT  COUNT(AAR.Id) OVER() as TotalCount
                                            ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
                                            ,UM.Id UserId
                                            ,UM.Guid UserGuid
                                            ,CASE WHEN AAR.Status=0  THEN 'Pending' WHEN AAR.Status=1 THEN 'Approved' 
                                            WHEN AAR.Status=2 THEN 'Pending' WHEN AAR.Status=3 THEN 'Rejected' 
                                            WHEN AAR.Status=6 THEN 'Hold' ELSE 'NA' END  [Status]

                                           ,CASE WHEN AAR.AdminStatus=0 OR AAR.AdminStatus IS NULL THEN 'Pending' WHEN AAR.AdminStatus=1 THEN 'Approved' 
                                            WHEN AAR.AdminStatus=2 THEN 'Pending' WHEN AAR.AdminStatus=3 THEN 'Rejected' 
                                            WHEN AAR.AdminStatus=6 THEN 'Hold' ELSE 'NA' END  [AdminStatus]
                                            ,UM.[IsActive]
                                            ,UM.[IsDeleted]
                                            ,AAR.Status StatusId
                                            ,AAR.AdminStatus AdminStatusId
                                            ,AAR.AccessAmount
                                            ,AAR.Id AccessAmountId
                                            ,AAR.Guid AccessAmountGuid
                                            ,AAR.CreatedAt
                                            FROM UserMaster UM 
                                            INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
                                            INNER JOIN tableearning EM ON EM.UserId=UM.Id
                                            where UM.IsActive=1 and UM.IsDeleted=0 AND UM.Id=@UserId
                                            AND ( (@month IS NULL OR @month=0) OR (MONTH(AAR.CreatedAt)=@month AND YEAR(AAR.CreatedAt)=YEAR(GETDATE())))
                                            ORDER BY AAR.Id DESC 
                                            OFFSET @pageSize * (@pageNumber - 1) ROWS 
                                            FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesWithdrawls>(query,
                        new
                        {
                            UserId = userid,
                            month = month,
                            pageSize = pageSize,
                            pageNumber = pageNumber,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesWithdrawls>(query,
                        new
                        {
                            UserId = userid,
                            month = month,
                            pageSize = pageSize,
                            pageNumber = pageNumber,
                        })).ToList();
            }
        }

        public async Task<EmployeeEwaDetail> GetEmployeesEwaRequestDetail(long userid, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT top 1
                                                UM.Id UserId
												,UM.Guid UserGuid
												,UM.FirstName
												,UM.LastName
                                                ,UM.Email
                                                ,UM.PhoneNumber
                                                ,UM.CountryCode
                                                ,UM.[IsActive]
                                                ,UM.[IsDeleted]
											    ,EM.EarnedAmount
												,ED.EndDate-ED.StartDate [WorkiingDays]
												,UM.StaffId
												--,cast(round(EM.AvailableAmount*33.3/100,2) as numeric(36,2)) AvailableAmount
                                                 -- ,UsableAmount AvailableAmount
                                                ,cast(round(EM.UsableAmount,2) as numeric(36,2)) AvailableAmount
                                                FROM UserMaster UM 
												LEFT JOIN EarningMaster EM ON EM.UserId=UM.Id
                                                INNER JOIN EmployerDetail ED ON ED.Id=UM.EmployerId
                                                 where UM.IsActive=1 and UM.IsDeleted=0 AND UM.Id=@UserId
												 order by EM.Id  desc";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeeEwaDetail>(query,
                        new
                        {
                            UserId = userid,

                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeeEwaDetail>(query,
                        new
                        {
                            UserId = userid,

                        })).FirstOrDefault();
            }
        }

        public async Task<List<EmployerResponse>> GetEmployerListForCsv(int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null)
        {
           
            string query = @"SELECT  COUNT(ED.Id) OVER() as TotalCount
                                                ,ROW_NUMBER() OVER(ORDER BY ED.Id DESC) AS RowNumber
                                                ,UM.Id UserId
                                                ,UM.Email
                                                ,UM.PhoneNumber
                                                ,UM.CountryCode
                                                ,CASE WHEN UM.Status=1 and UM.IsActive=1 AND ED.OrganisationName IS NOT NULL THEN 'Active' WHEN UM.Status=0 and UM.IsActive=1 AND ED.OrganisationName IS NOT NULL THEN 'Inactive' WHEN  UM.Status=1 and UM.IsActive=1 AND ED.OrganisationName IS NULL THEN 'Pending' ELSE 'NA' END  [Status]
                                                ,UM.CreatedAt
                                                ,UM.Guid
                                                ,ISNULL(ED.OrganisationName,'N/A')OrganisationName
                                                ,UM.[IsActive]
                                                ,UM.[IsDeleted]
                                                FROM UserMaster UM 
                                                LEFT JOIN EmployerDetail ED on ED.UserId=UM.Id
                                                 where UM.IsActive=1 and UM.IsDeleted=0 AND UM.UserType=3
												  AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,UM.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
		                                               
												 AND (
												     (@status IS NULL OR @status<0) OR (UM.Status=@status)
											        )
                                                ORDER BY ED.Id DESC 
                                                ;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployerResponse>(query,
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
                return (await exdbConnection.QueryAsync<EmployerResponse>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,
                            status = status,
                        })).ToList();
            }
        }

        public async Task<List<EmployeesListViewModel>> GetEmployeesListByEmployerIdForCsv(long employerId, IDbConnection exdbConnection = null)
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
                                          ,CASE WHEN Status=1 and IsActive=1 THEN 'Active' WHEN Status=0 and IsActive=1 THEN 'InActive' ELSE 'NA' END  [Status]
                                          ,[IsActive]
                                          ,[IsDeleted]
                                      FROM [dbo].[UserMaster]
                                      where EmployerId=@EmployerId and IsActive=1 and IsDeleted=0 AND IsEmployerRegister=1
                            ORDER BY Id DESC;	";
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
    }
}
