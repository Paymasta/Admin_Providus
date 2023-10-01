using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.CMSVM
{
    public class CmsReqeust
    {
        public CmsReqeust()
        {
            this.ContentId = 0;
        }
        public string TextContent { get; set; }
        public Guid AdminGuid { get; set; }
        public long ContentId { get; set; }
    }

    public class CmsResponse
    {
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }

    public class GetCmsResponse
    {
        public GetCmsResponse()
        {
            getContent = new GetContent();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public GetContent getContent { get; set; }
    }
    public class GetContent
    {
        public string Detail { get; set; }
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }

    public class FaqQuestionReqeust
    {

        public string TextContent { get; set; }
        public Guid AdminGuid { get; set; }
    }
    public class FaqQuestionDetailReqeust
    {

        public string TextContent { get; set; }
        public int FaqId { get; set; }
        public Guid AdminGuid { get; set; }
    }

    public class FaqQuestionResponse
    {

        public string QuestionText { get; set; }
        public string Detail { get; set; }
        public long Id { get; set; }
        public Guid Guid { get; set; }
    }
    public class FaqQuestion
    {
        public FaqQuestion()
        {
            faqQuestionResponses = new List<FaqQuestionResponse>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<FaqQuestionResponse> faqQuestionResponses { get; set; }
    }
    public class QuestionAnswerRequest
    {
        public QuestionAnswerRequest()
        {
            ToDate = null;
            FromDate = null;
            SearchTest = "";
            pageNumber = 1;
            PageSize = 10;
            Status = -1;
        }
        public Guid userGuid { get; set; }
        public string SearchTest { get; set; }
        public int pageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Status { get; set; }
    }
    public class QuestionAnswer
    {
        public QuestionAnswer()
        {
            faqQuestionResponses = new List<QuestionAnswerResponse>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<QuestionAnswerResponse> faqQuestionResponses { get; set; }
    }
    public class QuestionAnswerResponse
    {
        public int TotalCount { get; set; }
        public int RowNumber { get; set; }
        public int FaqId { get; set; }
        public string QuestionText { get; set; }
        public int FaqDetailId { get; set; }
        public string Detail { get; set; }
        public string FaqIdFromDetail { get; set; }
        public string CreatedAt { get; set; }
    }
    public class FAQResponse
    {
        public FAQResponse()
        {
            this.QuestionText = string.Empty;
            this.Detail = string.Empty;
            this.FaqDetails = new List<FaqDetailResponse>();
        }
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string Detail { get; set; }
        public List<FaqDetailResponse> FaqDetails { get; set; }
    }
    public class FaqDetailResponse
    {
        public FaqDetailResponse()
        {
            this.Detail = string.Empty;
        }
        public int Id { get; set; }
        public int FaqId { get; set; }
        public string Detail { get; set; }
    }
    public class UpdateQuestionAnswerRequest
    {
        public UpdateQuestionAnswerRequest()
        {
            // UpdateQuestionAnswerDetails = new List<UpdateQuestionAnswerDetails>();
        }
        public Guid userGuid { get; set; }
        public string Faq { get; set; }
        public int FaqId { get; set; }

        public string UpdateQuestionAnswerDetailsString { get; set; }
        //  public List<UpdateQuestionAnswerDetails> UpdateQuestionAnswerDetails { get; set; }
    }
    public class UpdateQuestionAnswerDetails
    {
        public int FaqDetailId { get; set; }
        public string FaqDetail { get; set; }
    }

}

