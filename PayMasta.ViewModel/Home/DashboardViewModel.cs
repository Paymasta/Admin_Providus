using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.Home
{
    public class Dashboard
    {
        public int TotalEmployees { get; set; }
        public int TotalUpdateProfileRequest { get; set; }

        public int TotalUser { get; set; }
        public int TotalEmployer { get; set; }
        public int TotalQueries { get; set; }
        public int TotalTransactions { get; set; }
        public int TotalWithdrawRequest { get; set; }

        public decimal TotalCommisionEarning { get; set; }
        public int TotalEWASent { get; set; }
        public int TotalEWACommisionEarned { get; set; }
    }
    public class DashboardRequest
    {
        public DashboardRequest()
        {
            FromDate = null;
            ToDate = null;
            Month = 0;
        }
        public Guid UserGuid { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int Month { get; set; }
    }
    public class DashboardResponse
    {
        public DashboardResponse()
        {
            dashboard = new Dashboard();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public Dashboard dashboard { get; set; }
    }
}
