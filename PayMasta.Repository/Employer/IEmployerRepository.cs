using PayMasta.ViewModel.EmployerVM;
using PayMasta.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer
{
    public interface IEmployerRepository
    {
        Task<List<EmployerResponse>> GetEmployerList(int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null);
        Task<EmployerProfileResponse> GetEmployerProfile(long userId, IDbConnection exdbConnection = null);
        Task<GetEmployerDetailResponse> GetEmployerDetailByGuid(Guid userGuid, IDbConnection exdbConnection = null);
        Task<List<EmployeesListViewModel>> GetEmployeesListByEmployerId(long employerId, int pageNumber, int pageSize, string searchText, IDbConnection exdbConnection = null);
        Task<List<EmployeesWithdrawls>> GetEmployeesWithdrwals(long userid, int month, int pageSize, int pageNumber, IDbConnection exdbConnection = null);
        Task<EmployeeEwaDetail> GetEmployeesEwaRequestDetail(long userid, IDbConnection exdbConnection = null);
        Task<List<EmployerResponse>> GetEmployerListForCsv(int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null);
        Task<List<EmployeesListViewModel>> GetEmployeesListByEmployerIdForCsv(long employerId, IDbConnection exdbConnection = null);
    }
}
