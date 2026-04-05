using AutoMapper;
using DevCourseHub.Application.DTOs.Category;
using DevCourseHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCourseHub.Application.Mappings
{
    public class CategoryProfile: Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Category, CategoryDetailDto>()
                .ForMember(dest => dest.CourseCount, opt => opt.MapFrom(src => src.Courses.Count)).ReverseMap();
        }
    }
}
