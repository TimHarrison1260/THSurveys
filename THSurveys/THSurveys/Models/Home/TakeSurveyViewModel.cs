using System.Collections.Generic;

namespace THSurveys.Models.Home
{
    public class TakeSurveyViewModel
    {
        public long SurveyId { get; set; }
        public string Title { get; set; }
        public string CategoryDescription { get; set; }
        public string UserName { get; set; }
        public string StatusDate { get; set; }
        public IList<SurveyQuestionsViewModel> Questions { get; set; }
    }

    public class SurveyQuestionsViewModel
    {
        /// <summary>
        /// QId_SeqNo is used within the view model because both the
        /// questionId and the SequenceNumber are needed within the
        /// model binder.
        /// </summary>
        /// <remarks>
        /// It is used within the <c>RadioButtonList</c> helper to 
        /// group the radio buttons so that multiple groups can exist
        /// on the same view and not interfere with each other.
        /// It is also used within the ModelBinder to include the
        /// sequence Number (question Number) in any error message 
        /// set in the ModelState.
        /// </remarks>
        public string QId_SeqNo { get; set; }  // Set to "questionId;sequenceNumber"
        public long SequenceNumber { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }            //  Gets the selected response, in the modelbinder.
        public IList<SurveyResponsesViewModel> Responses { get; set; }
    }
    
    public class SurveyResponsesViewModel
    {
        /// <summary>
        /// This represents the QuestionId_SequenceNumber, which allows the radio 
        /// buttons to be grouped for each question.
        /// </summary>
        public string QId_SeqNo{ get; set; }
        /// <summary>
        /// Contains the answer, if the survey is being redisplayed
        /// because of validation errors.  It allows the previous
        /// selected to be reinstated, where this answer has the
        /// same value as the LikertScaleNumber.
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// Selected one gets placed in the Question.Answer property, 
        /// in the model binder.  It is nullable in case no answer is
        /// currently available for the question responses.
        /// </summary>
        public long LikertScaleNumber { get; set; }
        public string Text { get; set; }
    }
}