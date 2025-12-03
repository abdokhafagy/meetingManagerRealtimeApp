using MeetingManager.Domain.Entities;
using MeetingManager.Domain.Enums;

namespace MeetingManager.Domain.Interfaces;

public interface IMeetingRequestRepository : IRepository<MeetingRequest>
{
    Task<IEnumerable<MeetingRequest>> GetByStatusAsync(MeetingRequestStatus status);
    Task<IEnumerable<MeetingRequest>> GetByGroupIdAsync(int groupId);
    Task<IEnumerable<MeetingRequest>> GetByManagerIdAsync(int managerId);
    Task<IEnumerable<MeetingRequest>> GetBySecretaryIdAsync(int secretaryId);
    Task<IEnumerable<MeetingRequest>> GetAllWithDetailsAsync();
    Task<MeetingRequest?> GetByIdWithDetailsAsync(int id);
}
