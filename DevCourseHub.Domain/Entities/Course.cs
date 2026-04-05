using DevCourseHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace DevCourseHub.Domain.Entities
{
    public class Course: BaseEntity
    {
       
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public CourseLevel Level { get; set; } = CourseLevel.Beginner;
        public string? ThumbnailUrl { get; set; }
        public bool IsPublished { get; set; } = false;
        public DateTime? UpdatedAt { get; set; }
        public Guid InstructorId { get; set; }
        public User Instructor { get; set; }
        public ICollection<Section> Sections { get; set; } = new List<Section>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<CourseReview> Reviews { get; set; } = new List<CourseReview>();
    }
}
