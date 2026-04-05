using AutoMapper;
using DevCourseHub.Application.DTOs.Category;
using DevCourseHub.Application.DTOs.Common;
using DevCourseHub.Application.Interfaces;
using DevCourseHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
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

        public CategoryServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryDetailDto> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            var exists = await _unitOfWork.Categories.ExistsByNameAsync(createCategoryDto.Name);

            if (exists)
            {
                throw new InvalidOperationException("Bu kategori zaten mevcut");
            }

            var category = new Category { Name = createCategoryDto.Name.Trim() };

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            var createdCategory = await _unitOfWork.Categories.GetByIdAsync(category.Id);
            return _mapper.Map<CategoryDetailDto>(createdCategory);

        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return false;
            }

            var hasCourses = await _unitOfWork.Categories.HasCoursesAsync(id);
            if (hasCourses)
            {
                throw new InvalidOperationException("Bu kategoriye ait kurslar mevcut, silinemez");
            }
                _unitOfWork.Categories.Remove(category);
                await _unitOfWork.SaveChangesAsync();

            return true;

            }

        public async Task<PagedResultDto<CategoryDto>> GetAllAsync(GetCategoryQueryDto query)
        {
            var categoryQuery = _unitOfWork.Categories.GetAllQueryable();

            if (!string.IsNullOrEmpty(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                categoryQuery = categoryQuery.Where(c => c.Name.ToLower().Contains(search));
            }

            var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
            var pageSize = query.PageSize < 1 ? 10 : query.PageSize;
            if (pageSize > 50) { pageSize = 50; }

            var totalCount = await categoryQuery.CountAsync();

            var categories = await categoryQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = _mapper.Map<List<CategoryDto>>(categories);

            return new PagedResultDto<CategoryDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<List<CategoryDto>> GetAllDropdownAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllQueryable()
                .OrderBy(c => c.Name)
                .ToListAsync();
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDetailDto> GetByIdAsync(Guid id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return null;
            }

            return _mapper.Map<CategoryDetailDto>(category);
        }

        public async Task<CategoryDetailDto> UpdateAsync(Guid id, UpdateCategoryDto updateCategoryDto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category is null)
            {
                return null;
            }

            var exists = await _unitOfWork.Categories.ExistsByNameAsync(updateCategoryDto.Name, id);
            if (exists)
            {
                throw new InvalidOperationException("Bu kategori zaten mevcut");
            }

            category.Name = updateCategoryDto.Name.Trim();

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();

            var updatedCategory = await _unitOfWork.Categories.GetByIdAsync(id);
            return _mapper.Map<CategoryDetailDto>(updatedCategory);
        }
    }
}
