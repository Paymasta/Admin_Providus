using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.Account
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public int DeviceType { get; set; }
        public string DeviceId { get; set; }
        public string DeviceToken { get; set; }
    }

    public class LoginResponse
    {
        public LoginResponse()
        {
            // Permissions = new List<PermissionModel>();
            //  RelationData = new RelationDataVM();
        }
        // [IgnoreDataMember]
        public long UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RstKey { get; set; }
        public string Token { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public string ProfileImage { get; set; }
        public string CountryCode { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public bool IsProfileCompleted { get; set; }
        public int RoleId { get; set; }
    }
    public class ForgotPasswordRequest
    {
        [Required]
        public string EmailorPhone { get; set; }
        /// <summary>
        /// 1-Email,2-Phone
        /// </summary>
        [Range(1, 2)]
        public int Type { get; set; }
    }

    public class ChangePasswordRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class UserModel
    {
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string NINNumber { get; set; }
        public string Gender { get; set; }
        public double WalletBalance { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string EmployerName { get; set; }
        public long EmployerId { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public string CurrentBalance { get; set; }
    }

    public class LogoutRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        [Required]
        public string DeviceId { get; set; }
    }
    public class OtpResponse : OtpCommonRequest
    {
        public OtpResponse()
        {
            this.IsSuccess = false;
            this.RstKey = 0;
            this.Message = string.Empty;
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        // public long OtpId { get; set; }


    }
    public class OtpCommonRequest
    {
        public OtpCommonRequest()
        {
            this.Otp = string.Empty;
        }
        public string Otp { get; set; }
    }

    public class ResetPasswordRequest
    {


        public string EmailorPhone { get; set; }
        [Required]
        public string OtpCode { get; set; }
        [Required]
        public string Password { get; set; }

    }

    public class ResendOTPRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        [Range(1, 2)]
        public int Type { get; set; }
    }
    public class VerifyOTPRequest
    {
        [Required]
        [Phone]
        public string MobileNumber { get; set; }
        [Required]
        public string OtpCode { get; set; }
        [Required]
        public int DeviceType { get; set; }
        [Required]
        public string DeviceId { get; set; }
        [Required]
        public string DeviceToken { get; set; }
    }
    public class VerifyForgetPasswordOTPRequest
    {


        public string EmailorPhone { get; set; }
        [Required]
        public string OtpCode { get; set; }
        public int Type { get; set; }
    }

    public class UpdateAdminProfileRequest
    {


        public string Name { get; set; }

        [Required]
        public Guid AdminGuid { get; set; }

    }

    public class UpdateAdminProfileResponse
    {
        public UpdateAdminProfileResponse()
        {
            this.IsSuccess = false;
            this.RstKey = 0;
            this.Message = string.Empty;
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
       
    }
    public class UploadProfileImageResponse
    {

        public int RstKey { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }

    }

    public class UploadProfileImageRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        [Required]
        public string ImageUrl { get; set; }

    }
}
