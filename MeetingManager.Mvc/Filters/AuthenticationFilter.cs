using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MeetingManager.Mvc.Filters;

/// <summary>
/// Authentication filter that redirects unauthenticated users to the login page
/// </summary>
public class AuthenticationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Get the controller and action names
        var controller = context.RouteData.Values["controller"]?.ToString();
        var action = context.RouteData.Values["action"]?.ToString();

        // Allow access to Auth controller and Home controller
        if (controller == "Auth" || controller == "Home")
        {
            return;
        }

        // Check if user has a valid token in cookie
        var hasToken = context.HttpContext.Request.Cookies.ContainsKey("AuthToken");
        var hasSession = !string.IsNullOrEmpty(context.HttpContext.Session.GetString("UserId"));

        // If no token or session, redirect to login
        if (!hasToken || !hasSession)
        {
            context.Result = new RedirectToActionResult("Login", "Auth", null);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No action needed after execution
    }
}
