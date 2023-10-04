using PayMasta.Service.User;
using PayMasta.ViewModel.BillHistory;
using PayMasta.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Admin.Controllers
{
    public class BillPamentHistoryController : Controller
    {
        private IUserService _userService;
        public BillPamentHistoryController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: BillPamentHistory
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetBillHistoryList(GetBillHistoryListRequest request)
        {
            var result = new GetBillHistoryListReponse();

            try
            {
                result = await _userService.GetBillHistoryList(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
    }
}