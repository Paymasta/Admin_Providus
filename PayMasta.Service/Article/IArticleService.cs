using PayMasta.ViewModel;
using PayMasta.ViewModel.ManageCategoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Article
{
    public interface IArticleService
    {
        Task<ApiResponseVM<ArticleViewModel>> GetArticleById(long articleId);
        Task<ArticleListResponse> GetArticleList();
        Task<ApiResponseVM<Object>> SaveArticle(ArticleViewModel request);
    }
}
