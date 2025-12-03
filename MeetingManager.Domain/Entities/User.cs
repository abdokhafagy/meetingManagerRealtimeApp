using MeetingManager.Domain.Enums;

namespace MeetingManager.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public UserRole Role { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; } = default!;
}

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<User> Users { get; set; } = default!;
}

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; } = default!;
}

public class MeetingRequest
{
    public int Id { get; set; }
    public string Notes { get; set; } = default!;

    public int StudentId { get; set; }
    public Student Student { get; set; } = default!;

    public int SecretaryId { get; set; }
    public User Secretary { get; set; } = default!;

    public int ManagerId { get; set; }
    public User Manager { get; set; } = default!;

    public int GroupId { get; set; }
    public Group Group { get; set; } = default!;

    public MeetingRequestStatus Status { get; set; } = MeetingRequestStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}