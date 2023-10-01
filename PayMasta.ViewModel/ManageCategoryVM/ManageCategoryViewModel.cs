using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.ManageCategoryVM
{
    public class ManageCategories
    {
        public long MainCategoryId { get; set; }
        public long SubCategoryId { get; set; }
        public string MainCategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid SubCategoryGuid { get; set; }
        public Guid MainCategoryGuid { get; set; }
        public string ServiceName { get; set; }
        public long WalletServiceId { get; set; }
        public string CreatedAt { get; set; }
        public int TotalCount { get; set; }
        public long RowNumber { get; set; }
        public string Status { get; set; }
    }
    public class ManageCategoriesReponse
    {
        public ManageCategoriesReponse()
        {
            manageCategories = new List<ManageCategories>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<ManageCategories> manageCategories { get; set; }
    }
    public class GetCategoryListRequest
    {
        public GetCategoryListRequest()
        {
            ToDate = null;
            FromDate = null;
            SearchTest = "";
            pageNumber = 1;
            PageSize = 10;
            Status = 0;
        }
        public string userGuid { get; set; }
        public string SearchTest { get; set; }
        public int pageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Status { get; set; }
    }
    public class BlockUnBlockCategoryResponse
    {
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }

    public class BlockUnBlockCategoryRequest
    {
        public long WalletServiceId { get; set; }
        public string AdminUserGuid { get; set; }
        [Range(1, 2)]
        public int DeleteOrBlock { get; set; }
    }
    public class GetCategoryDetailRequest
    {
        public Guid SubCategoryGuid { get; set; }
        public string AdminUserGuid { get; set; }
    }
    public class GetCategoryDetailResponse
    {
        public Guid SubCategoryGuid { get; set; }
        public Guid MainCategoryGuid { get; set; }
        public string SubCategoryName { get; set; }
        public string MainCategoryName { get; set; }
        public long Id { get; set; }
        public string ImageUrl { get; set; }
    }
    public class GetCategoryDetail
    {
        public GetCategoryDetail()
        {
            getCategoryDetailResponse=new GetCategoryDetailResponse();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public GetCategoryDetailResponse getCategoryDetailResponse { get; set; }
    }

    public class UpdateCategoryDetailRequest
    {
        public Guid SubCategoryGuid { get; set; }
        public string AdminUserGuid { get; set; }
        public string SubCategory { get; set; }
        public string MainCategory { get; set; }
    }
    public class UpdateCategoryDetailResponse
    {
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }

    public class AddCategoryDetailRequest
    {
        public string AdminUserGuid { get; set; }
        public string SubCategory { get; set; }
        public string MainCategory { get; set; }
    }
    public class CategoryResponse
    {
        public long Id { get; set; }
        public string CategoryName { get; set; }
        public int MainCategoryId { get; set; }
    }
    public class GetCategoryResponse
    {
        public GetCategoryResponse()
        {
            categoryResponse = new List<CategoryResponse>();
        }
        public List<CategoryResponse> categoryResponse { get; set; }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }
}
