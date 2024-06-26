using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tennishallen.Data.Models;
using Tennishallen.Data.Services;

namespace Tennishallen.Data.Utils;

public class AuthFilter(params Group.GroupName[] groups) : ActionFilterAttribute
{
    private readonly string[] allowedPaths =
    [
        "/auth/login"
    ];

    /// <summary>
    ///     When user is not logged in with the right role redirect them to the login page or show the unauthorized package
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        var service = new JwtService(context.HttpContext.Request);
        if (!context.HttpContext.Request.Cookies.ContainsKey("Token") ||
            !service.ValidateToken())
        {
            if (allowedPaths.All(s => s != context.HttpContext.Request.Path))
                context.HttpContext.Response.Redirect("/Auth/Login/");
            return;
        }

        context.HttpContext.User = service.GetClaimsIdentity();
        if (service.GetUserGroups()?.Any(groups.Contains) == true) return;
        context.HttpContext.Response.Clear();
        context.Result = new UnauthorizedObjectResult("Unauthorized");
    }
}