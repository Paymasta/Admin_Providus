using PayMasta.ViewModel.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Home
{
    public interface IDashboardService
    {
        Task<DashboardResponse> GetDashboardData(DashboardRequest request);
    }
}
