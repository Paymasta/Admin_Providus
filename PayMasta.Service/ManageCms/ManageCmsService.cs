using PayMasta.Entity.Faq;
using PayMasta.Entity.PrivacyPolicy;
using PayMasta.Entity.TermAndCondition;
using PayMasta.Repository.Account;
using PayMasta.Repository.ManageCms;
using PayMasta.Utilities;
using PayMasta.ViewModel.CMSVM;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ManageCms
{
    public class ManageCmsService : IManageCmsService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IManageCmsRepository _manageCmsRepository;
        public ManageCmsService()
        {
            _accountRepository = new AccountRepository();
            _manageCmsRepository = new ManageCmsRepository();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }
        public async Task<CmsResponse> SaveFaqData(CmsReqeust request)
        {
            var res = new CmsResponse();
            var user = await _accountRepository.GetUserByGuid(request.AdminGuid);
            if (user != null && user.UserType == (int)EnumUserType.SuperAdmin)
            {
                if (request.ContentId > 0)
                {
                    var faqData = await _manageCmsRepository.GetFaqById(request.ContentId);
                    faqData.Detail = request.TextContent;
                    faqData.UpdatedAt = DateTime.UtcNow;
                    if (await _manageCmsRepository.UpdateFaqInfo(faqData) > 0)
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_SAVED;
                        res.RstKey = 1;
                    }
                    else
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_NOT_SAVED;
                        res.RstKey = 3;
                    }
                }
                else
                {
                    var faqReq = new FAQ
                    {
                        Detail = request.TextContent,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };

                    if (await _manageCmsRepository.InsertFaqInfo(faqReq) > 0)
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_SAVED;
                        res.RstKey = 1;
                    }
                    else
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_NOT_SAVED;
                        res.RstKey = 3;
                    }

                }
            }
            else
            {
                res.IsSuccess = false;
                res.Message = ResponseMessages.INVALID_USER_TYPE;
                res.RstKey = 2;
            }
            return res;
        }

        public async Task<GetCmsResponse> GetFaqData(CmsReqeust request)
        {
            var res = new GetCmsResponse();
            var user = await _accountRepository.GetUserByGuid(request.AdminGuid);
            if (user != null && user.UserType == (int)EnumUserType.SuperAdmin)
            {
                var faqData = await _manageCmsRepository.GetFaqByFaqId();
                if (faqData != null)
                {
                    res.getContent = faqData;
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                    res.RstKey = 1;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                    res.RstKey = 3;
                }
            }
            else
            {
                res.IsSuccess = false;
                res.Message = ResponseMessages.INVALID_USER_TYPE;
                res.RstKey = 2;
            }
            return res;
        }

        public async Task<CmsResponse> SavePrivacyPolicyData(CmsReqeust request)
        {
            var res = new CmsResponse();
            var user = await _accountRepository.GetUserByGuid(request.AdminGuid);
            if (user != null && user.UserType == (int)EnumUserType.SuperAdmin)
            {
                if (request.ContentId > 0)
                {
                    var ppData = await _manageCmsRepository.GetPrivacyPolicyById(request.ContentId);
                    ppData.Detail = request.TextContent;
                    ppData.UpdatedAt = DateTime.UtcNow;
                    if (await _manageCmsRepository.UpdatePrivacyPolicyInfo(ppData) > 0)
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_SAVED;
                        res.RstKey = 1;
                    }
                    else
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_NOT_SAVED;
                        res.RstKey = 3;
                    }
                }
                else
                {
                    var faqReq = new PrivacyPolicy
                    {
                        Detail = request.TextContent,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };

                    if (await _manageCmsRepository.InsertPrivacyPolicyInfo(faqReq) > 0)
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_SAVED;
                        res.RstKey = 1;
                    }
                    else
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_NOT_SAVED;
                        res.RstKey = 3;
                    }

                }
            }
            else
            {
                res.IsSuccess = false;
                res.Message = ResponseMessages.INVALID_USER_TYPE;
                res.RstKey = 2;
            }
            return res;
        }

        public async Task<CmsResponse> SaveTandCData(CmsReqeust request)
        {
            var res = new CmsResponse();
            var user = await _accountRepository.GetUserByGuid(request.AdminGuid);
            if (user != null && user.UserType == (int)EnumUserType.SuperAdmin)
            {
                if (request.ContentId > 0)
                {
                    var tAndcData = await _manageCmsRepository.GetTermAndConditionById(request.ContentId);
                    tAndcData.Detail = request.TextContent;
                    tAndcData.UpdatedAt = DateTime.UtcNow;
                    if (await _manageCmsRepository.UpdateTermAndConditionInfo(tAndcData) > 0)
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_SAVED;
                        res.RstKey = 1;
                    }
                    else
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_NOT_SAVED;
                        res.RstKey = 3;
                    }
                }
                else
                {
                    var tandcReq = new TermAndCondition
                    {
                        Detail = request.TextContent,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };

                    if (await _manageCmsRepository.InsertTermAndConditionInfo(tandcReq) > 0)
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_SAVED;
                        res.RstKey = 1;
                    }
                    else
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_NOT_SAVED;
                        res.RstKey = 3;
                    }

                }
            }
            else
            {
                res.IsSuccess = false;
                res.Message = ResponseMessages.INVALID_USER_TYPE;
                res.RstKey = 2;
            }
            return res;
        }

        public async Task<GetCmsResponse> GetTandCData(CmsReqeust request)
        {
            var res = new GetCmsResponse();
            var user = await _accountRepository.GetUserByGuid(request.AdminGuid);
            if (user != null && user.UserType == (int)EnumUserType.SuperAdmin)
            {
                var tandcData = await _manageCmsRepository.GetTandCByFaqId();
                if (tandcData != null)
                {
                    res.getContent = tandcData;
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                    res.RstKey = 1;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                    res.RstKey = 3;
                }
            }
            else
            {
                res.IsSuccess = false;
                res.Message = ResponseMessages.INVALID_USER_TYPE;
                res.RstKey = 2;
            }
            return res;
        }
        public async Task<GetCmsResponse> GetPrivacyPolicyById(CmsReqeust request)
        {
            var res = new GetCmsResponse();
            var user = await _accountRepository.GetUserByGuid(request.AdminGuid);
            if (user != null && user.UserType == (int)EnumUserType.SuperAdmin)
            {
                var ppData = await _manageCmsRepository.GetPrivacyPolicyById();
                if (ppData != null)
                {
                    res.getContent = ppData;
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                    res.RstKey = 1;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                    res.RstKey = 3;
                }
            }
            else
            {
                res.IsSuccess = false;
                res.Message = ResponseMessages.INVALID_USER_TYPE;
                res.RstKey = 2;
            }
            return res;
        }


        public async Task<CmsResponse> SaveFaqQuestions(FaqQuestionReqeust request)
        {
            var res = new CmsResponse();
            var user = await _accountRepository.GetUserByGuid(request.AdminGuid);
            if (user != null && user.UserType == (int)EnumUserType.SuperAdmin)
            {
                if (request.TextContent != null)
                {

                    var faqReq = new FaqMaster
                    {
                        QuestionText = request.TextContent,
                        Detail = request.TextContent,
                        CreatedAt = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = user.Id,
                    };

                    if (await _manageCmsRepository.SaveFaqQuestions(faqReq) > 0)
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_SAVED;
                        res.RstKey = 1;
                    }
                    else
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_NOT_SAVED;
                        res.RstKey = 3;
                    }
                }

            }
            else
            {
                res.IsSuccess = false;
                res.Message = ResponseMessages.INVALID_USER_TYPE;
                res.RstKey = 2;
            }
            return res;
        }

        public async Task<CmsResponse> SaveFaqQuestionsDetail(FaqQuestionDetailReqeust request)
        {
            var res = new CmsResponse();
            var user = await _accountRepository.GetUserByGuid(request.AdminGuid);
            if (user != null && user.UserType == (int)EnumUserType.SuperAdmin)
            {
                if (request.TextContent != null)
                {

                    var faqReq = new FaqDetailMaster
                    {
                        Detail = request.TextContent,
                        FaqId = request.FaqId,
                        CreatedAt = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = user.Id,
                    };

                    if (await _manageCmsRepository.SaveFaqQuestionsDetail(faqReq) > 0)
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_SAVED;
                        res.RstKey = 1;
                    }
                    else
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_NOT_SAVED;
                        res.RstKey = 3;
                    }
                }

            }
            else
            {
                res.IsSuccess = false;
                res.Message = ResponseMessages.INVALID_USER_TYPE;
                res.RstKey = 2;
            }
            return res;
        }
        public async Task<FaqQuestion> GetFaqList(CmsReqeust request)
        {
            var res = new FaqQuestion();
            var user = await _accountRepository.GetUserByGuid(request.AdminGuid);
            if (user != null && user.UserType == (int)EnumUserType.SuperAdmin)
            {
                var tandcData = await _manageCmsRepository.GetFaqList();
                if (tandcData.Count > 0)
                {
                    res.faqQuestionResponses = tandcData;
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                    res.RstKey = 1;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                    res.RstKey = 3;
                }
            }
            else
            {
                res.IsSuccess = false;
                res.Message = ResponseMessages.INVALID_USER_TYPE;
                res.RstKey = 2;
            }
            return res;
        }

        public async Task<QuestionAnswer> GetQuestionAnswerList(QuestionAnswerRequest request)
        {
            var res = new QuestionAnswer();
            var user = await _accountRepository.GetUserByGuid(request.userGuid);
            if (user != null && user.UserType == (int)EnumUserType.SuperAdmin)
            {
                var tandcData = await _manageCmsRepository.GetQuestionAnswerList(request.pageNumber, request.PageSize, request.Status, request.FromDate, request.ToDate, request.SearchTest);
                if (tandcData.Count > 0)
                {
                    res.faqQuestionResponses = tandcData;
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                    res.RstKey = 1;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                    res.RstKey = 3;
                }
            }
            else
            {
                res.IsSuccess = false;
                res.Message = ResponseMessages.INVALID_USER_TYPE;
                res.RstKey = 2;
            }
            return res;
        }
        public async Task<FAQResponse> GetFaq(int faqId)
        {
            var faqResponses = new FAQResponse();
            var itemEntity = new List<FaqDetailResponse>();
            try
            {
                //var dd = await FaqTest();
                using (var dbConnection = Connection)
                {
                    faqResponses = await _manageCmsRepository.FAQ(faqId, dbConnection);
                    if (faqResponses != null)
                    {
                        if (faqResponses != null)
                        {

                            var details = await _manageCmsRepository.FAQAnswers(faqResponses.Id);
                            if (details != null && details.Count > 0)
                            {
                                faqResponses.FaqDetails = details;
                            }
                        }

                        //result.FaqDetails = faqResponses;
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return faqResponses;
        }
        public async Task<CmsResponse> DeleteQuestionAnswer(int faqId)
        {
            var faqResponses = new CmsResponse();
            try
            {
                //var dd = await FaqTest();
                using (var dbConnection = Connection)
                {
                    var faqRes = await _manageCmsRepository.DeleteFAQ(faqId, dbConnection);
                    var faqAnsReq = await _manageCmsRepository.DeleteFAQAnswers(faqId, dbConnection);
                    if (faqRes > 0 && faqAnsReq > 0)
                    {
                        faqResponses.RstKey = 1;
                        faqResponses.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return faqResponses;
        }
        public async Task<CmsResponse> UpdateQuestionAnswer(UpdateQuestionAnswerRequest request, UpdateQuestionAnswerDetails[] arrayDocs)
        {
            var faqResponses = new CmsResponse();
            try
            {
                //var dd = await FaqTest();
                using (var dbConnection = Connection)
                {
                    var faqRes = await _manageCmsRepository.GetFAQForUpdate(request.FaqId, dbConnection);

                    if (faqRes != null)
                    {
                        if (arrayDocs != null)
                        {
                            foreach (var item in arrayDocs)
                            {
                                var faqAnsReq = await _manageCmsRepository.GetFAQAnswerForUpdate(item.FaqDetailId, dbConnection);
                                faqAnsReq.Detail = item.FaqDetail;
                                await _manageCmsRepository.UpdateFAQDetail(faqAnsReq);
                            }
                        }

                        faqRes.QuestionText = request.Faq;
                        faqRes.Detail = request.Faq;
                        var res = await _manageCmsRepository.UpdateFAQ(faqRes);
                        faqResponses.RstKey = 1;
                        faqResponses.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return faqResponses;
        }
    }
}
