using DevCourseHub.Application.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCourseHub.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        ICourseRepository Courses { get; }
        ILessonRepository Lessons { get; }
        ISectionRepository Sections { get; }
        IEnrollmentRepository Enrollments { get; }
        ILessonProgressRepository LessonsProgress { get; }
        ICourseReviewRepository CourseReviews { get; }
        ICategoryRepository Categories { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
