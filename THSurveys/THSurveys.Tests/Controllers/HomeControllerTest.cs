using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using THSurveys;
using THSurveys.Controllers;
using Core.Interfaces;
using Moq;

namespace THSurveys.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            ISurveyRepository mockSurveyRepository = (ISurveyRepository)new Mock<ISurveyRepository>();
            ICategoryRepository mockCategoryRepository = (ICategoryRepository)new Mock<ICategoryRepository>();
            IQuestionRepository mockQuestionRepository = (IQuestionRepository)new Mock<IQuestionRepository>();
            IRespondentRepository mockRespondentRepository = (IRespondentRepository)new Mock<IRespondentRepository>();

            HomeController controller = new HomeController(mockSurveyRepository, mockCategoryRepository, mockQuestionRepository, mockRespondentRepository);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Modify this template to jump-start your ASP.NET MVC application.", result.ViewBag.Message);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            ISurveyRepository mockSurveyRepository = (ISurveyRepository)new Mock<ISurveyRepository>();
            ICategoryRepository mockCategoryRepository = (ICategoryRepository)new Mock<ICategoryRepository>();
            IQuestionRepository mockQuestionRepository = (IQuestionRepository)new Mock<IQuestionRepository>();
            IRespondentRepository mockRespondentRepository = (IRespondentRepository)new Mock<IRespondentRepository>();

            HomeController controller = new HomeController(mockSurveyRepository, mockCategoryRepository, mockQuestionRepository, mockRespondentRepository);


            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            ISurveyRepository mockSurveyRepository = (ISurveyRepository)new Mock<ISurveyRepository>();
            ICategoryRepository mockCategoryRepository = (ICategoryRepository)new Mock<ICategoryRepository>();
            IQuestionRepository mockQuestionRepository = (IQuestionRepository)new Mock<IQuestionRepository>();
            IRespondentRepository mockRespondentRepository = (IRespondentRepository)new Mock<IRespondentRepository>();

            HomeController controller = new HomeController(mockSurveyRepository, mockCategoryRepository, mockQuestionRepository, mockRespondentRepository);


            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }


}
