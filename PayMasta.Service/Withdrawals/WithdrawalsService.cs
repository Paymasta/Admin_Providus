using Newtonsoft.Json;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PayMasta.Repository.Account;
using PayMasta.Repository.Extention;
using PayMasta.Repository.Withdrawals;
using PayMasta.Service.Common;
using PayMasta.Service.ManageNotifications;
using PayMasta.Service.ThirdParty;
using PayMasta.Utilities;
using PayMasta.Utilities.EmailUtils;
using PayMasta.Utilities.PushNotification;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.EWAVM;
using PayMasta.ViewModel.ExpressWalletVM;
using PayMasta.ViewModel.ManageNotificationsVM;
using PayMasta.ViewModel.WithdrawlsVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Withdrawals
{
    public class WithdrawalsService : IWithdrawalsService
    {
        private readonly IWithdrawalsRepository _withdrawalsRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICommonService _commonService;
        private readonly IThirdParty _thirdParty;
        private readonly IPushNotification _pushNotification;
        private readonly IEmailUtils _emailUtils;
        private readonly IManageNotificationsService _manageNotificationsService;

        private HSSFWorkbook _hssfWorkbook;
        public WithdrawalsService()
        {
            _withdrawalsRepository = new WithdrawalsRepository();
            _accountRepository = new AccountRepository();
            _commonService = new CommonService();
            _thirdParty = new ThirdPartyService();
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

        public async Task<AccessAmountViewModelResponse> GetEmployeesEwaRequestList(AccessAmountRequest request)
        {
            var res = new AccessAmountViewModelResponse();

            try
            {
                var employerEWAList = await _withdrawalsRepository.GetEmployeesEwaRequestList(request.pageNumber, request.PageSize, request.Status, request.FromDate, request.ToDate, request.SearchTest);
                if (employerEWAList.Count > 0)
                {
                    res.accessAmountViewModels = employerEWAList;
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

        public async Task<AccessAmountViewDetailResponse> GetEmployeesEwaRequestDetail(AccessAmountViewDetailRequest request)
        {
            var res = new AccessAmountViewDetailResponse();

            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var employerEWADetail = await _withdrawalsRepository.GetEmployeesEwaRequestDetail1(userData.Id, request.AccessAmountId);
                if (employerEWADetail != null)
                {
                    res.accessAmountViewDetail = employerEWADetail;
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


        public async Task<MemoryStream> ExportEmployeesListReport(AccessAmountRequest request)
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
        private async Task GenerateEmployeesData(AccessAmountRequest request)
        {

            try
            {
                ICellStyle style1 = _hssfWorkbook.CreateCellStyle();
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style1.FillPattern = FillPattern.SolidForeground;

                var id = Guid.Parse(request.userGuid.ToString());
                //var result = await _employerRepository.GetEmployerDetailByGuid(id);
                var response = await _withdrawalsRepository.GetEmployeesEwaRequestListForCsv(request.Status, request.FromDate, request.ToDate);

                ISheet sheet1 = _hssfWorkbook.CreateSheet("PayMastaEmployeeListLog");
                sheet1.SetColumnWidth(0, 1500);
                sheet1.SetColumnWidth(1, 8000);
                sheet1.SetColumnWidth(2, 4000);
                sheet1.SetColumnWidth(3, 8000);
                sheet1.SetColumnWidth(4, 8000);
                sheet1.SetColumnWidth(5, 8000);
                sheet1.SetColumnWidth(6, 8000);
                sheet1.SetColumnWidth(7, 8000);
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
                C05.SetCellValue("Amt.Requested");
                C05.CellStyle = style1;

                var C06 = R0.CreateCell(6);
                C06.SetCellValue("Status");
                C06.CellStyle = style1;

                var C07 = R0.CreateCell(7);
                C07.SetCellValue("Date");
                C07.CellStyle = style1;

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
                    c5.SetCellValue(item.AccessAmount.ToString());

                    var c6 = row.CreateCell(6);
                    c6.SetCellValue(item.Status.ToString());

                    var c7 = row.CreateCell(7);
                    c7.SetCellValue(item.CreatedAt.ToString());
                    i++;
                }

            }
            catch (Exception ex)
            {

            }
        }


        public async Task<ProvidusFundResponse> ProvidusFundTransfer(ProvidusFundTransferRequest request)
        {
            var result = new ProvidusFundResponse();
            var providusFundTransferResponse = new ProvidusFundTransferResponse();
            try
            {
                using (var dbConnection = Connection)
                {
                    var user = await _accountRepository.GetUserByGuid(request.UserGuid, dbConnection);

                    var userSessionData = await _accountRepository.GetSessionByUserId(user.Id);
                    var ewaRequestData = await _withdrawalsRepository.GetEmployeesEwaRequestDetailByUserId(user.Id, request.AccessAmountId);
                    var bankAccountData = await _withdrawalsRepository.GetBankDetailByUserId(user.Id, dbConnection);
                    decimal usedLimit = 0;
                    var usedAccessPercentage = await _withdrawalsRepository.GetAccessAmountPercentage(user.Id);
                    if (usedAccessPercentage != null && usedAccessPercentage.AccessedPercentage > 0)
                    {
                        usedLimit = usedAccessPercentage.AccessedPercentage;
                    }
                    //usedLimit = usedAccessPercentage.AccessedPercentage;
                    ewaRequestData.AccessAmount = Convert.ToDecimal(ewaRequestData.AccessAmount);
                    var earning = await _withdrawalsRepository.GetEarnings(user.Id);
                    var earningEntity = await _withdrawalsRepository.GetEarningByEarningId(user.Id, earning.Id);
                    decimal val = Convert.ToDecimal(33.3);
                    var useableAmount = earning.AvailableAmount;// * val / 100;
                    var useablelAmountPercent = (ewaRequestData.AccessAmount / earning.AvailableAmount * 100);
                    if (bankAccountData != null)
                    {
                        if (ewaRequestData.AdminStatus != 1)
                        {
                            if (earning != null && Convert.ToDecimal(useableAmount) >= Convert.ToDecimal(ewaRequestData.AccessAmount))
                            {
                                if (ewaRequestData != null)
                                {
                                    var invoiceNumber = await _commonService.GetInvoiceNumber();
                                    var req = new NIPFundTransferTrquest
                                    {
                                        beneficiaryAccountName = bankAccountData.BankAccountHolderName,
                                        beneficiaryAccountNumber = bankAccountData.AccountNumber,
                                        beneficiaryBank = bankAccountData.BankCode,
                                        sourceAccountName = AppSetting.sourceAccountName,
                                        transactionAmount = ewaRequestData.AccessAmount.ToString(),
                                        currencyCode = AppSetting.currencyCode,
                                        narration = AppSetting.narration,
                                        transactionReference = invoiceNumber.InvoiceNumber,
                                        password = AppSetting.BankPassword,
                                        userName = AppSetting.BankUserName
                                    };
                                    var jsonReq = JsonConvert.SerializeObject(req);
                                    await _commonService.GetProvidusBankResponse("Request:UserId=" + user.Id + " " + jsonReq);
                                    var res = await _thirdParty.PostBankTransaction(jsonReq, AppSetting.NIPFundTransfer);
                                    providusFundTransferResponse = JsonConvert.DeserializeObject<ProvidusFundTransferResponse>(res);
                                    await _commonService.GetProvidusBankResponse("Response:UserId=" + user.Id + " " + res);

                                    if (providusFundTransferResponse != null && providusFundTransferResponse.responseCode == AggregatorySTATUSCODES.SUCCESSFUL)
                                    {
                                        earningEntity.AccessedAmount = earningEntity.AccessedAmount + ewaRequestData.TotalAmountWithCommission;
                                        earningEntity.AvailableAmount = earningEntity.AvailableAmount - ewaRequestData.TotalAmountWithCommission;
                                        earningEntity.UsableAmount = earningEntity.UsableAmount - ewaRequestData.TotalAmountWithCommission;
                                        earningEntity.UpdatedAt = DateTime.UtcNow;
                                        earningEntity.UpdatedBy = 0;

                                        ewaRequestData.AdminStatus = (int)TransactionStatus.Completed;
                                        ewaRequestData.UpdatedAt = DateTime.UtcNow;
                                        ewaRequestData.UpdatedBy = 0;
                                        var status = await _withdrawalsRepository.UpdateEwaStatus(ewaRequestData);
                                        var earningUpdate = await _withdrawalsRepository.UpdateEwaEarning(earningEntity);

                                        result.transferResponse = providusFundTransferResponse;
                                        result.Status = true;
                                        result.RstKey = 1;
                                        result.Message = providusFundTransferResponse.responseMessage;
                                        if (result.RstKey == 1 && userSessionData != null)
                                        {
                                            var pushReq = new PushModel
                                            {
                                                DeviceToken = userSessionData.DeviceToken,
                                                Title = "Successfully",
                                                Message = "Your ewa request has been approved & your account has been credited with ₦" + ewaRequestData.AccessAmount.ToString() + " successfully."
                                            };
                                            var noti1 = _pushNotification.SendPush(pushReq);

                                            var notiReq = new SendNotificationsRequest
                                            {
                                                NotificatiomMessage = pushReq.Message,
                                                UserIds = user.Id.ToString(),
                                                AdminGuid = request.AdminGuid,
                                            };
                                            await _manageNotificationsService.SendNotification(notiReq);
                                        }
                                    }
                                    else if (providusFundTransferResponse == null)
                                    {
                                        result.Status = false;
                                        result.RstKey = 2;
                                        result.Message = ResponseMessages.TRANSACTION_NOT_DONE;
                                    }
                                    else
                                    {

                                        result.Status = false;
                                        result.RstKey = 2;
                                        result.Message = providusFundTransferResponse.responseMessage;
                                    }
                                }
                            }

                        }
                        else
                        {
                            result.Status = false;
                            result.RstKey = 3;//alredy transferd
                                              // result.Message = providusFundTransferResponse.responseMessage;
                        }
                    }
                    else
                    {
                        result.Status = false;
                        result.RstKey = 31;
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Message.ErrorLog("WithdrawlsService.cs", "ProvidusFundTransfer", Connection);
                result.RstKey = 32;
            }
            return result;
        }

        public async Task<FundTransferResponse> FundTransferInWallet(ProvidusFundTransferRequest request)
        {
            var result = new FundTransferResponse();
            var providusFundTransferResponse = new WalletToWalletTransferResponse();
            try
            {
                using (var dbConnection = Connection)
                {
                    var user = await _accountRepository.GetUserByGuid(request.UserGuid, dbConnection);
                    var adminUser = await _accountRepository.GetUserByGuid(request.AdminGuid, dbConnection);
                    var userSessionData = await _accountRepository.GetSessionByUserId(user.Id);
                    var ewaRequestData = await _withdrawalsRepository.GetEmployeesEwaRequestDetailByUserId(user.Id, request.AccessAmountId);
                    var bankAccountData = await _withdrawalsRepository.GetBankDetailByUserId(user.Id, dbConnection);
                    var virtualAccountData = await _withdrawalsRepository.GetVirtualAccountDetailByUserId(user.Id, dbConnection);
                    var adminVirtualAccountData = await _withdrawalsRepository.GetVirtualAccountDetailByUserId(adminUser.Id, dbConnection);
                    decimal usedLimit = 0;
                    if (!string.IsNullOrEmpty(adminVirtualAccountData.AuthToken))
                    {
                        var usedAccessPercentage = await _withdrawalsRepository.GetAccessAmountPercentage(user.Id);
                        if (usedAccessPercentage != null && usedAccessPercentage.AccessedPercentage > 0)
                        {
                            usedLimit = usedAccessPercentage.AccessedPercentage;
                        }
                        ewaRequestData.AccessAmount = Convert.ToDecimal(ewaRequestData.AccessAmount);
                        var earning = await _withdrawalsRepository.GetEarnings(user.Id);
                        var earningEntity = await _withdrawalsRepository.GetEarningByEarningId(user.Id, earning.Id);
                        decimal val = Convert.ToDecimal(33.3);
                        var useableAmount = earning.AvailableAmount;// * val / 100;
                        var useablelAmountPercent = (ewaRequestData.AccessAmount / earning.AvailableAmount * 100);
                        if (virtualAccountData != null)
                        {
                            if (ewaRequestData.AdminStatus != 1)
                            {
                                if (earning != null && Convert.ToDecimal(useableAmount) >= Convert.ToDecimal(ewaRequestData.AccessAmount))
                                {
                                    if (ewaRequestData != null)
                                    {
                                        var invoiceNumber = await _commonService.GetInvoiceNumber();
                                        var req = new WalletToWalletTransferRequest
                                        {
                                            accountNumber = virtualAccountData.AccountNumber,
                                            amount = Convert.ToDecimal(ewaRequestData.AccessAmount),
                                            channel = "wallettowallet",
                                            sourceAccountNumber = AppSetting.SourceAccountNumberPouchii,
                                            beneficiaryName = "",
                                            destBankCode = "",
                                            isToBeSaved = true,
                                            pin = AppSetting.PIN,
                                            sourceBankCode = "",
                                            transRef = invoiceNumber.InvoiceNumber,
                                            // specificChannel = "payMasta Intra"
                                        };
                                        var jsonReq = JsonConvert.SerializeObject(req);
                                        string token = adminVirtualAccountData.AuthToken;
                                        await _commonService.GetProvidusBankResponse("Request:UserId=" + user.Id + " " + jsonReq);
                                        var res = await _thirdParty.FundTransaction(jsonReq, AppSetting.Fundwallet, token);
                                        providusFundTransferResponse = JsonConvert.DeserializeObject<WalletToWalletTransferResponse>(res);
                                        await _commonService.GetProvidusBankResponse("Response:UserId=" + user.Id + " " + res);

                                        if (providusFundTransferResponse != null && providusFundTransferResponse.code == "00")
                                        {
                                            earningEntity.AccessedAmount = earningEntity.AccessedAmount + ewaRequestData.TotalAmountWithCommission;
                                            earningEntity.AvailableAmount = earningEntity.AvailableAmount - ewaRequestData.TotalAmountWithCommission;
                                            earningEntity.UsableAmount = earningEntity.UsableAmount - ewaRequestData.TotalAmountWithCommission;
                                            earningEntity.UpdatedAt = DateTime.UtcNow;
                                            earningEntity.UpdatedBy = 0;

                                            ewaRequestData.AdminStatus = (int)TransactionStatus.Completed;
                                            ewaRequestData.UpdatedAt = DateTime.UtcNow;
                                            ewaRequestData.UpdatedBy = 0;
                                            var status = await _withdrawalsRepository.UpdateEwaStatus(ewaRequestData);
                                            var earningUpdate = await _withdrawalsRepository.UpdateEwaEarning(earningEntity);

                                            result.transferResponse = providusFundTransferResponse;
                                            result.Status = true;
                                            result.RstKey = 1;
                                            result.Message = providusFundTransferResponse.message;
                                            if (result.RstKey == 1 && userSessionData != null)
                                            {
                                                var pushReq = new PushModel
                                                {
                                                    DeviceToken = userSessionData.DeviceToken,
                                                    Title = "Successfully",
                                                    Message = "Your EWA request has been approved & your account has been credited with ₦" + ewaRequestData.AccessAmount.ToString() + " successfully."
                                                };
                                                var noti1 = _pushNotification.SendPush(pushReq);

                                                var notiReq = new SendNotificationsRequest
                                                {
                                                    NotificatiomMessage = pushReq.Message,
                                                    UserIds = user.Id.ToString(),
                                                    AdminGuid = request.AdminGuid,
                                                };
                                                await _manageNotificationsService.SendNotification(notiReq);
                                            }
                                        }
                                        else if (providusFundTransferResponse.code.ToLower() != "00")
                                        {
                                            result.Status = false;
                                            result.RstKey = 2;
                                            result.Message = providusFundTransferResponse.message;
                                        }
                                        else if (providusFundTransferResponse == null)
                                        {
                                            result.Status = false;
                                            result.RstKey = 2;
                                            result.Message = ResponseMessages.TRANSACTION_NOT_DONE;
                                        }
                                        else
                                        {
                                            result.Status = false;
                                            result.RstKey = 2;
                                            result.Message = providusFundTransferResponse.message;
                                        }
                                    }
                                }
                                else
                                {
                                    result.Status = false;
                                    result.RstKey = 2;
                                    result.Message = ResponseMessages.TRANSACTION_NOT_DONE;
                                }

                            }
                            else
                            {
                                result.Status = false;
                                result.RstKey = 3;//alredy transferd
                                                  // result.Message = providusFundTransferResponse.responseMessage;
                            }
                        }
                        else
                        {
                            result.Status = false;
                            result.RstKey = 31;
                        }
                    }
                    else
                    {
                        result.Status = false;
                        result.RstKey = 33;
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Message.ErrorLog("WithdrawlsService.cs", "ProvidusFundTransfer", Connection);
                result.RstKey = 32;
            }
            return result;
        }

        public async Task<bool> AuthenticateVirtualAccount(string username, string password, bool rememberMe, string schemeid, string deviceId, long userId)
        {
            bool res = false;
            try
            {
                // var virtualAccountDetail = await _virtualAccountRepository.GetVirtualAccountDetailByUserId(userId);
                var req = new AuthenticateRequest
                {
                    deviceId = "64784844-hhhd748849-g7378382",
                    username = username,
                    password = password,
                    rememberMe = rememberMe,
                    scheme = schemeid,
                };
                var json = JsonConvert.SerializeObject(req);

                var authenticateResponse = await _thirdParty.CreateProvidusVirtualAccount(json, AppSetting.AuthenticatePouchii.ToString());
                var JsonResult = JsonConvert.DeserializeObject<AuthenticateResponse>(authenticateResponse);

                if (JsonResult != null)
                {
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<FundTransferResponse> RejectSystemSpecsTransfer(ProvidusFundTransferRequest request)
        {
            var result = new FundTransferResponse();
            var providusFundTransferResponse = new WalletToWalletTransferResponse();
            try
            {
                using (var dbConnection = Connection)
                {
                    var user = await _accountRepository.GetUserByGuid(request.UserGuid, dbConnection);
                    var adminUser = await _accountRepository.GetUserByGuid(request.AdminGuid, dbConnection);
                    var userSessionData = await _accountRepository.GetSessionByUserId(user.Id);
                    var ewaRequestData = await _withdrawalsRepository.GetEmployeesEwaRequestDetailByUserId(user.Id, request.AccessAmountId);


                    ewaRequestData.AdminStatus = (int)TransactionStatus.Rejected;
                    ewaRequestData.UpdatedAt = DateTime.UtcNow;
                    ewaRequestData.UpdatedBy = 0;
                    var status = await _withdrawalsRepository.UpdateEwaStatus(ewaRequestData);
                    if (status == 1)
                    {
                        result.Status = true;
                        result.RstKey = 1;
                        result.Message = "EWA request has been rejeted.";
                        if (result.RstKey == 1 && userSessionData != null)
                        {
                            var pushReq = new PushModel
                            {
                                DeviceToken = userSessionData.DeviceToken,
                                Title = "Successfully",
                                Message = "Your EWA request has been rejected by admin."
                            };
                            var noti1 = _pushNotification.SendPush(pushReq);

                            var notiReq = new SendNotificationsRequest
                            {
                                NotificatiomMessage = pushReq.Message,
                                UserIds = user.Id.ToString(),
                                AdminGuid = request.AdminGuid,
                            };
                            await _manageNotificationsService.SendNotification(notiReq);
                        }
                    }
                    else
                    {
                        result.Status = false;
                        result.RstKey = 2;
                        result.Message = "Failed";
                    }

                }

            }
            catch (Exception ex)
            {
                ex.Message.ErrorLog("WithdrawlsService.cs", "RejectSystemSpecsTransfer", Connection);
                result.RstKey = 32;
            }
            return result;
        }

        public async Task<ExpressFundTransferResponse> FundTransferInExpressWallet(ProvidusFundTransferRequest request)
        {
            var result = new ExpressFundTransferResponse();
            var providusFundTransferResponse = new CustomerWalletCreditResponse();
            try
            {
                using (var dbConnection = Connection)
                {
                    var user = await _accountRepository.GetUserByGuid(request.UserGuid, dbConnection);
                    var adminUser = await _accountRepository.GetUserByGuid(request.AdminGuid, dbConnection);
                    var userSessionData = await _accountRepository.GetSessionByUserId(user.Id);
                    var ewaRequestData = await _withdrawalsRepository.GetEmployeesEwaRequestDetailByUserId(user.Id, request.AccessAmountId);
                    //  var bankAccountData = await _withdrawalsRepository.GetBankDetailByUserId(user.Id, dbConnection);
                    var virtualAccountData = await _withdrawalsRepository.GetExpressVirtualAccountDetailByUserId(user.Id, dbConnection);
                    // var adminVirtualAccountData = await _withdrawalsRepository.GetVirtualAccountDetailByUserId(adminUser.Id, dbConnection);
                    decimal usedLimit = 0;
                    if (!string.IsNullOrEmpty(virtualAccountData.CustomerId))
                    {
                        var usedAccessPercentage = await _withdrawalsRepository.GetAccessAmountPercentage(user.Id);
                        if (usedAccessPercentage != null && usedAccessPercentage.AccessedPercentage > 0)
                        {
                            usedLimit = usedAccessPercentage.AccessedPercentage;
                        }
                        ewaRequestData.AccessAmount = Convert.ToDecimal(ewaRequestData.AccessAmount);
                        var earning = await _withdrawalsRepository.GetEarnings(user.Id);
                        var earningEntity = await _withdrawalsRepository.GetEarningByEarningId(user.Id, earning.Id);
                        decimal val = Convert.ToDecimal(33.3);
                        var useableAmount = earning.AvailableAmount;// * val / 100;
                        var useablelAmountPercent = (ewaRequestData.AccessAmount / earning.AvailableAmount * 100);
                        if (virtualAccountData != null)
                        {
                            if (ewaRequestData.AdminStatus != 1)
                            {
                                if (earning != null && Convert.ToDecimal(useableAmount) >= Convert.ToDecimal(ewaRequestData.AccessAmount))
                                {
                                    if (ewaRequestData != null)
                                    {
                                        var invoiceNumber = await _commonService.GetInvoiceNumber();

                                        var req = new CustomerWalletCreditRequest
                                        {
                                            amount = Convert.ToDecimal(ewaRequestData.AccessAmount),
                                            customerId= virtualAccountData.CustomerId,
                                            reference= invoiceNumber.InvoiceNumber,
                                            metadata=new Metadata
                                            {
                                                moredata= virtualAccountData.UserId.ToString(),
                                                somedata="Paymasta Admin credit"
                                            }
                                        };
                                        var jsonReq = JsonConvert.SerializeObject(req);
                                        // string token = adminVirtualAccountData.AuthToken;
                                        await _commonService.GetProvidusBankResponse("Request:UserId=" + user.Id + " " + jsonReq);
                                        var res = await _thirdParty.FundTransaction(jsonReq, AppSetting.ExpressWalletBaseUrl + AppSetting.ExpressWalletCredit, AppSetting.ExpressWalletSecretKey);
                                        providusFundTransferResponse = JsonConvert.DeserializeObject<CustomerWalletCreditResponse>(res);
                                        await _commonService.GetProvidusBankResponse("Response:UserId=" + user.Id + " " + res);

                                        if (providusFundTransferResponse != null && providusFundTransferResponse.status == true)
                                        {
                                            earningEntity.AccessedAmount = earningEntity.AccessedAmount + ewaRequestData.TotalAmountWithCommission;
                                            earningEntity.AvailableAmount = earningEntity.AvailableAmount - ewaRequestData.TotalAmountWithCommission;
                                            earningEntity.UsableAmount = earningEntity.UsableAmount - ewaRequestData.TotalAmountWithCommission;
                                            earningEntity.UpdatedAt = DateTime.UtcNow;
                                            earningEntity.UpdatedBy = 0;

                                            ewaRequestData.AdminStatus = (int)TransactionStatus.Completed;
                                            ewaRequestData.UpdatedAt = DateTime.UtcNow;
                                            ewaRequestData.UpdatedBy = 0;
                                            var status = await _withdrawalsRepository.UpdateEwaStatus(ewaRequestData);
                                            var earningUpdate = await _withdrawalsRepository.UpdateEwaEarning(earningEntity);

                                            result.transferResponse = providusFundTransferResponse;
                                            result.Status = true;
                                            result.RstKey = 1;
                                            result.Message = providusFundTransferResponse.message;
                                            if (result.RstKey == 1 && userSessionData != null)
                                            {
                                                var pushReq = new PushModel
                                                {
                                                    DeviceToken = userSessionData.DeviceToken,
                                                    Title = "Successfully",
                                                    Message = "Your EWA request has been approved & your account has been credited with ₦" + ewaRequestData.AccessAmount.ToString() + " successfully."
                                                };
                                                var noti1 = _pushNotification.SendPush(pushReq);

                                                var notiReq = new SendNotificationsRequest
                                                {
                                                    NotificatiomMessage = pushReq.Message,
                                                    UserIds = user.Id.ToString(),
                                                    AdminGuid = request.AdminGuid,
                                                };
                                                await _manageNotificationsService.SendNotification(notiReq);
                                            }
                                        }
                                        else if (providusFundTransferResponse.status == false)
                                        {
                                            result.Status = false;
                                            result.RstKey = 2;
                                            result.Message = providusFundTransferResponse.message;
                                        }
                                        else if (providusFundTransferResponse == null)
                                        {
                                            result.Status = false;
                                            result.RstKey = 2;
                                            result.Message = ResponseMessages.TRANSACTION_NOT_DONE;
                                        }
                                        else
                                        {
                                            result.Status = false;
                                            result.RstKey = 2;
                                            result.Message = providusFundTransferResponse.message;
                                        }
                                    }
                                }
                                else
                                {
                                    result.Status = false;
                                    result.RstKey = 2;
                                    result.Message = ResponseMessages.TRANSACTION_NOT_DONE;
                                }

                            }
                            else
                            {
                                result.Status = false;
                                result.RstKey = 3;//alredy transferd
                                                  // result.Message = providusFundTransferResponse.responseMessage;
                            }
                        }
                        else
                        {
                            result.Status = false;
                            result.RstKey = 31;
                        }
                    }
                    else
                    {
                        result.Status = false;
                        result.RstKey = 33;
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Message.ErrorLog("WithdrawlsService.cs", "ProvidusFundTransfer", Connection);
                result.RstKey = 32;
            }
            return result;
        }
    }
}
