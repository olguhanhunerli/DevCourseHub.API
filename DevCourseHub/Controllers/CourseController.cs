using DevCourseHub.Application.DTOs.Course;
using DevCourseHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevCourseHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCourses([FromQuery] GetCourseQueryDto query)
        {
            var result = await _courseService.GetAllPublishedAsync(query);
            return Ok(result);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCoursesForAdmin([FromQuery] GetCourseQueryDto query)
        {
            var result = await _courseService.GetAllForAdminAsync(query);
            return Ok(result);
        }

        [HttpGet("my")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> GetMyCourses([FromQuery] GetCourseQueryDto query)
        {
            var result = await _courseService.GetMyCoursesAsync(query);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCourseById(Guid id)
        {
            var course = await _courseService.GetByIdAsync(id);

            if (course is null)
                return NotFound();

            return Ok(course);
        }

        [HttpPost]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto request)
        {
            var createdCourse = await _courseService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetCourseById),
                new { id = createdCourse.Id },
                createdCourse);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] UpdateCourseDto request)
        {
            var updatedCourse = await _courseService.UpdateAsync(id, request);

            if (updatedCourse is null)
                return NotFound();

            return Ok(updatedCourse);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var deleted = await _courseService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}/publish")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> PublishCourse(Guid id)
        {
            var published = await _courseService.PublishedAsync(id);
            return Ok(new { message = "Kurs başarıyla yayınlandı." });
        }


    }
}