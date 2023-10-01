using PayMasta.Admin.Models;
using PayMasta.Service.Home;
using PayMasta.ViewModel.Home;
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
    public class HomeController : Controller
    {
        private IDashboardService _dashboardService;
        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [HttpPost]
        //  [SessionExpireFilter]
        public async Task<JsonResult> GetDashboardData(DashboardRequest request)
        {
            var res = new DashboardResponse();
            try
            {
                res = await _dashboardService.GetDashboardData(request);
            }
            catch (Exception ex)
            {

            }
            return Json(res);
        }
        public ActionResult AdminProfile()
        {
            return View();
        }

    }
}