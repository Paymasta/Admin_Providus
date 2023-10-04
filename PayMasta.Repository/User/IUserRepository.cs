using PayMasta.Entity.BankDetail;
using PayMasta.Entity.UserMaster;
using PayMasta.Entity.UserSession;
using PayMasta.ViewModel.BillHistory;
using PayMasta.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.User
{
    public interface IUserRepository
    {
        Task<List<EmployeesListViewModel>> GetEmployeesList(int pendingemp,int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null);

        Task<EmployeeDetail> GetEmployeeDetailByGuid(Guid userGuid, IDbConnection exdbConnection = null);
        Task<GetAdminDetailResponse> GetAdminDetailByGuid(Guid userGuid, IDbConnection exdbConnection = null);
        Task<UserMaster> GetUserByGuid(Guid guid, IDbConnection exdbConnection = null);
        Task<int> UpdateUser(UserMaster userEntity, IDbConnection exdbConnection = null);
        Task<int> DeleteSession(long userId);
        long GetUserIdByGuid(Guid guid, IDbConnection exdbConnection = null);
        Task<UserSession> GetSessionByUserId(long userId);
        Task<int> UpdateSession(UserSession session);
        Task<List<EmployeesListViewModel>> GetEmployeesListForCsv(int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null);
        Task<List<EmployeesListViewModel>> GetEmployeesListForCsv(int pendingemp, int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null);
        Task<List<EmployeesListViewModel>> GetEmployeesListForTransactions(int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null);
        Task<List<EmployeesListViewModel>> GetEmployeesListForNotification(int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null);
        Task<GetEmployerDetailResponse> GetEmployerDetailByGuid(Guid userGuid, IDbConnection exdbConnection = null);
        Task<List<EmployeesListViewModel>> GetEmployeesListByEmployerId(long employerId, IDbConnection exdbConnection = null);
        Task<List<BankDetail>> GetBankDetailByUserId(long userId, IDbConnection exdbConnection = null);
        Task<int> DeleteBankByBankDetailId(BankDetail bankDetail, IDbConnection exdbConnection = null);
        Task<List<GetBillHistoryList>> GetBillHistoryList(int pendingemp, int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null);
    }
}
