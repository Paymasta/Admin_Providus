using PayMasta.ViewModel.TransactionsVM;
using PayMasta.ViewModel.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Transactions
{
    public interface ITransactionsService
    {
        Task<EmployeesReponse> GetEmployeesList(GetEmployeesListRequest request);
        Task<EmployeeTransactionDetailResponse> GetEmployeDetailByGuid(EmployeeDetailRequest request);
        Task<TransactionsResponse> GetEmployeeTransactionByGuid(EmployeeTransactionRequest request);
        Task<MemoryStream> ExportEmployeesListReport(GetEmployeesListRequest request);
        Task<EmployeesReponse> GetEmployeesListForTransactions(GetEmployeesListRequest request);
    }
}
