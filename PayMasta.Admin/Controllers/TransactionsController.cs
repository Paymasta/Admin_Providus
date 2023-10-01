using PayMasta.Admin.Models;
using PayMasta.Service.Transactions;
using PayMasta.ViewModel.TransactionsVM;
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
    //[SessionExpireFilter]
    //[CustomAuthorize(Roles = "Admin")]
    public class TransactionsController : Controller
    {
        private ITransactionsService _transactionsService;
      
        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService= transactionsService;
        }
        // GET: Transactions
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetEmployeDetailByGuid(string id)
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetEmployeesList(GetEmployeesListRequest request)
        {
            var result = new EmployeesReponse();

            try
            {
                result = await _transactionsService.GetEmployeesListForTransactions(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> GetEmployeDetailByGuid(EmployeeDetailRequest request)
        {
            var result = new EmployeeTransactionDetailResponse();

            try
            {
                result = await _transactionsService.GetEmployeDetailByGuid(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> GetEmployeeTransactionByGuid(EmployeeTransactionRequest request)
        {
            var result = new TransactionsResponse();

            try
            {
                result = await _transactionsService.GetEmployeeTransactionByGuid(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> ExportCsvReportForEmployees(GetEmployeesListRequest request)
        {
            // int langId = AppUtils.GetLangId(Request);
            string filename = "PayMastaLog";
            MemoryStream memoryStream = null;
            FileContentResult robj;
            memoryStream = await _transactionsService.ExportEmployeesListReport(request);
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