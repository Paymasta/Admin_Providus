using PayMasta.Entity.MainCategory;
using PayMasta.Entity.SubCategory;
using PayMasta.Entity.WalletService;
using PayMasta.ViewModel.ManageCategoryVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.ManageCategory
{
    public interface IManageCategoryRepository
    {
        Task<List<ManageCategories>> GetCategoryList(int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null);
        Task<WalletService> GetWalletServiceById(long id, IDbConnection exdbConnection = null);
        Task<int> UpdateWalletServiceStatus(WalletService walletService, IDbConnection exdbConnection = null);
        Task<GetCategoryDetailResponse> ViewCategoryDetailByGuid(Guid id, IDbConnection exdbConnection = null);
        Task<SubCategory> GetSubCategoryDetail(Guid id, IDbConnection exdbConnection = null);
        Task<MainCategory> GetMainCategoryDetail(long id, IDbConnection exdbConnection = null);
        Task<int> UpdateMainCategoryDetail(MainCategory mainCategory, IDbConnection exdbConnection = null);
        Task<int> UpdateSubCategoryDetail(SubCategory subCategory, IDbConnection exdbConnection = null);
        Task<SubCategory> GetSubCategoryDetailByName(string name, IDbConnection exdbConnection = null);
        Task<MainCategory> GetMainCategoryDetailByName(string name, IDbConnection exdbConnection = null);
        Task<int> AddMainCategoryDetail(MainCategory mainCategory, IDbConnection exdbConnection = null);
        Task<int> AddSubCategoryDetail(SubCategory subCategory, IDbConnection exdbConnection = null);
        Task<List<CategoryResponse>> GetCategories(bool isactive, IDbConnection exdbConnection = null);
        Task<List<ManageCategories>> GetCategoryListForCsv(int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null);
    }
}
