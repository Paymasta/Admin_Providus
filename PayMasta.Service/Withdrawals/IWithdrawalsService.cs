using PayMasta.ViewModel.EWAVM;
using PayMasta.ViewModel.ExpressWalletVM;
using PayMasta.ViewModel.WithdrawlsVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Withdrawals
{
    public interface IWithdrawalsService
    {
        Task<AccessAmountViewModelResponse> GetEmployeesEwaRequestList(AccessAmountRequest request);
        Task<AccessAmountViewDetailResponse> GetEmployeesEwaRequestDetail(AccessAmountViewDetailRequest request);
        Task<MemoryStream> ExportEmployeesListReport(AccessAmountRequest request);
        Task<ProvidusFundResponse> ProvidusFundTransfer(ProvidusFundTransferRequest request);
        Task<FundTransferResponse> FundTransferInWallet(ProvidusFundTransferRequest request);
        Task<FundTransferResponse> RejectSystemSpecsTransfer(ProvidusFundTransferRequest request);
        Task<ExpressFundTransferResponse> FundTransferInExpressWallet(ProvidusFundTransferRequest request);
    }
}
