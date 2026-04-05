using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevCourseHub.Application.DTOs.Common;
using DevCourseHub.Application.DTOs.Course;
using DevCourseHub.Application.Interfaces;
using DevCourseHub.Domain.Entities;
using DevCourseHub.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DevCourseHub.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<CourseDetailDto?> CreateAsync(CreateCourseDto request)
        {
            if (!Enum.TryParse<CourseLevel>(request.Level, true, out var level))
                throw new Exception("Geçersiz Course Level.");
            var currentUserId = _currentUserService.UserId;
            if(currentUserId is null)
                throw new UnauthorizedAccessException("Kullanıcı doğrulanamadı.");

            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
            if(category is null)
                throw new Exception("Kategori bulunamadı.");
            var course = new Course
            {
                Title = request.Title,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Level = level,
                IsPublished = false,
                UpdatedAt = DateTime.UtcNow,
                InstructorId = currentUserId.Value,
                ThumbnailUrl = "https://picsum.photos/200/300"

            };

            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.SaveChangesAsync();

            var createdCourse = await _unitOfWork.Courses.GetCourseWithInstructorAsync(course.Id) ?? throw new Exception("Oluşturulan Kurs Bulunamadı.");

            return _mapper.Map<CourseDetailDto>(createdCourse);
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var course = await GetOwnerAdminCourseAsync(id);
            _unitOfWork.Courses.Remove(course);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultDto<CourseDto>> GetAllForAdminAsync(GetCourseQueryDto query)
        {
            var coursesQuery = _unitOfWork.Courses.GetAllCoursesQueryable();
            return await ApplyPaginationAsync(coursesQuery,query);
        }

        public async Task<PagedResultDto<CourseDto>> GetAllPublishedAsync(GetCourseQueryDto query)
        {
            var coursesQuery = _unitOfWork.Courses.GetPublishedCoursesQueryable();
            return await ApplyPaginationAsync(coursesQuery, query);
        }

        public async Task<CourseDetailDto?> GetByIdAsync(Guid id)
        {
            var course = await _unitOfWork.Courses.GetCourseDetailAsync(id);
           if(course == null)
                return null;
           course.Sections = course.Sections.OrderBy(s => s.DisplayOrder).ToList();
            foreach (var section in course.Sections)
            {
                section.Lessons = section.Lessons.OrderBy(l => l.DisplayOrder).ToList();
            }

            var reviewQuery = _unitOfWork.CourseReviews.GetByCourseIdQueryable(id);

            var reviewCount = await reviewQuery.CountAsync();

            var averageRating = reviewCount > 0 ? await reviewQuery.AverageAsync(r => r.Rating) : 0;

            var result = _mapper.Map<CourseDetailDto>(course);
           
            result.ReviewCount = reviewCount;
            result.AverageRating = averageRating;

            return result;

        }

        public async Task<PagedResultDto<CourseDto>> GetMyCoursesAsync(GetCourseQueryDto query)
        {
            var currentUserId = _currentUserService.UserId;
            if (currentUserId is null)
                throw new UnauthorizedAccessException("Kullanıcı bilgisi bulunamadı.");

            var coursesQuery = _unitOfWork.Courses.GetCourseByInstructorQueryable(currentUserId.Value);
            return await ApplyPaginationAsync(coursesQuery, query);
        }

        public async Task<bool> PublishedAsync(Guid id)
        {
            var course = await GetOwnerAdminCourseAsync(id);
            
            await ValidateCourseForPublishAsync(id);

            if(course.IsPublished)
                throw new Exception("Kurs zaten yayınlanmış.");

            course.IsPublished = true;
            course.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Courses.Update(course);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<CourseDetailDto?> UpdateAsync(Guid id, UpdateCourseDto request)
        {
            var course = await GetOwnerAdminCourseAsync(id);
            if (!Enum.TryParse<CourseLevel>(request.Level, true, out var level))
                return null;

            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
            if (category == null)
                throw new Exception("Kategori bulunamadı.");

            course.Title = request.Title;
            course.Description = request.Description;
            course.CategoryId = request.CategoryId;
            course.Level = level;
            course.UpdatedAt = DateTime.UtcNow;
            course.ThumbnailUrl = "https://picsum.photos/200/300";
            _unitOfWork.Courses.Update(course);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CourseDetailDto>(course);
        }

        private async Task<PagedResultDto<CourseDto>> ApplyPaginationAsync(IQueryable<Course> coursesQuery, GetCourseQueryDto query)
        {
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                coursesQuery = coursesQuery.Where(x =>
                    x.Title.ToLower().Contains(search) ||
                    x.Description.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                var category = query.Category.Trim().ToLower();
                coursesQuery = coursesQuery.Where(x => x.Category.Name.ToLower() == category);
            }

            if (!string.IsNullOrWhiteSpace(query.Level) &&
                Enum.TryParse<CourseLevel>(query.Level, true, out var parsedLevel))
            {
                coursesQuery = coursesQuery.Where(x => x.Level == parsedLevel);
            }
            
            var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
            var pageSize = query.PageSize < 1 ? 10 : query.PageSize;
            if (pageSize > 50) pageSize = 50;

            var totalCount = await coursesQuery.CountAsync();

            var courses = await coursesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = new List<CourseDto>();

            foreach (var course in courses)
            {
                var reviewQuery = _unitOfWork.CourseReviews.GetByCourseIdQueryable(course.Id);
                var reviewCount = await reviewQuery.CountAsync();
                var averageRating = reviewCount == 0 ? 0 : await reviewQuery.AverageAsync(x => x.Rating);

                var dto = _mapper.Map<CourseDto>(course);
                dto.ReviewCount = reviewCount;
                dto.AverageRating = Math.Round(averageRating, 2);
                
                items.Add(dto);
            }

            return new PagedResultDto<CourseDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
        private async Task<Course> GetOwnerAdminCourseAsync(Guid id)
        {
            var course = await _unitOfWork.Courses.GetCourseWithInstructorAsync(id);
            if (course is null)
                throw new Exception("Course Bulunamadı.");

            var currentUserId = _currentUserService.UserId;
            var currentUserRole = _currentUserService.Role;

            if (currentUserRole == "Admin")
                return course;
            if(currentUserRole == "Instructor" && course.InstructorId == currentUserId)
                return course;

            throw new UnauthorizedAccessException("Bu işlemi gerçekleştirmek için yetkiniz yok.");
        }

        private async Task ValidateCourseForPublishAsync(Guid courseId)
        {
            var course = await _unitOfWork.Courses.GetCourseDetailAsync(courseId);

            if (course is null)
                throw new Exception("Course bulunamadı.");

            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(course.Title))
                errors.Add("Kurs başlığı zorunludur.");

            if (string.IsNullOrWhiteSpace(course.Description))
                errors.Add("Kurs açıklaması zorunludur.");

            if (course.CategoryId == Guid.Empty)
                errors.Add("Kurs kategorisi zorunludur.");

            if (course.Sections is null || !course.Sections.Any())
            {
                errors.Add("Kurs yayınlanabilmesi için en az 1 bölüm içermelidir.");
            }
            else
            {
                if (course.Sections.Any(s => s.Lessons == null || !s.Lessons.Any()))
                    errors.Add("Her bölümde en az 1 ders olmalıdır.");
            }

            if (errors.Any())
                throw new Exception(string.Join(" ", errors));
        }

    }
}

