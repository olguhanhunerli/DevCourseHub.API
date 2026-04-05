using AutoMapper;
using DevCourseHub.Application.DTOs.Common;
using DevCourseHub.Application.DTOs.Course;
using DevCourseHub.Application.DTOs.Enrollment;
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
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<EnrollmentDto> EnrollAsync(Guid courseId)
        {
            var currentUserId = _currentUserService.UserId;
            if (currentUserId is null)
            {
                throw new Exception("Kullanıcı doğrulanamadı.");
            }
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
            {
                throw new Exception("Kurs bulunamadı.");
            }

            if (!course.IsPublished)
            {
                throw new Exception("Yayınlanmamış kursa kayıt yapılamaz.");
            }

            var isAlreadyEnrolled = await _unitOfWork.Enrollments.IsUserEnrolledAsync(currentUserId.Value, courseId);

            if (isAlreadyEnrolled)
                throw new Exception("Bu kursa zaten kayıtlısınız.");

            var newEnrollment = new Enrollment
            {
                CourseId = courseId,
                UserId = currentUserId.Value,
                EnrolledAt = DateTime.UtcNow
            };

            await _unitOfWork.Enrollments.AddAsync(newEnrollment);
            await _unitOfWork.SaveChangesAsync();

            newEnrollment.Course = course;

            return _mapper.Map<EnrollmentDto>(newEnrollment);
        }

       
        public async Task<PagedResultDto<EnrollmentDto>> GetMyCourseAsync(GetEnrollmentQueryDto query)
        {
            var currentUserId = _currentUserService.UserId;
            if (currentUserId is null)
                throw new UnauthorizedAccessException("Kullanıcı bilgisi bulunamadı.");

            var enrollmentQuery = _unitOfWork.Enrollments
                .GetUserEnrollmentAsync(currentUserId.Value);

            return await ApplyPaginationAsync(enrollmentQuery, query);
        }

        private async Task<PagedResultDto<EnrollmentDto>> ApplyPaginationAsync(
            IQueryable<Domain.Entities.Enrollment> enrollmentQuery,
            GetEnrollmentQueryDto query)
        {
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                enrollmentQuery = enrollmentQuery.Where(x =>
                    x.Course.Title.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                var category = query.Category.Trim().ToLower();
                enrollmentQuery = enrollmentQuery.Where(x =>
                    x.Course.Category.Name.ToLower() == category);
            }

            if (!string.IsNullOrWhiteSpace(query.Level) &&
                Enum.TryParse<CourseLevel>(query.Level, true, out var parsedLevel))
            {
                enrollmentQuery = enrollmentQuery.Where(x => x.Course.Level == parsedLevel);
            }

            var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
            var pageSize = query.PageSize < 1 ? 10 : query.PageSize;
            if (pageSize > 50) pageSize = 50;

            var totalCount = await enrollmentQuery.CountAsync();

            var enrollments = await enrollmentQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = _mapper.Map<List<EnrollmentDto>>(enrollments);

            return new PagedResultDto<EnrollmentDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

      
    }

}
