using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.WithdrawlsVM
{
    public class ProvidusFundResponse
    {
        public ProvidusFundResponse()
        {
            transferResponse = new ProvidusFundTransferResponse();
        }
        public ProvidusFundTransferResponse transferResponse { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public int RstKey { get; set; }
    }
    public class FundTransferResponse
    {
        public FundTransferResponse()
        {
            transferResponse = new WalletToWalletTransferResponse();
        }
        public WalletToWalletTransferResponse transferResponse { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public int RstKey { get; set; }
    }
    public class ProvidusFundTransferResponse
    {
        public string amount { get; set; }
        public string transactionReference { get; set; }
        public string currency { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
    }
    public class ProvidusFundTransferRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        [Required]
        public Guid AdminGuid { get; set; }
        [Required]
        public long AccessAmountId { get; set; }
    }
    public class ProvidusFundTransfer
    {
        public string creditAccount { get; set; }
        public string debitAccount { get; set; }
        public string transactionAmount { get; set; }
        public string currencyCode { get; set; }
        public string narration { get; set; }
        public string transactionReference { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class NIPFundTransferTrquest
    {
        public string beneficiaryAccountName { get; set; }
        public string transactionAmount { get; set; }
        public string currencyCode { get; set; }
        public string narration { get; set; }
        public string sourceAccountName { get; set; }
        public string beneficiaryAccountNumber { get; set; }
        public string beneficiaryBank { get; set; }
        public string transactionReference { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }

    #region wallettowallet transfer
    public class WalletToWalletTransferRequest
    {
        public string accountNumber { get; set; }
        public decimal amount { get; set; }
        public string channel { get; set; }
        public string sourceBankCode { get; set; }
        public string sourceAccountNumber { get; set; }
        public string destBankCode { get; set; }
        public string pin { get; set; }
        public string transRef { get; set; }
        public bool isToBeSaved { get; set; }
        public string beneficiaryName { get; set; }
        // public string specificChannel { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    //public class WalletToWalletTransferResponse
    //{
    //    public string message { get; set; }
    //    public string code { get; set; }
    //    public bool error { get; set; }
    //    public string paymentTransactionDTO { get; set; }
    //}

    #endregion

    #region Authenticate
    public class AuthenticateRequest
    {
        public string password { get; set; }
        public bool rememberMe { get; set; }
        public string username { get; set; }
        public string scheme { get; set; }
        public string deviceId { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Kyc
    {
        public int id { get; set; }
        public int kycID { get; set; }
        public string kyc { get; set; }
        public string description { get; set; }
        public int kycLevel { get; set; }
        public bool phoneNumber { get; set; }
        public bool emailAddress { get; set; }
        public bool firstName { get; set; }
        public bool lastName { get; set; }
        public bool gender { get; set; }
        public bool dateofBirth { get; set; }
        public bool address { get; set; }
        public bool photoUpload { get; set; }
        public bool verifiedBVN { get; set; }
        public bool verifiedValidID { get; set; }
        public bool evidenceofAddress { get; set; }
        public object verificationofAddress { get; set; }
        public object employmentDetails { get; set; }
        public double dailyTransactionLimit { get; set; }
        public double cumulativeBalanceLimit { get; set; }
        public object paymentTransaction { get; set; }
        public object billerTransaction { get; set; }
    }

    public class AuthenticateResponse
    {
        public AuthenticateResponse()
        {
            user = new User();
            walletAccountList = new List<WalletAccountList>();
        }
        public string message { get; set; }
        public object code { get; set; }
        public string token { get; set; }
        public User user { get; set; }
        public string userType { get; set; }
        public List<WalletAccountList> walletAccountList { get; set; }
    }

    public class User
    {
        public User()
        {
            user = new UserData();
            kyc = new Kyc();
        }
        public DateTime createdDate { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public int id { get; set; }
        public string profileID { get; set; }
        public string pin { get; set; }
        public object deviceNotificationToken { get; set; }
        public string phoneNumber { get; set; }
        public string gender { get; set; }
        public string dateOfBirth { get; set; }
        public string address { get; set; }
        public object secretQuestion { get; set; }
        public object secretAnswer { get; set; }
        public object photo { get; set; }
        public object photoContentType { get; set; }
        public object bvn { get; set; }
        public object validID { get; set; }
        public string validDocType { get; set; }
        public object nin { get; set; }
        public object profilePicture { get; set; }
        public double totalBonus { get; set; }
        public object walletAccounts { get; set; }
        public object myDevices { get; set; }
        public object paymentTransactions { get; set; }
        public object billerTransactions { get; set; }
        public object customersubscriptions { get; set; }
        public UserData user { get; set; }
        public object bonusPoints { get; set; }
        public object approvalGroup { get; set; }
        public object profileType { get; set; }
        public Kyc kyc { get; set; }
        public object beneficiaries { get; set; }
        public object addresses { get; set; }
        public string fullName { get; set; }
        public string login { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public bool activated { get; set; }
        public object langKey { get; set; }
        public string imageUrl { get; set; }
        public object resetDate { get; set; }
        public string status { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class UserData
    {
        public DateTime createdDate { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public int id { get; set; }
        public string login { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public bool activated { get; set; }
        public object langKey { get; set; }
        public string imageUrl { get; set; }
        public object resetDate { get; set; }
        public string status { get; set; }
    }


    public class WalletAccountList
    {
        public int id { get; set; }
        public string accountNumber { get; set; }
        public double currentBalance { get; set; }
        public string dateOpened { get; set; }
        public int schemeId { get; set; }
        public string schemeName { get; set; }
        public int walletAccountTypeId { get; set; }
        public int accountOwnerId { get; set; }
        public string accountOwnerName { get; set; }
        public string accountOwnerPhoneNumber { get; set; }
        public string accountName { get; set; }
        public string status { get; set; }
        public double actualBalance { get; set; }
        public string walletLimit { get; set; }
        public string trackingRef { get; set; }
        public string nubanAccountNo { get; set; }
        public string accountFullName { get; set; }
        public double totalCustomerBalances { get; set; }
    }



    #endregion Authenticate

    public class PaymentTransactionDTO
    {
        public int id { get; set; }
        public double changes { get; set; }
        public DateTime dueDate { get; set; }
        public string paymentType { get; set; }
        public string reference { get; set; }
        public string memo { get; set; }
        public object narration { get; set; }
        public object externalRef { get; set; }
        public string transDate { get; set; }
        public string transactionStatus { get; set; }
        public object userComment { get; set; }
        public double amount { get; set; }
        public string displayMemo { get; set; }
    }

    public class WalletToWalletTransferResponse
    {
        public WalletToWalletTransferResponse()
        {
            paymentTransactionDTO = new PaymentTransactionDTO();
        }
        public string message { get; set; }
        public string code { get; set; }
        public bool error { get; set; }
        public string status { get; set; }
        public PaymentTransactionDTO paymentTransactionDTO { get; set; }
    }
}
