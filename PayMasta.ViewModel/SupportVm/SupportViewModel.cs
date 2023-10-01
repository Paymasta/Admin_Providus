using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.SupportVm
{
    public class SupportViewModel
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployerName { get; set; }
        public long EmployerId { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string CreatedAt { get; set; }
        public string Status { get; set; }
        public string TicketNumber { get; set; }
        public string Title { get; set; }
        public string DescriptionText { get; set; }
        public int TotalCount { get; set; }
        public int RowNumber { get; set; }
        public Guid UserGuid { get; set; }
        public int StatusId { get; set; }
    }
    public class UserViewTicketListReponse
    {
        public UserViewTicketListReponse()
        {
            supportViewModels = new List<SupportViewModel>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<SupportViewModel> supportViewModels { get; set; }
    }

    public class SupportTicketRequest
    {
        public SupportTicketRequest()
        {
            FromDate = null;
            ToDate = null;
            Month = 0;
            PageSize = 10;
            PageNumber = 1;
            UserType = 0;
            StatusId = -1;
        }
        public Guid UserGuid { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int Month { get; set; }
        public string SearchText { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int UserType { get; set; }
        public int StatusId { get; set; }
    }

    public class UpdateSupportTicketStatusRequest
    {
        public Guid AdminUserGuid { get; set; }
        public long TicketId { get; set; }

        public int Status { get; set; }
    }

    public class UpdateSupportTicketStatusResponse
    {
        public UpdateSupportTicketStatusResponse()
        {

        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }
    public class ViewSupportTicketDetailRequest
    {
        public Guid UserGuid { get; set; }

        public long SupportTicketId { get; set; }
    }

    public class ViewSupportTicketDetailResponse
    {
        public ViewSupportTicketDetailResponse()
        {
            supportViewModel = new SupportViewModel();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        public SupportViewModel supportViewModel { get; set; }
    }
}
