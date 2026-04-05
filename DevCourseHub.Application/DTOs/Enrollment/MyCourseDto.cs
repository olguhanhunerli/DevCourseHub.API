using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCourseHub.Application.DTOs.Enrollment
{
    public class MyCourseDto
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid CategoryId { get; set; } 
        public string CategoryName { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}
