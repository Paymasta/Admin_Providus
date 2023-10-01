using PayMasta.Service.Article;
using PayMasta.Service.ManageCms;
using PayMasta.ViewModel;
using PayMasta.ViewModel.ManageCategoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Admin.Controllers
{
    public class ManageArticleController : Controller
    {
        private IArticleService  _articleService;

        public ManageArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        // GET: ManageArticle
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SaveArtical()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SaveArticle(ArticleViewModel request)
        {
            var result = new ApiResponseVM<Object>();

            try
            {
                result = await _articleService.SaveArticle(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetArticleById(long articleId)
        {
            var result = new ApiResponseVM<ArticleViewModel>();

            try
            {
                result = await _articleService.GetArticleById(articleId);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetArticleList()
        {
            var result = new ArticleListResponse();

            try
            {
                result = await _articleService.GetArticleList();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
    }
}