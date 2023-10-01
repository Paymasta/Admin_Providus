using PayMasta.ViewModel.WithdrawlsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.ExpressWalletVM
{
    public class Metadata
    {
        public string somedata { get; set; }
        public string moredata { get; set; }
    }

    public class CustomerWalletCreditRequest
    {
        public decimal amount { get; set; }
        public string reference { get; set; }
        public string customerId { get; set; }
        public Metadata metadata { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public Data()
        {
            metadata = new Metadata();
        }
        public int amount { get; set; }
        public string reference { get; set; }
        public string customer_id { get; set; }
        public Metadata metadata { get; set; }
        public int transaction_fee { get; set; }
        public string customer_wallet_id { get; set; }
    }

    public class CustomerWalletCreditResponse
    {
        public CustomerWalletCreditResponse()
        {
            data = new Data();
        }
        public bool status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class ExpressFundTransferResponse
    {
        public ExpressFundTransferResponse()
        {
            transferResponse = new CustomerWalletCreditResponse();
        }
        public CustomerWalletCreditResponse transferResponse { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public int RstKey { get; set; }
    }
}
