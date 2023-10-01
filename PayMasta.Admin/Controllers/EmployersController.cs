using PayMasta.Admin.Models;
using PayMasta.Service.Employer;
using PayMasta.Service.User;
using PayMasta.Service.Withdrawals;
using PayMasta.ViewModel.EmployerVM;
using PayMasta.ViewModel.EWAVM;
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
    // [SessionExpireFilter]
    // [CustomAuthorize(Roles = "Admin")]
    public class EmployersController : Controller
    {
        private IEmployerService _employerService;
        private IUserService _userService;
        private IWithdrawalsService _withdrawalsService;
        public EmployersController(IEmployerService employerService, IUserService userService, IWithdrawalsService withdrawalsService)
        {
            _employerService = employerService;
            _userService = userService;
            _withdrawalsService = withdrawalsService;
        }

        // GET: Employers
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetEmployerProfile(string id)
        {
            return View();
        }
        [HttpPost]

        public async Task<JsonResult> GetEmployerList(GetEmployerListRequest request)
        {
            var result = new EmployerListReponse();

            try
            {
                result = await _employerService.GetEmployerList(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        [HttpPost]

        public async Task<JsonResult> GetEmployerProfile(Guid id)
        {
            var result = new EmployerReponse();

            try
            {
                var req = new GetEmployerProfileRequest
                {
                    EmployerGuid = id
                };
                result = await _employerService.GetEmployerProfile(req);
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
                res = await _userService.BlockUnBlockEmployer(blockUnBlockEmployeeRequest);

            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }
            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> GetEmployeesList(GetEmployeesListRequest request)
        {
            var result = new EmployeesReponse();

            try
            {
                result = await _employerService.GetEmployeesByEmployerGuid(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        public ActionResult GetEmployeesList(string id)
        {
            return View();
        }
        public ActionResult GetEmployeeProfile(string id)
        {
            return View();
        }
        public ActionResult GetEmployeeWithdrawals(string id)
        {
            return View();
        }
        public ActionResult GetEmployeeSalaryStructure(string id)
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetEmployeeEarningDetailByUserGuid(EmployeesEWAWithdrawlsRequest request)
        {
            var result = new EmployeeEwaDetailReponse();

            try
            {
                result = await _employerService.GetEmployeesEwaRequestDetail(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// first API for user detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        public async Task<JsonResult> GetEmployeesWithdrwals(EmployeesWithdrawlsRequest request)
        {
            var result = new EmployeesWithdrawlsResponse();

            try
            {

                result = await _employerService.GetEmployeesWithdrwals(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> ExportCsvReport(GetEmployerListRequest request)
        {
            // int langId = AppUtils.GetLangId(Request);
            string filename = "PayMastaLog";
            MemoryStream memoryStream = null;
            FileContentResult robj;
            memoryStream = await _employerService.ExportEmployerListReport(request);
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

        [HttpPost]
        public async Task<JsonResult> ExportCsvReportForEmployees(GetEmployeesListRequest request)
        {
            // int langId = AppUtils.GetLangId(Request);
            string filename = "PayMastaLog";
            MemoryStream memoryStream = null;
            FileContentResult robj;
            memoryStream = await _employerService.ExportEmployeesListReport(request);
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