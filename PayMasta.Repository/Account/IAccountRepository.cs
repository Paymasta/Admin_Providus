using PayMasta.Entity.OtpInfo;
using PayMasta.Entity.UserMaster;
using PayMasta.Entity.UserSession;
using PayMasta.Entity.VirtualAccountDetail;
using PayMasta.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Account
{
    public interface IAccountRepository
    {
        Task<UserMaster> Login(LoginRequest request);
        Task<UserSession> GetSessionByDeviceId(long userId, string deviceId);
        Task<int> CreateSession(UserSession userSessionEntity);
        Task<int> UpdateSession(UserSession session);
        Task<UserMaster> GetUserByEmailOrPhone(int type, string emailorPhone);
        Task<int> InsertOtpInfo(OtpInfo otpInfoEntity);
        Task<UserMaster> GetUserByGuid(Guid guid, IDbConnection exdbConnection = null);
        Task<int> UpdateUserPassword(UserMaster userEntity, IDbConnection exdbConnection = null);
        Task<UserMaster> GetUserByEmailOrPhone(string emailorPhone);
        Task<OtpInfo> GetOtpInfoByUserId(long userId);
        Task<UserMaster> GetUserByMobile(string mobile, IDbConnection exdbConnection = null);
        Task<OtpInfo> GetOtpInfoByUserId(string mobile, string otp);
        Task<int> VerifyUserPhoneNumber(UserMaster userEntity, IDbConnection exdbConnection = null);
        Task<int> UpdateAdminProfile(UserMaster userEntity, IDbConnection exdbConnection = null);

        Task<int> UploadProfileImage(UserMaster userEntity, IDbConnection exdbConnection = null);
        Task<UserSession> GetSessionByUserId(long userId);
        Task<int> DeleteSession(long userId);
        Task<VirtualAccountDetail> GetVirtualAccountDetailByUserId(long userId, IDbConnection exdbConnection = null);
        Task<int> UpdateVirtualAccountDetailByUserId(VirtualAccountDetail virtualAccountDetail, IDbConnection exdbConnection = null);
        Task<int> InsertVirtualAccountDetail(VirtualAccountDetail virtualAccountDetail, IDbConnection exdbConnection = null);
    }
}
