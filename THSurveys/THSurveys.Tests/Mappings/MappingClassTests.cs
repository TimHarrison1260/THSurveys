using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using THSurveys.Models.Home;            //  TakeSurveyviewmodel
using Core.Model;                       //  Survey
using Core.Interfaces;                  //  ISurveyRepository
using Core.Factories;                   //  RespondentFactory
using THSurveys.Tests.Model;            //  MockData classes

namespace THSurveys.Tests.Mappings
{
    [TestClass]
    public class MappingClassTests
    {
        private MockData mockData = new MockData();

        [TestMethod]
        public void MapTakeSurveyVMToSurveyOK()
        {
            //  Assert
            //      1   Mock the SurveyRepository class
            var mockSurveyRepository = new Mock<ISurveyRepository>();
            mockSurveyRepository.Setup(r => r.GetSurvey(3)).Returns(mockData.GetSurvey3());
            //      2   Mock the RespondentFactory class
            var mockRespondentFactory = new Mock<RespondentFactory>();
            mockRespondentFactory.Setup(r => r.Create()).Returns(mockData.CreateRespondent());
            //      3   Mock the ActualResponseFactory class
            var mockActualResponseFactory = new Mock<ActualResponseFactory>();
            mockActualResponseFactory.Setup(f => f.Create()).Returns(mockData.CreateResponse());
            //      4   set up the TakeSurveyViewModel class
            var inputViewModel = mockData.SetTakeSurveyViewModel_3();
            //  Instantiate the class being tested
            var mapper = new THSurveys.Mappings.MapTakeSurveyViewModelToSurvey(mockSurveyRepository.Object, mockRespondentFactory.Object, mockActualResponseFactory.Object);

            //  Act
            //      Execute the Map method
            var mappedSurvey = mapper.Map(inputViewModel);

            //  Assert
            //      1   Check the Survey class is not null
            Assert.IsNotNull(mappedSurvey, "Should have returned an instance of Survey class.");
            //      2   Check the Survey class has the correct survey
            Assert.AreEqual(3D, mappedSurvey.SurveyId, "Expected survey 3 to be returned");
            //      3   Check the Survey class has the correct number of responses
            Assert.AreEqual(1, mappedSurvey.Respondents.Count(), "Expected 2 responses, as there are 2 questions.");
            //      4   Check the Survey class has the correct answer for the 1st question.
            var respondent = mappedSurvey.Respondents.First();
            var q1Response = respondent.Responses.First();
            Assert.AreEqual(5, q1Response.Response, "Expected answer 5 to question 1");

        }

        [TestMethod]
        public void MapTakeSurveyVMToSurveyFailsIfSurveyNotFound()
        {
            //  Testing for a NullReferenceException if the survey cannot
            //  be found.  This would bubble up to be handled by the
            //  application error handler.

            //  Assert
            //      1   Mock the SurveyRepository class
            var mockSurveyRepository = new Mock<ISurveyRepository>();
            mockSurveyRepository.Setup(r => r.GetSurvey(0)).Returns(mockData.GetSurvey3());
            //      2   Mock the RespondentFactory class
            var mockRespondentFactory = new Mock<RespondentFactory>();
            mockRespondentFactory.Setup(r => r.Create()).Returns(mockData.CreateRespondent());
            //      3   Mock the ActualResponseFactory class
            var mockActualResponseFactory = new Mock<ActualResponseFactory>();
            mockActualResponseFactory.Setup(f => f.Create()).Returns(mockData.CreateResponse());
            //      4   set up the TakeSurveyViewModel class
            var inputViewModel = mockData.SetTakeSurveyViewModel_3();
            //  Instantiate the class being tested
            var mapper = new THSurveys.Mappings.MapTakeSurveyViewModelToSurvey(mockSurveyRepository.Object, mockRespondentFactory.Object, mockActualResponseFactory.Object);

            //  Act
            //      Execute the Map method
            try
            {
                var mappedSurvey = mapper.Map(inputViewModel);
                //  Assert failure
                Assert.Fail("No exception thrown, expected a NullReferenece exception");
            }
            catch (Exception e)
            {
                //  Assert pass, the exception has been thrown.
                Assert.IsNotNull(e, "Exception expected");
                Assert.IsInstanceOfType(e, typeof(NullReferenceException), "Expected NullReferenceException");
            }            
        }

