using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.User
{
    public class GetEmployeesListRequest
    {
        public GetEmployeesListRequest()
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

    public class EmployeesReponse
    {
        public EmployeesReponse()
        {
            employeesListViewModel = new List<EmployeesListViewModel>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<EmployeesListViewModel> employeesListViewModel { get; set; }
    }

    public class EmployeesListViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StaffId { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalCount { get; set; }
        public long RowNumber { get; set; }
        public string Status { get; set; }
        public Guid UserGuid { get; set; }
        public long UserId { get; set; }
        public string EmployerName { get; set; }
        public string CreatedAt { get; set; }
        public int StatusId { get; set; }
        public string IsD2CUser { get; set; }
    }
    public class EmployeeDetailResponse
    {
        public EmployeeDetailResponse()
        {
            employeeDetail = new EmployeeDetail();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public EmployeeDetail employeeDetail { get; set; }
    }

    public class EmployeeDetail
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StaffId { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Status { get; set; }
        public Guid UserGuid { get; set; }
        public long UserId { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string CountryName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string EmployerName { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string BVN { get; set; }
        public string BankAccountHolderName { get; set; }
        public string ProfileImage { get; set; }
        public string NetPay { get; set; }
        public string GrossPay { get; set; }
    }
    public class EmployeeDetailRequest
    {
        public Guid AdminGuid { get; set; }
        public Guid UserGuid { get; set; }
    }
    public class BlockUnBlockEmployeeRequest
    {
        public string EmployeeUserGuid { get; set; }
        public string AdminUserGuid { get; set; }
        [Range(1, 2)]
        public int DeleteOrBlock { get; set; }
    }
    public class BlockUnBlockEmployeeResponse
    {
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }
    public class LogoutRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        [Required]
        public string DeviceId { get; set; }
    }
    public class GetAdminDetailResponse
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public int UserType { get; set; }
        public string OrganisationName { get; set; }
    }
    public class GetEmployerDetailResponse
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public int UserType { get; set; }
        public string OrganisationName { get; set; }
    }

    public class EmployeesWithdrawls
    {

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalCount { get; set; }
        public long RowNumber { get; set; }
        public string Status { get; set; }
        public Guid UserGuid { get; set; }
        public long UserId { get; set; }
        public string CreatedAt { get; set; }
        public decimal AccessAmount { get; set; }
        public int StatusId { get; set; }
        public long AccessAmountId { get; set; }
        public Guid AccessAmountGuid { get; set; }
        public string AdminStatus { get; set; }
        public int AdminStatusId { get; set; }
    }

    public class EmployeesWithdrawlsResponse
    {
        public EmployeesWithdrawlsResponse()
        {
            employeesWithdrawls = new List<EmployeesWithdrawls>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<EmployeesWithdrawls> employeesWithdrawls { get; set; }
    }
    public class EmployeesWithdrawlsRequest
    {
        public int Month { get; set; }
        public Guid UserGuid { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

    }
    public class EmployeesEWAWithdrawlsRequest
    {
        public Guid UserGuid { get; set; }
    }
}
