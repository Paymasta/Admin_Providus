using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.TransactionsVM
{
    public class EmployeeTransactionDetailResponse
    {
        public EmployeeTransactionDetailResponse()
        {
            employeeDetail = new EmployeeTranactionDetail();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public EmployeeTranactionDetail employeeDetail { get; set; }
    }

    public class EmployeeTranactionDetail
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string Email { get; set; }
     
        public Guid UserGuid { get; set; }
        public long UserId { get; set; }
    
        public string EmployerName { get; set; }
        public long EmployerId { get; set; }

        public decimal EarnedAmount { get; set; }
        public decimal AvailableAmount { get; set; }
        public decimal TotalWorkingHours { get; set; }
        public string CreatedAt { get; set; }
    }

    public class TransactionsResponse
    {
        public TransactionsResponse()
        {
            employeeTransactions = new List<EmployeeTransactions>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<EmployeeTransactions> employeeTransactions { get; set; }
    }

    public class EmployeeTransactions
    {
        public long TotalCount { get; set; }
        public long RowNumber { get; set; }
        public long WalletTransactionId { get; set; }
        public Guid Guid { get; set; }
        public string TotalAmount { get; set; }
        public string BillerName { get; set; }
        public string CreatedAt { get; set; }
    }
    public class EmployeeTransactionRequest
    {
        public EmployeeTransactionRequest()
        {
            Month=0;
            PageNumber=1;
            PageSize=10;
        }
        public Guid AdminGuid { get; set; }
        public Guid UserGuid { get; set; }

        public int Month { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
