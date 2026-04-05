using DevCourseHub.Application.Interfaces.Repository;
using DevCourseHub.Domain.Entities;
using DevCourseHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCourseHub.Infrastructure.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Category> GetAllWithCourses()
        {
            return _context.Categories.OrderBy(x => x.Name);
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Name.ToLower() == name.Trim().ToLower());
        }
        public async Task<Category> GetByIdAsync(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            var normalizedName = name.Trim().ToLower();
            return await _context.Categories.AnyAsync(x => x.Name.ToLower() == normalizedName);
        }

        public Task<bool> ExistsByNameAsync(string name, Guid excludeId)
        {
            var normalizedName = name.Trim().ToLower();
            return _context.Categories.AnyAsync(x => x.Id != excludeId && x.Name.ToLower() == normalizedName);
        }

        public async Task<bool> HasCoursesAsync(Guid categoryId)
        {
            return await _context.Courses.AnyAsync(c => c.CategoryId == categoryId);
        }
    }
}
