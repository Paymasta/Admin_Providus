using Newtonsoft.Json;
using PayMasta.Admin.Models;
using PayMasta.Service.Account;
using PayMasta.Service.ThirdParty;
using PayMasta.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PayMasta.Admin.Controllers
{
    /// <summary>
    /// AccountController
    /// </summary>
    public class AccountController : Controller
    {
        private IAccountService _accountService;
        private IThirdParty _thirdParty;
        public AccountController(IAccountService accountService, IThirdParty thirdParty)
        {
            _accountService = accountService;
            _thirdParty = thirdParty;
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]

        public async Task<JsonResult> Login(LoginRequest request)
        {
            var res = new LoginResponse();
            List<string> listUserRoles = new List<string>();
            try
            {
                if (!string.IsNullOrWhiteSpace(request.Email) || !string.IsNullOrWhiteSpace(request.Password) || !string.IsNullOrWhiteSpace(request.DeviceToken) || !string.IsNullOrWhiteSpace(request.DeviceId))
                {
                    if (ModelState.IsValid)
                    {
                        res = await _accountService.Login(request);
                        if (res.RstKey == 1)
                        {
                            if (res.RoleId == 1)
                            {
                                listUserRoles.Add("Admin");
                            }
                            CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel
                            {
                                UserId = res.UserId,
                                FirstName = res.FirstName,
                                RoleId = res.RoleId,
                                Token = res.Token,
                                roles = listUserRoles.ToArray()
                            };

                            Session["User"] = res;

                            string userData = JsonConvert.SerializeObject(serializeModel);
                            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, res.FirstName, DateTime.Now, DateTime.Now.AddMinutes(10), false, userData);
                            string encTicket = FormsAuthentication.Encrypt(authTicket);
                            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                            //if (authTicket.IsPersistent)
                            //{
                            //    faCookie.Expires = authTicket.Expiration;
                            //}
                            Response.Cookies.Add(faCookie);
                        }
                    }

                    //var jsonResult = JsonConvert.SerializeObject(result);
                }
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> ForgotPassword(ForgotPasswordRequest request)
        {
            string res = string.Empty;

            try
            {
                if (!string.IsNullOrWhiteSpace(request.EmailorPhone) && request.Type > 0)
                {
                    res = await _accountService.ForgotPassword(request);
                }
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> ChangePassword(ChangePasswordRequest request)
        {
            int result = 0;
            result = await _accountService.ChangePassword(request);
            if (result == 1)
            {
                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();
            }
            else
            {

            }
            //if (ModelState.IsValid)
            //{
            //    result = await _accountService.ChangePassword(request);
            //}
            //else
            //{
            //    result = 2;
            //}
            return Json(result);
        }

        [HttpPost]
        /*[SessionExpireFilter]*/
        public async Task<JsonResult> MyProfile(string guid)
        {
            var res = new UserModel();
            var id = Guid.Parse(guid.ToString());
            res = await _accountService.GetProfile(id);
            return Json(res);
        }

        [HttpPost]
        //[SessionExpireFilter]
        public JsonResult Logout(LogoutRequest request)
        {
            int res = 0;
            if (request.UserGuid != null)
            {
                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();
                res = 1;
            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> ResetPassword(ResetPasswordRequest request)
        {
            var res = new OtpResponse();

            try
            {
                if (!string.IsNullOrWhiteSpace(request.EmailorPhone) || !string.IsNullOrWhiteSpace(request.OtpCode) || !string.IsNullOrWhiteSpace(request.Password))
                {
                    res = await _accountService.ResetPassword(request);
                }
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        //[HttpPost]
        //public async Task<JsonResult> ResendOtp(ResendOTPRequest request)
        //{
        //    int result = 0;

        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            result = await _accountService.ResendOTP(request);

        //        }
        //    }

        //    catch (Exception ex)
        //    {

        //    }
        //    return Json(result);
        //}

        [HttpPost]
        public async Task<JsonResult> VerifyForgetPasswordOTP(VerifyForgetPasswordOTPRequest request)
        {
            var result = new LoginResponse();

            try
            {
                if (!string.IsNullOrWhiteSpace(request.EmailorPhone) || !string.IsNullOrWhiteSpace(request.OtpCode) || request.Type > 0)
                {
                    //if (ModelState.IsValid)
                    //{
                    result = await _accountService.VerifyForgetPasswordOTP(request);
                    // }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(result);
        }

        [HttpPost]
        /*[SessionExpireFilter]*/
        public async Task<JsonResult> UpdateAdminProfile(UpdateAdminProfileRequest request)
        {
            var res = new UpdateAdminProfileResponse();
            try
            {
                res = await _accountService.UpdateAdminProfile(request);
            }
            catch (Exception ex)
            {

            }
            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> UploadFiles(HttpPostedFileBase file, string guid)
        {
            string fileName = "";
            var res = new UploadProfileImageResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        var id = Guid.Parse(guid.ToString());
                        string path = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        fileName = Guid.NewGuid().ToString("n") + "." + file.FileName.Split('.')[1];
                        var imageUrl = await _thirdParty.UploadFiles(path, fileName);
                        if (imageUrl != null)
                        {
                            var req = new UploadProfileImageRequest { ImageUrl = imageUrl, UserGuid = id };
                            res = await _accountService.UploadProfileImage(req);

                        }
                        string path1 = Server.MapPath("~/UploadedFiles/" + file.FileName);
                        FileInfo file1 = new FileInfo(path1);
                        if (file1.Exists)//check file exsit or not  
                        {
                            file1.Delete();

                        }
                    }
                }
                catch (Exception ex)
                {
                    res.RstKey = 3;
                    res.Message = ex.Message;
                }
            }
            return Json(res);
        }
    }
}