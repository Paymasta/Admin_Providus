using PayMasta.ViewModel.SupportVm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Support
{
    public interface ISupportService
    {
        Task<UserViewTicketListReponse> GetSupportTicketList(SupportTicketRequest request);
        Task<UpdateSupportTicketStatusResponse> UpdateSupportTicketStatus(UpdateSupportTicketStatusRequest request);
        Task<ViewSupportTicketDetailResponse> GetSupportTicketDetailByUserId(ViewSupportTicketDetailRequest request);
        Task<MemoryStream> ExportUserListReport(SupportTicketRequest request);
    }
}
