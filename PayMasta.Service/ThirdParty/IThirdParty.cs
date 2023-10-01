using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ThirdParty
{
    public interface IThirdParty
    {
        Task<string> UploadFiles(string image, string fileName);
        Task<string> PostBankTransaction(string req, string url);
        Task<string> FundTransaction(string req, string url, string token);
        Task<string> CreateProvidusVirtualAccount(string req, string url);
        Task<string> GetVirtualAccount(string token, string url);
    }
}
