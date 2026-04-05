using DevCourseHub.Domain.Entities;
using DevCourseHub.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCourseHub.Infrastructure.Persistence.Seed
{
    public class DbInitializer
    {

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

            if (await context.Users.AnyAsync())
                return;

            var admin = new User
            {
                Id = Guid.NewGuid(),
                FullName = "System Admin",
                Email = "admin@devcoursehub.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123*"),
                Role = UserRole.Admin
            };

            var instructor = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Demo Instructor",
                Email = "instructor@devcoursehub.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Instructor123*"),
                Role = UserRole.Instructor
            };

            var student = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Demo Student",
                Email = "student@devcoursehub.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student123*"),
                Role = UserRole.Student
            };

            await context.Users.AddRangeAsync(admin, instructor, student);
            await context.SaveChangesAsync();
            var backendCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Backend"
            };

            var frontendCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Frontend"
            };

            var databaseCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Database"
            };

            var securityCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Security"
            };

            await context.Categories.AddRangeAsync(
                backendCategory,
                frontendCategory,
                databaseCategory,
                securityCategory);

            await context.SaveChangesAsync();
            var course1 = new Course
            {
                Id = Guid.NewGuid(),
                Title = "ASP.NET Core Web API Mastery",
                Description = "Learn how to build production-ready Web APIs with ASP.NET Core, EF Core, JWT and clean architecture.",
                CategoryId = backendCategory.Id,
                Level = CourseLevel.Intermediate,
                ThumbnailUrl = "https://picsum.photos/200/300",
                IsPublished = true,
                UpdatedAt = DateTime.UtcNow,
                InstructorId = instructor.Id
            };

            var course2 = new Course
            {
                Id = Guid.NewGuid(),
                Title = "React for Modern Frontend",
                Description = "Build modern frontend applications with React, hooks, routing and reusable components.",
                CategoryId = frontendCategory.Id,
                Level = CourseLevel.Beginner,
                ThumbnailUrl = "https://picsum.photos/200/300",
                IsPublished = true,
                UpdatedAt = DateTime.UtcNow,
                InstructorId = instructor.Id
            };

            var course3 = new Course
            {
                Id = Guid.NewGuid(),
                Title = "PostgreSQL Essentials",
                Description = "Master relational database design, indexing, querying and optimization with PostgreSQL.",
                CategoryId = databaseCategory.Id,
                Level = CourseLevel.Beginner,
                ThumbnailUrl = "https://picsum.photos/200/300",
                IsPublished = true,
                UpdatedAt = DateTime.UtcNow,
                InstructorId = instructor.Id
            };

            var course4 = new Course
            {
                Id = Guid.NewGuid(),
                Title = "Advanced Authentication Systems",
                Description = "Deep dive into JWT, refresh tokens, authorization strategies and secure backend design.",
                CategoryId = securityCategory.Id,
                Level = CourseLevel.Advanced,
                ThumbnailUrl = "https://picsum.photos/200/300",
                IsPublished = false,
                UpdatedAt = DateTime.UtcNow,
                InstructorId = instructor.Id
            };

            await context.Courses.AddRangeAsync(course1, course2, course3, course4);
            await context.SaveChangesAsync();

            var sections = new List<Section>
        {
            new()
            {
                Id = Guid.NewGuid(),
                CourseId = course1.Id,
                Title = "Introduction to ASP.NET Core",
                DisplayOrder = 1
            },
            new()
            {
                Id = Guid.NewGuid(),
                CourseId = course1.Id,
                Title = "Architecture and Layers",
                DisplayOrder = 2
            },
            new()
            {
                Id = Guid.NewGuid(),
                CourseId = course2.Id,
                Title = "React Basics",
                DisplayOrder = 1
            },
            new()
            {
                Id = Guid.NewGuid(),
                CourseId = course2.Id,
                Title = "Hooks and State",
                DisplayOrder = 2
            },
            new()
            {
                Id = Guid.NewGuid(),
                CourseId = course3.Id,
                Title = "Database Fundamentals",
                DisplayOrder = 1
            },
            new()
            {
                Id = Guid.NewGuid(),
                CourseId = course3.Id,
                Title = "Queries and Performance",
                DisplayOrder = 2
            }
        };

            await context.Sections.AddRangeAsync(sections);
            await context.SaveChangesAsync();

            var lessons = new List<Lesson>
        {
            new()
            {
                Id = Guid.NewGuid(),
                SectionId = sections[0].Id,
                Title = "Welcome to the Course",
                Content = "Overview of the course and what you will build.",
                VideoUrl = "https://example.com/videos/welcome",
                DisplayOrder = 1,
                DurationInMinutes = 6,
                IsPreview = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                SectionId = sections[0].Id,
                Title = "What is ASP.NET Core?",
                Content = "Understanding ASP.NET Core and why it is used for backend systems.",
                VideoUrl = "https://example.com/videos/aspnetcore-intro",
                DisplayOrder = 2,
                DurationInMinutes = 12,
                IsPreview = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                SectionId = sections[1].Id,
                Title = "Clean Architecture Overview",
                Content = "Learn how clean architecture helps structure scalable software.",
                VideoUrl = "https://example.com/videos/clean-architecture",
                DisplayOrder = 1,
                DurationInMinutes = 15,
                IsPreview = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                SectionId = sections[1].Id,
                Title = "Dependency Injection in Practice",
                Content = "Use dependency injection to manage services and dependencies.",
                VideoUrl = "https://example.com/videos/dependency-injection",
                DisplayOrder = 2,
                DurationInMinutes = 18,
                IsPreview = false
            },

            new()
            {
                Id = Guid.NewGuid(),
                SectionId = sections[2].Id,
                Title = "React Project Setup",
                Content = "Set up a modern React project and understand the folder structure.",
                VideoUrl = "https://example.com/videos/react-setup",
                DisplayOrder = 1,
                DurationInMinutes = 9,
                IsPreview = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                SectionId = sections[2].Id,
                Title = "Components and Props",
                Content = "Learn component-based UI development and props.",
                VideoUrl = "https://example.com/videos/components-props",
                DisplayOrder = 2,
                DurationInMinutes = 11,
                IsPreview = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                SectionId = sections[3].Id,
                Title = "useState and Events",
                Content = "Manage local state and user interactions with useState.",
                VideoUrl = "https://example.com/videos/usestate-events",
                DisplayOrder = 1,
                DurationInMinutes = 14,
                IsPreview = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                SectionId = sections[3].Id,
                Title = "useEffect Explained",
                Content = "Understand side effects and data fetching in React.",
                VideoUrl = "https://example.com/videos/useeffect",
                DisplayOrder = 2,
                DurationInMinutes = 16,
                IsPreview = false
            },

            new()
            {
                Id = Guid.NewGuid(),
                SectionId = sections[4].Id,
                Title = "PostgreSQL Basics",
                Content = "Introduction to relational databases and PostgreSQL essentials.",
                VideoUrl = "https://example.com/videos/postgresql-basics",
                DisplayOrder = 1,
                DurationInMinutes = 10,
                IsPreview = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                SectionId = sections[4].Id,
                Title = "Tables, Keys and Relationships",
                Content = "Design strong relational schemas with foreign keys.",
                VideoUrl = "https://example.com/videos/keys-relationships",
                DisplayOrder = 2,
                DurationInMinutes = 13,
                IsPreview = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                SectionId = sections[5].Id,
                Title = "Writing Better Queries",
                Content = "Use filtering, joins and ordering effectively.",
                VideoUrl = "https://example.com/videos/better-queries",
                DisplayOrder = 1,
                DurationInMinutes = 17,
                IsPreview = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                SectionId = sections[5].Id,
                Title = "Indexes and Performance",
                Content = "Learn when and how to use indexes for performance.",
                VideoUrl = "https://example.com/videos/indexes-performance",
                DisplayOrder = 2,
                DurationInMinutes = 19,
                IsPreview = false
            }
        };

            await context.Lessons.AddRangeAsync(lessons);
            await context.SaveChangesAsync();

            var enrollments = new List<Enrollment>
        {
            new()
            {
                Id = Guid.NewGuid(),
                UserId = student.Id,
                CourseId = course1.Id,
                EnrolledAt = DateTime.UtcNow.AddDays(-5)
            },
            new()
            {
                Id = Guid.NewGuid(),
                UserId = student.Id,
                CourseId = course2.Id,
                EnrolledAt = DateTime.UtcNow.AddDays(-3)
            },
            new()
            {
                Id = Guid.NewGuid(),
                UserId = student.Id,
                CourseId = course3.Id,
                EnrolledAt = DateTime.UtcNow.AddDays(-2)
            }
        };

            await context.Enrollments.AddRangeAsync(enrollments);
            await context.SaveChangesAsync();

            var lessonProgresses = new List<LessonProgress>
        {
            new()
            {
                Id = Guid.NewGuid(),
                UserId = student.Id,
                LessonId = lessons[0].Id,
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow.AddDays(-4)
            },
            new()
            {
                Id = Guid.NewGuid(),
                UserId = student.Id,
                LessonId = lessons[1].Id,
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow.AddDays(-4)
            },
            new()
            {
                Id = Guid.NewGuid(),
                UserId = student.Id,
                LessonId = lessons[4].Id,
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow.AddDays(-2)
            }
        };

            await context.LessonProgresses.AddRangeAsync(lessonProgresses);
            await context.SaveChangesAsync();

            var reviews = new List<CourseReview>
        {
            new()
            {
                Id = Guid.NewGuid(),
                UserId = student.Id,
                CourseId = course1.Id,
                Rating = 5,
                Comment = "Harika bir kurs, backend mimarisini çok iyi anlattı.",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new()
            {
                Id = Guid.NewGuid(),
                UserId = student.Id,
                CourseId = course2.Id,
                Rating = 4,
                Comment = "React tarafında başlangıç için gayet anlaşılır ve akıcıydı.",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            }
        };

            await context.CourseReviews.AddRangeAsync(reviews);
            await context.SaveChangesAsync();
        }
    }
}
