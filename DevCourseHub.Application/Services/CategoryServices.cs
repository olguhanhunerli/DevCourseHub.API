using AutoMapper;
using DevCourseHub.Application.DTOs.Category;
using DevCourseHub.Application.DTOs.Common;
using DevCourseHub.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCourseHub.Application.Services
{
    public class CategoryServices : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public Task<CategoryDetailDto> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<CategoryDto>> GetAllAsync(GetCategoryQueryDto query)
        {
            throw new NotImplementedException();
        }

        public Task<List<CategoryDto>> GetAllDropdownAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDetailDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<CreateCategoryDto> UpdateAsync(UpdateCategoryDto updateCategoryDto)
        {
            throw new NotImplementedException();
        }
    }
}
