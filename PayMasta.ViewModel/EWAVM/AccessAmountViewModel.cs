using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.EWAVM
{
    public class AccessAmountViewModel
    {
        public int TotalCount { get; set; }
        public int RowNumber { get; set; }
        public long UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployerName { get; set; }
        public long EmployerId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int StatusId { get; set; }
        public bool IsEwaApprovalAccess { get; set; }
        public decimal AccessAmount { get; set; }
        public long AccessAmountId { get; set; }
        public Guid AccessAmountGuid { get; set; }
        public string CreatedAt { get; set; }
        public string AdminStatus { get; set; }
        public int AdminStatusId { get; set; }
        public string IsD2CUser { get; set; }
    }
    public class AccessAmountViewModelResponse
    {
        public AccessAmountViewModelResponse()
        {
            accessAmountViewModels = new List<AccessAmountViewModel>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<AccessAmountViewModel> accessAmountViewModels { get; set; }
    }

    public class AccessAmountRequest
    {
        public AccessAmountRequest()
        {
            ToDate = null;
            FromDate = null;
            SearchTest = "";
            pageNumber = 1;
            PageSize = 10;
            Status = -1;
        }
        public string userGuid { get; set; }
        public string SearchTest { get; set; }
        public int pageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Status { get; set; }
    }


    public class AccessAmountViewDetail
    {
        public int TotalCount { get; set; }
        public int RowNumber { get; set; }
        public long UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployerName { get; set; }
        public long EmployerId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int StatusId { get; set; }
        public string ProfileImage { get; set; }
        public decimal AccessAmount { get; set; }
        public long AccessAmountId { get; set; }
        public Guid AccessAmountGuid { get; set; }
        public string CreatedAt { get; set; }
        public decimal EarnedAmount { get; set; }
        public int Hours { get; set; }
        public string StaffId { get; set; }
        public decimal AvailableAmount { get; set; }

        public int AdminStatus { get; set; }
    }
    public class AccessAmountViewDetailResponse
    {
        public AccessAmountViewDetailResponse()
        {
            accessAmountViewDetail = new AccessAmountViewDetail();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public AccessAmountViewDetail accessAmountViewDetail { get; set; }
    }

    public class AccessAmountViewDetailRequest
    {
        public Guid UserGuid { get; set; }
        public long AccessAmountId { get; set; }
    }
    public class AccessdAmountPercentageResponse
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public decimal AccessedPercentage { get; set; }
    }
    public class EarningMasterResponse
    {
        public long UserId { get; set; }
        public decimal EarnedAmount { get; set; }
        public decimal AccessedAmount { get; set; }
        public decimal AvailableAmount { get; set; }
        public string PayCycleFrom { get; set; }
        public string PayCycleTo { get; set; }
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
