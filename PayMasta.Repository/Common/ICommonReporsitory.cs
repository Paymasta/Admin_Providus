using PayMasta.Entity.ErrorLog;
using PayMasta.Entity.UserMaster;
using PayMasta.Entity.UserSession;
using PayMasta.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Common
{
    public interface ICommonReporsitory
    {
        Task<RandomInvoiceNumber> GetInvoiceNumber(IDbConnection dbConnection);
        UserSession GetUserSessionByToken(string token, IDbConnection exdbConnection = null);
        UserMaster GetUserByIdForSession(long userId, IDbConnection exdbConnection = null);
        Task<int> InsertProvidusBankResponse(ErrorLog errorLog, IDbConnection exdbConnection = null);
    }
}
