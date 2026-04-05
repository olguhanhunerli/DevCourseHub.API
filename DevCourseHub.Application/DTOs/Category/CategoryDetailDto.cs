using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCourseHub.Application.DTOs.Category
{
    public class CategoryDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int CourseCount { get; set; }
    }
}
