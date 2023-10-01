using PayMasta.Entity;
using PayMasta.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Article
{
    public interface IArticleRepository
    {
        Task<int> InsertArticle(ArticleMaster entity, IDbConnection exdbConnection = null);
        Task<int> UpdateArticle(ArticleMaster entity, IDbConnection exdbConnection = null);
        Task<ArticleMaster> GetById(long id, IDbConnection exdbConnection = null);
        Task<List<ArticleResponseVM>> GetArticleList(IDbConnection exdbConnection = null);
        Task<ArticleViewModel> GetArticleById(long id);
    }
}
