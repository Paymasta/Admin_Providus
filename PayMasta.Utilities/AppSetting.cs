using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Utilities
{
    public class AppSetting
    {
        public static string GetImagePath = ConfigurationManager.AppSettings["GetImagePath"];

        public static string ConnectionStrings = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static string ForgotTemplate = "ForgotPasswordEmailTemplate.html";
        public static string AccountStatusTemplate = "AccountStatusTemplate.html";
        public static string RegistrationTemplate = "RegistrationTemplate.html";
        public static string EmailVerificationTemplate = "EmailVerificationTemplate.html";
        public static string ChangePassword = "ChangePasswordEmailTemplate.html";

        public static string SMS_Path = ConfigurationManager.AppSettings["SMS_Path"];
        public static string SMS_application = ConfigurationManager.AppSettings["SMS_application"];
        public static string SMS_source = ConfigurationManager.AppSettings["SMS_source"];
        public static string SMS_Password = ConfigurationManager.AppSettings["SMS_Password"];
        public static string SMS_mask = ConfigurationManager.AppSettings["SMS_mask"];

        public static string HostNameAdmin = ConfigurationManager.AppSettings["HostNameAdmin"];
        public static string VerifyMailLink = HostNameAdmin + "/" + ConfigurationManager.AppSettings["VerifyMailLink"];
        /// <summary>
        /// SMTP
        /// </summary>
        public static string SMTP_USERNAME = ConfigurationManager.AppSettings["SMTP_USERNAME"];
        public static string SMTP_PASSWORD = ConfigurationManager.AppSettings["SMTP_PASSWORD"];
        public static string CONFIGSET = ConfigurationManager.AppSettings["CONFIGSET"];
        public static string HOST = ConfigurationManager.AppSettings["HOST"];
        public static string FROM = ConfigurationManager.AppSettings["FROM"];
        public static string FROMNAME = ConfigurationManager.AppSettings["FROMNAME"];
        public static string PORT = ConfigurationManager.AppSettings["PORT"];

        public static string AccountSidTwilio = ConfigurationManager.AppSettings["AccountSidTwilio"];
        public static string AuthTokenTwilio = ConfigurationManager.AppSettings["AuthTokenTwilio"];
        public static string MessagingServiceSid = ConfigurationManager.AppSettings["MessagingServiceSid"];

        /// <summary>
        /// Send Grid SMTP
        /// </summary>
        public static string sendgridKey = ConfigurationManager.AppSettings["sendgridKey"];
        public static string SendEmailFrom = ConfigurationManager.AppSettings["SendEmailFrom"];
        public static string DisplayName = ConfigurationManager.AppSettings["DisplayName"];

        /// <summary>
        /// JWT
        /// </summary>
        public static string JwtKey = ConfigurationManager.AppSettings["JwtKey"];
        public static string JwtExpireDays = ConfigurationManager.AppSettings["JwtExpireDays"];
        public static string JwtIssuer = ConfigurationManager.AppSettings["JwtIssuer"];

        /// <summary>
        /// NIN
        /// </summary>
        public static string NinVerifyUrl = ConfigurationManager.AppSettings["NinVerifyUrl"];
        public static string NinSecretKey = ConfigurationManager.AppSettings["NinSecretKey"];

        /// <summary>
        /// flutter wave urls
        /// </summary>
        public static string FlutterWaveVertualAccountUrl = ConfigurationManager.AppSettings["FlutterWaveVertualAccountUrl"];
        public static string FlutterWaveSecretKey = ConfigurationManager.AppSettings["FlutterWaveSecretKey"];
        public static string FlutterWaveAirtimeOperator = ConfigurationManager.AppSettings["FlutterWaveAirtimeOperator"];
        public static string FlutterWaveInternetOperator = ConfigurationManager.AppSettings["FlutterWaveInternetOperator"];
        public static string FlutterWaveTvOperator = ConfigurationManager.AppSettings["FlutterWaveTvOperator"];
        public static string FlutterWaveBillsOperator = ConfigurationManager.AppSettings["FlutterWaveBillsOperator"];
        /// <summary>
        /// Bank details
        /// </summary>
        public static string GetNIPBanks = ConfigurationManager.AppSettings["GetNIPBanks"];
        public static string BankUserName = ConfigurationManager.AppSettings["BankUserName"];
        public static string BankPassword = ConfigurationManager.AppSettings["BankPassword"];
        public static string GetNIPAccount = ConfigurationManager.AppSettings["GetNIPAccount"];
        public static string NIPFundTransfer = ConfigurationManager.AppSettings["NIPFundTransfer"];
        public static string ProvidusFundTransfer = ConfigurationManager.AppSettings["ProvidusFundTransfer"];
        public static string currencyCode = ConfigurationManager.AppSettings["currencyCode"];
        public static string narration = ConfigurationManager.AppSettings["narration"];
        public static string GetProvidusAccount = ConfigurationManager.AppSettings["GetProvidusAccount"];
        public static string PayMastaAccountNumber = ConfigurationManager.AppSettings["PayMastaAccountNumber"];
        public static string sourceAccountName = ConfigurationManager.AppSettings["sourceAccountName"];
        /// <summary>
        /// okra details
        /// </summary>
        public static string OkraLinkUrl = ConfigurationManager.AppSettings["OkraLinkUrl"];
        public static string okracallback_url = ConfigurationManager.AppSettings["okracallback_url"];
        public static string Okralogo = ConfigurationManager.AppSettings["Okralogo"];
        public static string OkraUserName = ConfigurationManager.AppSettings["OkraUserName"];
        public static string Okrasupport_email = ConfigurationManager.AppSettings["Okrasupport_email"];
        public static string continue_cta = ConfigurationManager.AppSettings["continue_cta"];
        public static string OkraToken = ConfigurationManager.AppSettings["OkraToken"];
        public static string WidgetLink = ConfigurationManager.AppSettings["WidgetLink"];
        public static string OkraGetBankList = ConfigurationManager.AppSettings["OkraGetBankList"];

        /// <summary>
        /// fireBase detail
        /// </summary>
        public static string Bucket = ConfigurationManager.AppSettings["Bucket"];
        public static string FireBaseAPIKey = ConfigurationManager.AppSettings["FireBaseAPIKey"];
        public static string FirebaseUserName = ConfigurationManager.AppSettings["FirebaseUserName"];
        public static string FirebasePassword = ConfigurationManager.AppSettings["FirebasePassword"];



        /// <summary>
        /// push detail
        /// </summary>
        public static string FCM_ServerKey = ConfigurationManager.AppSettings["FCM_ServerKey"];
        public static string FCM_SenderId = ConfigurationManager.AppSettings["FCM_SenderId"];
        public static string FCMURL = ConfigurationManager.AppSettings["FCMURL"];

        /// <summary>
        /// 
        /// </summary>
        public static string Fundwallet = ConfigurationManager.AppSettings["Fundwallet"];
        public static string SourceAccountNumberPouchii = ConfigurationManager.AppSettings["SourceAccountNumberPouchii"];
        public static string PIN = ConfigurationManager.AppSettings["PIN"];
        public static string AuthenticatePouchii = ConfigurationManager.AppSettings["AuthenticatePouchii"];
        public static string LoginPasswordPouchi = ConfigurationManager.AppSettings["LoginPasswordPouchi"];
        public static string LoginAccountPouchi = ConfigurationManager.AppSettings["LoginAccountPouchi"];

        public static string CurrentBalanceAndNuban = ConfigurationManager.AppSettings["CurrentBalanceAndNuban"];
        public static string schemeId = ConfigurationManager.AppSettings["schemeId"];
        // public static string PIN = ConfigurationManager.AppSettings["Token"];

        /// <summary>
        /// Express wallet details
        /// </summary>
        public static string ExpressWalletSecretKey = ConfigurationManager.AppSettings["ExpressWalletSecretKey"];
        public static string ExpressWalletPublicKey = ConfigurationManager.AppSettings["ExpressWalletPublicKey"];
        public static string ExpressWalletCredit = ConfigurationManager.AppSettings["ExpressWalletCredit"];
        public static string ExpressWalletBaseUrl = ConfigurationManager.AppSettings["ExpressWalletBaseUrl"];
    }

    public static class AggregatorySTATUSCODES
    {
        public static string SUCCESSFUL = "00";
        public static string TRANSACTIONDOESNOTEXIST = "01";
        public static string REQUESTINPROCESS = "09";
        public static string FAILED = "32";
        public static string DEBITACCOUNTINVALID = "7701";
        public static string CREDITACCOUNTINVALID = "7702";
        public static string CREDITACCOUNTDORMANT = "7703";
        public static string INSUFFICIENTBALANCE = "7704";
        public static string INVALIDTRANSACTIONAMOUNT = "7706";
        public static string CURRENCYMISSMATCH = "7708";
        public static string TRANSACTIONREFERANCEEXISTS = "7709";
        public static string AUTHENTICATIONFAILED = "8004";
        public static string METHODNOTALLOWED = "8005";
        public static string NOTCONNECTION = "8803";
        public static string RESTRICTIONONACCOUNT = "8888";
        public static string ERRORDEBITINGACCOUNT = "7799";
    }
    public static class AggregatorySystemSpecsSTATUSCODES
    {
        public static string SUCCESSFUL = "success";
        public static string TRANSACTIONDOESNOTEXIST = "01";
        public static string REQUESTINPROCESS = "09";
        public static string FAILED = "Failed";
        public static string DEBITACCOUNTINVALID = "7701";
        public static string CREDITACCOUNTINVALID = "7702";
        public static string CREDITACCOUNTDORMANT = "7703";
        public static string INSUFFICIENTBALANCE = "7704";
        public static string INVALIDTRANSACTIONAMOUNT = "7706";
        public static string CURRENCYMISSMATCH = "7708";
        public static string TRANSACTIONREFERANCEEXISTS = "7709";
        public static string AUTHENTICATIONFAILED = "8004";
        public static string METHODNOTALLOWED = "8005";
        public static string NOTCONNECTION = "8803";
        public static string RESTRICTIONONACCOUNT = "8888";
        public static string ERRORDEBITINGACCOUNT = "7799";
    }
    public static class AggregatoryMESSAGE
    {
        public static string SUCCESSFUL = "APPROVED OR COMPLETED SUCCESSFULLY";
        public static string TRANSACTIONDOESNOTEXIST = "TRANSACTION DOES NOT EXIST";
        public static string REQUESTINPROCESS = "REQUEST IN PROCESS";
        public static string FAILED = "TRANSACTION NOT SUCCESSFULL";
        public static string DEBITACCOUNTINVALID = "DEBIT ACCOUNT INVALID";
        public static string CREDITACCOUNTINVALID = "CREDIT ACCOUNT INVALID";
        public static string CREDITACCOUNTDORMANT = "CREDIT ACCOUNT DORMANT";
        public static string INSUFFICIENTBALANCE = "INSUFFICIENT BALANCE";
        public static string INVALIDTRANSACTIONAMOUNT = "INVALID TRANSACTION AMOUNT";
        public static string CURRENCYMISSMATCH = "CURRENCY MISSMATCH";
        public static string TRANSACTIONREFERANCEEXISTS = "TRANSACTION REFERANCE EXISTS";
        public static string AUTHENTICATIONFAILED = "AUTHENTICATION FAILED";
        public static string METHODNOTALLOWED = "METHOD NOT ALLOWED";
        public static string NOTCONNECTION = "NOT CONNECTION";
        public static string RESTRICTIONONACCOUNT = "RESTRICTION ON ACCOUNT";
        public static string ERRORDEBITINGACCOUNT = "ERROR DEBITING ACCOUNT";
    }
    public static class OkraCallBackType
    {
        public static string TRANSACTIONS = "TRANSACTIONS";
        public static string INCOME = "INCOME";
        public static string AUTH = "AUTH";
        public static string IDENTITY = "IDENTITY";
        public static string BALANCE = "BALANCE";
        public static string ACCOUNTS = "ACCOUNTS";
        // public static string CREDITACCOUNTDORMANT = "CREDIT ACCOUNT DORMANT";
    }
    public class CommonSetting
    {
        public static string GetUniqueNumber()
        {
            Random random = new Random();
            const string numbers = "0123456789";

            return "PAY-MASTA" + (DateTime.UtcNow.Date.Year).ToString() + "-" + new string(Enumerable.Repeat(numbers, 4).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string AlphaNumericString(int stringLength)
        {
            Random random = new Random();


            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$%&*()0123456789";
            return new string(Enumerable.Repeat(chars, stringLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
