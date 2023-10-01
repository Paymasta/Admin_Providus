using PayMasta.Admin.Models;
using PayMasta.Service.Withdrawals;
using PayMasta.ViewModel.EWAVM;
using PayMasta.ViewModel.ExpressWalletVM;
using PayMasta.ViewModel.WithdrawlsVM;
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
    public class WithdrawalsController : Controller
    {
        private IWithdrawalsService _withdrawalsService;
        // private IUserService _userService;
        public WithdrawalsController(IWithdrawalsService withdrawalsService)
        {
            _withdrawalsService = withdrawalsService;
        }

        // GET: Withdrawals
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<JsonResult> GetEmployeesEWARequestList(AccessAmountRequest request)
        {
            var result = new AccessAmountViewModelResponse();

            try
            {
                result = await _withdrawalsService.GetEmployeesEwaRequestList(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        public ActionResult GetEmployeeEarningDetailByUserGuid(string id, string AccessAmountId)
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetEmployeeEarningDetailByUserGuid(AccessAmountViewDetailRequest request)
        {
            var result = new AccessAmountViewDetailResponse();

            try
            {
                result = await _withdrawalsService.GetEmployeesEwaRequestDetail(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }


        [HttpPost]
        public async Task<JsonResult> ExportCsvReportForEmployees(AccessAmountRequest request)
        {
            // int langId = AppUtils.GetLangId(Request);
            string filename = "PayMastaLog";
            MemoryStream memoryStream = null;
            FileContentResult robj;
            memoryStream = await _withdrawalsService.ExportEmployeesListReport(request);
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

        public async Task<JsonResult> ProvidusFundTransfer(ProvidusFundTransferRequest request)
        {
            // var result = new FundTransferResponse();
            var result = new ExpressFundTransferResponse();

            try
            {
                result = await _withdrawalsService.FundTransferInExpressWallet(request);
                //  result = await _withdrawalsService.FundTransferInWallet(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]

        public async Task<JsonResult> RejectSystemSpecsTransfer(ProvidusFundTransferRequest request)
        {
            var result = new FundTransferResponse();

            try
            {
                result = await _withdrawalsService.RejectSystemSpecsTransfer(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
    }
}