using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.EmployerVM
{
    public class EmployerResponse
    {
        public long TotalCount { get; set; }
        public int RowNumber { get; set; }
        public long UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string Status { get; set; }
        public string CreatedAt { get; set; }

        public Guid Guid { get; set; }
        public string OrganisationName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class EmployerListReponse
    {
        public EmployerListReponse()
        {
            employerResponses = new List<EmployerResponse>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<EmployerResponse> employerResponses { get; set; }
    }

    public class GetEmployerListRequest
    {
        public GetEmployerListRequest()
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

    public class GetEmployerProfileRequest
    {
        public Guid EmployerGuid { get; set; }
    }

    public class EmployerProfileResponse
    {

        public long UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public int Status { get; set; }
        public string CreatedAt { get; set; }
        public Guid Guid { get; set; }
        public string OrganisationName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CountryName { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string ProfileImage { get; set; }
    }

    public class EmployerReponse
    {
        public EmployerReponse()
        {
            employerResponses = new EmployerProfileResponse();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public EmployerProfileResponse employerResponses { get; set; }
    }

    public class EmployeeEwaDetailReponse
    {
        public EmployeeEwaDetailReponse()
        {
            employeeEwaDetail = new EmployeeEwaDetail();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public EmployeeEwaDetail employeeEwaDetail { get; set; }
    }
    public class EmployeeEwaDetail
    {
        public long UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StaffId { get; set; }
        public int WorkiingDays { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public decimal EarnedAmount { get; set; }
        public decimal AvailableAmount { get; set; }
    }
}
