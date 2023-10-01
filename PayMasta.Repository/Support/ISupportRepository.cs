using PayMasta.Entity.SupportMaster;
using PayMasta.ViewModel.SupportVm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Support
{
    public interface ISupportRepository
    {
        Task<List<SupportViewModel>> GetSupportTicketList(int userType,string searchText, int pageNumber, int PageSize, DateTime? fromDate = null, DateTime? toDate = null, int status = 0, IDbConnection exdbConnection = null);
        Task<SupportMaster> GetTicketStatusByTicketId(long ticketId, IDbConnection exdbConnection = null);
        Task<int> UpdateTicketStatus(SupportMaster supportMaster, IDbConnection exdbConnection = null);
        Task<SupportViewModel> GetSupportTicketDetailByUserId(long UserId, long id, IDbConnection exdbConnection = null);
        Task<List<SupportViewModel>> GetSupportTicketListForCsv(int userType, DateTime? fromDate = null, DateTime? toDate = null, int status = -1, IDbConnection exdbConnection = null);
    }
}
