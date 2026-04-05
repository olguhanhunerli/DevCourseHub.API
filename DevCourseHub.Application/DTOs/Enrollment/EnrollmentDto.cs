using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCourseHub.Application.DTOs.Enrollment
{
    public class EnrollmentDto
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public Guid CategoryId { get; set; } 
        public string CategoryName { get; set; } 
        public string Level { get; set; } = string.Empty;
        public DateTime EnrollAt { get; set; }
    }
}
