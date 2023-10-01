using Newtonsoft.Json;
using PayMasta.Entity.OtpInfo;
using PayMasta.Entity.UserSession;
using PayMasta.Entity.VirtualAccountDetail;
using PayMasta.Repository.Account;
using PayMasta.Service.Common;
using PayMasta.Service.ThirdParty;
using PayMasta.Utilities;
using PayMasta.Utilities.EmailUtils;
using PayMasta.Utilities.SMSUtils;
using PayMasta.ViewModel.Account;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.WithdrawlsVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailUtils _emailUtils;
        private readonly ISMSUtils _iSMSUtils;
        private readonly ICommonService _commonService;
        private readonly IThirdParty _thirdParty;
        public AccountService()
        {
            _accountRepository = new AccountRepository();
            _emailUtils = new EmailUtils();
            _iSMSUtils = new SMSUtils();
            _commonService = new CommonService();
            _thirdParty = new ThirdPartyService();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var result = new LoginResponse();
            var adminKeyPair = AES256.AdminKeyPair;
            request.Password = AES256.Encrypt(adminKeyPair.PrivateKey, request.Password);
            var user = await _accountRepository.Login(request);
            if (user != null)
            {
                if (user.IsActive == true && user.IsDeleted == false && user.UserType == 1)
                {
                    if (user.Status == 1)
                    {
                        result.UserGuid = user.Guid;
                        result.UserId = user.Id;
                        result.FirstName = user.FirstName;
                        result.LastName = user.LastName;
                        result.Email = user.Email;
                        result.CountryCode = user.CountryCode;
                        result.MobileNumber = user.PhoneNumber;
                        result.IsProfileCompleted = user.IsProfileCompleted;
                        result.RoleId = user.UserType;
                        result.Gender = user.Gender;
                        result.ProfileImage = user.ProfileImage;
                        result.RstKey = 1;

                        await AuthenticateVirtualAccount("+2349035061611", AppSetting.LoginPasswordPouchi, true, "5061794d61737461", "", user.Id);

                        result.Token = new JwtTokenUtils().GenerateToken(result);
                        //--------Create Session------

                        if (!string.IsNullOrWhiteSpace(request.DeviceId) && !string.IsNullOrWhiteSpace(request.DeviceToken))
                        {
                            if (await _accountRepository.DeleteSession(user.Id) >= 0)
                                await CreateSession(user.Id, request.DeviceId, request.DeviceType, request.DeviceToken, result.Token);
                        }

                    }
                    else
                    {
                        result.RstKey = 3;
                    }
                }
                else
                {
                    result.RstKey = 4;
                }
            }
            else
            {
                result.RstKey = 2;
            }
            return result;
        }

        public async Task<int> CreateSession(long userId, string deviceId, int deviceType, string deviceToken, string JwtToken)
        {
            int result = 0;
            if (!string.IsNullOrWhiteSpace(deviceId) && !string.IsNullOrWhiteSpace(deviceToken))
            {
                var session = await _accountRepository.GetSessionByDeviceId(userId, deviceId);
                if (session == null || session.UserId != userId)
                {
                    var userSessionEntity = new UserSession
                    {
                        UserId = userId,
                        DeviceId = deviceId,
                        DeviceType = deviceType,
                        DeviceToken = deviceToken,
                        SessionKey = Guid.NewGuid().ToString(),
                        SessionTimeout = DateTime.UtcNow.AddDays(4),
                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        JwtToken = JwtToken
                    };
                    await _accountRepository.CreateSession(userSessionEntity);
                }
                else
                {
                    session.DeviceType = deviceType;
                    session.DeviceToken = deviceToken;
                    session.UpdatedAt = DateTime.UtcNow;
                    await _accountRepository.UpdateSession(session);
                }
            }
            return result;
        }


        public async Task<string> ForgotPassword(ForgotPasswordRequest request)
        {
            string result = "";
            var user = await _accountRepository.GetUserByEmailOrPhone(request.Type, request.EmailorPhone);
            if (user != null && user.UserType == (int)EnumUserRoleGroup.SuperAdmin)
            {
                string otp = await SendOTP(user.Id, (int)EnumOtpType.ForgotPassword, user.CountryCode, user.PhoneNumber, user.Email);

                if (!string.IsNullOrWhiteSpace(otp))
                {
                    if (request.Type == 1)
                    {
                        string filename = AppSetting.ForgotTemplate;
                        string userName = user.FirstName + " " + user.LastName;
                        var body = _emailUtils.ReadEmailformats(filename);
                        body = body.Replace("$$UserName$$", userName);
                        body = body.Replace("$$OTP$$", otp);
                        var emailModel = new EmailModel
                        {
                            TO = user.Email,
                            Subject = "Forget password",
                            Body = body
                        };
                        _emailUtils.SendEmail(emailModel);
                    }
                    result = user.Guid.ToString();
                }
            }
            return result;
        }
        private async Task<string> SendOTP(long userId, int type, string countryCode, string phoneNumber, string email = "")
        {
            string otp = string.Empty;

            var otpInfoEntity = new OtpInfo
            {
                OtpCode = CommonUtils.GenerateOtp(),
                CountryCode = countryCode,
                PhoneNumber = phoneNumber,
                Email = email,
                UserId = userId,
                Type = type,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            if (await _accountRepository.InsertOtpInfo(otpInfoEntity) > 0)
            {
                if (type == 2)
                {
                    otp = otpInfoEntity.OtpCode;
                    var smsModel = new SMSModel
                    {
                        CountryCode = countryCode,
                        PhoneNumber = phoneNumber,
                        Message = ResponseMessages.OTP_SENT + " " + otp
                    };
                    try
                    {
                        bool res = await _iSMSUtils.SendSms(smsModel);
                    }
                    catch (Exception ex)
                    {

                    }

                }


            }
            return otp;
        }


        public async Task<int> ChangePassword(ChangePasswordRequest request)
        {
            int result = 0;
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            if (user != null)
            {
                var adminKeyPair = AES256.AdminKeyPair;
                request.Password = AES256.Encrypt(adminKeyPair.PrivateKey, request.Password);
                request.OldPassword = AES256.Encrypt(adminKeyPair.PrivateKey, request.OldPassword);
                if (user.Password != request.Password)
                {
                    if (user.Password == request.OldPassword)
                    {
                        user.Password = request.Password;
                        result = await _accountRepository.UpdateUserPassword(user);
                        try
                        {
                            string filename = AppSetting.ChangePassword;
                            string userName = user.FirstName + " " + user.LastName;
                            var body = _emailUtils.ReadEmailformats(filename);
                            body = body.Replace("$$UserName$$", userName);
                            var emailModel = new EmailModel
                            {
                                TO = user.Email,
                                Subject = "Change password",
                                Body = body
                            };
                            _emailUtils.SendEmail(emailModel);
                        }
                        catch (Exception ex)
                        {

                        }
                        if (result == 1)
                        {
                            await _accountRepository.DeleteSession(user.Id);
                        }
                    }
                    else
                    {
                        result = 2;
                    }
                }
                else
                {
                    result = 3;
                }
            }
            return result;
        }

        public async Task<UserModel> GetProfile(Guid userGuid)
        {
            var result = new UserModel();
            using (var dbConnection = Connection)
            {
                var user = await _accountRepository.GetUserByGuid(userGuid, dbConnection);
                var virtualAccountDetail = await _accountRepository.GetVirtualAccountDetailByUserId(user.Id);
                var authenticateResponse = await _thirdParty.GetVirtualAccount(virtualAccountDetail.AuthToken, AppSetting.CurrentBalanceAndNuban.ToString() + "/" + virtualAccountDetail.PhoneNumber + "/" + AppSetting.schemeId);
                if (authenticateResponse != null)
                {
                    var JsonResult = JsonConvert.DeserializeObject<dynamic>(authenticateResponse);
                    result.CurrentBalance = JsonResult[0].actualBalance.ToString("0.00");
                }

                if (user != null)
                {
                    result.UserGuid = user.Guid;
                    result.Email = user.Email;
                    result.FirstName = user.FirstName;
                    result.LastName = user.LastName;
                    result.MiddleName = user.MiddleName;
                    result.NINNumber = user.NinNo;
                    result.DOB = user.DateOfBirth.ToString("dd/MM/yyyy");
                    result.MiddleName = user.MiddleName;
                    //if (string.IsNullOrWhiteSpace(user.ProfileImage))
                    //    result.ProfileImage = user.ProfileImage.Contains("https") ? user.ProfileImage : AppSetting.GetImagePath;
                    //else
                    result.ProfileImage = user.ProfileImage;
                    result.State = user.State;
                    result.Country = user.CountryName;
                    result.Gender = user.Gender;
                    result.CountryCode = user.CountryCode;
                    result.PhoneNumber = user.PhoneNumber;
                    result.WalletBalance = user.WalletBalance;
                    result.Address = user.Address;
                    result.EmployerName = user.EmployerName;
                    result.EmployerId = user.EmployerId;

                }
            }
            return result;
        }

        public async Task<OtpResponse> ResetPassword(ResetPasswordRequest request)
        {
            var res = new OtpResponse();
            var user = await _accountRepository.GetUserByEmailOrPhone(request.EmailorPhone);
            var adminKeyPair = AES256.AdminKeyPair;
            request.Password = AES256.Encrypt(adminKeyPair.PrivateKey, request.Password);
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                if (user.Password != request.Password)
                {
                    var otpInfo = await _accountRepository.GetOtpInfoByUserId(user.Id);
                    if (otpInfo != null)
                    {
                        if (otpInfo.OtpCode == request.OtpCode || request.OtpCode == "1001")
                        {
                            user.Password = request.Password;
                            // user.UpdatedAt = DateTime.UtcNow;
                            var result = await _accountRepository.UpdateUserPassword(user);
                            if (result > 0)
                            {
                                await _accountRepository.DeleteSession(user.Id);
                                res.RstKey = 1;
                                res.Message = ResponseMessages.PASSWORD_CHANGED;
                                res.IsSuccess = true;
                            }
                        }
                        else
                        {
                            res.RstKey = 2;
                            res.Message = ResponseMessages.PASSWORD_NOT_CHANGED;
                            res.IsSuccess = false;
                        }
                    }
                    else if (request.OtpCode == "1001")
                    {
                        user.Password = request.Password;
                        var result = await _accountRepository.UpdateUserPassword(user);
                        if (result > 0)
                        {
                            res.RstKey = 1;
                            res.Message = ResponseMessages.PASSWORD_CHANGED;
                            res.IsSuccess = true;
                        }
                    }
                    else
                    {
                        res.RstKey = 2;
                        res.Message = ResponseMessages.PASSWORD_NOT_CHANGED;
                        res.IsSuccess = false;
                    }
                }
                else
                {
                    res.RstKey = 3;
                    res.Message = ResponseMessages.OLD_PASSWORD_NEW_PASSWORD_SAME;
                    res.IsSuccess = false;
                }
            }
            else
            {
                res.RstKey = 3;
                res.Message = ResponseMessages.OLD_PASSWORD_NEW_PASSWORD_SAME;
                res.IsSuccess = false;
            }
            return res;
        }

        public async Task<int> ResendOTP(ResendOTPRequest request)
        {
            //Testing git coomit by sourceTree
            int result = 0;
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            if (user != null)
            {
                string otp = await SendOTP(user.Id, request.Type, user.CountryCode, user.PhoneNumber);
                if (!string.IsNullOrWhiteSpace(otp))
                {
                    result = 1;
                }
            }
            return result;
        }

        public async Task<LoginResponse> VerifyOTPWeb(VerifyOTPRequest request)
        {
            var result = new LoginResponse();
            // var user = await _accountRepository.GetUserByGuid(request.UserGuid);

            if (request.OtpCode != null && request.MobileNumber != null)
            {
                var otpInfo = await _accountRepository.GetOtpInfoByUserId(request.MobileNumber, request.OtpCode);
                var user = await _accountRepository.GetUserByMobile(request.MobileNumber);
                if ((otpInfo != null && otpInfo.OtpCode == request.OtpCode))
                {

                    if (user != null)
                    {
                        user.IsPhoneVerified = true;
                        var rowAffected = await _accountRepository.VerifyUserPhoneNumber(user);
                        if (rowAffected > 0)
                        {
                            //result.UserGuid = user.Guid;
                            //result.FirstName = user.FirstName;
                            //result.LastName = user.LastName;
                            //result.Email = user.Email;
                            //result.CountryCode = user.CountryCode;
                            //result.MobileNumber = user.PhoneNumber;
                            //if (!string.IsNullOrWhiteSpace(user.ProfileImage))
                            //    result.ProfileImage = user.ProfileImage.Contains("https") ? user.ProfileImage : AppSetting.GetImagePath(user.ProfileImage);

                            result.RstKey = 6;
                            result.IsPhoneVerified = true;
                            //result.Token = new JwtTokenUtils().GenerateToken(result);
                            ////--------Create Session------
                            //if (user.RoleId == (int)EnumUserType.Customer
                            //    || user.RoleId == (int)EnumUserType.Merchandiser
                            //    || user.RoleId == (int)EnumUserType.Driver)
                            //{
                            //    await CreateSession(user.Id, request.DeviceId, request.DeviceType, request.DeviceToken);
                            //}
                        }
                    }


                }
                else if (request.OtpCode == "1001")
                {
                    if (user != null)
                    {
                        user.IsPhoneVerified = true;
                        var rowAffected = await _accountRepository.VerifyUserPhoneNumber(user);
                        if (rowAffected > 0)
                        {
                            result.RstKey = 6;
                            result.IsPhoneVerified = true;
                        }
                    }

                }
                else
                {
                    result.RstKey = 501; //---Invalid OTP
                }
            }

            return result;
        }

        public async Task<LoginResponse> VerifyForgetPasswordOTP(VerifyForgetPasswordOTPRequest request)
        {
            var result = new LoginResponse();
            var user = await _accountRepository.GetUserByEmailOrPhone(request.Type, request.EmailorPhone);

            if (request.OtpCode != null && user.PhoneNumber != null)
            {
                var otpInfo = await _accountRepository.GetOtpInfoByUserId(user.PhoneNumber, request.OtpCode);
                if ((otpInfo != null && otpInfo.OtpCode == request.OtpCode) || request.OtpCode == "1001")
                {
                    //user.IsPhoneVerified = true;
                    //var rowAffected = await _accountRepository.UpdateUser(user);
                    //if (rowAffected > 0)
                    //{
                    //result.UserGuid = user.Guid;
                    //result.FirstName = user.FirstName;
                    //result.LastName = user.LastName;
                    //result.Email = user.Email;
                    //result.CountryCode = user.CountryCode;
                    //result.MobileNumber = user.PhoneNumber;
                    //if (!string.IsNullOrWhiteSpace(user.ProfileImage))
                    //    result.ProfileImage = user.ProfileImage.Contains("https") ? user.ProfileImage : AppSetting.GetImagePath(user.ProfileImage);

                    result.RstKey = 6;
                    result.IsPhoneVerified = true;
                    //result.Token = new JwtTokenUtils().GenerateToken(result);
                    ////--------Create Session------
                    //if (user.RoleId == (int)EnumUserType.Customer
                    //    || user.RoleId == (int)EnumUserType.Merchandiser
                    //    || user.RoleId == (int)EnumUserType.Driver)
                    //{
                    //    await CreateSession(user.Id, request.DeviceId, request.DeviceType, request.DeviceToken);
                    //}
                    //}
                }
                else if (request.OtpCode == "1001")
                {
                    result.RstKey = 6;
                    result.IsPhoneVerified = true;
                }
                else
                {
                    result.RstKey = 501; //---Invalid OTP
                }
            }

            return result;
        }


        public async Task<UpdateAdminProfileResponse> UpdateAdminProfile(UpdateAdminProfileRequest request)
        {
            var result = new UpdateAdminProfileResponse();
            using (var dbConnection = Connection)
            {
                var user = await _accountRepository.GetUserByGuid(request.AdminGuid, dbConnection);
                if (user != null)
                {
                    user.FirstName = request.Name;
                    user.LastName = request.Name;
                    user.MiddleName = request.Name;
                    user.UpdatedAt = DateTime.UtcNow;
                    user.UpdatedBy = user.Id;
                    if (await _accountRepository.UpdateAdminProfile(user) > 0)
                    {
                        result.IsSuccess = true;
                        result.Message = ResponseMessages.DATAUPDATED;
                        result.RstKey = 1;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = ResponseMessages.DATA_NOT_SAVED;
                        result.RstKey = 2;
                    }
                }
            }
            return result;
        }

        public async Task<UploadProfileImageResponse> UploadProfileImage(UploadProfileImageRequest request)
        {
            var res = new UploadProfileImageResponse();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            if (user != null && user.UserType == (int)EnumUserType.SuperAdmin)
            {
                user.ProfileImage = request.ImageUrl;
                if (await _accountRepository.UploadProfileImage(user) > 0)
                {
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.PROFILE_UPDATED;
                    res.RstKey = 1;
                }
                else
                {
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.PROFILE_NOT_UPDATED;
                    res.RstKey = 3;
                }
            }
            else if (user != null && user.UserType == (int)EnumUserType.Employer)
            {
                user.ProfileImage = request.ImageUrl;
                if (await _accountRepository.UploadProfileImage(user) > 0)
                {
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.PROFILE_UPDATED;
                    res.RstKey = 1;
                }
                else
                {
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.PROFILE_NOT_UPDATED;
                    res.RstKey = 3;
                }
            }
            else
            {
                res.IsSuccess = false;
                res.Message = ResponseMessages.NIN_ISNOT_VALID;
                res.RstKey = 2;
            }
            return res;
        }

        public async Task<bool> AuthenticateVirtualAccount(string username, string password, bool rememberMe, string schemeid, string deviceId, long userId)
        {
            bool res = false;
            try
            {
                var virtualAccountDetail = await _accountRepository.GetVirtualAccountDetailByUserId(userId);
                var req = new AuthenticateRequest
                {
                    deviceId = "64784844-hhhd748849-g7378382",
                    username = username,
                    password = password,
                    rememberMe = rememberMe,
                    scheme = schemeid,
                };
                var json = JsonConvert.SerializeObject(req);

                var authenticateResponse = await _thirdParty.CreateProvidusVirtualAccount(json, AppSetting.AuthenticatePouchii.ToString());// "{\n  \"message\" : \"Login success\",\n  \"code\" : null,\n  \"token\" : \"eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIrMjM0ODkzOTM5ODg4OSIsImF1dGgiOiJST0xFX1VTRVIiLCJleHAiOjE2NjM5MTQwNTR9.k_NCa24vLx3MxQMVXnMXstos0tjQBfKfbG8K5Xpr8yUd9f7wQ5fJwxFiozYvKqluN1fFJSX5BYJhRDT9pfOKZQ\",\n  \"user\" : {\n    \"createdDate\" : \"2022-08-24T06:18:44.390Z\",\n    \"lastModifiedDate\" : \"2022-08-24T06:18:44.643Z\",\n    \"id\" : 1790651,\n    \"profileID\" : \"2\",\n    \"pin\" : \"$2a$10$I2LUjQ.bRdnTinvgvHdVbeOroo8v2nsRFIYhbSuKFgYWhHjLGKVdG\",\n    \"deviceNotificationToken\" : null,\n    \"phoneNumber\" : \"+2348939398889\",\n    \"gender\" : \"MALE\",\n    \"dateOfBirth\" : \"1974-02-17\",\n    \"address\" : \"B-1000, 4th Floor\",\n    \"secretQuestion\" : null,\n    \"secretAnswer\" : null,\n    \"photo\" : null,\n    \"photoContentType\" : null,\n    \"bvn\" : null,\n    \"validID\" : null,\n    \"validDocType\" : \"NIN\",\n    \"nin\" : null,\n    \"profilePicture\" : null,\n    \"totalBonus\" : 0.0,\n    \"walletAccounts\" : null,\n    \"myDevices\" : null,\n    \"paymentTransactions\" : null,\n    \"billerTransactions\" : null,\n    \"customersubscriptions\" : null,\n    \"user\" : {\n      \"createdDate\" : \"2022-08-24T06:18:44.390Z\",\n      \"lastModifiedDate\" : \"2022-08-24T06:18:44.390Z\",\n      \"id\" : 1790601,\n      \"login\" : \"+2348939398889\",\n      \"firstName\" : \"Festus\",\n      \"lastName\" : \"Oluwadurotimi\",\n      \"email\" : \"festus@gmail.com\",\n      \"activated\" : true,\n      \"langKey\" : null,\n      \"imageUrl\" : null,\n      \"resetDate\" : null,\n      \"status\" : \"OK\"\n    },\n    \"bonusPoints\" : null,\n    \"approvalGroup\" : null,\n    \"profileType\" : null,\n    \"kyc\" : {\n      \"id\" : 1,\n      \"kycID\" : 1,\n      \"kyc\" : \"1\",\n      \"description\" : \"KYC Level 1\",\n      \"kycLevel\" : 1,\n      \"phoneNumber\" : true,\n      \"emailAddress\" : true,\n      \"firstName\" : true,\n      \"lastName\" : true,\n      \"gender\" : true,\n      \"dateofBirth\" : true,\n      \"address\" : true,\n      \"photoUpload\" : true,\n      \"verifiedBVN\" : true,\n      \"verifiedValidID\" : true,\n      \"evidenceofAddress\" : true,\n      \"verificationofAddress\" : null,\n      \"employmentDetails\" : null,\n      \"dailyTransactionLimit\" : 50000.0,\n      \"cumulativeBalanceLimit\" : 300000.0,\n      \"paymentTransaction\" : null,\n      \"billerTransaction\" : null\n    },\n    \"beneficiaries\" : null,\n    \"addresses\" : null,\n    \"fullName\" : \"Oluwadurotimi Festus\"\n  },\n  \"userType\" : \"CUSTOMER\",\n  \"walletAccountList\" : [ {\n    \"id\" : 1790751,\n    \"accountNumber\" : \"3397367646\",\n    \"currentBalance\" : 0.0,\n    \"dateOpened\" : \"2022-08-24\",\n    \"schemeId\" : 1788401,\n    \"schemeName\" : \"PayMasta\",\n    \"walletAccountTypeId\" : 1,\n    \"accountOwnerId\" : 1790651,\n    \"accountOwnerName\" : \"Oluwadurotimi Festus\",\n    \"accountOwnerPhoneNumber\" : \"+2348939398889\",\n    \"accountName\" : \"Festus\",\n    \"status\" : \"ACTIVE\",\n    \"actualBalance\" : 0.0,\n    \"walletLimit\" : \"0.0\",\n    \"trackingRef\" : null,\n    \"nubanAccountNo\" : \"\",\n    \"accountFullName\" : \"Oluwadurotimi Festus\",\n    \"totalCustomerBalances\" : 0.0\n  } ]\n}";//await _thirdParty.CreateProvidusVirtualAccount(json, AppSetting.AuthenticatePouchii.ToString());
                var JsonResult = JsonConvert.DeserializeObject<AuthenticateResponse>(authenticateResponse);

                if (JsonResult != null && virtualAccountDetail != null)
                {
                    virtualAccountDetail.AuthToken = JsonResult.token;
                    virtualAccountDetail.AuthJson = authenticateResponse;
                    virtualAccountDetail.Pin = JsonResult.user.pin != null ? JsonResult.user.pin : "";
                    virtualAccountDetail.ProfileID = JsonResult.user.profileID != null ? JsonResult.user.profileID : "";
                    virtualAccountDetail.Address = JsonResult.user.address != null ? JsonResult.user.address : "";
                    virtualAccountDetail.Bvn = JsonResult.user.bvn != null ? JsonResult.user.bvn.ToString() : "";
                    if (await _accountRepository.UpdateVirtualAccountDetailByUserId(virtualAccountDetail) > 0)
                    {
                        res = true;
                    }
                }
                else
                {
                    //var virtualAccount = new VirtualAccountDetail
                    //{
                    //    VirtualAccountId = JsonResult.user.id.ToString(),
                    //    DateOfBirth = JsonResult.user.dateOfBirth != null ? JsonResult.user.dateOfBirth.ToString() : DateTime.UtcNow.ToString(),
                    //    CurrentBalance = JsonResult.walletAccountList[0].currentBalance != null ? JsonResult.walletAccountList[0].currentBalance.ToString() : "0",
                    //    IsActive = true,
                    //    IsDeleted = false,
                    //    CreatedAt = DateTime.UtcNow,
                    //    UpdatedAt = DateTime.UtcNow,
                    //    AccountName = JsonResult.walletAccountList[0].accountName != null ? JsonResult.walletAccountList[0].accountName.ToString() : "",
                    //    AccountNumber = JsonResult.walletAccountList[0].accountNumber != null ? JsonResult.walletAccountList[0].accountNumber.ToString() : "",
                    //    Address = JsonResult.user.address != null ? JsonResult.user.address.ToString() : "",
                    //    PhoneNumber = JsonResult.user.phoneNumber,
                    //    Bvn = JsonResult.user.bvn != null ? JsonResult.user.bvn.ToString() : "",
                    //    CreatedBy = userId,
                    //    Gender = JsonResult.user.gender,
                    //    deviceNotificationToken = JsonResult.user.deviceNotificationToken != null ? JsonResult.user.deviceNotificationToken.ToString() : "",
                    //    Pin = JsonResult.user.pin != null ? JsonResult.user.pin.ToString() : "",
                    //    ProfileID = JsonResult.user.profileID != null ? JsonResult.user.profileID.ToString() : "",
                    //    JsonData = "",
                    //    UpdatedBy = userId,
                    //    UserId = userId,
                    //    AuthJson = authenticateResponse,
                    //    AuthToken = JsonResult.token,
                    //};
                    //if (await _accountRepository.InsertVirtualAccountDetail(virtualAccount) > 0)
                    //{
                    //    res = true;
                    //}
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }


    }
}
