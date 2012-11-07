using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;           //  Access to AutoMapper
using Core.Model;           //  Access to the Core.Model: Business model.
using THSurveys.Models;     //  Access to the view models for the aplication pages
using THSurveys.Models.Question;
using THSurveys.Models.Survey;

namespace THSurveys.Mappings
{
    /// <summary>
    /// This <c>AutoMapperConfiguration</c> class is responsible for 
    /// initialising the AutoMapper instance.
    /// It registers an instance of an AutoMapper.Profile class, also in this
    /// module.
    /// </summary>
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            //  Instantiate the maps
            Mapper.Initialize(c => c.AddProfile(new THSurveysProfile()));
        }
    }

    /// <summary>
    /// The profile used for this application.
    /// It contains the individual mappings for classes
    /// used within this application.
    /// </summary>
    public class THSurveysProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            //  Map by convention: where possible, and add specific rules where the object names don't match.

            //  Map the Survey model to the Survey Summary partial view
            Mapper.CreateMap<Survey, SurveySummaryViewModel>()
                //  Map the Category description to the display category
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))       //This needs to call a getUserName method of the User Repository.
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.GetName(typeof(SurveyStatusEnum), src.Status)))   // Map to the enumeration getName
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Description))     // change to function to get Description
                .ForMember(dest => dest.Responses, opt => opt.MapFrom(src => src.Respondents.Count()))
                ;

            //  Map the CreateSurvey view model to the survey model.
            //  Title, CategoryId, Status, StatusDate and Category all map by convention
            Mapper.CreateMap<CreateSurveyViewModel, Survey>()
                .ForMember(dest => dest.SurveyId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.StatusDate, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.IsTemplate, opt => opt.Ignore())
                .ForMember(dest => dest.Respondents, opt => opt.Ignore())
                .ForMember(dest => dest.Questions, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                ;

            //  For mapping the Likert Templated responses to the partial view
            //  displaying them as part of the Add Questions.
            Mapper.CreateMap<TemplateResponse, LikertResponseViewModel>();

            //  For mapping the AddQuestions to the Question model.
            Mapper.CreateMap<AddQuestionsViewModel, Question>()
                //  This .ConstructUsing allows us to use the QuestionFactory to create the instance of Question 
                //  (because  Question is an abstract class).
                .ConstructUsing((Func<ResolutionContext, Question>)(r => Core.Factories.QuestionFactory.CreateQuestion()))
                .ForMember(dest => dest.SequenceNumber, opt => opt.Ignore())
                .ForMember(dest => dest.Survey, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionId, opt => opt.Ignore())
                .ForMember(dest => dest.AvailableResponses, opt => opt.Ignore())
                ;

            //  For mapping the Likert Templated Responses to the Available responses for a question
            Mapper.CreateMap<TemplateResponse, AvailableResponse>()
                .ForMember(dest => dest.Question, opt => opt.Ignore())
                ;

            //  Map the questions to the Question list partial view, displayed in the AddQuestions
            Mapper.CreateMap<Question, AddQuestionsListViewModel>()
                .ForMember(dest => dest.Title, opt => opt.UseValue(string.Empty))
                ;

            //  For mapping the survey to the ApprovalListViewModel
            Mapper.CreateMap<Survey, ApprovalListViewModel>()
                .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.Category.Description))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                //.ForMember(d => d.SurveyId, o => o.MapFrom(s => s.SurveyId))
                //.ForMember(dest => dest.Approve, opt => opt.UseValue(false))
                //.ForMember(dest => dest.Input.SurveyId, opt => opt.MapFrom(src => src.SurveyId))
                //.ForMember(dest => dest.Input.Approve, opt => opt.UseValue(false))
                .ForMember(dest => dest.Input, opt => opt.Ignore()) 
                .AfterMap((src, dest) => dest.Input = new ApprovalListViewModel.ApprovalInputViewModel())
                .AfterMap((src, dest) => dest.Input.SurveyId = src.SurveyId )
                .AfterMap((src, dest) => dest.Input.Approve = false)
                ;
            //  See https://groups.google.com/forum/?fromgroups=#!topic/automapper-users/YG13vBf9lX8 for explanation of 
            //  the above use of .AfterMap, the dest side cannot contain child classes.

            //  Assert the mappings are valid and can be used.
            Mapper.AssertConfigurationIsValid();
        }
    }

}