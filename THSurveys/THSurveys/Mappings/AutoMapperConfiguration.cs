using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;           //  Access to AutoMapper
using Core.Model;           //  Access to the Core.Model: Business model.
using THSurveys.Models;     //  Access to the view models for the aplication pages

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

            //  Include this map as the Survey doesn't match the view model exactly.
            Mapper.CreateMap<Survey, SurveySummaryViewModel>()
                //  Map the Category description to the display category
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserId.ToString()))       //This needs to call a getUserName method of the User Repository.
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.GetName(typeof(SurveyStatusEnum), src.Status)))   // Map to the enumeration getName
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Description))     // change to function to get Description
                .ForMember(dest => dest.Responses, opt => opt.MapFrom(src => src.Respondents.Count()))
                ;

            //  Assert the mappings are valid and can be used.
            Mapper.AssertConfigurationIsValid();
        }
    }

}