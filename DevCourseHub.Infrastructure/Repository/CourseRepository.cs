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
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Course> GetAllCoursesQueryable()
        {
            return _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Category)
                .OrderByDescending(c => c.CreatedAt);
        }

        public IQueryable<Course> GetCourseByInstructorQueryable(Guid instructorId)
        {
            return _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Category)

                .Where(x => x.InstructorId == instructorId)
                .OrderByDescending(c => c.CreatedAt);
        }

        public async Task<Course?> GetCourseDetailAsync(Guid id)
        {
            return await _context.Courses
                .Include(x => x.Instructor)
                .Include(c => c.Category)

                .Include(x => x.Sections)
                    .ThenInclude(s => s.Lessons)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Course?> GetCourseWithInstructorAsync(Guid id)
        {
            return await _context.Courses
                .Include(x => x.Instructor)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<Course> GetPublishedCoursesQueryable()
        {
            return _context.Courses
                 .Where(x => x.IsPublished)
                 .Include(x => x.Instructor)
                 .Include(c => c.Category)
                 .OrderByDescending(x => x.CreatedAt);

        }
    }
}
