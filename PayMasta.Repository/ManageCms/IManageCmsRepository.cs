using PayMasta.Entity.Faq;
using PayMasta.Entity.PrivacyPolicy;
using PayMasta.Entity.TermAndCondition;
using PayMasta.ViewModel.CMSVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.ManageCms
{
    public interface IManageCmsRepository
    {
        Task<int> InsertFaqInfo(FAQ faqInfoEntity, IDbConnection exdbConnection = null);
        Task<int> InsertPrivacyPolicyInfo(PrivacyPolicy privacyInfoEntity, IDbConnection exdbConnection = null);
        Task<int> InsertTermAndConditionInfo(TermAndCondition termInfoEntity, IDbConnection exdbConnection = null);
        Task<FAQ> GetFaqById(long id, IDbConnection exdbConnection = null);
        Task<PrivacyPolicy> GetPrivacyPolicyById(long id, IDbConnection exdbConnection = null);
        Task<int> UpdateFaqInfo(FAQ faq);
        Task<int> UpdatePrivacyPolicyInfo(PrivacyPolicy privacyPolicy);
        Task<GetContent> GetFaqByFaqId(IDbConnection exdbConnection = null);
        Task<int> UpdateTermAndConditionInfo(TermAndCondition termAndCondition);
        Task<TermAndCondition> GetTermAndConditionById(long id, IDbConnection exdbConnection = null);
        Task<GetContent> GetTandCByFaqId(IDbConnection exdbConnection = null);
        Task<GetContent> GetPrivacyPolicyById(IDbConnection exdbConnection = null);
        Task<int> SaveFaqQuestions(FaqMaster faqInfoEntity, IDbConnection exdbConnection = null);
        Task<int> SaveFaqQuestionsDetail(FaqDetailMaster faqInfoEntity, IDbConnection exdbConnection = null);
        Task<List<FaqQuestionResponse>> GetFaqList(IDbConnection exdbConnection = null);
        Task<List<QuestionAnswerResponse>> GetQuestionAnswerList(int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null);
        Task<FAQResponse> FAQ(int faqId, IDbConnection exdbConnection = null);
        Task<List<FaqDetailResponse>> FAQAnswers(int FaqId, IDbConnection exdbConnection = null);
        Task<int> DeleteFAQ(int faqId, IDbConnection exdbConnection = null);
        Task<int> DeleteFAQAnswers(int faqId, IDbConnection exdbConnection = null);
        Task<FaqMaster> GetFAQForUpdate(int FaqId, IDbConnection exdbConnection = null);
        Task<FaqDetailMaster> GetFAQAnswerForUpdate(int FaqDetailId, IDbConnection exdbConnection = null);
        Task<int> UpdateFAQ(FaqMaster faqMaster, IDbConnection exdbConnection = null);
        Task<int> UpdateFAQDetail(FaqDetailMaster faqMaster, IDbConnection exdbConnection = null);
    }
}
