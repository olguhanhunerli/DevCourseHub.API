using DevCourseHub.Application.DTOs.Course;
using DevCourseHub.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCourseHub.Application.Validators.Course
{
    public class UpdateCourseDtoValidator : AbstractValidator<UpdateCourseDto>
    {
        public UpdateCourseDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Kurs başlığı boş olamaz.")
                .MaximumLength(200).WithMessage("Kurs başlığı en fazla 200 karakter olabilir.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Kurs açıklaması boş olamaz.")
                .MaximumLength(2000).WithMessage("Kurs açıklaması en fazla 2000 karakter olabilir.");

            RuleFor(x => x.CategoryId)
               .NotEmpty().WithMessage("Kategori seçimi zorunludur.");

            RuleFor(x => x.Level)
                .NotEmpty().WithMessage("Kurs seviyesi boş olamaz.")
                .Must(BeValidCourseLevel)
                .WithMessage("Geçersiz kurs seviyesi. Geçerli değerler: Beginner, Intermediate, Advanced.");
        }

        private bool BeValidCourseLevel(string level)
        {
            return Enum.TryParse<CourseLevel>(level, true, out _);
        }
    }
}
