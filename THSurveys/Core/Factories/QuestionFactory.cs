using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Model;

namespace Core.Factories
{
    public static class QuestionFactory
    {
        public static Question CreateQuestion()
        {
            Question question = new ConcreteQuestion();

            //  Set default values
            question.QuestionId = 0;
            question.SequenceNumber = 0;
            question.Survey = new ConcreteSurvey();
            question.AvailableResponses = new Collection<AvailableResponse>();

            return question;
        }
    }
}
