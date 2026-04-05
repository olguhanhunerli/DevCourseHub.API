using DevCourseHub.Application.Interfaces;
using DevCourseHub.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
namespace DevCourseHub.Application.DependencyInjection
{
    public static class ServicesRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ServicesRegistration).Assembly);

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(typeof(ServicesRegistration).Assembly);


            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<IProgressService, ProgressService>();
            services.AddScoped<IReviewService, ReviewService>();

            return services;
        }
    }
}

