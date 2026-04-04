using AutoMapper;
using DevCourseHub.Application.DTOs.Course;
using DevCourseHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCourseHub.Application.Mappings
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>()
               .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.ToString()))
               .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.FullName)).ReverseMap();

            CreateMap<Course, CourseDetailDto>()
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.ToString()))
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.FullName)).ReverseMap();
            ;
        }
    }
}