using PayMasta.Entity.UpdateUserProfileRequest;
using PayMasta.Entity.UserMaster;
using PayMasta.ViewModel.UpdateProfileRequestVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.UpdateProfileRequest
{
    public interface IUpdateProfileRequestRepository
    {
        Task<int> RemoveUpdateProfileRequest(Guid requestGuid, IDbConnection dbConnection = null);
        Task<int> UpdateUserProfileStatusByAdmin(UpdateUserProfileRequest userEntity, IDbConnection exdbConnection = null);
        Task<int> UpdateUserProfileByAdmin(UserMaster userEntity, IDbConnection exdbConnection = null);
        Task<UpdateProfileRequestResponse> GetOldProfileDetail(long userId, IDbConnection exdbConnection = null);
        Task<List<UpdateProfileRequestResponse>> GetUpdateProfileRequestList(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null, string searchText = null, IDbConnection exdbConnection = null);
        Task<UpdateProfileRequestResponse> GetNewProfileDetail(long userId, IDbConnection exdbConnection = null);
        Task<UpdateProfileRequestResponse> GetNewProfileDetailForUpdateByUpdateRequestId(long userId, long UpdateUserProfileRequestId, IDbConnection exdbConnection = null);
        Task<long> GetUserIdFromUpdateProfileRequestTable(Guid requestGuid, IDbConnection exdbConnection = null);
        long GetUserIdByUpdateProfileRequestGuid(Guid guid, IDbConnection exdbConnection = null);
        Task<List<UpdateProfileRequestResponse>> GetUpdateProfileRequestListForScv(DateTime? fromDate = null, DateTime? toDate = null, IDbConnection exdbConnection = null);
    }
}
