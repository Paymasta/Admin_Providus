using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PayMasta.Entity.Notifications;
using PayMasta.Repository.ManageNotifications;
using PayMasta.Repository.User;
using PayMasta.Service.ManageNotifications;
using PayMasta.Utilities;
using PayMasta.Utilities.PushNotification;
using PayMasta.ViewModel.BillHistory;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.ManageNotificationsVM;
using PayMasta.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPushNotification _pushNotification;
        private readonly IManageNotificationsRepository _manageNotificationsRepository;
        private HSSFWorkbook _hssfWorkbook;
        public UserService()
        {
            _userRepository = new UserRepository();
            _pushNotification = new PushNotification();
            _manageNotificationsRepository = new ManageNotificationsRepository();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }
        public async Task<EmployeesReponse> GetEmployeesList(GetEmployeesListRequest request)
        {
            var res = new EmployeesReponse();

            try
            {
                int pendingEmp = -1;
                if (request.Status == 3)
                {
                    pendingEmp = 0;
                    request.Status = -1;
                }
                if (request.Status == 1)
                {
                    pendingEmp = 1;
                }

                var employeesList = await _userRepository.GetEmployeesList(pendingEmp,request.pageNumber, request.PageSize, request.Status, request.FromDate, request.ToDate, request.SearchTest);
                if (employeesList.Count > 0)
                {
                    res.employeesListViewModel = employeesList;
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
        public async Task<EmployeesReponse> GetEmployeesListForNotification(GetEmployeesListRequest request)
        {
            var res = new EmployeesReponse();

            try
            {

                var employeesList = await _userRepository.GetEmployeesListForNotification(request.pageNumber, request.PageSize, request.Status, request.FromDate, request.ToDate, request.SearchTest);
                if (employeesList.Count > 0)
                {
                    res.employeesListViewModel = employeesList;
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
        public async Task<EmployeeDetailResponse> GetEmployeDetailByGuid(EmployeeDetailRequest request)
        {
            var res = new EmployeeDetailResponse();

            try
            {

                var employeceDetail = await _userRepository.GetEmployeeDetailByGuid(request.UserGuid);
                if (employeceDetail != null)
                {
                    res.employeeDetail = employeceDetail;
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

        public async Task<BlockUnBlockEmployeeResponse> BlockUnBlockEmployees(BlockUnBlockEmployeeRequest request)
        {
            var res = new BlockUnBlockEmployeeResponse();
            var result = new GetAdminDetailResponse();
            try
            {
                var employeerId = Guid.Parse(request.AdminUserGuid.ToString());
                var employeeId = Guid.Parse(request.EmployeeUserGuid.ToString());
                result = await _userRepository.GetAdminDetailByGuid(employeerId);
                var userData = await _userRepository.GetUserByGuid(employeeId);
                var usersession = await _userRepository.GetSessionByUserId(userData.Id);
                var bankData = await _userRepository.GetBankDetailByUserId(userData.Id);
                if (userData != null)
                {
                    if (request.DeleteOrBlock == 1)
                    {
                        if (userData.Status == 1)
                        {
                            userData.Status = 0;
                            userData.UpdatedAt = DateTime.UtcNow;
                            userData.UpdatedBy = result.Id;
                            res.Message = AdminResponseMessages.USER_BLOCKED;

                            var sesReq = new LogoutRequest
                            {
                                DeviceId = "",
                                UserGuid = employeeId
                            };
                            await UpdateSession(sesReq, AdminResponseMessages.DEACTIVATED_SUBADMIN, AdminResponseMessages.DEACTIVATED_SUBADMIN);
                        }
                        else
                        {
                            userData.Status = 1;
                            userData.UpdatedAt = DateTime.UtcNow;
                            userData.UpdatedBy = result.Id;
                            res.Message = AdminResponseMessages.USER_UNBLOCKED;
                        }
                        var req = new Notifications
                        {
                            AlterMessage = res.Message,
                            DeviceToken = string.Empty,
                            DeviceType = 1,
                            CreatedAt = DateTime.Now,
                            CreatedBy = result.Id,
                            IsActive = true,
                            IsDeleted = false,
                            IsDelivered = true,
                            IsRead = false,
                            NotificationType = 1,
                            SenderId = result.Id,
                            ReceiverId = Convert.ToInt32(userData.Id),
                            UpdatedAt = DateTime.Now,
                            UpdatedBy = result.Id,
                            NotificationJson = string.Empty,
                        };
                        await _manageNotificationsRepository.InsertNotification(req);
                        try
                        {
                            var pushReq = new PushModel
                            {
                                DeviceToken = usersession.DeviceToken,
                                Message = res.Message,
                                Title = "Your account status",

                            };
                            _pushNotification.SendPush(pushReq);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    if (request.DeleteOrBlock == 2)
                    {
                        userData.Status = 0;
                        userData.IsActive = false;
                        userData.IsDeleted = true;
                        userData.UpdatedAt = DateTime.UtcNow;
                        userData.UpdatedBy = result.Id;
                        res.Message = AdminResponseMessages.USER_DELETED;
                        var sesReq = new LogoutRequest
                        {
                            DeviceId = "",
                            UserGuid = employeeId
                        };
                        await UpdateSession(sesReq, AdminResponseMessages.DELETED_SUBADMIN, AdminResponseMessages.DELETED_SUBADMIN);
                    }
                    if (await _userRepository.UpdateUser(userData) > 0)
                    {
                        res.IsSuccess = true;
                        res.RstKey = 1;

                        if (bankData.Count > 0)
                        {
                            bankData.ForEach(async x => {
                                x.IsActive = false;
                                x.IsDeleted = true;
                                x.UpdatedAt = DateTime.UtcNow;

                                await _userRepository.DeleteBankByBankDetailId(x);
                            });
                        }
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.RstKey = 2;
                        res.Message = AdminResponseMessages.USER_NOT_FOUND;
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = AdminResponseMessages.USER_NOT_FOUND;
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<BlockUnBlockEmployeeResponse> BlockUnBlockEmployer(BlockUnBlockEmployeeRequest request)
        {
            var res = new BlockUnBlockEmployeeResponse();
            var result = new GetAdminDetailResponse();
            var employeesByEmployerid = new List<EmployeesListViewModel>();
            try
            {
                var employeerId = Guid.Parse(request.AdminUserGuid.ToString());
                var employeeId = Guid.Parse(request.EmployeeUserGuid.ToString());
                result = await _userRepository.GetAdminDetailByGuid(employeerId);
                var userData = await _userRepository.GetUserByGuid(employeeId);
                var employerDetail = await _userRepository.GetEmployerDetailByGuid(employeeId);
                var bankData = await _userRepository.GetBankDetailByUserId(userData.Id);
                if (employerDetail != null) { employeesByEmployerid = await _userRepository.GetEmployeesListByEmployerId(employerDetail.Id); }
                if (userData != null)
                {
                    if (request.DeleteOrBlock == 1)
                    {
                        if (userData.Status == 1)
                        {
                            userData.Status = 0;
                            userData.UpdatedAt = DateTime.UtcNow;
                            userData.UpdatedBy = result.Id;
                            res.Message = AdminResponseMessages.USER_BLOCKED;

                            var sesReq = new LogoutRequest
                            {
                                DeviceId = "",
                                UserGuid = employeeId
                            };
                            await UpdateSession(sesReq, AdminResponseMessages.DEACTIVATED_SUBADMIN, AdminResponseMessages.DEACTIVATED_SUBADMIN);
                        }
                        else
                        {
                            userData.Status = 1;
                            userData.UpdatedAt = DateTime.UtcNow;
                            userData.UpdatedBy = result.Id;
                            res.Message = AdminResponseMessages.USER_UNBLOCKED;
                        }
                        var req = new Notifications
                        {
                            AlterMessage = res.Message,
                            DeviceToken = string.Empty,
                            DeviceType = 1,
                            CreatedAt = DateTime.Now,
                            CreatedBy = result.Id,
                            IsActive = true,
                            IsDeleted = false,
                            IsDelivered = true,
                            IsRead = false,
                            NotificationType = 1,
                            SenderId = result.Id,
                            ReceiverId = Convert.ToInt32(userData.Id),
                            UpdatedAt = DateTime.Now,
                            UpdatedBy = result.Id,
                            NotificationJson = string.Empty,
                        };
                        await _manageNotificationsRepository.InsertNotification(req);


                        if (employeesByEmployerid.Count > 0)
                        {
                            employeesByEmployerid.ForEach(async x =>
                            {
                                var usersession = await _userRepository.GetSessionByUserId(x.UserId);

                                var userReq = new Notifications
                                {
                                    AlterMessage = res.Message,
                                    DeviceToken = string.Empty,
                                    DeviceType = 1,
                                    CreatedAt = DateTime.Now,
                                    CreatedBy = result.Id,
                                    IsActive = true,
                                    IsDeleted = false,
                                    IsDelivered = true,
                                    IsRead = false,
                                    NotificationType = 1,
                                    SenderId = result.Id,
                                    ReceiverId = Convert.ToInt32(x.UserId),
                                    UpdatedAt = DateTime.Now,
                                    UpdatedBy = result.Id,
                                    NotificationJson = string.Empty,
                                };
                                await _manageNotificationsRepository.InsertNotification(userReq);
                                try
                                {
                                    var pushReq = new PushModel
                                    {
                                        DeviceToken = usersession.DeviceToken,
                                        Message = res.Message,
                                        Title = "Your employer account status",
                                    };
                                    _pushNotification.SendPush(pushReq);
                                }
                                catch (Exception ex)
                                {

                                }

                            });
                        }
                    }
                    if (request.DeleteOrBlock == 2)
                    {
                        userData.Status = 0;
                        userData.IsActive = false;
                        userData.IsDeleted = true;
                        userData.UpdatedAt = DateTime.UtcNow;
                        userData.UpdatedBy = result.Id;
                        res.Message = AdminResponseMessages.USER_DELETED;
                        var sesReq = new LogoutRequest
                        {
                            DeviceId = "",
                            UserGuid = employeeId
                        };
                        await UpdateSession(sesReq, AdminResponseMessages.DELETED_SUBADMIN, AdminResponseMessages.DELETED_SUBADMIN);

                        if (employeesByEmployerid.Count > 0)
                        {
                            employeesByEmployerid.ForEach(async x =>
                            {
                                var usersession = await _userRepository.GetSessionByUserId(x.UserId);

                                var userReq = new Notifications
                                {
                                    AlterMessage = res.Message,
                                    DeviceToken = string.Empty,
                                    DeviceType = 1,
                                    CreatedAt = DateTime.Now,
                                    CreatedBy = result.Id,
                                    IsActive = true,
                                    IsDeleted = false,
                                    IsDelivered = true,
                                    IsRead = false,
                                    NotificationType = 1,
                                    SenderId = result.Id,
                                    ReceiverId = Convert.ToInt32(x.UserId),
                                    UpdatedAt = DateTime.Now,
                                    UpdatedBy = result.Id,
                                    NotificationJson = string.Empty,
                                };
                                await _manageNotificationsRepository.InsertNotification(userReq);
                            });

                        }
                    }
                    if (await _userRepository.UpdateUser(userData) > 0)
                    {
                        res.IsSuccess = true;
                        res.RstKey = 1;

                        if (bankData.Count > 0)
                        {
                            bankData.ForEach(async x => {
                                x.IsActive = false;
                                x.IsDeleted = true;
                                x.UpdatedAt = DateTime.UtcNow;

                                await _userRepository.DeleteBankByBankDetailId(x);
                            });
                        }
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.RstKey = 2;
                        res.Message = AdminResponseMessages.USER_NOT_FOUND;
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = AdminResponseMessages.USER_NOT_FOUND;
                }
            }
            catch (Exception ex)
            {
            }
            return res;
        }
        private async Task<int> UpdateSession(LogoutRequest request, string Title, string Message)
        {
            var result = 0;
            long userId = _userRepository.GetUserIdByGuid(request.UserGuid);
            if (userId > 0)
            {
                var session = await _userRepository.GetSessionByUserId(userId);
                if (session != null)
                {
                    session.IsActive = false;
                    session.IsDeleted = true;
                    session.UpdatedAt = DateTime.UtcNow;
                    result = await _userRepository.UpdateSession(session);
                    try
                    {
                        var pushModel = new PushModel
                        {
                            DeviceToken = session.DeviceToken,
                            Title = Title,
                            Message = Message
                        };
                        _pushNotification.SendPush(pushModel);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return result;
        }


        public async Task<MemoryStream> ExportEmployeesListReport(GetEmployeesListRequest request)
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
        private async Task GenerateEmployeesData(GetEmployeesListRequest request)
        {

            try
            {
                int pendingemp = -1;
                if (request.Status == 3)
                {
                    pendingemp = 0;
                    request.Status = -1;
                }
                ICellStyle style1 = _hssfWorkbook.CreateCellStyle();
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style1.FillPattern = FillPattern.SolidForeground;

                var id = Guid.Parse(request.userGuid.ToString());
                //var result = await _employerRepository.GetEmployerDetailByGuid(id);
                var response = await _userRepository.GetEmployeesListForCsv(pendingemp,request.Status, request.FromDate, request.ToDate);

                ISheet sheet1 = _hssfWorkbook.CreateSheet("PayMastaEmployeeListLog");
                sheet1.SetColumnWidth(0, 1500);
                sheet1.SetColumnWidth(1, 8000);
                sheet1.SetColumnWidth(2, 4000);
                sheet1.SetColumnWidth(3, 8000);
                sheet1.SetColumnWidth(4, 8000);
                sheet1.SetColumnWidth(5, 8000);
                sheet1.SetColumnWidth(6, 8000);
                //----------Create Header-----------------
                var R0 = sheet1.CreateRow(0);

                var C00 = R0.CreateCell(0);
                C00.SetCellValue("S.No");
                C00.CellStyle = style1;

                var C01 = R0.CreateCell(1);
                C01.SetCellValue("Name");
                C01.CellStyle = style1;

                var C02 = R0.CreateCell(2);
                C02.SetCellValue("Employer Name");
                C02.CellStyle = style1;

                var C03 = R0.CreateCell(3);
                C03.SetCellValue("Email");
                C03.CellStyle = style1;

                var C04 = R0.CreateCell(4);
                C04.SetCellValue("MobileNo");
                C04.CellStyle = style1;

                var C05 = R0.CreateCell(5);
                C05.SetCellValue("Status");
                C05.CellStyle = style1;

                var C06 = R0.CreateCell(6);
                C06.SetCellValue("Date");
                C06.CellStyle = style1;

                int i = 1;
                foreach (var item in response)
                {

                    IRow row = sheet1.CreateRow(i);

                    var C0 = row.CreateCell(0);
                    C0.SetCellValue(item.RowNumber);

                    var C1 = row.CreateCell(1);
                    C1.SetCellValue(item.FirstName + " " + item.LastName);

                    var C2 = row.CreateCell(2);
                    C2.SetCellValue(item.EmployerName);

                    var c3 = row.CreateCell(3);
                    c3.SetCellValue(item.Email);

                    var c4 = row.CreateCell(4);
                    c4.SetCellValue(item.CountryCode.ToString() + "-" + item.PhoneNumber.ToString());

                    var c5 = row.CreateCell(5);
                    c5.SetCellValue(item.Status.ToString());

                    var c6 = row.CreateCell(6);
                    c6.SetCellValue(item.CreatedAt.ToString());
                    i++;
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<GetBillHistoryListReponse> GetBillHistoryList(GetBillHistoryListRequest request)
        {
            var res = new GetBillHistoryListReponse();

            try
            {
                int pendingEmp = -1;
                if (request.Status == 3)
                {
                    pendingEmp = 0;
                    request.Status = -1;
                }
                if (request.Status == 1)
                {
                    pendingEmp = 1;
                }

                var employeesList = await _userRepository.GetBillHistoryList(pendingEmp, request.pageNumber, request.PageSize, request.Status, request.FromDate, request.ToDate, request.SearchTest);
                if (employeesList.Count > 0)
                {
                    res.getBillHistoryLists = employeesList;
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
    }
}
