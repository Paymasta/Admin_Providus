using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PayMasta.Entity.MainCategory;
using PayMasta.Entity.SubCategory;
using PayMasta.Repository.Account;
using PayMasta.Repository.ManageCategory;
using PayMasta.Repository.User;
using PayMasta.Utilities;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.ManageCategoryVM;
using PayMasta.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ManageCategory
{
    public class ManageCategoryService : IManageCategoryService
    {
        private readonly IManageCategoryRepository _manageCategoryRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private HSSFWorkbook _hssfWorkbook;
        public ManageCategoryService()
        {
            _manageCategoryRepository = new ManageCategoryRepository();
            _accountRepository = new AccountRepository();
            _userRepository = new UserRepository();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }
        public async Task<ManageCategoriesReponse> GetCategoryList(GetCategoryListRequest request)
        {
            var res = new ManageCategoriesReponse();

            try
            {
                var categoryList = await _manageCategoryRepository.GetCategoryList(request.pageNumber, request.PageSize, request.Status, request.FromDate, request.ToDate, request.SearchTest);
                if (categoryList.Count > 0)
                {
                    res.manageCategories = categoryList;
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

        public async Task<BlockUnBlockCategoryResponse> BlockUnBlockCategory(BlockUnBlockCategoryRequest request)
        {
            var res = new BlockUnBlockCategoryResponse();
            var result = new GetAdminDetailResponse();
            try
            {
                var employeerId = Guid.Parse(request.AdminUserGuid.ToString());

                result = await _userRepository.GetAdminDetailByGuid(employeerId);
                var walletserviceData = await _manageCategoryRepository.GetWalletServiceById(request.WalletServiceId);
                if (walletserviceData != null)
                {
                    if (request.DeleteOrBlock == 1)
                    {
                        if (walletserviceData.IsActive == true)
                        {
                            walletserviceData.IsActive = false;
                            walletserviceData.UpdatedAt = DateTime.UtcNow;
                            walletserviceData.UpdatedBy = result.Id;
                            res.Message = AdminResponseMessages.CATEGORY_BLOCKED;
                        }
                        else
                        {
                            walletserviceData.IsActive = true;
                            walletserviceData.UpdatedAt = DateTime.UtcNow;
                            walletserviceData.UpdatedBy = result.Id;
                            res.Message = AdminResponseMessages.CATEGORY_UNBLOCKED;
                        }
                    }
                    else if (request.DeleteOrBlock == 2)
                    {

                        walletserviceData.IsActive = false;
                        walletserviceData.IsDeleted = true;
                        walletserviceData.UpdatedAt = DateTime.UtcNow;
                        walletserviceData.UpdatedBy = result.Id;
                        res.Message = AdminResponseMessages.CATEGORY_DELETED;
                    }
                    if (await _manageCategoryRepository.UpdateWalletServiceStatus(walletserviceData) > 0)
                    {
                        res.IsSuccess = true;
                        res.RstKey = 1;
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.RstKey = 2;
                        res.Message = AdminResponseMessages.CATEGORY_BLOCKED;
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = AdminResponseMessages.DATA_NOT_FOUND_GENERIC;

                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<GetCategoryDetail> ViewCategoryDetail(GetCategoryDetailRequest request)
        {
            var res = new GetCategoryDetail();

            try
            {
                var categoryDetail = await _manageCategoryRepository.ViewCategoryDetailByGuid(request.SubCategoryGuid);
                if (categoryDetail != null)
                {
                    res.getCategoryDetailResponse = categoryDetail;
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

        public async Task<UpdateCategoryDetailResponse> UpdateCategoryDetail(UpdateCategoryDetailRequest request)
        {
            var res = new UpdateCategoryDetailResponse();

            try
            {
                var subCategoryDetail = await _manageCategoryRepository.GetSubCategoryDetail(request.SubCategoryGuid);
                var mainCategoryDetail = await _manageCategoryRepository.GetMainCategoryDetail(subCategoryDetail.MainCategoryId);

                if (subCategoryDetail != null && mainCategoryDetail != null)
                {
                    subCategoryDetail.CategoryName = request.SubCategory;
                    subCategoryDetail.UpdatedAt = DateTime.UtcNow;
                    mainCategoryDetail.CategoryName = request.MainCategory;
                    mainCategoryDetail.UpdatedAt = DateTime.UtcNow;
                    if (await _manageCategoryRepository.UpdateSubCategoryDetail(subCategoryDetail) > 0)
                    {
                        if (await _manageCategoryRepository.UpdateMainCategoryDetail(mainCategoryDetail) > 0)
                        {
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
                        res.RstKey = 2;
                        res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                    }

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

        public async Task<UpdateCategoryDetailResponse> AddCategoryAndSubCategory(AddCategoryDetailRequest request)
        {
            var res = new UpdateCategoryDetailResponse();
            var subReq = new SubCategory();
            var mainReq = new MainCategory();
            try
            {
                var subCategoryDetail = await _manageCategoryRepository.GetSubCategoryDetailByName(request.SubCategory);
                var mainCategoryDetail = await _manageCategoryRepository.GetMainCategoryDetailByName(request.MainCategory);

                if (subCategoryDetail != null)
                {

                    res.IsSuccess = false;
                    res.RstKey = 4;
                    res.Message = AdminResponseMessages.DUPLICATE_SUBCATEGORY;
                    return res;
                }
                if (mainCategoryDetail != null)
                {
                    res.IsSuccess = false;
                    res.RstKey = 5;
                    res.Message = AdminResponseMessages.DUPLICATE_MAINCATEGORY;
                    return res;
                }
                if (subCategoryDetail == null && mainCategoryDetail == null)
                {
                    mainReq.CategoryName = request.MainCategory;
                    mainReq.IsActive = true;
                    mainReq.IsDeleted = false;
                    mainReq.CreatedAt = DateTime.Now;
                    mainReq.UpdatedAt = DateTime.Now;

                    var mainCategoryId = await _manageCategoryRepository.AddMainCategoryDetail(mainReq);
                    if (mainCategoryId > 0)
                    {
                        subReq.CategoryName = request.SubCategory;
                        subReq.MainCategoryId = mainCategoryId;
                        subReq.IsActive = true;
                        subReq.IsDeleted = false;
                        subReq.CreatedAt = DateTime.Now;
                        subReq.UpdatedAt = DateTime.Now;
                        var subRes = await _manageCategoryRepository.AddSubCategoryDetail(subReq);
                        if (subRes > 0)
                        {
                            res.IsSuccess = true;
                            res.RstKey = 1;
                            res.Message = ResponseMessages.DATA_SAVED;
                        }
                        else
                        {
                            res.IsSuccess = false;
                            res.RstKey = 2;
                            res.Message = ResponseMessages.DATA_NOT_SAVED;
                        }
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<GetCategoryResponse> GetCategories()
        {
            var result = new GetCategoryResponse();
            try
            {
                using (var dbConnection = Connection)
                {
                    var categoryResponses = await _manageCategoryRepository.GetCategories(true, dbConnection);
                    if (categoryResponses.Count > 0)
                    {
                        result.categoryResponse = categoryResponses;
                        result.RstKey = 1;
                        result.IsSuccess = true;
                        result.Message = ResponseMessages.DATA_RECEIVED;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = ResponseMessages.DATA_NOT_RECEIVED;
                        result.RstKey = 2;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<MemoryStream> ExportGetCategoryListtReport(GetCategoryListRequest request)
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
        private async Task GenerateData(GetCategoryListRequest request)
        {

            try
            {
                ICellStyle style1 = _hssfWorkbook.CreateCellStyle();
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style1.FillPattern = FillPattern.SolidForeground;

                var id = Guid.Parse(request.userGuid.ToString());
                //var result = await _employerRepository.GetEmployerDetailByGuid(id);
                var response = await _manageCategoryRepository.GetCategoryListForCsv(request.Status, request.FromDate, request.ToDate);

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
                C01.SetCellValue("Category Name");
                C01.CellStyle = style1;

                var C02 = R0.CreateCell(2);
                C02.SetCellValue("Sub-Category");
                C02.CellStyle = style1;

                var C03 = R0.CreateCell(3);
                C03.SetCellValue("Service Name");
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
                    C1.SetCellValue(item.MainCategoryName);

                    var C2 = row.CreateCell(2);
                    C2.SetCellValue(item.SubCategoryName);

                    var c3 = row.CreateCell(3);
                    c3.SetCellValue(item.ServiceName);

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
