using Dapper;
using PayMasta.Entity.Faq;
using PayMasta.Entity.PrivacyPolicy;
using PayMasta.Entity.TermAndCondition;
using PayMasta.Utilities;
using PayMasta.ViewModel.CMSVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.ManageCms
{
    public class ManageCmsRepository : IManageCmsRepository
    {
        private string connectionString;

        public ManageCmsRepository()
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
        public async Task<int> InsertFaqInfo(FAQ faqInfoEntity, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[FAQ]
                                                   ([Detail]
                                                   ,[IsActive]
                                                   ,[IsDeleted]
                                                   ,[CreatedAt]
                                                   ,[UpdatedAt])
                                             VALUES
                                                   (@Detail
                                                   ,@IsActive
                                                   ,@IsDeleted
                                                   ,@CreatedAt
                                                   ,@UpdatedAt);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, faqInfoEntity));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, faqInfoEntity));
            }
        }
        public async Task<int> InsertPrivacyPolicyInfo(PrivacyPolicy privacyInfoEntity, IDbConnection exdbConnection = null)
        {

            string query = @"INSERT INTO [dbo].[PrivacyPolicy]
                                                   ([Detail]
                                                   ,[IsActive]
                                                   ,[IsDeleted]
                                                   ,[CreatedAt]
                                                   ,[UpdatedAt])
                                             VALUES
                                                   (@Detail
                                                   ,@IsActive
                                                   ,@IsDeleted
                                                   ,@CreatedAt
                                                   ,@UpdatedAt); ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, privacyInfoEntity));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, privacyInfoEntity));
            }


        }
        public async Task<int> InsertTermAndConditionInfo(TermAndCondition termInfoEntity, IDbConnection exdbConnection = null)
        {

            string query = @"INSERT INTO [dbo].[TermAndCondition]
                                                   ([Detail]
                                                   ,[IsActive]
                                                   ,[IsDeleted]
                                                   ,[CreatedAt]
                                                   ,[UpdatedAt])
                                             VALUES
                                                   (@Detail
                                                   ,@IsActive
                                                   ,@IsDeleted
                                                   ,@CreatedAt
                                                   ,@UpdatedAt); ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, termInfoEntity));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, termInfoEntity));
            }

        }


        public async Task<FAQ> GetFaqById(long id, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[Detail]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                  FROM [dbo].[FAQ]
                                  WHERE Id=@Id;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<FAQ>(query,
                        new
                        {
                            Id = id
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<FAQ>(query,
                        new
                        {
                            Id = id
                        })).FirstOrDefault();
            }
        }
        public async Task<PrivacyPolicy> GetPrivacyPolicyById(long id, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[Detail]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                  FROM [dbo].[PrivacyPolicy]
                                  WHERE Id=@Id;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<PrivacyPolicy>(query,
                        new
                        {
                            Id = id
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<PrivacyPolicy>(query,
                        new
                        {
                            Id = id
                        })).FirstOrDefault();
            }
        }
        public async Task<TermAndCondition> GetTermAndConditionById(long id, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[Detail]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                  FROM [dbo].[TermAndCondition]
                                  WHERE Id=@Id;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<TermAndCondition>(query,
                        new
                        {
                            Id = id
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<TermAndCondition>(query,
                        new
                        {
                            Id = id
                        })).FirstOrDefault();
            }
        }
        public async Task<int> UpdateFaqInfo(FAQ faq)
        {
            using (var dbConnection = Connection)
            {
                string query = @" UPDATE [dbo].[FAQ]
                                               SET [Detail] = @Detail
                                                  ,[UpdatedAt] = @UpdatedAt
                                             WHERE Id=@Id;";
                return (await dbConnection.ExecuteAsync(query, faq)); ;
            }
        }
        public async Task<int> UpdatePrivacyPolicyInfo(PrivacyPolicy privacyPolicy)
        {
            using (var dbConnection = Connection)
            {
                string query = @" UPDATE [dbo].[PrivacyPolicy]
                                               SET [Detail] = @Detail
                                                  ,[UpdatedAt] = @UpdatedAt
                                             WHERE Id=@Id;";
                return (await dbConnection.ExecuteAsync(query, privacyPolicy)); ;
            }
        }
        public async Task<int> UpdateTermAndConditionInfo(TermAndCondition termAndCondition)
        {
            using (var dbConnection = Connection)
            {
                string query = @" UPDATE [dbo].[TermAndCondition]
                                               SET [Detail] = @Detail
                                                  ,[UpdatedAt] = @UpdatedAt
                                             WHERE Id=@Id;";
                return (await dbConnection.ExecuteAsync(query, termAndCondition)); ;
            }
        }

        public async Task<GetContent> GetFaqByFaqId(IDbConnection exdbConnection = null)
        {
            string query = @"SELECT TOP 1 [Id]
                                      ,[Guid]
                                      ,[Detail]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                  FROM [dbo].[FAQ]
                                  ORDER BY Id Desc;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetContent>(query)).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetContent>(query)).FirstOrDefault();
            }
        }

        public async Task<GetContent> GetTandCByFaqId(IDbConnection exdbConnection = null)
        {
            string query = @"SELECT TOP 1 [Id]
                                      ,[Guid]
                                      ,[Detail]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                  FROM [dbo].[TermAndCondition]
                                  ORDER BY Id Desc;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetContent>(query)).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetContent>(query)).FirstOrDefault();
            }
        }
        public async Task<GetContent> GetPrivacyPolicyById(IDbConnection exdbConnection = null)
        {
            string query = @"SELECT TOP 1 [Id]
                                      ,[Guid]
                                      ,[Detail]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                  FROM [dbo].[PrivacyPolicy]
                                  ORDER BY Id Desc;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetContent>(query)).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetContent>(query)).FirstOrDefault();
            }
        }


        public async Task<int> SaveFaqQuestions(FaqMaster faqInfoEntity, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[FaqMaster]
                                               ([QuestionText]
                                               ,[Detail]
                                               ,[IsActive]
                                               ,[IsDeleted]
                                               ,[CreatedAt]
                                               ,[CreatedBy])
                                         VALUES
                                               (@QuestionText
                                               ,@Detail
                                               ,@IsActive
                                               ,@IsDeleted
                                               ,@CreatedAt
                                               ,@CreatedBy);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, faqInfoEntity));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, faqInfoEntity));
            }
        }

        public async Task<int> SaveFaqQuestionsDetail(FaqDetailMaster faqInfoEntity, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[FaqDetailMaster]
                                               ([FaqId]
                                               ,[Detail]
                                               ,[IsActive]
                                               ,[IsDeleted]
                                               ,[CreatedAt]
                                               ,[CreatedBy])
                                         VALUES
                                               (@FaqId
                                               ,@Detail
                                               ,@IsActive
                                               ,@IsDeleted
                                               ,@CreatedAt
                                               ,@CreatedBy);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, faqInfoEntity));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, faqInfoEntity));
            }
        }
        public async Task<List<FaqQuestionResponse>> GetFaqList(IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[QuestionText]
                                      ,[Detail]
                                  FROM [dbo].[FaqMaster]
                                  ORDER BY Id Asc;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<FaqQuestionResponse>(query)).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<FaqQuestionResponse>(query)).ToList();
            }
        }
        public async Task<List<QuestionAnswerResponse>> GetQuestionAnswerList(int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"select COUNT(FDM.Id) OVER() as TotalCount,
                                                ROW_NUMBER() OVER(ORDER BY FDM.Id Asc) AS RowNumber,
                                                FM.Id FaqId,
                                                FM.QuestionText,
                                                FDM.Id FaqDetailId,
                                                FDM.Detail,
                                                FDM.FaqId FaqIdFromDetail,
                                                FDM.CreatedAt
                                                from FaqMaster FM
                                                INNER JOIN FaqDetailMaster FDM ON FDM.FaqId=FM.Id
                                                WHERE FM.IsActive=1
                                                ORDER BY FDM.Id asc
                                                OFFSET @pageSize * (@pageNumber - 1) ROWS 
                                                FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<QuestionAnswerResponse>(query, new
                    {
                        //fromDate = fromDate,
                        //toDate = toDate,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        //searchText = searchText,
                        //status = status,
                    })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<QuestionAnswerResponse>(query, new
                {
                    //fromDate = fromDate,
                    //toDate = toDate,
                    pageNumber = pageNumber,
                    pageSize = pageSize,
                    //searchText = searchText,
                    //status = status,
                })).ToList();
            }
        }

        public async Task<FAQResponse> FAQ(int faqId, IDbConnection exdbConnection = null)
        {
            string query = @"select 
                                Id,
                                QuestionText,
                                Detail
                                from FAQMaster
                                 WHERE Id=@faqId
                                 order by Id Asc";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<FAQResponse>(query, new { faqId = faqId })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<FAQResponse>(query, new { faqId = faqId })).FirstOrDefault();
            }
        }
        public async Task<List<FaqDetailResponse>> FAQAnswers(int FaqId, IDbConnection exdbConnection = null)
        {
            string query = @"select 
                                    Id,
                                    Detail,FaqId
                                    from [FaqDetailMaster] where FaqId=@FaqId And IsActive=1 And IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<FaqDetailResponse>(query, new
                    {
                        FaqId = FaqId
                    })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<FaqDetailResponse>(query, new
                {
                    FaqId = FaqId
                })).ToList();
            }
        }

        public async Task<int> DeleteFAQ(int faqId, IDbConnection exdbConnection = null)
        {
            string query = @"DELETE FROM FaqMaster WHERE Id=@Id";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, new { Id = faqId }));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, new { Id = faqId }));
            }
        }
        public async Task<int> DeleteFAQAnswers(int faqId, IDbConnection exdbConnection = null)
        {
            string query = @"DELETE FROM FaqDetailMaster WHERE FaqId=@Id";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, new { Id = faqId }));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, new { Id = faqId }));
            }
        }

        public async Task<FaqMaster> GetFAQForUpdate(int FaqId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[QuestionText]
                                      ,[Detail]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                      ,[UpdatedBy]
                                      ,[CreatedBy]
                                  FROM [dbo].[FaqMaster]
                                  WHERE Id=@FaqId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<FaqMaster>(query, new
                    {
                        FaqId = FaqId
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<FaqMaster>(query, new
                {
                    FaqId = FaqId
                })).FirstOrDefault();
            }
        }
        public async Task<FaqDetailMaster> GetFAQAnswerForUpdate(int FaqDetailId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                  ,[Guid]
                                  ,[FaqId]
                                  ,[Detail]
                                  ,[IsActive]
                                  ,[IsDeleted]
                                  ,[CreatedAt]
                                  ,[UpdatedAt]
                                  ,[UpdatedBy]
                                  ,[CreatedBy]
                              FROM [dbo].[FaqDetailMaster]
                              WHERE Id=@FaqDetailId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<FaqDetailMaster>(query, new
                    {
                        FaqDetailId = FaqDetailId
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<FaqDetailMaster>(query, new
                {
                    FaqDetailId = FaqDetailId
                })).FirstOrDefault();
            }
        }

        public async Task<int> UpdateFAQ(FaqMaster faqMaster, IDbConnection exdbConnection = null)
        {
            string query = @"UPDATE [dbo].[FaqMaster]
                                       SET [QuestionText] = @QuestionText
                                          ,[Detail] = @Detail
                                     WHERE Id=@Id";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, faqMaster));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, faqMaster));
            }
        }
        public async Task<int> UpdateFAQDetail(FaqDetailMaster faqMaster, IDbConnection exdbConnection = null)
        {
            string query = @"UPDATE [dbo].[FaqDetailMaster]
                                           SET [Detail] = @Detail
                                         WHERE Id=@Id";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, faqMaster));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, faqMaster));
            }
        }
    }
}
