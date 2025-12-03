using MeetingManager.Application.DTOs.Auth;
using MeetingManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManager.Mvc.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        // If user already has valid session, redirect to MeetingRequest Index
        var hasToken = Request.Cookies.ContainsKey("AuthToken");
        var hasSession = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));
        
        if (hasToken && hasSession)
        {
            return RedirectToAction("Index", "MeetingRequest");
        }
        
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return View(loginDto);
        }

        var result = await _authService.LoginAsync(loginDto);

        if (result == null)
        {
            ModelState.AddModelError("", "البريد الإلكتروني أو كلمة المرور غير صحيحة");
            return View(loginDto);
        }

        // Store token in cookie
        Response.Cookies.Append("AuthToken", result.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // Set to true in production with HTTPS
            SameSite = SameSiteMode.Lax, // Changed from Strict to Lax for better compatibility
            Expires = DateTimeOffset.UtcNow.AddMinutes(60)
        });

        // Store user info in session for easy access
        HttpContext.Session.SetString("UserId", result.UserId.ToString());
        HttpContext.Session.SetString("UserName", result.FullName);
        HttpContext.Session.SetString("UserRole", result.Role.ToString());
        HttpContext.Session.SetString("GroupId", result.GroupId.ToString());

        return RedirectToAction("Index", "MeetingRequest");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return View(registerDto);
        }

        var result = await _authService.RegisterAsync(registerDto);

        if (result == null)
        {
            ModelState.AddModelError("", "البريد الإلكتروني مستخدم بالفعل");
            return View(registerDto);
        }

        // Store token in cookie
        Response.Cookies.Append("AuthToken", result.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // Set to true in production with HTTPS
            SameSite = SameSiteMode.Lax, // Changed from Strict to Lax for better compatibility
            Expires = DateTimeOffset.UtcNow.AddMinutes(60)
        });

        // Store user info in session
        HttpContext.Session.SetString("UserId", result.UserId.ToString());
        HttpContext.Session.SetString("UserName", result.FullName);
        HttpContext.Session.SetString("UserRole", result.Role.ToString());
        HttpContext.Session.SetString("GroupId", result.GroupId.ToString());

        return RedirectToAction("Index", "MeetingRequest");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AuthToken");
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    [HttpPost]
    public IActionResult LogoutPost()
    {
        Response.Cookies.Delete("AuthToken");
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
