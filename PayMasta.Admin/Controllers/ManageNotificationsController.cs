using PayMasta.Admin.Models;
using PayMasta.Service.ManageNotifications;
using PayMasta.Service.User;
using PayMasta.ViewModel.ManageNotificationsVM;
using PayMasta.ViewModel.User;
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
    public class ManageNotificationsController : Controller
    {
        private IUserService _userService;
        private IManageNotificationsService _manageNotificationsService;
        public ManageNotificationsController(IUserService userService, IManageNotificationsService manageNotificationsService)
        {
            _userService = userService;
            _manageNotificationsService = manageNotificationsService;
        }
        // GET: ManageNotifications
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ManageNotification()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetEmployeesList(GetEmployeesListRequest request)
        {
            var result = new EmployeesReponse();

            try
            {
                result = await _userService.GetEmployeesListForNotification(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> SendNotification(SendNotificationsRequest request)
        {
            var result = new SendNotificationsResponse();

            try
            {
                result = await _manageNotificationsService.SendNotification(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
    }
}