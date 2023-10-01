using PayMasta.Admin.Models;
using PayMasta.Service.UpdateProfileRequest;
using PayMasta.ViewModel.UpdateProfileRequestVM;
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
    public class UpdateProfileRequestController : Controller
    {
        private IUpdateProfileRequestService _updateProfileRequestService;
        public UpdateProfileRequestController(IUpdateProfileRequestService updateProfileRequestService)
        {
            _updateProfileRequestService = updateProfileRequestService;

        }
        // GET: UpdateProfileRequest
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public async Task<JsonResult> GetEmployeeList(UpdateProfileListRequest request)
        {
            var result = new UpdateProfileResponse();

            try
            {
                result = await _updateProfileRequestService.GetUpdateProfileRequestList(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        public ActionResult ViewUpdateProfileDetail(string id)
        {
            return View();
        }
        [HttpPost]

        public async Task<JsonResult> GetOldProfileDetail(GetOldProfileDetailRequest request)
        {
            var result = new GetOldProfileDetailResponse();

            try
            {
                result = await _updateProfileRequestService.GetOldProfileDetail(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetNewProfileDetail(GetOldProfileDetailRequest request)
        {
            var result = new GetOldProfileDetailResponse();

            try
            {
                result = await _updateProfileRequestService.GetNewProfileDetail(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateEmployeeProfileByAdmin(UpdateProfileDetailRequest request)
        {
            var result = new UpdateUserProfileResponse();

            try
            {
                result = await _updateProfileRequestService.UpdateEmployeeProfileByAdmin(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> DeleteProfileRequest(DeleteProfileDetailRequest request)
        {
            var result = new UpdateUserProfileResponse();

            try
            {
                result = await _updateProfileRequestService.DeleteProfileRequest(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> ExportCsvReportForEmployees(UpdateProfileListRequest request)
        {
            // int langId = AppUtils.GetLangId(Request);
            string filename = "PayMastaLog";
            MemoryStream memoryStream = null;
            FileContentResult robj;
            memoryStream = await _updateProfileRequestService.ExportEmployeesListReport(request);
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