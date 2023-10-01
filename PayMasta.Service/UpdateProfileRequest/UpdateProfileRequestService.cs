using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PayMasta.Entity.UpdateUserProfileRequest;
using PayMasta.Repository.Account;
using PayMasta.Repository.UpdateProfileRequest;
using PayMasta.Service.ManageNotifications;
using PayMasta.Utilities;
using PayMasta.Utilities.EmailUtils;
using PayMasta.Utilities.PushNotification;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.ManageNotificationsVM;
using PayMasta.ViewModel.UpdateProfileRequestVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.UpdateProfileRequest
{
    public class UpdateProfileRequestService : IUpdateProfileRequestService
    {
        private readonly IUpdateProfileRequestRepository _updateProfileRequestRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPushNotification _pushNotification;
        private readonly IEmailUtils _emailUtils;
        private readonly IManageNotificationsService _manageNotificationsService;

        private HSSFWorkbook _hssfWorkbook;
        public UpdateProfileRequestService()
        {
            _accountRepository = new AccountRepository();
            _updateProfileRequestRepository = new UpdateProfileRequestRepository();
            _pushNotification = new PushNotification();
            _emailUtils = new EmailUtils();
            _manageNotificationsService = new ManageNotificationsService();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        public async Task<UpdateProfileResponse> GetUpdateProfileRequestList(UpdateProfileListRequest request)
        {
            var res = new UpdateProfileResponse();
            var result = new List<UpdateProfileRequestResponse>();
            try
            {
                //var req = new PushModel
                //{
                //    DeviceToken = "exCTaKPsT7q2Ek4ex3gmUP:APA91bG-rd77mDRbZjJ8ef7aX-e00pNeRWlZMEpHMgf6Xvc4KDGmqnGS7uLyL7y0QHUhyegZcwF1kffy4X3WSVNatyOi_s2KPNCQ1VumeeQo37Y1lCubDimrxys2nPskAnym8aECaUzH",
                //    Title = "PayMasta",
                //    Message = "Hi how are you?android"
                //};

                //var noti = _pushNotification.SendPush(req);

                //var req1 = new PushModel
                //{
                //    DeviceToken = "fZwZFbZkb0jQo_TUVXTb_I:APA91bGAl3xdikUKM_vj6Pisq7GLW_ykk7Y7EYXrNWb4xSlmmeu6FK2aPtJizqapMX_qAnsMhsmafXRFk6172yZbu6j5hcwdJ6BICUAi-AqTcu29Z06_f2-u6nlMmve0Gb4mrPCCyrFH",
                //    Title = "PayMasta",
                //    Message = "Hi how are you?Ios"
                //};

                //var noti1 = _pushNotification.SendPush(req1);
                //var email = await _emailUtils.SendEmailBySendGrid();
                result = await _updateProfileRequestRepository.GetUpdateProfileRequestList(request.pageNumber, request.PageSize, request.FromDate, request.Todate, request.SearchTest);
                if (result.Count > 0)
                {
                    res.updateProfileRequestResponses = result;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<GetOldProfileDetailResponse> GetOldProfileDetail(GetOldProfileDetailRequest request)
        {
            var res = new GetOldProfileDetailResponse();

            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.userGuid);
                var result = await _updateProfileRequestRepository.GetOldProfileDetail(userData.Id);
                if (result != null)
                {
                    res.updateProfileRequestResponse = result;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<GetOldProfileDetailResponse> GetNewProfileDetail(GetOldProfileDetailRequest request)
        {
            var res = new GetOldProfileDetailResponse();

            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.userGuid);
                var result = await _updateProfileRequestRepository.GetNewProfileDetail(userData.Id);
                if (result != null)
                {
                    res.updateProfileRequestResponse = result;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<UpdateUserProfileResponse> UpdateEmployeeProfileByAdmin(UpdateProfileDetailRequest request)
        {
            var res = new UpdateUserProfileResponse();

            try
            {
                var adminData = await _accountRepository.GetUserByGuid(request.AdminGuid);
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var result = await _updateProfileRequestRepository.GetNewProfileDetailForUpdateByUpdateRequestId(userData.Id, request.UpdateUserProfileRequestId);
                var userSessionData = await _accountRepository.GetSessionByUserId(userData.Id);

                if (result != null)
                {
                    userData.FirstName = result.FirstName;
                    userData.LastName = result.LastName;
                    userData.Email = result.Email;
                    userData.PhoneNumber = result.PhoneNumber;
                    userData.CountryCode = result.CountryCode;
                    userData.MiddleName = result.MiddleName;
                    userData.Address = result.Address;
                    userData.UpdatedAt = DateTime.UtcNow;
                    userData.UpdatedBy = adminData.Id;



                    if (await _updateProfileRequestRepository.UpdateUserProfileByAdmin(userData) > 0)
                    {
                        var profileRequest = new UpdateUserProfileRequest();
                        profileRequest.Status = (int)EnumProfileStatus.Verified;
                        profileRequest.Id = result.RowNumber;
                        var statusRes = await _updateProfileRequestRepository.UpdateUserProfileStatusByAdmin(profileRequest);
                        res.IsSuccess = true;
                        res.RstKey = 1;
                        res.Message = ResponseMessages.PROFILE_UPDATED;
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.RstKey = 2;
                        res.Message = ResponseMessages.PROFILE_NOT_UPDATED;
                    }

                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = ResponseMessages.REQUESTDATA_NOT_EXIST;
                }

                if (res.RstKey > 0 && userSessionData != null)
                {
                    //var req = new PushModel
                    //{
                    //    DeviceToken = userSessionData.DeviceToken,
                    //    Title = "Update profile request",
                    //    Message = res.Message
                    //};

                    //var noti1 = _pushNotification.SendPush(req);
                    try
                    {
                        var profileNoti = new SendNotificationsRequest
                        {
                            AdminGuid = request.AdminGuid,
                            NotificatiomMessage = res.Message,
                            UserIds = userData.Id.ToString(),
                        };
                        await _manageNotificationsService.SendNotification(profileNoti);
                    }
                    catch (Exception ex)
                    {

                    }

                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<UpdateUserProfileResponse> DeleteProfileRequest(DeleteProfileDetailRequest request)
        {
            var res = new UpdateUserProfileResponse();

            try
            {
                var adminData = await _accountRepository.GetUserByGuid(request.AdminGuid);
                var userId = _updateProfileRequestRepository.GetUserIdByUpdateProfileRequestGuid(request.UpdateUserProfileRequestId);
                var userSessionData = await _accountRepository.GetSessionByUserId(userId);
                if (adminData != null)
                {
                    if (await _updateProfileRequestRepository.RemoveUpdateProfileRequest(request.UpdateUserProfileRequestId) > 0)
                    {
                        res.IsSuccess = true;
                        res.RstKey = 1;
                        res.Message = ResponseMessages.CONTENT_DELETED;
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.RstKey = 2;
                        res.Message = ResponseMessages.CONTENT_NOT_DELETED;
                    }

                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = ResponseMessages.REQUESTDATA_NOT_EXIST;
                }
                if (res.RstKey > 0 && userSessionData != null)
                {
                    var req = new PushModel
                    {
                        DeviceToken = userSessionData.DeviceToken,
                        Title = "Update profile request",
                        Message = "Your update profile request deleted by admin"
                    };

                    var noti1 = _pushNotification.SendPush(req);
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<MemoryStream> ExportEmployeesListReport(UpdateProfileListRequest request)
        {
            InitializeWorkbook();
            await GenerateEmployeesData(request);
            return GetExcelStream();
        }

        MemoryStream GetExcelStream()
        {
            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            _hssfWorkbook.Write(file);
            return file;
        }
        void InitializeWorkbook()
        {
            _hssfWorkbook = new HSSFWorkbook();

            ////create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "NPOI Team";
            _hssfWorkbook.DocumentSummaryInformation = dsi;

            ////create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "NPOI SDK Example";
            _hssfWorkbook.SummaryInformation = si;
        }
        private async Task GenerateEmployeesData(UpdateProfileListRequest request)
        {

            try
            {
                ICellStyle style1 = _hssfWorkbook.CreateCellStyle();
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style1.FillPattern = FillPattern.SolidForeground;

                var id = Guid.Parse(request.userGuid.ToString());
                //var result = await _employerRepository.GetEmployerDetailByGuid(id);
                var response = await _updateProfileRequestRepository.GetUpdateProfileRequestListForScv(request.FromDate, request.Todate);

                ISheet sheet1 = _hssfWorkbook.CreateSheet("PayMastaEmployeeListLog");
                sheet1.SetColumnWidth(0, 1500);
                sheet1.SetColumnWidth(1, 8000);
                sheet1.SetColumnWidth(2, 4000);
                sheet1.SetColumnWidth(3, 8000);
                sheet1.SetColumnWidth(4, 8000);
                sheet1.SetColumnWidth(5, 8000);

                //----------Create Header-----------------
                var R0 = sheet1.CreateRow(0);

                var C00 = R0.CreateCell(0);
                C00.SetCellValue("S.No");
                C00.CellStyle = style1;

                var C01 = R0.CreateCell(1);
                C01.SetCellValue("First Name");
                C01.CellStyle = style1;

                var C02 = R0.CreateCell(2);
                C02.SetCellValue("Last Name");
                C02.CellStyle = style1;

                var C03 = R0.CreateCell(3);
                C03.SetCellValue("Email Id");
                C03.CellStyle = style1;

                var C04 = R0.CreateCell(4);
                C04.SetCellValue("Phone Number");
                C04.CellStyle = style1;

                var C05 = R0.CreateCell(5);
                C05.SetCellValue("Date");
                C05.CellStyle = style1;


                int i = 1;
                foreach (var item in response)
                {

                    IRow row = sheet1.CreateRow(i);

                    var C0 = row.CreateCell(0);
                    C0.SetCellValue(item.RowNumber);

                    var C1 = row.CreateCell(1);
                    C1.SetCellValue(item.FirstName);

                    var C2 = row.CreateCell(2);
                    C2.SetCellValue(item.LastName);

                    var c3 = row.CreateCell(3);
                    c3.SetCellValue(item.Email);

                    var c4 = row.CreateCell(4);
                    c4.SetCellValue(item.CountryCode.ToString() + "-" + item.PhoneNumber.ToString());

                    var c5 = row.CreateCell(5);
                    c5.SetCellValue(item.CreatedAt.ToString());


                    i++;
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
