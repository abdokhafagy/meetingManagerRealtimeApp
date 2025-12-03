using MeetingManager.Application.DTOs.MeetingRequest;
using MeetingManager.Application.Interfaces;
using MeetingManager.Domain.Enums;
using MeetingManager.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace MeetingManager.Mvc.Controllers;

[Authorize]
public class MeetingRequestController : Controller
{
    private readonly IMeetingRequestService _meetingRequestService;
    private readonly IUserRepository _userRepository;

    public MeetingRequestController(
        IMeetingRequestService meetingRequestService,
        IUserRepository userRepository)
    {
        _meetingRequestService = meetingRequestService;
        _userRepository = userRepository;
    }

    // GET: MeetingRequest
    public async Task<IActionResult> Index()
    {
        // Get user info from session
        var userIdStr = HttpContext.Session.GetString("UserId");
        var userRole = HttpContext.Session.GetString("UserRole");

        if (string.IsNullOrEmpty(userIdStr) || string.IsNullOrEmpty(userRole))
        {
            return RedirectToAction("Login", "Auth");
        }

        int userId = int.Parse(userIdStr);
        IEnumerable<MeetingRequestDto> meetingRequests;

        // Role-based filtering
        if (userRole == "Admin")
        {
            // Admin sees all meeting requests
            meetingRequests = await _meetingRequestService.GetAllAsync();
        }
        else if (userRole == "Manager")
        {
            // Manager sees only meeting requests assigned to them
            meetingRequests = await _meetingRequestService.GetByManagerIdAsync(userId);
        }
        else if (userRole == "Secretary")
        {
            // Secretary sees only meeting requests they created
            meetingRequests = await _meetingRequestService.GetBySecretaryIdAsync(userId);
        }
        else
        {
            // Default: empty list
            meetingRequests = new List<MeetingRequestDto>();
        }

        return View(meetingRequests);
    }

    // GET: MeetingRequest/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var meetingRequest = await _meetingRequestService.GetByIdAsync(id);
        
        if (meetingRequest == null)
        {
            return NotFound();
        }

        return View(meetingRequest);
    }

    // GET: MeetingRequest/Create
    public IActionResult Create()
    {
        // Get current user info from session
        var userIdStr = HttpContext.Session.GetString("UserId");
        var groupIdStr = HttpContext.Session.GetString("GroupId");

        if (string.IsNullOrEmpty(userIdStr) || string.IsNullOrEmpty(groupIdStr))
        {
            return RedirectToAction("Login", "Auth");
        }

        // No need to load managers - will be auto-assigned from group
        return View();
    }

    // POST: MeetingRequest/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMeetingRequestDto createDto)
    {
        // Get current user info from session
        var userIdStr = HttpContext.Session.GetString("UserId");
        var groupIdStr = HttpContext.Session.GetString("GroupId");
        
        if (string.IsNullOrEmpty(userIdStr) || string.IsNullOrEmpty(groupIdStr))
        {
            return RedirectToAction("Login", "Auth");
        }

        int userId = int.Parse(userIdStr);
        int groupId = int.Parse(groupIdStr);

        // Auto-assign SecretaryId and GroupId from session
        createDto.SecretaryId = userId;
        createDto.GroupId = groupId;

        // Auto-assign Manager from the same group
        var managers = await _userRepository.GetManagersByGroupIdAsync(groupId);
        var manager = managers.FirstOrDefault();
        
        if (manager == null)
        {
            ModelState.AddModelError("", "No manager found in your group. Please contact administrator.");
            return View(createDto);
        }

        createDto.ManagerId = manager.Id;

        if (!ModelState.IsValid)
        {
            return View(createDto);
        }

        try
        {
            await _meetingRequestService.CreateAsync(createDto);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            ModelState.AddModelError("", "An error occurred while creating the meeting request");
            return View(createDto);
        }
    }

    // GET: MeetingRequest/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var meetingRequest = await _meetingRequestService.GetByIdAsync(id);
        
        if (meetingRequest == null)
        {
            return NotFound();
        }

        var updateDto = new UpdateMeetingRequestDto
        {
            Id = meetingRequest.Id,
            Notes = meetingRequest.Notes,
            StudentId = meetingRequest.StudentId,
            SecretaryId = meetingRequest.SecretaryId,
            ManagerId = meetingRequest.ManagerId,
            GroupId = meetingRequest.GroupId,
            Status = meetingRequest.Status
        };

        // Get managers from the same group for dropdown
        var managers = await _userRepository.GetManagersByGroupIdAsync(meetingRequest.GroupId);
        ViewBag.Managers = new SelectList(managers, "Id", "FullName", meetingRequest.ManagerId);

        return View(updateDto);
    }

    // POST: MeetingRequest/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateMeetingRequestDto updateDto)
    {
        if (id != updateDto.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            // Reload managers for dropdown
            var managers = await _userRepository.GetManagersByGroupIdAsync(updateDto.GroupId);
            ViewBag.Managers = new SelectList(managers, "Id", "FullName", updateDto.ManagerId);
            return View(updateDto);
        }

        try
        {
            var result = await _meetingRequestService.UpdateAsync(updateDto);
            
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            ModelState.AddModelError("", "An error occurred while updating the meeting request");
            
            // Reload managers for dropdown
            var managers = await _userRepository.GetManagersByGroupIdAsync(updateDto.GroupId);
            ViewBag.Managers = new SelectList(managers, "Id", "FullName", updateDto.ManagerId);
            
            return View(updateDto);
        }
    }

    // GET: MeetingRequest/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var meetingRequest = await _meetingRequestService.GetByIdAsync(id);
        
        if (meetingRequest == null)
        {
            return NotFound();
        }

        return View(meetingRequest);
    }

    // POST: MeetingRequest/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var result = await _meetingRequestService.DeleteAsync(id);
            
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return RedirectToAction(nameof(Delete), new { id });
        }
    }

    // POST: MeetingRequest/UpdateStatus/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, MeetingRequestStatus status)
    {
        try
        {
            var result = await _meetingRequestService.UpdateStatusAsync(id, status);
            
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return RedirectToAction(nameof(Index));
        }
    }

}
