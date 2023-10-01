using PayMasta.ViewModel.CMSVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ManageCms
{
    public interface IManageCmsService
    {
        Task<GetCmsResponse> GetFaqData(CmsReqeust request);
        Task<CmsResponse> SaveFaqData(CmsReqeust request);
        Task<CmsResponse> SavePrivacyPolicyData(CmsReqeust request);
        Task<CmsResponse> SaveTandCData(CmsReqeust request);
        Task<GetCmsResponse> GetTandCData(CmsReqeust request);
        Task<GetCmsResponse> GetPrivacyPolicyById(CmsReqeust request);
        Task<CmsResponse> SaveFaqQuestions(FaqQuestionReqeust request);
        Task<CmsResponse> SaveFaqQuestionsDetail(FaqQuestionDetailReqeust request);
        Task<FaqQuestion> GetFaqList(CmsReqeust request);
        Task<QuestionAnswer> GetQuestionAnswerList(QuestionAnswerRequest request);
        Task<FAQResponse> GetFaq(int faqId);
        Task<CmsResponse> DeleteQuestionAnswer(int faqId);
        Task<CmsResponse> UpdateQuestionAnswer(UpdateQuestionAnswerRequest request, UpdateQuestionAnswerDetails[] arrayDocs);
    }
}
