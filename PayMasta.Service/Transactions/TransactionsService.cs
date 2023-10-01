using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PayMasta.Repository.Transactions;
using PayMasta.Repository.User;
using PayMasta.Utilities;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.TransactionsVM;
using PayMasta.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Transactions
{
    public class TransactionsService : ITransactionsService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionsRepository _transactionsRepository;
        private HSSFWorkbook _hssfWorkbook;
        public TransactionsService()
        {
            _transactionsRepository = new TransactionsRepository();
            _userRepository = new UserRepository();
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

                var employeesList = await _userRepository.GetEmployeesList(-1,request.pageNumber, request.PageSize, request.Status, request.FromDate, request.ToDate, request.SearchTest);
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

        public async Task<EmployeesReponse> GetEmployeesListForTransactions(GetEmployeesListRequest request)
        {
            var res = new EmployeesReponse();

            try
            {

                var employeesList = await _userRepository.GetEmployeesListForTransactions(request.pageNumber, request.PageSize, request.Status, request.FromDate, request.ToDate, request.SearchTest);
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

        public async Task<EmployeeTransactionDetailResponse> GetEmployeDetailByGuid(EmployeeDetailRequest request)
        {
            var res = new EmployeeTransactionDetailResponse();

            try
            {
                var userdata = await _userRepository.GetUserByGuid(request.UserGuid);
                var employeceDetail = await _transactionsRepository.GetEmployeeDetailByUserId(userdata.Id);
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

        public async Task<TransactionsResponse> GetEmployeeTransactionByGuid(EmployeeTransactionRequest request)
        {
            var res = new TransactionsResponse();

            try
            {
                var userdata = await _userRepository.GetUserByGuid(request.UserGuid);
                var employeceDetail = await _transactionsRepository.GetEmployeeTransactionByUserId(userdata.Id, request.PageSize, request.PageNumber, request.Month);
                if (employeceDetail.Count > 0)
                {
                    res.employeeTransactions = employeceDetail;
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
                ICellStyle style1 = _hssfWorkbook.CreateCellStyle();
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style1.FillPattern = FillPattern.SolidForeground;

                var id = Guid.Parse(request.userGuid.ToString());
                //var result = await _employerRepository.GetEmployerDetailByGuid(id);
                var response = await _userRepository.GetEmployeesListForCsv(request.Status, request.FromDate, request.ToDate);

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
                C02.SetCellValue("Email");
                C02.CellStyle = style1;

                var C03 = R0.CreateCell(3);
                C03.SetCellValue("MobileNo");
                C03.CellStyle = style1;

                var C04 = R0.CreateCell(4);
                C04.SetCellValue("Employer Name");
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
                    C1.SetCellValue(item.FirstName + " " + item.LastName);

                    var C2 = row.CreateCell(2);
                    C2.SetCellValue(item.Email);

                    var c3 = row.CreateCell(3);
                    c3.SetCellValue(item.CountryCode.ToString() + "-" + item.PhoneNumber.ToString());

                    var c4 = row.CreateCell(4);
                    c4.SetCellValue(item.EmployerName);

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
