using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.UpdateProfileRequestVM
{
    public class UpdateProfileRequestResponse
    {
        public int TotalCount { get; set; }
        public int RowNumber { get; set; }
        public long UserId { get; set; }
        public Guid Guid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string MiddleName { get; set; }
        public string ProfileImage { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
        public Guid UpdateUserProfileRequestGuid { get; set; }

        public string Comment { get; set; }
    }
    public class UpdateProfileResponse
    {
        public UpdateProfileResponse()
        {
            updateProfileRequestResponses = new List<UpdateProfileRequestResponse>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<UpdateProfileRequestResponse> updateProfileRequestResponses { get; set; }
    }

    public class UpdateProfileListRequest
    {
        public Guid userGuid { get; set; }
        public string SearchTest { get; set; }
        public int pageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? Todate { get; set; }
    }
    public class GetOldProfileDetailRequest
    {
        public Guid userGuid { get; set; }
    }

    public class GetOldProfileDetailResponse
    {
        public GetOldProfileDetailResponse()
        {
            updateProfileRequestResponse = new UpdateProfileRequestResponse();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public UpdateProfileRequestResponse updateProfileRequestResponse { get; set; }
    }
    public class UpdateProfileDetailRequest
    {
        public Guid UserGuid { get; set; }
        public Guid AdminGuid { get; set; }
        public long UpdateUserProfileRequestId { get; set; }
    }
    public class UpdateUserProfileResponse
    {

        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

    }

    public class DeleteProfileDetailRequest
    {
      
        public Guid AdminGuid { get; set; }
        public Guid UpdateUserProfileRequestId { get; set; }
    }
}
