using Dapper;
using PayMasta.Entity.SupportMaster;
using PayMasta.Utilities;
using PayMasta.ViewModel.SupportVm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Support
{
    public class SupportRepository : ISupportRepository
    {
        private string connectionString;

        public SupportRepository()
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

        public async Task<List<SupportViewModel>> GetSupportTicketList(int userType, string searchText, int pageNumber, int PageSize, DateTime? fromDate = null, DateTime? toDate = null, int status = -1, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"select  
                                    COUNT(SM.Id) OVER() as TotalCount
                                   ,ROW_NUMBER() OVER(ORDER BY SM.Id DESC) AS RowNumber
                                    ,SM.Id
                                    ,SM.Guid
                                    ,UM.Guid UserGuid
                                    ,UM.FirstName
                                    ,UM.LastName
                                    ,CASE WHEN um.EmployerName IS NULL THEN 'N/A' WHEN um.EmployerName='' THEN 'N/A' ELSE  um.EmployerName END EmployerName
                                    --,um.EmployerName
                                    ,um.EmployerId
                                    ,um.Email
                                    ,UM.CountryCode
                                    ,UM.PhoneNumber
                                    ,SM.CreatedAt
                                    ,CASE WHEN SM.[Status]=0 THEN 'Pending' WHEN SM.[Status]=2 THEN 'InProgress' WHEN SM.[Status]=3 THEN 'Resolved'  WHEN SM.[Status]=4 THEN 'Hold' WHEN SM.[Status]=5 THEN 'Rejected' ELSE 'Failed' END Status
                                    ,SM.TicketNumber
									,SM.Title
                                    ,SM.Status StatusId
									,SM.DescriptionText
                                    from SupportMaster SM
                                    inner join UserMaster UM on UM.Id=SM.UserId
                                    where UM.UserType=@UserType AND UM.IsActive=1 AND um.IsDeleted=0 AND SM.IsActive=1
                                   AND (@searchText='' 
                                                OR UM.FirstName LIKE('%'+@searchText+'%') OR UM.EmployerName LIKE('%'+@searchText+'%'))

                                    AND (
									(@fromDate IS NULL OR @todate is null) 
										OR 
									(CONVERT(DATE,SM.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
									)
									-------------month
									AND (
												     (@status IS NULL OR @status<0) OR (SM.Status=@status)
											        )
									ORDER BY SM.Id DESC 
                                                OFFSET @pageSize * (@pageNumber - 1) ROWS 
                                                FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<SupportViewModel>(query,
                        new
                        {
                            UserType = userType,
                            searchText = searchText,
                            fromDate = fromDate,
                            todate = toDate,
                            status = status,
                            pageNumber = pageNumber,
                            pageSize = PageSize
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<SupportViewModel>(query,
                        new
                        {
                            UserType = userType,
                            searchText = searchText,
                            fromDate = fromDate,
                            todate = toDate,
                            status = status,
                            pageNumber = pageNumber,
                            pageSize = PageSize
                        })).ToList();
            }
        }

        public async Task<SupportMaster> GetTicketStatusByTicketId(long ticketId, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[UserId]
                                      ,[TicketNumber]
                                      ,[Title]
                                      ,[DescriptionText]
                                      ,[Status]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                      ,[CreatedBy]
                                      ,[UpdatedBy]
                                  FROM [dbo].[SupportMaster]
                                  WHERE Id=@Id;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<SupportMaster>(query,
                        new
                        {
                            Id = ticketId,
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<SupportMaster>(query,
                        new
                        {
                            Id = ticketId
                        })).FirstOrDefault();
            }
        }

        public async Task<int> UpdateTicketStatus(SupportMaster supportMaster, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[SupportMaster]
                                           SET
                                              [Status] = @Status
                                              ,[IsActive] = @IsActive
                                              ,[IsDeleted] = @IsDeleted
                                              ,[UpdatedAt] = @UpdatedAt
                                              ,[UpdatedBy] = @UpdatedBy
                                         WHERE Id=@Id";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, supportMaster));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, supportMaster));
            }
        }

        public async Task<SupportViewModel> GetSupportTicketDetailByUserId(long UserId,long id, IDbConnection exdbConnection = null)
        {
            string query = @"select  
                                    COUNT(SM.Id) OVER() as TotalCount
                                   ,ROW_NUMBER() OVER(ORDER BY SM.Id DESC) AS RowNumber
                                    ,SM.Id
                                    ,SM.Guid
                                    ,UM.FirstName
                                    ,UM.LastName
                                    ,um.EmployerName
                                    ,um.EmployerId
                                    ,um.Email
                                    ,UM.CountryCode
                                    ,UM.PhoneNumber
                                    ,SM.CreatedAt
                                    ,CASE WHEN SM.[Status]=0 THEN 'Pending' WHEN SM.[Status]=2 THEN 'Inprogress' WHEN SM.[Status]=3 THEN 'Resolved'  WHEN SM.[Status]=4 THEN 'Hold' WHEN SM.[Status]=5 THEN 'Rejected' ELSE 'Failed' END Status
                                    ,SM.TicketNumber
									,SM.Title
									,SM.DescriptionText,SM.[Status] StatusId
                                    from SupportMaster SM
                                    inner join UserMaster UM on UM.Id=SM.UserId
                                    where UM.Id=@UserId AND SM.Id=@Id AND UM.IsActive=1 AND um.IsDeleted=0 AND SM.IsActive=1";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<SupportViewModel>(query,
                        new
                        {
                            Id= id,
                            UserId = UserId,

                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<SupportViewModel>(query,
                        new
                        {
                            Id = id,
                            UserId = UserId,

                        })).FirstOrDefault();
            }
        }

        public async Task<List<SupportViewModel>> GetSupportTicketListForCsv(int userType, DateTime? fromDate = null, DateTime? toDate = null, int status = -1, IDbConnection exdbConnection = null)
        {
            string query = @"select  
                                    COUNT(SM.Id) OVER() as TotalCount
                                   ,ROW_NUMBER() OVER(ORDER BY SM.Id DESC) AS RowNumber
                                    ,SM.Id
                                    ,SM.Guid
                                    ,UM.Guid UserGuid
                                    ,UM.FirstName
                                    ,UM.LastName
                                    ,um.EmployerName
                                    ,um.EmployerId
                                    ,um.Email
                                    ,UM.CountryCode
                                    ,UM.PhoneNumber
                                    ,SM.CreatedAt
                                    ,CASE WHEN SM.[Status]=0 THEN 'Pending' WHEN SM.[Status]=2 THEN 'InProgress' WHEN SM.[Status]=3 THEN 'Approved'  WHEN SM.[Status]=4 THEN 'Hold' WHEN SM.[Status]=5 THEN 'Rejected' ELSE 'Failed' END Status
                                    ,SM.TicketNumber
									,SM.Title
                                    ,SM.Status StatusId
									,SM.DescriptionText
                                    from SupportMaster SM
                                    inner join UserMaster UM on UM.Id=SM.UserId
                                    where UM.UserType=@UserType AND UM.IsActive=1 AND um.IsDeleted=0 AND SM.IsActive=1

                                    AND (
									(@fromDate IS NULL OR @todate is null) 
										OR 
									(CONVERT(DATE,SM.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
									)
									-------------month
									AND (
												     (@status IS NULL OR @status<0) OR (SM.Status=@status)
											        )
									ORDER BY SM.Id DESC ;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<SupportViewModel>(query,
                        new
                        {
                            UserType = userType,
                            fromDate = fromDate,
                            todate = toDate,
                            status = status,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<SupportViewModel>(query,
                        new
                        {
                            UserType = userType,
                            fromDate = fromDate,
                            todate = toDate,
                            status = status,
                        })).ToList();
            }
        }
    }
}
