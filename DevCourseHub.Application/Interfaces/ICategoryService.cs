using DevCourseHub.Application.DTOs.Category;
using DevCourseHub.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCourseHub.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<PagedResultDto<CategoryDto>> GetAllAsync(GetCategoryQueryDto query);
        Task<List<CategoryDto>> GetAllDropdownAsync();
        Task<CategoryDetailDto> GetByIdAsync(Guid id);
        Task<CategoryDetailDto> CreateAsync(CreateCategoryDto createCategoryDto);
        Task<CategoryDetailDto> UpdateAsync(Guid id, UpdateCategoryDto updateCategoryDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
