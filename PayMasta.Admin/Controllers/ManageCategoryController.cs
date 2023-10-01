using PayMasta.Admin.Models;
using PayMasta.Service.ManageCategory;
using PayMasta.ViewModel.ManageCategoryVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Admin.Controllers
{
    //[SessionExpireFilter]
    //[CustomAuthorize(Roles = "Admin")]
    public class ManageCategoryController : Controller
    {
        private IManageCategoryService _manageCategoryService;

        public ManageCategoryController(IManageCategoryService manageCategoryService)
        {
            _manageCategoryService = manageCategoryService;
        }

        // GET: ManageCategory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewManageCotegory(string id)
        {
            return View();
        }
        public ActionResult AddManageCotegory()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetCategoryList(GetCategoryListRequest request)
        {
            var result = new ManageCategoriesReponse();

            try
            {
                result = await _manageCategoryService.GetCategoryList(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> BlockUnBlockCategory(BlockUnBlockCategoryRequest request)
        {
            var result = new BlockUnBlockCategoryResponse();

            try
            {
                result = await _manageCategoryService.BlockUnBlockCategory(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> ViewCategoryDetail(GetCategoryDetailRequest request)
        {
            var result = new GetCategoryDetail();

            try
            {
                result = await _manageCategoryService.ViewCategoryDetail(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateCategoryDetail(UpdateCategoryDetailRequest request)
        {
            var result = new UpdateCategoryDetailResponse();

            try
            {
                result = await _manageCategoryService.UpdateCategoryDetail(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AddCategories(AddCategoryDetailRequest request)
        {
            var result = new UpdateCategoryDetailResponse();

            try
            {
                result = await _manageCategoryService.AddCategoryAndSubCategory(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetCategories(Guid Id)
        {
            var result = new GetCategoryResponse();

            try
            {
                result = await _manageCategoryService.GetCategories();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        public async Task<JsonResult> ExportCsvReportCategory(GetCategoryListRequest request)
        {
            // int langId = AppUtils.GetLangId(Request);
            string filename = "PayMastaLog";
            MemoryStream memoryStream = null;
            FileContentResult robj;
            memoryStream = await _manageCategoryService.ExportGetCategoryListtReport(request);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(memoryStream.ToArray())
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue
                      ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content.Headers.ContentDisposition =
                   new ContentDispositionHeaderValue("attachment")
                   {
                       FileName = $"{filename}_{DateTime.Now.Ticks.ToString()}.xls"
                   };
            //response.Content.Headers.ContentLength = stream.Length;
            memoryStream.WriteTo(memoryStream);
            memoryStream.Close();
            robj = File(memoryStream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet, "TeamMembers.xlsx");
            return Json(robj, JsonRequestBehavior.AllowGet);
        }
    }
}