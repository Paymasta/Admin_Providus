using PayMasta.Admin.Models;
using PayMasta.Service.Support;
using PayMasta.ViewModel.SupportVm;
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
    public class SupportController : Controller
    {
        private ISupportService _supportService;
        public SupportController(ISupportService supportService)
        {
            _supportService = supportService;

        }
        // GET: Support
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewSupportTicket(string id)
        {
            return View();
        }
        [HttpPost]

        public async Task<JsonResult> GetSupportTicketList(SupportTicketRequest request)
        {
            var result = new UserViewTicketListReponse();

            try
            {
                result = await _supportService.GetSupportTicketList(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        [HttpPost]

        public async Task<JsonResult> UpdateSupportTicketStatus(UpdateSupportTicketStatusRequest request)
        {
            var result = new UpdateSupportTicketStatusResponse();

            try
            {
                result = await _supportService.UpdateSupportTicketStatus(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);

        }

        public async Task<JsonResult> ViewSupportTicketDetail(ViewSupportTicketDetailRequest request)
        {
            var result = new ViewSupportTicketDetailResponse();

            try
            {
                result = await _supportService.GetSupportTicketDetailByUserId(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }


        [HttpPost]
        public async Task<JsonResult> ExportCsvReportForEmployees(SupportTicketRequest request)
        {
            // int langId = AppUtils.GetLangId(Request);
            string filename = "PayMastaLog";
            MemoryStream memoryStream = null;
            FileContentResult robj;
            memoryStream = await _supportService.ExportUserListReport(request);
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