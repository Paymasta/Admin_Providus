using PayMasta.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Account
{
    public interface IAccountService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<string> ForgotPassword(ForgotPasswordRequest request);
        Task<UserModel> GetProfile(Guid userGuid);
        Task<int> ChangePassword(ChangePasswordRequest request);
        Task<OtpResponse> ResetPassword(ResetPasswordRequest request);
        Task<int> ResendOTP(ResendOTPRequest request);
        Task<LoginResponse> VerifyOTPWeb(VerifyOTPRequest request);
        Task<LoginResponse> VerifyForgetPasswordOTP(VerifyForgetPasswordOTPRequest request);
        Task<UpdateAdminProfileResponse> UpdateAdminProfile(UpdateAdminProfileRequest request);
        Task<UploadProfileImageResponse> UploadProfileImage(UploadProfileImageRequest request);
    }
}
