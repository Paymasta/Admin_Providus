using PayMasta.Entity.BankDetail;
using PayMasta.Entity.Earning;
using PayMasta.Entity.ErrorLog;
using PayMasta.Entity.ExpressWallet;
using PayMasta.Entity.TransactionLog;
using PayMasta.Entity.VirtualAccountDetail;
using PayMasta.ViewModel.EWAVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Withdrawals
{
    public interface IWithdrawalsRepository
    {
        Task<List<AccessAmountViewModel>> GetEmployeesEwaRequestList(int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null);
        Task<AccessAmountViewDetail> GetEmployeesEwaRequestDetail(long UserId, long AccessAmountId, IDbConnection exdbConnection = null);
        Task<List<AccessAmountViewModel>> GetEmployeesEwaRequestListForCsv(int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null);
        Task<BankDetail> GetBankDetailByUserId(long UserId, IDbConnection exdbConnection = null);
        Task<PayMasta.Entity.AccessAmountRequest.AccessAmountRequest> GetEmployeesEwaRequestDetailByUserId(long UserId, long AccessAmountId, IDbConnection exdbConnection = null);
        Task<int> UpdateEwaStatus(PayMasta.Entity.AccessAmountRequest.AccessAmountRequest accessAmountRequest, IDbConnection exdbConnection = null);
        Task<AccessdAmountPercentageResponse> GetAccessAmountPercentage(long userId, IDbConnection exdbConnection = null);
        Task<EarningMasterResponse> GetEarnings(long userId, IDbConnection exdbConnection = null);
        Task<EarningMaster> GetEarningByEarningId(long userId, long id, IDbConnection exdbConnection = null);
        Task<int> UpdateEwaEarning(EarningMaster earningMaster, IDbConnection exdbConnection = null);
        Task<int> InsertTransactionLog(TransactionLog transactionLog, IDbConnection exdbConnection = null);
        Task<VirtualAccountDetail> GetVirtualAccountDetailByUserId(long UserId, IDbConnection exdbConnection = null);
        Task<AccessAmountViewDetail> GetEmployeesEwaRequestDetail1(long UserId, long AccessAmountId, IDbConnection exdbConnection = null);
        Task<ExpressVirtualAccountDetail> GetExpressVirtualAccountDetailByUserId(long UserId, IDbConnection exdbConnection = null);
    }
}
