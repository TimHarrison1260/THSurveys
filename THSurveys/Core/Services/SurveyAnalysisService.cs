using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Model;
using Core.Interfaces;

namespace Core.Services
{
    public abstract class SurveyAnalysisService
    {
        private readonly Survey _survey;

        public SurveyAnalysisService(Survey survey)
        {
            if (survey == null)
                throw new ArgumentNullException("Survey", "No survey supplied to analyse.");
            _survey = survey;
        }

        public long TotalRespondents()
        {
            var total = _survey.Respondents.Count();
            return total;
        }

        public DateTime DateOfFirstResponse()
        {
            var date = _survey.Respondents.Min(r => r.DateTaken);
            return date;
        }

        public DateTime DateOfLatestResponse()
        {
            var date = _survey.Respondents.Max(r => r.DateTaken);
            return date;
        }

        /// <summary>
        /// Get the total number of responses to a specific question, availablerespons combintation
        /// </summary>
        /// <param name="questionId">The question number</param>
        /// <param name="ResponseNumber">The response being totaled</param>
        /// <returns>A <c>double</c> containing the number of responses</returns>
        public double TotalForQuestionResponse(long questionId, long ResponseNumber)
        {
            return Convert.ToDouble(CountEntries(questionId, ResponseNumber));
        }

        public double PercentageForQuestionResponse(long questionId, long ResponseNumber)
        {
            var entries = CountEntries(questionId, ResponseNumber);
            var total = this.TotalRespondents();
            double percent = (Convert.ToDouble(entries) / Convert.ToDouble(total)) * 100D;
            return percent;
        }


        /// <summary>
        /// Locate and count the response entries corresponding to the 
        /// specified questionId and responseNumber.
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="ResponseNumber"></param>
        /// <returns></returns>
        private long CountEntries(long questionId, long ResponseNumber)
        {
            long counter = 0;
            foreach (var respondent in _survey.Respondents)
            {
                foreach (var ans in respondent.Responses)
                {
                    if (ans.Question == questionId && ans.Response == ResponseNumber)
                        counter++;
                }
            }
            return counter;
        }

    }
}
