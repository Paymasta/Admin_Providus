using Dapper;
using PayMasta.Entity.UpdateUserProfileRequest;
using PayMasta.Entity.UserMaster;
using PayMasta.Utilities;
using PayMasta.ViewModel.UpdateProfileRequestVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.UpdateProfileRequest
{
    public class UpdateProfileRequestRepository : IUpdateProfileRequestRepository
    {
        private string connectionString;

        public UpdateProfileRequestRepository()
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

        public async Task<List<UpdateProfileRequestResponse>> GetUpdateProfileRequestList(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null, string searchText = null, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"SELECT 
                                 COUNT(UM.Id) OVER() as TotalCount
                                ,ROW_NUMBER() OVER(ORDER BY UPR.Id DESC) AS RowNumber
                                ,UM.Id UserId
                                ,UM.Guid
                                ,UM.FirstName
                                ,UM.LastName
                                ,UM.Email
                                ,um.CountryCode
                                ,UM.PhoneNumber
                                ,UPR.CreatedAt
                                ,UM.IsActive
                                ,UM.IsDeleted,UPR.Guid UpdateUserProfileRequestGuid
                                FROM UpdateUserProfileRequest UPR
                                INNER JOIN UserMaster UM on UM.Id=UPR.UserId 
                                WHERE UM.IsDeleted=0 and UM.IsActive=1 AND UPR.Status<>1
                                 AND ( @searchText='' OR UM.FirstName LIKE('%'+@searchText+'%') OR UM.LastName LIKE('%'+@searchText+'%'))

                                  AND ( (@fromDate IS NULL OR @todate is null) 
                                                                 OR 
                                                                (CONVERT(DATE,UPR.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
                                                               )
                                                            ORDER BY UPR.Id DESC 
                                                            OFFSET @pageSize * (@pageNumber - 1) ROWS 
                                                            FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE)";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UpdateProfileRequestResponse>(query,
                        new
                        {

                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            fromDate = fromDate,
                            toDate = toDate,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UpdateProfileRequestResponse>(query,
                        new
                        {

                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            fromDate = fromDate,
                            toDate = toDate,
                        })).ToList();
            }
        }

        public async Task<UpdateProfileRequestResponse> GetOldProfileDetail(long userId, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT TOP 1
                                 COUNT(UM.Id) OVER() as TotalCount
                                ,ROW_NUMBER() OVER(ORDER BY UM.Id DESC) AS RowNumber
                                ,UM.Id UserId
                                ,UM.Guid
                                ,UM.FirstName
                                ,ISNULL(UM.ProfileImage,'') ProfileImage
                                ,ISNULL(UM.MiddleName,'') MiddleName
                                ,UM.LastName
                                ,UM.Email
                                ,um.CountryCode
                                ,UM.PhoneNumber
                                ,UM.CreatedAt
                                ,UM.IsActive
                                ,UM.IsDeleted
                                FROM UpdateUserProfileRequest UPR
                                INNER JOIN UserMaster UM on UM.Id=UPR.UserId
                                WHERE UM.IsDeleted=0 and UM.IsActive=1 AND UPR.UserId=@UserId AND UPR.Status<>1
								ORDER by UPR.Id desc;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UpdateProfileRequestResponse>(query,
                        new
                        {
                            UserId = userId,
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UpdateProfileRequestResponse>(query,
                        new
                        {
                            UserId = userId,
                        })).FirstOrDefault();
            }
        }

        public async Task<UpdateProfileRequestResponse> GetNewProfileDetail(long userId, IDbConnection exdbConnection = null)
        {

            string query = @"
								SELECT TOP 1
                                 COUNT(UM.Id) OVER() as TotalCount
                                ,UPR.Id RowNumber
                                ,UM.Id UserId
                                ,UM.Guid
                                ,UPR.FirstName
                                ,UPR.LastName,ISNULL(UPR.MiddleName,'') MiddleName
                                ,ISNULL(UPR.Address,'')Address 
                                ,UPR.Email
                                ,UPR.CountryCode
                                ,UPR.PhoneNumber
                                ,UPR.CreatedAt
                                ,UPR.IsActive
                                ,UPR.IsDeleted,ISNULL(UPR.Comment,'')Comment
                                FROM UpdateUserProfileRequest UPR
                                INNER JOIN UserMaster UM on UM.Id=UPR.UserId
                                WHERE UM.IsDeleted=0 and UM.IsActive=1 AND UPR.UserId=@UserId AND UPR.Status<>1
								ORDER by UPR.Id DESC;;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UpdateProfileRequestResponse>(query,
                        new
                        {
                            UserId = userId,
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UpdateProfileRequestResponse>(query,
                        new
                        {
                            UserId = userId,
                        })).FirstOrDefault();
            }
        }

        public async Task<UpdateProfileRequestResponse> GetNewProfileDetailForUpdateByUpdateRequestId(long userId, long UpdateUserProfileRequestId, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT TOP 1
                                 COUNT(UM.Id) OVER() as TotalCount
                                ,UPR.Id RowNumber
                                ,UM.Id UserId
                                ,UM.Guid
                                ,UPR.FirstName
                                ,UPR.LastName
                                ,ISNULL(UPR.MiddleName,'') MiddleName
                                ,UPR.Email
                                ,UPR.CountryCode
                                ,UPR.PhoneNumber
                                ,UPR.CreatedAt
                                ,ISNULL(UPR.Address,'')Address 
                                ,UPR.IsActive
                                ,UPR.IsDeleted
                                ,UPR.Status
                                FROM UpdateUserProfileRequest UPR
                                INNER JOIN UserMaster UM on UM.Id=UPR.UserId
                                WHERE UM.IsDeleted=0 and UM.IsActive=1 AND UPR.UserId=@UserId AND UPR.Id=@UpdateUserProfileRequestId
								ORDER by UPR.Id DESC;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UpdateProfileRequestResponse>(query,
                        new
                        {
                            UserId = userId,
                            UpdateUserProfileRequestId= UpdateUserProfileRequestId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UpdateProfileRequestResponse>(query,
                        new
                        {
                            UserId = userId,
                            UpdateUserProfileRequestId= UpdateUserProfileRequestId
                        })).FirstOrDefault();
            }
        }
        public async Task<int> UpdateUserProfileByAdmin(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"UPDATE [dbo].[UserMaster]
                                       SET [FirstName] = @FirstName
                                          ,[MiddleName] = @MiddleName
                                          ,[LastName] = @LastName
                                          ,[Email] = @Email
                                          ,[CountryCode] = @CountryCode
                                          ,[PhoneNumber] = @PhoneNumber
                                          ,[Address] = @Address
                                          ,[UpdatedAt] = @UpdatedAt
                                          ,[UpdatedBy] = @UpdatedBy
                                     WHERE Id=@Id
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

        public async Task<int> UpdateUserProfileStatusByAdmin(UpdateUserProfileRequest userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"UPDATE [dbo].[UpdateUserProfileRequest]
                                       SET [Status] = @Status
                                     WHERE Id=@Id
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

        public async Task<int> RemoveUpdateProfileRequest(Guid requestGuid, IDbConnection exdbConnection = null)
        {
            string query = @"
                             DELETE FROM UpdateUserProfileRequest WHERE Guid=@Guid;
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, new
                    {
                        Guid = requestGuid
                    }));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, new
                {
                    Guid = requestGuid
                }));
            }
            //return (await dbConnection.ExecuteAsync(query,
            //    new
            //    {
            //        Guid = requestGuid
            //    }));
        }

        public async Task<long> GetUserIdFromUpdateProfileRequestTable(Guid requestGuid, IDbConnection exdbConnection = null)
        {
            string query = @"
                             SELECT TOP 1 UserId FROM UpdateUserProfileRequest WHERE Guid=@Guid;
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await exdbConnection.QueryAsync<long>(query,
                        new
                        {
                            Guid = requestGuid,
                         
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<long>(query,
                       new
                       {
                           Guid = requestGuid,

                       })).FirstOrDefault();
            }
           
        }

        public long GetUserIdByUpdateProfileRequestGuid(Guid guid, IDbConnection exdbConnection = null)
        {
            string query = @"
                            SELECT TOP 1 UserId FROM UpdateUserProfileRequest WHERE Guid=@Guid;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<long>(query,
                        new
                        {
                            Guid = guid
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (exdbConnection.Query<long>(query,
                        new
                        {
                            Guid = guid
                        })).FirstOrDefault();
            }
        }

        public async Task<List<UpdateProfileRequestResponse>> GetUpdateProfileRequestListForScv(DateTime? fromDate = null, DateTime? toDate = null, IDbConnection exdbConnection = null)
        {
         
            string query = @"SELECT 
                                 COUNT(UM.Id) OVER() as TotalCount
                                ,ROW_NUMBER() OVER(ORDER BY UPR.Id DESC) AS RowNumber
                                ,UM.Id UserId
                                ,UM.Guid
                                ,UM.FirstName
                                ,UM.LastName
                                ,UM.Email
                                ,um.CountryCode
                                ,UM.PhoneNumber
                                ,UPR.CreatedAt
                                ,UM.IsActive
                                ,UM.IsDeleted,UPR.Guid UpdateUserProfileRequestGuid
                                FROM UpdateUserProfileRequest UPR
                                INNER JOIN UserMaster UM on UM.Id=UPR.UserId 
                                WHERE UM.IsDeleted=0 and UM.IsActive=1 AND UPR.Status<>1
                                  AND ( (@fromDate IS NULL OR @todate is null) 
                                                                 OR 
                                                                (CONVERT(DATE,UPR.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
                                                               )
                                                            ORDER BY UPR.Id DESC;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UpdateProfileRequestResponse>(query,
                        new
                        {

                            fromDate = fromDate,
                            toDate = toDate,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UpdateProfileRequestResponse>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,
                        })).ToList();
            }
        }
    }
}

