using PayMasta.Entity.Faq;
using PayMasta.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using PayMasta.Entity;
using PayMasta.ViewModel;
using System.Data.Common;

namespace PayMasta.Repository.Article
{
    public class ArticleRepository : IArticleRepository
    {
        private string connectionString;

        public ArticleRepository()
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
        public async Task<int> InsertArticle(ArticleMaster entity, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[ArticleMaster2]
                               ([ArticleText]
                               ,[PriceMoney]
                               ,[IsActive]
                               ,[IsDeleted]
                               ,[CreatedAt]
                               ,[UpdatedAt]
                               ,[CreatedBy]
                               ,[UpdatedBy]
                               ,[Option1Text]
                               ,[Option2Text]
                               ,[Option3Text]
                               ,[Option4Text]
                               ,[CorrectOption])
                         VALUES
                               (@ArticleText
                               ,@PriceMoney
                               ,@IsActive
                               ,@IsDeleted
                               ,@CreatedAt
                               ,@UpdatedAt
                               ,@CreatedBy
                               ,@UpdatedBy
                               ,@Option1Text
                               ,@Option2Text
                               ,@Option3Text
                               ,@Option4Text
                               ,@CorrectOption);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, entity));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, entity));
            }
        }

        public async Task<int> UpdateArticle(ArticleMaster entity, IDbConnection exdbConnection = null)
        {
            string query = @"UPDATE [dbo].[ArticleMaster2]
                               SET [ArticleText] = @ArticleText
                                  ,[PriceMoney] = @PriceMoney
                                  ,[UpdatedAt] = @UpdatedAt
                                  ,[UpdatedBy] = @UpdatedBy
                                  ,[Option1Text] = @Option1Text
                                  ,[Option2Text] = @Option2Text
                                  ,[Option3Text] = @Option3Text
                                  ,[Option4Text] = @Option4Text
                                  ,[CorrectOption] = @CorrectOption
                             WHERE Id=@Id;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, entity));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, entity));
            }
        }

        public async Task<ArticleMaster> GetById(long id, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                  ,[Guid]
                                  ,[ArticleText]
                                  ,[PriceMoney]
                                  ,[IsActive]
                                  ,[IsDeleted]
                                  ,[CreatedAt]
                                  ,[UpdatedAt]
                                  ,[CreatedBy]
                                  ,[UpdatedBy]
                                  ,[Option1Text]
                                  ,[Option2Text]
                                  ,[Option3Text]
                                  ,[Option4Text]
                                  ,[CorrectOption]
                              FROM [dbo].[ArticleMaster2]
                              WHERE Id=@Id;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<ArticleMaster>(query,
                        new
                        {
                            Id = id
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<ArticleMaster>(query,
                        new
                        {
                            Id = id
                        })).FirstOrDefault();
            }
        }

        public async Task<List<ArticleResponseVM>> GetArticleList(IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id] AS ArticleId
                                  ,[ArticleText]
                                  ,[PriceMoney]
                                  ,[IsActive]
                                  ,[IsDeleted]
                                  ,[CreatedAt]
                                  ,[UpdatedAt]
                                  ,[CreatedBy]
                                  ,[UpdatedBy]
                                  ,[Option1Text]
                                  ,[Option2Text]
                                  ,[Option3Text]
                                  ,[Option4Text]
                                  ,[CorrectOption]
                              FROM [dbo].[ArticleMaster2];";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<ArticleResponseVM>(query)).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<ArticleResponseVM>(query)).ToList();
            }
        }

        public async Task<ArticleViewModel> GetArticleById(long id)
        {
            string query = @"SELECT [Id] AS ArticleId
                                  ,[ArticleText]
                                  ,[PriceMoney]
                                  ,[IsActive]
                                  ,[IsDeleted]
                                  ,[CreatedAt]
                                  ,[UpdatedAt]
                                  ,[CreatedBy]
                                  ,[UpdatedBy]
                                  ,[Option1Text]
                                  ,[Option2Text]
                                  ,[Option3Text]
                                  ,[Option4Text]
                                  ,[CorrectOption]
                              FROM [dbo].[ArticleMaster2]
                              WHERE Id=@Id;";
           
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<ArticleViewModel>(query,
                        new
                        {
                            Id = id
                        })).FirstOrDefault();
                }
            
        }
    }
}
