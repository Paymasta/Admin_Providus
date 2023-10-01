using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PayMasta.Repository.Account;
using PayMasta.Repository.Employer;
using PayMasta.Utilities;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.EmployerVM;
using PayMasta.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer
{
    public class EmployerService : IEmployerService
    {
        private readonly IEmployerRepository _employerRepository;
        private readonly IAccountRepository _accountRepository;
        private HSSFWorkbook _hssfWorkbook;
        public EmployerService()
        {
            _employerRepository = new EmployerRepository();
            _accountRepository = new AccountRepository();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }


        public async Task<EmployerListReponse> GetEmployerList(GetEmployerListRequest request)
        {
            var res = new EmployerListReponse();

            try
            {
                var employerList = await _employerRepository.GetEmployerList(request.pageNumber, request.PageSize, request.Status, request.FromDate, request.ToDate, request.SearchTest);
                if (employerList.Count > 0)
                {
                    res.employerResponses = employerList;
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
        public async Task<EmployerReponse> GetEmployerProfile(GetEmployerProfileRequest request)
        {
            var res = new EmployerReponse();

            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.EmployerGuid);
                var employerProfile = await _employerRepository.GetEmployerProfile(userData.Id);
                if (employerProfile != null)
                {
                    res.employerResponses = employerProfile;
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

        public async Task<EmployeesReponse> GetEmployeesByEmployerGuid(GetEmployeesListRequest request)
        {
            var res = new EmployeesReponse();
            var result = new GetEmployerDetailResponse();
            try
            {
                var id = Guid.Parse(request.userGuid.ToString());
                result = await _employerRepository.GetEmployerDetailByGuid(id);
                if (result != null)
                {
                    var employeesList = await _employerRepository.GetEmployeesListByEmployerId(result.Id, request.pageNumber, request.PageSize, request.SearchTest);
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
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = ResponseMessages.INVALID_USER_TYPE;

                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<EmployeesWithdrawlsResponse> GetEmployeesWithdrwals(EmployeesWithdrawlsRequest request)
        {
            var res = new EmployeesWithdrawlsResponse();
            //var result = new EmployeesWithdrawlsResponse();
            try
            {
                var id = Guid.Parse(request.UserGuid.ToString());
                var result = await _accountRepository.GetUserByGuid(id);
                if (result != null)
                {
                    var employeesWithdrawlsList = await _employerRepository.GetEmployeesWithdrwals(result.Id, request.Month, request.PageSize, request.PageNumber);
                    if (employeesWithdrawlsList.Count > 0)
                    {
                        res.employeesWithdrawls = employeesWithdrawlsList;
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
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = ResponseMessages.INVALID_USER_TYPE;

                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<EmployeeEwaDetailReponse> GetEmployeesEwaRequestDetail(EmployeesEWAWithdrawlsRequest request)
        {
            var res = new EmployeeEwaDetailReponse();

            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var employerEWADetail = await _employerRepository.GetEmployeesEwaRequestDetail(userData.Id);
                if (employerEWADetail != null)
                {
                    res.employeeEwaDetail = employerEWADetail;
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

        public async Task<MemoryStream> ExportEmployerListReport(GetEmployerListRequest request)
        {
            InitializeWorkbook();
            await GenerateData(request);
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

        private async Task GenerateData(GetEmployerListRequest request)
        {

            try
            {
                ICellStyle style1 = _hssfWorkbook.CreateCellStyle();
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style1.FillPattern = FillPattern.SolidForeground;

                var id = Guid.Parse(request.userGuid.ToString());
                //var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var response = await _employerRepository.GetEmployerListForCsv(request.Status, request.FromDate, request.ToDate);

                ISheet sheet1 = _hssfWorkbook.CreateSheet("PayMastaEmployerListLog");
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
                C02.SetCellValue("Email");
                C02.CellStyle = style1;

                var C03 = R0.CreateCell(3);
                C03.SetCellValue("MobileNo");
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
                    C1.SetCellValue(item.OrganisationName);

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

        public async Task<MemoryStream> ExportEmployeesListReport(GetEmployeesListRequest request)
        {
            InitializeWorkbook();
            await GenerateEmployeesData(request);
            return GetExcelStream();
        }


        private async Task GenerateEmployeesData(GetEmployeesListRequest request)
        {

            try
            {
                ICellStyle style1 = _hssfWorkbook.CreateCellStyle();
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style1.FillPattern = FillPattern.SolidForeground;

                var id = Guid.Parse(request.userGuid.ToString());
                var result = await _employerRepository.GetEmployerDetailByGuid(id);
                var response = await _employerRepository.GetEmployeesListByEmployerIdForCsv(result.Id);

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
                C01.SetCellValue("Name");
                C01.CellStyle = style1;

                var C02 = R0.CreateCell(2);
                C02.SetCellValue("Staff Id");
                C02.CellStyle = style1;

                var C03 = R0.CreateCell(3);
                C03.SetCellValue("MobileNo");
                C03.CellStyle = style1;

                var C04 = R0.CreateCell(4);
                C04.SetCellValue("Email");
                C04.CellStyle = style1;

                var C05 = R0.CreateCell(5);
                C05.SetCellValue("Status");
                C05.CellStyle = style1;


                int i = 1;
                foreach (var item in response)
                {

                    IRow row = sheet1.CreateRow(i);

                    var C0 = row.CreateCell(0);
                    C0.SetCellValue(item.RowNumber);

                    var C1 = row.CreateCell(1);
                    C1.SetCellValue(item.FirstName + " " + item.LastName);

                    var C2 = row.CreateCell(2);
                    C2.SetCellValue(item.StaffId);

                    var c3 = row.CreateCell(3);
                    c3.SetCellValue(item.CountryCode.ToString() + "-" + item.PhoneNumber.ToString());

                    var c4 = row.CreateCell(4);
                    c4.SetCellValue(item.Email);

                    var c5 = row.CreateCell(5);
                    c5.SetCellValue(item.Status.ToString());

                    i++;
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
