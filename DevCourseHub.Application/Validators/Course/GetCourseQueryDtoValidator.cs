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
    public class GetCourseQueryDtoValidator : AbstractValidator<GetCourseQueryDto>
    {
        public GetCourseQueryDtoValidator()
        {
            RuleFor(x => x.Search)
                .MaximumLength(200)
                .WithMessage("Arama metni en fazla 200 karakter olabilir.")
                .When(x => !string.IsNullOrWhiteSpace(x.Search));

            RuleFor(x => x.Category)
                .MaximumLength(100)
                .WithMessage("Kategori en fazla 100 karakter olabilir.")
                .When(x => !string.IsNullOrWhiteSpace(x.Category));

            RuleFor(x => x.Level)
                .Must(BeValidCourseLevel)
                .WithMessage("Geçersiz kurs seviyesi. Geçerli değerler: Beginner, Intermediate, Advanced.")
                .When(x => !string.IsNullOrWhiteSpace(x.Level));

            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("PageNumber 0'dan büyük olmalıdır.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("PageSize 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(50).WithMessage("PageSize en fazla 50 olabilir.");
        }

        private bool BeValidCourseLevel(string? level)
        {
            if (string.IsNullOrWhiteSpace(level))
                return true;

            return Enum.TryParse<CourseLevel>(level, true, out _);
        }
    }
}
