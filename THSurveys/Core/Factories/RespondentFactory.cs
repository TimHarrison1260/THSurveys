using System;
using System.Collections.ObjectModel;

using Core.Model;

namespace Core.Factories
{
    public static class RespondentFactory
    {
        public static Respondent Create()
        {
            //  Create concrete implementation of Respondent class
            var respondent = new ConcreteRespondent();
            //  set default values and initialise responses.
            respondent.DateTaken = DateTime.Now;
            respondent.Responses = new Collection<ActualResponse>();

            return respondent;
        }
    }
}
