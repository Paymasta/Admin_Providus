using PayMasta.ViewModel.Home;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Home
{
    public interface IDashboardRepository
    {
        Task<Dashboard> GetDashboardData(DateTime? fromDate = null, DateTime? toDate = null, int month = 0, IDbConnection exdbConnection = null);
    }
}
