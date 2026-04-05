using DevCourseHub.Application.DTOs.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCourseHub.Application.Validators.Category
{
    public class GetCategoryQueryValidator : AbstractValidator<GetCategoryQueryDto>
    {
        public GetCategoryQueryValidator() 
        {
            RuleFor(x => x.Search)
                .MaximumLength(100).WithMessage("Search term cannot exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Search));

            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");
            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");
        }
    }
}
