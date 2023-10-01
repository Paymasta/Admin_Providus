using PayMasta.ViewModel.ManageCategoryVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ManageCategory
{
    public interface IManageCategoryService
    {
        Task<ManageCategoriesReponse> GetCategoryList(GetCategoryListRequest request);
        Task<BlockUnBlockCategoryResponse> BlockUnBlockCategory(BlockUnBlockCategoryRequest request);
        Task<GetCategoryDetail> ViewCategoryDetail(GetCategoryDetailRequest request);
        Task<UpdateCategoryDetailResponse> UpdateCategoryDetail(UpdateCategoryDetailRequest request);
        Task<UpdateCategoryDetailResponse> AddCategoryAndSubCategory(AddCategoryDetailRequest request);
        Task<GetCategoryResponse> GetCategories();
        Task<MemoryStream> ExportGetCategoryListtReport(GetCategoryListRequest request);
    }
}
