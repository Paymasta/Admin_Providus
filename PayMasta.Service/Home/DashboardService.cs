using PayMasta.Repository.Home;
using PayMasta.Utilities;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Home;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Home
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardService()
        {
            _dashboardRepository = new DashboardRepository();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        public async Task<DashboardResponse> GetDashboardData(DashboardRequest request)
        {
            var res = new DashboardResponse();

            try
            {
                //if (request.FromDate != null && request.ToDate != null)
                //{
                //    var from = request.FromDate.ToString();
                //    var to = request.FromDate.ToString();
                //    // DateTime dt = DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //    request.FromDate = DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //    request.ToDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //}

                var dashboardData = await _dashboardRepository.GetDashboardData(request.FromDate, request.ToDate,request.Month);
                if (dashboardData != null)
                {
                    res.dashboard = dashboardData;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }
    }
}
