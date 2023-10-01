using PayMasta.Entity;
using PayMasta.Repository.Article;
using PayMasta.Repository.ManageCategory;
using PayMasta.ViewModel;
using PayMasta.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Article
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService()
        {
            _articleRepository = new ArticleRepository();
        }

        public async Task<ApiResponseVM<ArticleViewModel>> GetArticleById(long articleId)
        {
            var result = new ApiResponseVM<ArticleViewModel>();

            result.Result = await _articleRepository.GetArticleById(articleId);

            return result;
        }

        public async Task<ArticleListResponse> GetArticleList()
        {
            var result = new ArticleListResponse();
            result.IsSuccess = true;
            result.RstKey = 1;
            result.Result = await _articleRepository.GetArticleList();
            return result;
        }

        public async Task<ApiResponseVM<Object>> SaveArticle(ArticleViewModel request)
        {
            var result = new ApiResponseVM<Object>();

            if (request.ArticleId == 0)
            {
                var entity = new ArticleMaster
                {
                    ArticleText = request.ArticleText,
                    PriceMoney = request.PriceMoney,
                    Option1Text = request.Option1Text,
                    Option2Text = request.Option2Text,
                    Option3Text = request.Option3Text,
                    Option4Text = request.Option4Text,
                    CorrectOption = request.CorrectOption,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                if (await _articleRepository.InsertArticle(entity) > 0)
                {
                    result.IsSuccess = true;
                    result.RstKey = 1;
                    result.Message = ResponseMessages.DATA_SAVED;
                }
            }
            else
            {
                var entity = await _articleRepository.GetById(request.ArticleId);
                if (entity != null)
                {
                    entity.ArticleText = request.ArticleText;
                    entity.PriceMoney = request.PriceMoney;
                    entity.Option1Text = request.Option1Text;
                    entity.Option2Text = request.Option2Text;
                    entity.Option3Text = request.Option3Text;
                    entity.Option4Text = request.Option4Text;
                    entity.CorrectOption = request.CorrectOption;
                    entity.UpdatedAt = DateTime.UtcNow;

                    if (await _articleRepository.UpdateArticle(entity) > 0)
                    {
                        result.IsSuccess = true;
                        result.RstKey = 2;
                        result.Message = ResponseMessages.DATA_SAVED;
                    }
                }
            }
            return result;
        }
    }
}
