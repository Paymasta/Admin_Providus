using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PayMasta.Entity.Notifications;
using PayMasta.Repository.Account;
using PayMasta.Repository.ManageNotifications;
using PayMasta.Repository.Support;
using PayMasta.Utilities;
using PayMasta.Utilities.PushNotification;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.SupportVm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Support
{
    public class SupportService : ISupportService
    {
        private readonly ISupportRepository _supportRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPushNotification _pushNotification;
        private readonly IManageNotificationsRepository _manageNotificationsRepository;
        private HSSFWorkbook _hssfWorkbook;
        public SupportService()
        {
            _supportRepository = new SupportRepository();
            _accountRepository = new AccountRepository();
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
        public async Task<UserViewTicketListReponse> GetSupportTicketList(SupportTicketRequest request)
        {
            var res = new UserViewTicketListReponse();

            try
            {
                var employeeList = await _supportRepository.GetSupportTicketList(request.UserType, request.SearchText, request.PageNumber, request.PageSize, request.FromDate, request.ToDate, request.StatusId);
                if (employeeList.Count > 0)
                {
                    res.supportViewModels = employeeList;
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


        public async Task<UpdateSupportTicketStatusResponse> UpdateSupportTicketStatus(UpdateSupportTicketStatusRequest request)
        {
            var res = new UpdateSupportTicketStatusResponse();
            string msg = string.Empty;
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.AdminUserGuid);
                var ticketDetail = await _supportRepository.GetTicketStatusByTicketId(request.TicketId);
                var employeeData = await _accountRepository.GetSessionByUserId(ticketDetail.UserId); ;
                if (ticketDetail != null)
                {
                    if (request.Status == (int)EnumSupportStatus.InProgess)
                    {
                        ticketDetail.Status = (int)EnumSupportStatus.InProgess;
                        msg = "Your support ticket (" + ticketDetail.TicketNumber + ") is inprogress.";
                    }
                    else if (request.Status == (int)EnumSupportStatus.Closed)
                    {
                        ticketDetail.Status = (int)EnumSupportStatus.Closed;
                        msg = "Your ticket (" + ticketDetail.TicketNumber + ") has been resolved.";
                    }
                    else if (request.Status == (int)EnumSupportStatus.Hold)
                    {
                        ticketDetail.Status = (int)EnumSupportStatus.Hold;
                        msg = "Your ticket (" + ticketDetail.TicketNumber + ") is on hold.";
                    }
                    else if (request.Status == (int)EnumSupportStatus.Reject)
                    {
                        ticketDetail.Status = (int)EnumSupportStatus.Reject;
                        msg = "Your ticket (" + ticketDetail.TicketNumber + ") has been rejected.";
                    }

                    ticketDetail.UpdatedAt = DateTime.UtcNow;
                    ticketDetail.UpdatedBy = userData.Id;
                    ticketDetail.IsActive = true;
                    ticketDetail.IsDeleted = false;

                    if (await _supportRepository.UpdateTicketStatus(ticketDetail) > 0)
                    {
                        try
                        {
                            var req = new Notifications
                            {
                                AlterMessage = msg,
                                DeviceToken = string.Empty,
                                DeviceType = 1,
                                CreatedAt = DateTime.Now,
                                CreatedBy = userData.Id,
                                IsActive = true,
                                IsDeleted = false,
                                IsDelivered = true,
                                IsRead = false,
                                NotificationType = 1,
                                SenderId = userData.Id,
                                ReceiverId = Convert.ToInt32(ticketDetail.UserId),
                                UpdatedAt = DateTime.Now,
                                UpdatedBy = userData.Id,
                                NotificationJson = string.Empty,
                            };
                            await _manageNotificationsRepository.InsertNotification(req);

                            var pushModel = new PushModel
                            {
                                DeviceToken = employeeData.DeviceToken,
                                Title = "Support",
                                Message = msg
                            };
                            _pushNotification.SendPush(pushModel);
                        }
                        catch (Exception ex)
                        {

                        }

                        res.IsSuccess = true;
                        res.RstKey = 1;
                        res.Message = ResponseMessages.DATAUPDATED;
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.RstKey = 2;
                        res.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                    }

                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<ViewSupportTicketDetailResponse> GetSupportTicketDetailByUserId(ViewSupportTicketDetailRequest request)
        {
            var res = new ViewSupportTicketDetailResponse();

            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var employeeTicketDetail = await _supportRepository.GetSupportTicketDetailByUserId(userData.Id, request.SupportTicketId);
                if (employeeTicketDetail != null)
                {
                    res.supportViewModel = employeeTicketDetail;
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

        public async Task<MemoryStream> ExportUserListReport(SupportTicketRequest request)
        {
            InitializeWorkbook();
            if (request.UserType == 4)
            {
                await GenerateEmployeesData(request);
            }
            else
            {
                await GenerateEmployersData(request);
            }

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
        private async Task GenerateEmployeesData(SupportTicketRequest request)
        {

            try
            {
                ICellStyle style1 = _hssfWorkbook.CreateCellStyle();
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style1.FillPattern = FillPattern.SolidForeground;

                var id = Guid.Parse(request.UserGuid.ToString());
                //var result = await _employerRepository.GetEmployerDetailByGuid(id);
                var response = await _supportRepository.GetSupportTicketListForCsv(request.UserType, request.FromDate, request.ToDate, request.StatusId);

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
                C02.SetCellValue("Company Name");
                C02.CellStyle = style1;

                var C03 = R0.CreateCell(3);
                C03.SetCellValue("Email Id");
                C03.CellStyle = style1;

                var C04 = R0.CreateCell(4);
                C04.SetCellValue("Phone Number");
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

        private async Task GenerateEmployersData(SupportTicketRequest request)
        {

            try
            {
                ICellStyle style1 = _hssfWorkbook.CreateCellStyle();
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style1.FillPattern = FillPattern.SolidForeground;

                var id = Guid.Parse(request.UserGuid.ToString());
                //var result = await _employerRepository.GetEmployerDetailByGuid(id);
                var response = await _supportRepository.GetSupportTicketListForCsv(request.UserType, request.FromDate, request.ToDate, request.StatusId);

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
                C01.SetCellValue("Company Name");
                C01.CellStyle = style1;

                var C02 = R0.CreateCell(2);
                C02.SetCellValue("Email Id");
                C02.CellStyle = style1;

                var C03 = R0.CreateCell(3);
                C03.SetCellValue("Phone Number");
                C03.CellStyle = style1;

                var C04 = R0.CreateCell(4);
                C04.SetCellValue("Status");
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
                    C2.SetCellValue(item.Email);

                    var c3 = row.CreateCell(3);
                    c3.SetCellValue(item.CountryCode.ToString() + "-" + item.PhoneNumber.ToString());

                    var c4 = row.CreateCell(4);
                    c4.SetCellValue(item.Status);

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

