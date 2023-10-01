using PayMasta.ViewModel.TransactionsVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Transactions
{
    public interface ITransactionsRepository
    {
        Task<EmployeeTranactionDetail> GetEmployeeDetailByUserId(long userId, IDbConnection exdbConnection = null);
        Task<List<EmployeeTransactions>> GetEmployeeTransactionByUserId(long userId, int pageSize, int pageNumber, int month, IDbConnection exdbConnection = null);
    }
}
