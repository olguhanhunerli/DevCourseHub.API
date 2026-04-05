using AutoMapper;
using DevCourseHub.Application.DTOs.Enrollment;
using DevCourseHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCourseHub.Application.Mappings
{
    public class EnrollmentProfile: Profile
    {
        public EnrollmentProfile() 
        {
            CreateMap<Enrollment, EnrollmentDto>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Course.Category.Name))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Course.Level.ToString()))
                .ForMember(dest => dest.EnrollAt, opt => opt.MapFrom(src => src.EnrolledAt));

            CreateMap<Enrollment, MyCourseDto>()
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Course.Title))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Course.Category.Name))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Course.Level.ToString()))
                .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(src => src.Course.IsPublished));
        }
    }
}
