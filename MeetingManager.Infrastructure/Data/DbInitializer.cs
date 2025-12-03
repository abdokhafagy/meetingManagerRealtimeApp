using MeetingManager.Domain.Entities;
using MeetingManager.Domain.Enums;
using MeetingManager.Infrastructure.Data;
using BCrypt.Net;

namespace MeetingManager.Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        // Ensure database is created
        context.Database.EnsureCreated();

        // Check if data already exists
        if (context.Groups.Any())
        {
            return; // Database has been seeded
        }

        // Seed Groups
        var groups = new Group[]
        {
            new Group { Name = "Computer Science" },
            new Group { Name = "Engineering" },
            new Group { Name = "Business Administration" }
        };

        context.Groups.AddRange(groups);
        context.SaveChanges();

        // Seed Users
        var users = new User[]
        {
            new User
            {
                FullName = "Admin User",
                Email = "admin@admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
                Role = UserRole.Admin,
                GroupId = groups[0].Id
            },
            new User
            {
                FullName = "John Manager",
                Email = "manager@manager",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
                Role = UserRole.Manager,
                GroupId = groups[0].Id
            },
            new User
            {
                FullName = "Jane Secretary",
                Email = "secretary@secretary",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
                Role = UserRole.Secretary,
                GroupId = groups[0].Id
            },
            new User
            {
                FullName = "Bob Manager",
                Email = "bob.manager@meetingmanager.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
                Role = UserRole.Manager,
                GroupId = groups[1].Id
            }
        };

        context.Users.AddRange(users);
        context.SaveChanges();

        // Seed Students
        var students = new Student[]
        {
            new Student { FullName = "Alice Johnson" },
            new Student { FullName = "Bob Smith" },
            new Student { FullName = "Charlie Brown" },
            new Student { FullName = "Diana Prince" },
            new Student { FullName = "Eve Wilson" }
        };

        context.Students.AddRange(students);
        context.SaveChanges();

        // Seed Meeting Requests
        var meetingRequests = new MeetingRequest[]
        {
            new MeetingRequest
            {
                Notes = "Need to discuss project requirements and timeline",
                StudentId = students[0].Id,
                SecretaryId = users[2].Id, // Jane Secretary
                ManagerId = users[1].Id,   // John Manager
                GroupId = groups[0].Id,
                Status = MeetingRequestStatus.Pending,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new MeetingRequest
            {
                Notes = "Request for internship approval",
                StudentId = students[1].Id,
                SecretaryId = users[2].Id,
                ManagerId = users[1].Id,
                GroupId = groups[0].Id,
                Status = MeetingRequestStatus.Accepted,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UpdatedAt = DateTime.UtcNow.AddDays(-4)
            },
            new MeetingRequest
            {
                Notes = "Discuss course selection for next semester",
                StudentId = students[2].Id,
                SecretaryId = users[2].Id,
                ManagerId = users[1].Id,
                GroupId = groups[0].Id,
                Status = MeetingRequestStatus.Rejected,
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                UpdatedAt = DateTime.UtcNow.AddDays(-6)
            },
            new MeetingRequest
            {
                Notes = "Academic performance review meeting",
                StudentId = students[3].Id,
                SecretaryId = users[2].Id,
                ManagerId = users[3].Id, // Bob Manager
                GroupId = groups[1].Id,
                Status = MeetingRequestStatus.Pending,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            }
        };

        context.MeetingRequests.AddRange(meetingRequests);
        context.SaveChanges();
    }
}
