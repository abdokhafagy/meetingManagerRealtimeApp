using MeetingManager.Domain.Entities;
using MeetingManager.Domain.Enums;
using MeetingManager.Domain.Interfaces;
using MeetingManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MeetingManager.Infrastructure.Repositories;

public class MeetingRequestRepository : Repository<MeetingRequest>, IMeetingRequestRepository
{
    public MeetingRequestRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MeetingRequest>> GetByStatusAsync(MeetingRequestStatus status)
    {
        return await _dbSet
            .Include(mr => mr.Student)
            .Include(mr => mr.Secretary)
            .Include(mr => mr.Manager)
            .Include(mr => mr.Group)
            .Where(mr => mr.Status == status)
            .ToListAsync();
    }

    public async Task<IEnumerable<MeetingRequest>> GetByGroupIdAsync(int groupId)
    {
        return await _dbSet
            .Include(mr => mr.Student)
            .Include(mr => mr.Secretary)
            .Include(mr => mr.Manager)
            .Include(mr => mr.Group)
            .Where(mr => mr.GroupId == groupId)
            .ToListAsync();
    }

    public async Task<IEnumerable<MeetingRequest>> GetByManagerIdAsync(int managerId)
    {
        return await _dbSet
            .Include(mr => mr.Student)
            .Include(mr => mr.Secretary)
            .Include(mr => mr.Manager)
            .Include(mr => mr.Group)
            .Where(mr => mr.ManagerId == managerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<MeetingRequest>> GetBySecretaryIdAsync(int secretaryId)
    {
        return await _dbSet
            .Include(mr => mr.Student)
            .Include(mr => mr.Secretary)
            .Include(mr => mr.Manager)
            .Include(mr => mr.Group)
            .Where(mr => mr.SecretaryId == secretaryId)
            .ToListAsync();
    }

    public async Task<IEnumerable<MeetingRequest>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(mr => mr.Student)
            .Include(mr => mr.Secretary)
            .Include(mr => mr.Manager)
            .Include(mr => mr.Group)
            .ToListAsync();
    }

    public async Task<MeetingRequest?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(mr => mr.Student)
            .Include(mr => mr.Secretary)
            .Include(mr => mr.Manager)
            .Include(mr => mr.Group)
            .FirstOrDefaultAsync(mr => mr.Id == id);
    }
}
