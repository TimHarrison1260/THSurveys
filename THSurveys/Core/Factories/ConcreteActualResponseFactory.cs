using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Model;

namespace Core.Factories
{
    public class ConcreteActualResponseFactory : ActualResponseFactory
    {
        public override Model.ActualResponse Create()
        {
            //  Create concrete implementation of ActualResponse class
            //  no default values requred, just return concrete class
            return new ConcreteActualResponse();
        }
    }
}
