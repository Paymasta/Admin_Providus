using PayMasta.ViewModel.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.User
{
    public interface IUserService
    {
        Task<EmployeesReponse> GetEmployeesList(GetEmployeesListRequest request);
        Task<EmployeeDetailResponse> GetEmployeDetailByGuid(EmployeeDetailRequest request);
        Task<BlockUnBlockEmployeeResponse> BlockUnBlockEmployees(BlockUnBlockEmployeeRequest request);
        Task<MemoryStream> ExportEmployeesListReport(GetEmployeesListRequest request);
        Task<EmployeesReponse> GetEmployeesListForNotification(GetEmployeesListRequest request);
        Task<BlockUnBlockEmployeeResponse> BlockUnBlockEmployer(BlockUnBlockEmployeeRequest request);
    }
}
