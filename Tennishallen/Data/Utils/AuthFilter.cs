
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tennishallen.Data.Models;
using Tennishallen.Data.Services;

namespace Tennishallen.Data.Utils;

public class AuthFilter(params Group.GroupName[] groups) : ActionFilterAttribute
{
    
    private string[] allowedPaths =
    [
        "/auth/login",
    ];

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        var service = new JwtService(context.HttpContext.Request);
        if (!context.HttpContext.Request.Cookies.ContainsKey("Token") ||
            !service.ValidateToken())
        {
            if (allowedPaths.All(s => s != context.HttpContext.Request.Path))
                context.HttpContext.Response.Redirect($"/Auth/Login/");
            return;
        }

        var token = context.HttpContext.Request.Cookies["Token"];
        context.HttpContext.User = service.GetClaimsIdentity(token);
        if (service.GetUserGroups()?.Any(groups.Contains) == true) return;
        context.HttpContext.Response.Clear();
        context.Result = new UnauthorizedObjectResult("Unauthorized");
    }
}