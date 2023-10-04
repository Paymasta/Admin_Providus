using PayMasta.ViewModel.EmployerVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.BillHistory
{
    public class GetBillHistoryListRequest
    {
        public GetBillHistoryListRequest()
        {
            ToDate = null;
            FromDate = null;
            SearchTest = "";
            pageNumber = 1;
            PageSize = 10;
            Status = -1;
        }
        public string userGuid { get; set; }
        public string SearchTest { get; set; }
        public int pageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Status { get; set; }
    }

    public class GetBillHistoryList
    {
        public long TotalCount { get; set; }
        public int RowNumber { get; set; }
        public Guid Guid { get; set; }
        public string TotalAmount { get; set; }
        public string WalletAmount { get; set; }
        public int ServiceCategoryId { get; set; }
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public string AccountNo { get; set; }
        public string TransactionId { get; set; }
        public string InvoiceNo { get; set; }
        public int TransactionStatus { get; set; }
        public string TransactionType { get; set; }
        public int SubCategoryId { get; set; }
        public bool IsAmountPaid { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string ServiceName { get; set; }
        public string CreatedAt { get; set; }
    }
    public class GetBillHistoryListReponse
    {
        public GetBillHistoryListReponse()
        {
            getBillHistoryLists = new List<GetBillHistoryList>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<GetBillHistoryList> getBillHistoryLists { get; set; }
    }
}
