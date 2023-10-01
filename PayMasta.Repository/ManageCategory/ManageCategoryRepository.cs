using Dapper;
using PayMasta.Entity.MainCategory;
using PayMasta.Entity.SubCategory;
using PayMasta.Entity.WalletService;
using PayMasta.Utilities;
using PayMasta.ViewModel.ManageCategoryVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.ManageCategory
{
    public class ManageCategoryRepository : IManageCategoryRepository
    {
        private string connectionString;

        public ManageCategoryRepository()
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

        public async Task<List<ManageCategories>> GetCategoryList(int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"select 
                                         COUNT(WC.Id) OVER() as TotalCount
                                        ,ROW_NUMBER() OVER(ORDER BY WC.Id DESC) AS RowNumber
                                        ,MC.Id MainCategoryId
                                        ,sc.Id SubCategoryId
                                        ,MC.CategoryName MainCategoryName
                                        ,sc.CategoryName SubCategoryName
                                        ,wc.IsActive
                                        ,wc.IsDeleted
                                        ,sc.Guid SubCategoryGuid
                                        ,MC.Guid MainCategoryGuid
                                        ,ISNULL(WC.ServiceName,'') ServiceName
                                        ,WC.Id WalletServiceId
                                        ,MC.CreatedAt
                                        ,CASE WHEN wc.IsActive=1 AND wc.IsDeleted=0 THEN 'Active' WHEN wc.IsActive=0 AND wc.IsDeleted=0 THEN 'Inactive' ELSE 'NA' END Status
                                        from SubCategory sc
                                        LEFT JOIN MainCategory MC on MC.Id=sc.MainCategoryId
                                        LEFT JOIN WalletService WC on WC.SubCategoryId=sc.Id
                                        WHERE WC.IsDeleted=0 AND sc.IsActive=1 AND sc.IsDeleted=0 AND wc.CountryCode='NG'
                                         AND (
                                                        @searchText='' 
                                                        OR MC.CategoryName LIKE('%'+@searchText+'%') OR sc.CategoryName LIKE('%'+@searchText+'%') OR  WC.ServiceName LIKE('%'+@searchText+'%')) 
								                            AND (
												                                (@fromDate IS NULL OR @todate is null) 
												                                    OR 
												        (CONVERT(DATE,MC.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
                                                  AND (
												     (@status IS NULL OR @status=0) OR (sc.Id=@status)
											        )
                                                        ORDER BY wc.Id DESC 
                                                        OFFSET @pageSize * (@pageNumber - 1) ROWS 
                                                        FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<ManageCategories>(query,
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
                return (await exdbConnection.QueryAsync<ManageCategories>(query,
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

        public async Task<WalletService> GetWalletServiceById(long id, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[ServiceName]
                                      ,[SubCategoryId]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                      ,[ImageUrl]
                                      ,[BankCode]
                                      ,[HttpVerbs]
                                      ,[RawData]
                                      ,[CountryCode]
                                      ,[BillerName]
                                      ,[OperatorId]
                                  FROM [dbo].[WalletService]
                                  WHERE Id=@Id";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<WalletService>(query,
                        new
                        {
                            Id = id
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<WalletService>(query,
                        new
                        {
                            Id = id
                        })).FirstOrDefault();
            }
        }

        public async Task<int> UpdateWalletServiceStatus(WalletService walletService, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[WalletService]
                                       SET 
                                          [IsActive] = @IsActive
                                          ,[IsDeleted] = @IsDeleted
                                          ,[UpdatedAt] = @UpdatedAt
                                     WHERE Id=@Id
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, walletService));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, walletService));
            }
        }

        public async Task<GetCategoryDetailResponse> ViewCategoryDetailByGuid(Guid id, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT 
                             sc.Guid SubCategoryGuid
                            ,sc.CategoryName SubCategoryName
                            ,sc.Id
                            ,MC.CategoryName MainCategoryName
                            ,mc.Guid MainCategoryGuid 
                            ,'' ImageUrl
                            FROM SubCategory sc
                            INNER join MainCategory MC on MC.Id=sc.MainCategoryId
                            WHERE sc.Guid=@Guid";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetCategoryDetailResponse>(query,
                        new
                        {
                            Guid = id
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetCategoryDetailResponse>(query,
                        new
                        {
                            Guid = id
                        })).FirstOrDefault();
            }
        }
        public async Task<SubCategory> GetSubCategoryDetail(Guid id, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[CategoryName]
                                      ,[MainCategoryId]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                  FROM [dbo].[SubCategory]
                                  WHERE Guid=@Guid";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<SubCategory>(query,
                        new
                        {
                            Guid = id
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<SubCategory>(query,
                        new
                        {
                            Guid = id
                        })).FirstOrDefault();
            }
        }
        public async Task<MainCategory> GetMainCategoryDetail(long id, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[CategoryName]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                  FROM [dbo].[MainCategory]
                                  WHERE Id=@Id";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<MainCategory>(query,
                        new
                        {
                            Id = id
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<MainCategory>(query,
                        new
                        {
                            Id = id
                        })).FirstOrDefault();
            }
        }


        public async Task<int> UpdateMainCategoryDetail(MainCategory mainCategory, IDbConnection exdbConnection = null)
        {
            string query = @"
                           UPDATE [dbo].[MainCategory]
                                       SET [CategoryName] = @CategoryName
                                          ,[UpdatedAt] = @UpdatedAt
                                     WHERE Id=@Id
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, mainCategory));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, mainCategory));
            }
        }

        public async Task<int> UpdateSubCategoryDetail(SubCategory subCategory, IDbConnection exdbConnection = null)
        {
            string query = @"
                           UPDATE [dbo].[SubCategory]
                                       SET [CategoryName] = @CategoryName
                                          ,[UpdatedAt] = @UpdatedAt
                                     WHERE Id=@Id
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, subCategory));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, subCategory));
            }
        }

        public async Task<SubCategory> GetSubCategoryDetailByName(string name, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[CategoryName]
                                      ,[MainCategoryId]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                  FROM [dbo].[SubCategory]
                                  WHERE CategoryName=@CategoryName";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<SubCategory>(query,
                        new
                        {
                            CategoryName = name
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<SubCategory>(query,
                        new
                        {
                            CategoryName = name
                        })).FirstOrDefault();
            }
        }
        public async Task<MainCategory> GetMainCategoryDetailByName(string name, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[CategoryName]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                  FROM [dbo].[MainCategory]
                                  WHERE CategoryName=@CategoryName";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<MainCategory>(query,
                        new
                        {
                            CategoryName = name
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<MainCategory>(query,
                        new
                        {
                            CategoryName = name
                        })).FirstOrDefault();
            }
        }

        public async Task<int> AddMainCategoryDetail(MainCategory mainCategory, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[MainCategory]
                                                   ([CategoryName]
                                                   ,[IsActive]
                                                   ,[IsDeleted]
                                                   ,[CreatedAt]
                                                   ,[UpdatedAt])
                                             VALUES
                                                   (@CategoryName
                                                   ,@IsActive
                                                   ,@IsDeleted
                                                   ,@CreatedAt
                                                   ,@UpdatedAt);
                             SELECT Id from MainCategory WHERE ID=CAST(SCOPE_IDENTITY() as BIGINT)";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, mainCategory));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, mainCategory));
            }
        }
        public async Task<int> AddSubCategoryDetail(SubCategory subCategory, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[SubCategory]
                                               ([CategoryName]
                                               ,[MainCategoryId]
                                               ,[IsActive]
                                               ,[IsDeleted]
                                               ,[CreatedAt]
                                               ,[UpdatedAt])
                                         VALUES
                                               (@CategoryName
                                               ,@MainCategoryId
                                               ,@IsActive
                                               ,@IsDeleted
                                               ,@CreatedAt
                                               ,@UpdatedAt)";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, subCategory));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, subCategory));
            }
        }

        public async Task<List<CategoryResponse>> GetCategories(bool isactive, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT sc.Id,sc.CategoryName,sc.MainCategoryId from SubCategory SC
                                    INNER JOIN MainCategory MC on MC.Id=SC.MainCategoryId
                                    WHERE sc.IsActive=@IsActive AND SC.IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<CategoryResponse>(query,
                        new
                        {
                            IsActive = isactive
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<CategoryResponse>(query,
                        new
                        {
                            IsActive = isactive
                        })).ToList();
            }
        }

        public async Task<List<ManageCategories>> GetCategoryListForCsv(int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null)
        {
            string query = @"select 
                                         COUNT(WC.Id) OVER() as TotalCount
                                        ,ROW_NUMBER() OVER(ORDER BY WC.Id DESC) AS RowNumber
                                        ,MC.Id MainCategoryId
                                        ,sc.Id SubCategoryId
                                        ,MC.CategoryName MainCategoryName
                                        ,sc.CategoryName SubCategoryName
                                        ,wc.IsActive
                                        ,wc.IsDeleted
                                        ,sc.Guid SubCategoryGuid
                                        ,MC.Guid MainCategoryGuid
                                        ,ISNULL(WC.ServiceName,'') ServiceName
                                        ,WC.Id WalletServiceId
                                        ,MC.CreatedAt
                                        ,CASE WHEN wc.IsActive=1 AND wc.IsDeleted=0 THEN 'Active' WHEN wc.IsActive=0 AND wc.IsDeleted=0 THEN 'Inactive' ELSE 'NA' END Status
                                        from SubCategory sc
                                        LEFT JOIN MainCategory MC on MC.Id=sc.MainCategoryId
                                        LEFT JOIN WalletService WC on WC.SubCategoryId=sc.Id
                                        WHERE WC.IsDeleted=0 AND sc.IsActive=1 AND sc.IsDeleted=0 AND wc.CountryCode='NG'
                                         
								                            AND (
												                                (@fromDate IS NULL OR @todate is null) 
												                                    OR 
												        (CONVERT(DATE,MC.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
                                                  AND (
												     (@status IS NULL OR @status=0) OR (sc.Id=@status)
											        )
                                                        ORDER BY wc.Id DESC ;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<ManageCategories>(query,
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
                return (await exdbConnection.QueryAsync<ManageCategories>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,
                            status = status,
                        })).ToList();
            }
        }
    }
}
