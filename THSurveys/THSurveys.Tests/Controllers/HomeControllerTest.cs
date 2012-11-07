using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using THSurveys;
using THSurveys.Controllers;

using Core.Interfaces;

namespace THSurveys.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController(new MockSurveyRepository(), new MockCategoryRepository());

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Modify this template to jump-start your ASP.NET MVC application.", result.ViewBag.Message);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController(new MockSurveyRepository(), new MockCategoryRepository());

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController(new MockSurveyRepository(),new MockCategoryRepository());

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }

    public class MockSurveyRepository : ISurveyRepository
    {

        public IQueryable<Core.Model.Survey> GetTopTenSurveys()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Core.Model.Survey> GetAvailableSurveys()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Core.Model.Survey> GetAllSurveys()
        {
            throw new NotImplementedException();
        }

        public Core.Model.Survey GetSurvey(long id)
        {
            throw new NotImplementedException();
        }

        public long CreateSurvey(Core.Model.Survey survey)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Core.Model.Survey > GetSurveysForCategory( long categoryId)
        {
            throw new NotImplementedException();
        }
    }


    public class MockCategoryRepository : ICategoryRepository
    {

        public IQueryable<Core.Model.Category> GetAll()
        {
            throw new NotImplementedException();
        }

        public Core.Model.Category GetCategory(long Id)
        {
            throw new NotImplementedException();
        }

        public void Add(Core.Model.Category category)
        {
            throw new NotImplementedException();
        }
    }



}
