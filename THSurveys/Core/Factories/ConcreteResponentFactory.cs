using System; 
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Model;

namespace Core.Factories
{
    public class ConcreteResponentFactory : RespondentFactory
    {
        public override Model.Respondent Create()
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