        [TestMethod]
        public void ReinstateTakeSurveyViewModelOK()
        {
            //  Assert
            //      1   Mock the SurveyRepository class
            var mockSurveyRepository = new Mock<ISurveyRepository>();
            mockSurveyRepository.Setup(r => r.GetSurvey(3)).Returns(mockData.GetSurvey3());
            //      2   Mock the QuestionRepository class
            var mockQuestionRepository = new Mock<IQuestionRepository>();

            //  Define a Queue containing multiple calls to the Mocked repository method
            //  see: http://haacked.com/archive/2009/09/28/moq-sequences.aspx.  Workaround, no longer necessary
            //var questionQueue = new Queue<Question>(new Question[] {
            //    mockData.GetQuestion(1),
            //    mockData.GetQuestion(2)
            //});
            //mockQuestionRepository.Setup(r => r.GetQuestion(0)).Returns(() => questionQueue.Dequeue());
            
            //  see: http://codecontracts.info/2011/07/28/moq-setupsequence-is-great-for-mocking/: Moq SetupSequence<>.  Awesome.
            //  Also, Phil's would've worked if I'd used the 'It.IsAny<long>()' to allow it to match any parameter and therefore
            //  receive the value from the sequence. (or Phil's Queue).  this is the learning curve of Moq, and it's good that
            //  I didn't realise the 'It.IsAny<>()' existed as I wouldn't have found the SetupSequence method.
            mockQuestionRepository.SetupSequence(r => r.GetQuestion(It.IsAny<long>()))  // hopefully allows any long value to be passed so it matched the call.
                .Returns(mockData.GetQuestion(1))
                .Returns(mockData.GetQuestion(2));

            //      4   set up the TakeSurveyViewModel class
            var inputViewModel = mockData.SetTakeSurveyViewModel_3();
            //  Instantiate the class being tested
            var mapper = new THSurveys.Mappings.ReinstateTakeSurveyViewModel(mockSurveyRepository.Object, mockQuestionRepository.Object);

            //  Act
            //      Execute the Map method
            var mappedSurvey = mapper.Map(inputViewModel);

            //  Assert
            //      Check the missing fields are reinstated.
            //      1   Check the Category description is "First Category".
            Assert.AreEqual("First Category", mappedSurvey.CategoryDescription, "Should have reinstated the Category Description");
            //      2   Check the statusData is "10/11/12")
            Assert.AreEqual(string.Format("{0:d}", DateTime.Parse("10/11/2012")), mappedSurvey.StatusDate, "Shauld have reinstated the status date of '10/11/2012'");
            //      3   Check the Title has been reinstated "A Third Survey (Live)"
            Assert.AreEqual("A Third Survey (Live)", mappedSurvey.Title, "Should have reinstated the Title of 'A Third Survey (Live)'");
            //      4   Check the UserName has been reinstated "Tim"
            Assert.AreEqual("Tim", mappedSurvey.UserName, "Should have reinstated the UserName of 'Tim'");
            //      5   Check the text for the 1st question has been reinstated

            var question1 = mappedSurvey.Questions.First();

            Assert.AreEqual("Text for Question 1", question1.Text, "Should have reinstated the Text for Question 1");
            //      6   Check the available responses have been reinstated for the first question            
            Assert.AreEqual(5, question1.Responses.Count(), "Should have reinstated 5 responses to question 1");
            //      7   Check the text for the first available response has been reinstated, for the first question.
            Assert.AreEqual("Strongly Disagree", question1.Responses.First().Text, "Should have reinstated the text for the first available response to Question 1");
        }


        [TestMethod]
        public void ReinstateTakeSurveyViewModelFailsIfQuestionNotFound()
        {
            //  Assert
            //      1   Mock the SurveyRepository class
            var mockSurveyRepository = new Mock<ISurveyRepository>();
            mockSurveyRepository.Setup(r => r.GetSurvey(3)).Returns(mockData.GetSurvey3());
            //      2   Mock the QuestionRepository class
            var mockQuestionRepository = new Mock<IQuestionRepository>();
            mockQuestionRepository.SetupSequence(r => r.GetQuestion(It.IsAny<long>()))  // hopefully allows any long value to be passed so it matched the call.
            //    .Returns(mockData.GetQuestion(0))  // Leave this out, a call will get a null back and cause the exception.
                .Returns(mockData.GetQuestion(2));
            //      3   set up the TakeSurveyViewModel class
            var inputViewModel = mockData.SetTakeSurveyViewModel_3();
            //  Instantiate the class being tested
            var mapper = new THSurveys.Mappings.ReinstateTakeSurveyViewModel(mockSurveyRepository.Object, mockQuestionRepository.Object);

            //  Act
            //      Execute the Map method
            try
            {
                var mappedSurvey = mapper.Map(inputViewModel);
                //  Assert failure
                Assert.Fail("No exception thrown, expected a NullReferenece exception");
            }
            catch (Exception e)
            {
                //  Assert pass, the exception has been thrown.
                Assert.IsNotNull(e, "Exception expected");
                Assert.IsInstanceOfType(e, typeof(NullReferenceException), "Expected NullReferenceException");
            }
        }

    }
}
