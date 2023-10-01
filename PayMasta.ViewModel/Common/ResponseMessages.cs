using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.Common
{
    public class ResponseMessages
    {
        public static string REQUEST_SENTTO_ADMIN = "Your access amount request sent to the admin or employer please wait till approval.";
        public static string USER_BLOCKED = "You can not login ,you are blocked by admin.";
        public static string USER_DELETED = "You can not login ,your account is deleted by admin.";
        public static string ACCESS_AMOUTN_ERROR = "You can not access amount more than 33.33 percent.";
        public static string INVALID_txnAmountREQUEST = "Transaction Limit Exceeded";
        public static string INVALID_AmountREQUEST = "Invalid amount request.";
        public static string Bank_DELETED = "Bank deleted successfully.";
        public static string BANK_NOT_DELETED = "Bank not deleted";

        public static string CONTENT_DELETED = "Content deleted successfully.";
        public static string CONTENT_NOT_DELETED = "Content not deleted";

        public static string RESORT_DELETED = "Resort deleted successfully.";
        public static string RESORT_NOT_DELETED = "Resort not deleted";

        public static string USER_DETAIL_NOT_RECEIVED = "User detail not received.";
        public static string USER_DETAIL_RECEIVED = "User detail received.";
        public static string USER_VERIFIED = "User verified Successfully.";
        public static string TRANSACTION_DONE = "Transaction done.";
        public static string TRANSACTION_NOT_DONE = "Transaction not done, due to an error.";
        public static string INSUFICIENT_BALANCE = "Your balance is insuficient for the transaction.";

        public static string KEY_GENERATED = "Transaction key generated.";
        public static string KEY_NOT_GENERATED = "Transaction key not generated.";
        public static string KEY_EXPIRED = "Transaction key expired.";

        public static string OTP_MESSAGE = "EziPay mobile no verification OTP is ";
        public static string PROFILE_UPDATED = "User profile updated successfully.";
        public static string PROFILE_UPDATED_REQUEST = "Your update profile request sent to the admin successfully.";
        public static string PROFILE_NOT_UPDATED = "User profile not updated successfully.";
        public static string USER_PROFILE_UPDATED = "Email send successfully please verify your email.";
        public static string PROFILE_EMAILNOT_UPDATED = "Your email already verified";
        public static string PROFILE_EMAILNOT = "This email id is already exist";
        public static string PASSWORD_CHANGED = "Password changed successfully.";
        public static string PASSWORD_NOT_CHANGED = "Password not changed.";
        public static string PASSWORD_NOT_CURRECT = "Please enter correct old password.";
        public static string PASSWORD_INVALID = "Please enter your correct password.";
        public static string PASSWORD_CURRECT = "Password is correct";
        public static string OLD_PASSWORD_NEW_PASSWORD_SAME = "The old password and new password are the same,please enter a different password.";
        public static string DATA_SAVED = "Data saved successfully.";
        public static string DATA_NOT_SAVED = "Data not saved.";
        public static string BANK_ALREADY_EXIST = "Bank already added you can not add same bank";
        public static string DOCUMENT_UPLOADED = "Document uploaded successfully.";
        public static string DOCUMENT_NOT_UPLOADED = "Document not uploaded.";
        public static string DATA_FEEDBAK_SAVED = "Feedback submitted successfully.";
        public static string DATAUPDATED = "Updated successfully.";
        public static string DATA_FEEDBAK_NOT_SAVED = "Feedback not submitted.";

        public static string BALANCE_FOUND = "Balance found.";
        public static string BALANCE_NOT_FOUND = "Balance not found.";


        public static string PAY_REQUEST_ACCEPT_SUCCESS = "Payment Request accepted successfully.";
        public static string PAY_REQUEST_ACCEPT_UNSUCCESS = "Payment Request not accepted.";
        public static string PAY_REQUEST_REJECT_SUCCESS = "Payment Request rejected successfully.";
        public static string PAY_REQUEST_REJECT_UNSUCCESS = "Payment Request not rejected.";
        public static string PAY_MONEY_REQUEST_SUCCESS = "Amount Requested !";
        public static string PAY_MONEY_REQUEST_UNSUCCESS = "Amount request unsuccessful.";
        public static string PAY_MONEY_SUCCESS = "Amount paid successfully.";
        public static string PAY_MONEY_UNSUCCESS = "Amount not paid.";

        public static string NOTIFICATIONSENT = "Notification sent successfully";
        public static string NIN_ISNOT_VALID = "Nin number is not valid.";

        public static string VERIFICATION_MAILNOT_SENT = "Please send a verification mail on your email id, And verifiy the email id before signup signup.";
        public static string DATA_RECEIVED = "Data received successfully.";
        public static string SUCCESS = "Successfully.";
        public static string POINT_REDEEM_SUCCESS = "Points redeem Successfully.";
        public static string DATA_NOT_RECEIVED = "Data not received.";
        public static string TOKEN_SENT = "Token received successfully.";
        public static string TOKEN_NOT_SENT = "Token not received.";
        public static string HOTEL_BOOKED = "Hotel booked successfully";
        public static string OTP_SENT = "OTP sent successfully.";
        public static string OTP_SENT_RANGE = "You can only sent OTP 3 times in a day for same mobile no.";
        public static string OTP_NOT_SENT = "OTP not sent.";
        public static string OTP_VERIFIED = "OTP verified successfully.";
        public static string OTP_NOT_VERIFIED = "OTP not verified.";
        public static string FORGOT_PASSWORD_SUCCESS = "Your temporary password has been sent on your registered email id.";
        public static string FORGOT_PASSWORD_UNSUCCESS = "Your temporary password has not been sent due to some error.";
        public static string VALID_OTP = "OTP validate successfully.";
        public static string INVALID_USER_TYPE = "Invalid user type.";
        public static string INVALID_OTP = "Please enter a valid OTP.";
        public static string INVALID_FORM_DATA = "Please fill all the values.";
        public static string INVALID_MOBILE_NO = "Please enter a valid mobile no.";
        public static string INVALID_EMAIL = "Please enter a valid email.";
        public static string INVALID_KEY = "Invalid transaction key.";
        public static string INVALID_HASHED_KEY = "Invalid hashed key.";
        public static string INVALID_TRANSACTION_TYPE = "Transaction type should be CREDIT or DEBIT type.";
        public static string INVALID_REQUEST = "Invalid request.";
        public static string INVALID_AMOUNT = "Invalid amount.";
        public static string INVALID_CUSTOMER = "Invalid customer details.";
        public static string INVALID_PASSWORD = "You have entered an invalid password.";
        public static string INACTIVE_USER = "Your account has been deactivated by admin.";
        public static string EMAIL_UNVERIFIED = "Your email id is not verified. Please verify your email id before signup.";
        public static string EMPTY_KEY = "Secret key should not be empty.";
        public static string EMPTY_AMOUNT = "Amount should not be empty.";
        public static string ZERO_AMOUNT = "Amount should not be zero.";
        public static string IMPROPER_AMOUNT = "Only two digit allowed after decimal in amount.";
        public static string EMAIL_VERIFICATION_PENDING = "Your email id verification is still pending.";
        public static string VERIFICATION_PRODUCT = "Product is not exist.";
        public static string EMPTY_TRANSACTION_TYPE = "Transaction type should not be empty.";
        public static string EMPTY_BANK_BRANCH_CODE = "Bank Branch Code should not be empty.";
        public static string EMPTY_BANK_BANKTRANSACTION_ID = "Bank Transaction Id should not be empty.";
        public static string ZERO_BANK_BANKTRANSACTION_ID = "Bank Transaction Id should not be zero (0).";
        public static string DUPLICATE_BANK_BANKTRANSACTION_ID = "Bank Transaction Id should not be duplicate.";
        public static string EMPTY_MOBILE_NO = "Please enter a mobile no.";
        public static string EMPTY_ISD = "ISD code should not be empty.";
        public static string EMPTY_EMAIL = "Please enter an email.";
        public static string EMPTY_PASSWORD = "Please enter your password.";
        public static string EXIST_MOBILE_NO = "This mobile no or email  is already exist, Please try with another mobile no or email.";
        public static string EXIST_ACCOUNT_NO = "This account no is already exist, Please try with another account no.";
        public static string EXIST_EMAIL = "This email already exist, Please try with another email.";

        public static string DUPLICATE_CREDENTIALS = "This email and mobile no.  already exist, Please try with another email and mobile no.";
        public static string UNIQUE_CREDENTIALS = "You can procced with mobile and email.";
        public static string USER_REGISTERED = "Registration Successful!";
        public static string USER_NOT_REGISTERED = "Unable to sign up due to some error.";
        public static string REQUESTDATA_NOT_EXIST = "Your request is not complete.";
        public static string USER_LOGIN = "User logged in successfully.";
        public static string USER_LOGIN_PasswordExpiry = "Your password has been expired please contact admin.";
        public static string USER_LOGIN_ENTERED_WRONG_PASSWORD = "Your entered wrong password more then 3 time ,please wait for 30 minutes.";
        public static string Merchant_Login_Failed = "You can not loing,you need to approve your docs from admin.";
        public static string USER_PASSWORD_EXPIRED = "Your password has been expired please change your password.";
        public static string LOGOUT_SUCCESS = "User logged out successfully.";
        public static string LOGOUT_UNSUCCESS = "Unable to logout.";
        public static string EXCEPTION_OCCURED = "We have encountered an error. Please try after some time.";
        public static string UNATHORIZED_REQUEST = "It seems that you have logged in from some other device.";
        public static string UNATHORIZED_REQUEST2 = "Please update your application.";

        public static string ALREADY_VERIFIED = "This email has been already verified.";
        public static string VERFIED_SUCCESSFULLY = "Welcome to EziPay!";
        public static string NO_EMAIL_RECORD_FOUND = "Unable to process this request.";
        public static string EMAIL_NOT_EXIST = "This email Id is not registered.";
        public static string USER_NOT_EXIST = "User not exist.";
        public static string BothExist = "Email or phone number both is exist pllease with other creadential";
        public static string SENDER_NOT_EXIST = "User's wallet account does not exist.";
        public static string RECEIVER_NOT_EXIST = "Receiver's wallet account does not exist.";
        public static string LOGIN_FAILED = "Login failed due to invalid credentials.";
        public static string NOTIFICATION_CHANGE = "Settings updated successfully.";
        public static string NOTIFICATION_CHANGE_UNSUCCESS = "Settings not updated.";
        public static string SELF_WALLET = "Self account transaction cann't be done.";
        public static string TRANSACTION_NOT_FOUND = "No Transactions Found!";

        public static string SHARED_SUCCESS = "Your code has been shared successfully!";
        public static string SHARED_FAILED = "Unable to share code.";
        //  public static string SOME_THING_WENT_WRONG = "Some thing went wrong.";
        public static string OPERATOR_LIST_FOUND = "Operator list found successfully.";
        public static string OPERATOR_LIST_NOT_FOUND = "Operator list found successfully.";
        //TRANSACTION
        public static string TRANSACTION_SERVICE_CATEGORY_NOT_FOUND = "Service category not found.";
        public static string TRANSACTION_SERVICE_CHANNEL_NOT_REGISTERED = "Service channel not registered with us.";
        public static string TRANSACTION_ERROR = "Transaction status: Internal server error by Aggregator.";
        public static string AGGREGATOR_FAILED_ERROR = "Transaction FAILED(01).";
        public static string AGGREGATOR_FAILED_EXCEPTION = "Transaction FAILED(02).";
        public static string EZEEPAY_FAILED_EXCEPTION = "Transaction FAILED(03).";
        public static string TRANSACTION_NULL_ERROR = "Null value respond by Aggregator.";
        public static string INVALID_DEVICETYPE = "Device type should Android or IOS.";
        public static string TRANSACTION_DISABLED = "This action can't be performed right now. Please contact support team for more information. \nE: support@ezipaygh.com\nM: +233 (0) 264099742\nM: +233 (0) 275762461\nM: +233 (0) 202933156\nwhatsapp : +233 (0) 275762461";
        //KYC Update
        public static string USER_KYC_UPDATED = "User KYC updated successfully.";
        public static string USER_KYC_NOT_UPDATED = "Document verification is pending.Administrator will approve shortly for you to transact soon.please view my profile in the menu section for verification status.";
        public static string USER_KYC_NOT_UPLOADED = "Your KYC verification document(Selfie holding identification proof driving license/Non-Citizen card/Voters ID/Passport and photo of the debit/credit card used for the recent transaction) are not uploaded,please upload!";
        #region TrasferToBank
        public static string TRANSACTION_SUCCESS = "Transaction successful.";
        public static string TRANSACTION_FAIL = "Transaction not completed.";

        public static string UTILITY_ACCOUNT_ACTIVE = "Active account.";
        public static string UTILITY_ACCOUNT_DEACTIVE = "Account is not active.";
        public static string UTILITY_ACCOUNT_NOT_FOUND = "Account not found";
        public static string UTILITY_BENEFFICIARY_NOT_EXIST = "This account holder doesn't exist.";
        // Call Back
        public static string REQUEST_SENT = "Request sent successfully.";
        public static string REQUEST_SENT_FAILD = "There is some issue, Please try later.";
        #endregion

        #region subscription email
        public static string SUBSCRIPTION_EMAIL_SUCCESS = "Subscription email has been sent.";
        public static string SUBSCRIPTION_EMAIL_FAILURE = "Subscription email sending failed. please try again.";
        #endregion
    }
    public static class ResponseStatusCode
    {
        /// <summary>
        /// Email is not verifieid
        /// </summary>
        public static string EMAIL_UNVERIFIED = "506";
    }

    public static class ResponseMessageKyc
    {
        public static string FAILED_Doc_NotUploaded = "Your KYC verification document(Selfie holding identification proof driving license/Non-Citizen card/Voters ID/Passport and photo of the debit/credit card used for the recent transaction) are not uploaded,please upload!";
        public static string FAILED_Doc_Pending = "Document verification is pending.Administrator will approve shortly for you to transact soon.please view my profile in the menu section for verification status.";
        public static string Doc_Not_visible = "Sorry your uploaded documents were not visible as per our requirements. Please upload again.";
        public static string Doc_Rejected = "Sorry your uploaded documents have been rejected, please contact administrator for further information on support@ezipaygh.com or +233 275762461.";
        public static string EmailNot_Verified = "Your email id verification still pending.";
        public static string RECEIVER_NOT_EXIST = "Receiver's wallet account does not exist.";
        public static string TRANSACTION_DISABLED = "Your transaction is disabled by admin";
        public static string TRANSACTION_LIMIT = "Your transaction limit exceeded.";
    }

    public static class ResponseEmailMessage
    {
        public static string VERIFICATION_EMAIL = "Verification email has been sent on your email id. Please verify your email before signup.";
        public static string PAYMENT_SUCCESS = "Transaction Successfull";
    }


    public class AdminResponseMessages
    {
        public static string LOGIN_SUCCESS = "Login successful.";
        public static string LOGIN_FAILED = "Invalid Credentials or User doesn’t exist.";

        public static string USER_MANAGE_SUCCESS = "User {0} successfully.";
        public static string USER_MANAGE_FAILURE = "User not {0}.";

        public static string SUB_ADMIN_CREATED = "Successful";
        public static string MANAGE_SUBADMIN_SUCCESS = "Subadmin  {0} successfully.";
        public static string MANAGE_SUBADMIN_FAILURE = "Subadmin  not {0}.";

        public static string MANAGE_MARCHANT_SUCCESS = "Marchant  {0} successfully.";
        public static string MANAGE_MARCHANT_FAILURE = "Marchant  not {0}.";

        public static string CHANGE_PASSWORD_SUCCESS = "Password changed successfully.";
        public static string CHANGE_PASSWORD_FAILED = "Unable to change password.";

        public static string USER_TRANSACTION_SUCCESS = "Account {0} successfully.";
        public static string USER_TRANSACTION_FAILURE = "Account not {0}.";
        public static string USER_TRANSACTION_LOW_BALANCE = "Insufficient Balance!";

        public static string DATA_FOUND = "Success.";
        public static string DATA_NOT_FOUND = "No {0} found.";
        public static string DATA_NOT_FOUND_GENERIC = "No data found.";
        public static string UNABLE_TO_PROCESS = "Unable to process your request.";


        public static string USER_NOT_FOUND = "User does not exist.";
        public static string USER_DELETED = "User deleted successfully.";
        public static string CATEGORY_DELETED = "Category deleted successfully.";
        public static string USER_BLOCKED = "User disabled successfully";
        public static string CATEGORY_BLOCKED = "Category disabled successfully";
        public static string USER_UNBLOCKED = "User enabled successfully";
        public static string CATEGORY_UNBLOCKED = "Category enabled successfully";
        public static string INVALID_FORM_DATA = "Please fill all the required details.";
        public static string EXCEPTION_OCCURED = "Sorry! We have encountered an error, please try after sometime.";
        public static string UNATHORIZED_REQUEST = "Your request is unauthorized.";
        public static string SUBADMIN_CREATED = "Subadmin created successfully.";
        public static string SUBADMIN_UPDATED = "Subadmin edited successfully.";

        public static string MERCHANT_MANAGE_SUCCESS = "Merchant {0} successfully.";
        public static string MERCHANT_MANAGE_FAILURE = "Merchant not {0}.";
        public static string MERCHANT_CREATED = "Merchant created successfully.";
        public static string MERCHANT_UPDATED = "Merchant updated successfully.";

        public static string COMMISSION_SET = "Commission set successfully.";
        public static string COMMISSION_NOT_SET = "Commission not set.";
        public static string DUPLICATE_USER = "User already exist.";
        public static string DEACTIVATED_SUBADMIN = "Your account deactivated by admin.";
        public static string DELETED_SUBADMIN = "Your account Deleted by admin.";
        public static string STATUS_CHANGED = "Document status has been changed please refresh your screen.";
        public static string DATA_SAVED = "Data Saved successfully.";
        public static string DATA_NOT_SAVED = "Data not Saved .";
        //dashboard
        public static string TRANSACTION_ENABLED = "Transaction enabled successful.";
        public static string TRANSACTION_NOT_ENABLED = "Failed.";
        public static string CURRENCY_SET = "Currency converted successfully.";
        public static string CURRENCY_NOT_SET = "Currency not converted.";

        public static string DUPLICATE_SUBCATEGORY = "Subcategory already exist, Please try with another Subcategory";
        public static string DUPLICATE_MAINCATEGORY = "Maincategory already exist, Please try with another Maincategory";
    }
}
