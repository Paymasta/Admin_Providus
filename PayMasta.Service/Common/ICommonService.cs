using PayMasta.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Common
{
    public interface ICommonService
    {
        Task<InvoiceNumberResponse> GetInvoiceNumber(int digit = 6);
        bool IsSessionValid(string token);
        Task<int> GetProvidusBankResponse(string request);
    }
}
