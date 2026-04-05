using DevCourseHub.Application.DTOs.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCourseHub.Application.Validators.Category
{
    public class UpdateCategoryDtoValidator: AbstractValidator<UpdateCategoryDto>
    {
      public UpdateCategoryDtoValidator() 
        {
            RuleFor(x => x.Name)
              .NotEmpty().WithMessage("Category name is required.")
              .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");
        }
    }
}
