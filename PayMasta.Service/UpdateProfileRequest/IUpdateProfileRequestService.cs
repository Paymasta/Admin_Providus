using PayMasta.ViewModel.UpdateProfileRequestVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.UpdateProfileRequest
{
    public interface IUpdateProfileRequestService
    {
        Task<UpdateProfileResponse> GetUpdateProfileRequestList(UpdateProfileListRequest request);
        Task<GetOldProfileDetailResponse> GetOldProfileDetail(GetOldProfileDetailRequest request);
        Task<GetOldProfileDetailResponse> GetNewProfileDetail(GetOldProfileDetailRequest request);
        Task<UpdateUserProfileResponse> UpdateEmployeeProfileByAdmin(UpdateProfileDetailRequest request);
        Task<UpdateUserProfileResponse> DeleteProfileRequest(DeleteProfileDetailRequest request);
        Task<MemoryStream> ExportEmployeesListReport(UpdateProfileListRequest request);
    }
}
