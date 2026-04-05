using DevCourseHub.Application.Interfaces;
using DevCourseHub.Application.Interfaces.Repository;
using DevCourseHub.Infrastructure.Persistence;
using DevCourseHub.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DevCourseHub.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUserRepository Users { get; }
        public ICourseRepository Courses { get; }

        public ILessonRepository Lessons { get; }
        public ISectionRepository Sections { get; }
        public IEnrollmentRepository Enrollments { get; }
        public ILessonProgressRepository LessonsProgress { get; }
        public ICourseReviewRepository CourseReviews { get; }
        public ICategoryRepository Categories { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Users = new UserRepository(context);
            Courses = new CourseRepository(context);
            Lessons = new LessonRepository(context);
            Sections = new SectionRepository(context);
            Enrollments = new EnrollmentRepository(context);
            LessonsProgress = new LessonProgressRepository(context);
            CourseReviews = new CourseReviewRepository(context);
            Categories = new CategoryRepository(context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
