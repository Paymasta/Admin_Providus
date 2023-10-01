using PayMasta.ViewModel.EmployerVM;
using PayMasta.ViewModel.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer
{
    public interface IEmployerService
    {
        Task<EmployerListReponse> GetEmployerList(GetEmployerListRequest request);
        Task<EmployerReponse> GetEmployerProfile(GetEmployerProfileRequest request);
        Task<EmployeesReponse> GetEmployeesByEmployerGuid(GetEmployeesListRequest request);
        Task<EmployeesWithdrawlsResponse> GetEmployeesWithdrwals(EmployeesWithdrawlsRequest request);
        Task<EmployeeEwaDetailReponse> GetEmployeesEwaRequestDetail(EmployeesEWAWithdrawlsRequest request);
        Task<MemoryStream> ExportEmployerListReport(GetEmployerListRequest request);
        Task<MemoryStream> ExportEmployeesListReport(GetEmployeesListRequest request);
    }
}
