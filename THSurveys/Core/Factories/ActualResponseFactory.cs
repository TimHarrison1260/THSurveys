using Core.Model;

namespace Core.Factories
{
    public static class ActualResponseFactory
    {
        public static ActualResponse Create()
        {
            //  Create concrete implementation of ActualResponse class
            var actualResponse = new ConcreteActualResponse();
            //  no default values requred, just return concrete class
            return actualResponse;
        }
    }
}
