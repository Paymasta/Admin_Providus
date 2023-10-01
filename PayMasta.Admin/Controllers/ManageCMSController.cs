using Newtonsoft.Json;
using PayMasta.Admin.Models;
using PayMasta.Service.ManageCms;
using PayMasta.ViewModel.CMSVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Admin.Controllers
{
    //[SessionExpireFilter]
    //[CustomAuthorize(Roles = "Admin")]
    public class ManageCMSController : Controller
    {
        private IManageCmsService _manageCmsService;
        public ManageCMSController(IManageCmsService manageCmsService)
        {
            _manageCmsService = manageCmsService;
        }
        // GET: ManageCMS
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<JsonResult> SaveFaqData(CmsReqeust request)
        {
            var result = new CmsResponse();

            try
            {
                result = await _manageCmsService.SaveFaqData(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetFaqData(CmsReqeust request)
        {
            var result = new GetCmsResponse();

            try
            {
                result = await _manageCmsService.GetFaqData(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<JsonResult> SavePrivacyPolicyData(CmsReqeust request)
        {
            var result = new CmsResponse();

            try
            {
                result = await _manageCmsService.SavePrivacyPolicyData(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<JsonResult> SaveTandCData(CmsReqeust request)
        {
            var result = new CmsResponse();

            try
            {
                result = await _manageCmsService.SaveTandCData(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetTandCData(CmsReqeust request)
        {
            var result = new GetCmsResponse();

            try
            {
                result = await _manageCmsService.GetTandCData(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> GetPrivacyPolicyById(CmsReqeust request)
        {
            var result = new GetCmsResponse();

            try
            {
                result = await _manageCmsService.GetPrivacyPolicyById(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<JsonResult> SaveFaqQuestions(FaqQuestionReqeust request)
        {
            var result = new CmsResponse();

            try
            {
                result = await _manageCmsService.SaveFaqQuestions(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<JsonResult> SaveFaqQuestionsDetail(FaqQuestionDetailReqeust request)
        {
            var result = new CmsResponse();

            try
            {
                result = await _manageCmsService.SaveFaqQuestionsDetail(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> GetFaqList(CmsReqeust request)
        {
            var result = new FaqQuestion();

            try
            {
                result = await _manageCmsService.GetFaqList(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetQuestionAnswerList(QuestionAnswerRequest request)
        {
            var result = new QuestionAnswer();

            try
            {
                result = await _manageCmsService.GetQuestionAnswerList(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        public ActionResult EditQuestionAndAnswer(string id)
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetQuestionAnswerListForView(int faqId)
        {
            var result = new FAQResponse();

            try
            {
                result = await _manageCmsService.GetFaq(faqId);
            }
            catch (Exception ex)
            {

            }

            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> DeleteQuestionAnswer(int faqId)
        {
            var result = new CmsResponse();

            try
            {
                result = await _manageCmsService.DeleteQuestionAnswer(faqId);
            }
            catch (Exception ex)
            {

            }

            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> UpdateQuestionAnswer(UpdateQuestionAnswerRequest request, object updateQuestionAnswerDetails)
        {
            var result = new CmsResponse();
            UpdateQuestionAnswerDetails[] arrayDocs = JsonConvert.DeserializeObject<UpdateQuestionAnswerDetails[]>(request.UpdateQuestionAnswerDetailsString);
            try
            {
                result = await _manageCmsService.UpdateQuestionAnswer(request, arrayDocs);
            }
            catch (Exception ex)
            {

            }

            return Json(result);
        }
    }
}