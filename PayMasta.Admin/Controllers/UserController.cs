using PayMasta.Admin.Models;
using PayMasta.Service.User;
using PayMasta.ViewModel.User;
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
    //M3
    //[SessionExpireFilter]
    //[CustomAuthorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetEmployeesList(GetEmployeesListRequest request)
        {
            var result = new EmployeesReponse();

            try
            {
                result = await _userService.GetEmployeesList(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        public ActionResult GetEmployeDetailByGuid(string id)
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetEmployeDetailByGuid(Guid id)
        {
            var result = new EmployeeDetailResponse();

            try
            {
                var req = new EmployeeDetailRequest
                {
                    UserGuid = id
                };
                result = await _userService.GetEmployeDetailByGuid(req);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }


        [HttpPost]
        public async Task<JsonResult> BlockUnBlockEmployees(BlockUnBlockEmployeeRequest blockUnBlockEmployeeRequest)
        {
            var res = new BlockUnBlockEmployeeResponse();
            try
            {
                res = await _userService.BlockUnBlockEmployees(blockUnBlockEmployeeRequest);

            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }
            return Json(res);
        }
        public ActionResult GetEmployeeWithdrawals(string id)
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ExportCsvReportForEmployees(GetEmployeesListRequest request)
        {
            // int langId = AppUtils.GetLangId(Request);
            string filename = "PayMastaLog";
            MemoryStream memoryStream = null;
            FileContentResult robj;
            memoryStream = await _userService.ExportEmployeesListReport(request);
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