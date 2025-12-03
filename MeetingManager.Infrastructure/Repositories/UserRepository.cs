using MeetingManager.Domain.Entities;
using MeetingManager.Domain.Enums;
using MeetingManager.Domain.Interfaces;
using MeetingManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MeetingManager.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(u => u.Group)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbSet.AnyAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetManagersByGroupIdAsync(int groupId)
    {
        return await _dbSet
            .Where(u => u.GroupId == groupId && u.Role == UserRole.Manager)
            .ToListAsync();
    }
}
