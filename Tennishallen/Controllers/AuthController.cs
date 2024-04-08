using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Tennishallen.Data;
using Tennishallen.Data.Models;
using Tennishallen.Data.Services;
using Tennishallen.Data.Utils;
using Tennishallen.Models.Auth;

namespace Tennishallen.Controllers;

public class AuthController(ApplicationDbContext context) : Controller
{
    private AuthService service = new(context);

    /// <summary>
    /// Show the user the login page. if user is logged in redirect to home.
    /// </summary>
    public IActionResult Login()
    {
        if (new JwtService(Request).ValidateToken())
            return RedirectToAction("Index", "Home");
        return View(new LoginViewModel());
    }


    /// <summary>
    /// Login the user if the model is valid else show the login page with the problems.
    /// When logged in redirect to home page
    /// </summary>
    /// <param name="loginViewModel">The login model that should be valid.</param>
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid) return View(loginViewModel);
        var user = await service.GetUserByEmailAndPassword(loginViewModel.Email, loginViewModel.Password);
        if (user == null)
        {
            ModelState.AddModelError("Error", "Het gegeven email of wachtwoord is onjuist.");
            return View(loginViewModel);
        }

        SetTokenCookie(user);
        return RedirectToAction("Index", "Home");
    }


    /// <summary>
    /// Logout the user
    /// </summary>
    public IActionResult Logout()
    {
        SetTokenCookie(null);
        return RedirectToAction("Login");
    }


    /// <summary>
    /// Show the user who he is
    /// TODO: make debug mode only
    /// </summary>
    /// <returns></returns>
    //public async Task<IActionResult> WhoAmI()
    //{
    //    var jwtService = new JwtService(Request);
    //    if (!jwtService.ValidateToken())
    //        return View();
    //    var id = jwtService.GetUserId();
    //    return id == null
    //        ? View()
    //        : View(await service.GetByIdAsync(id.Value));
    //}

    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (new JwtService(Request).ValidateToken())
            return RedirectToAction("Index", "Home");
        if (!ModelState.IsValid)
            return View(model);

        var user = new User
        {
            Email = model.Email,
            Phone = model.Phone,
            Password = PasswordHasher.HashPassword(model.Password),
            FirstName = model.FirstName,
            LastName = model.LastName,
            Address = model.Address,
            PostalCode = model.PostalCode,
            City = model.City,
            Birthdate = DateOnly.FromDateTime(model.Birthdate),
            Active = true,
            Groups = [],
        };
        user.Groups.Add(new Group { User = user,  Name = Group.GroupName.Member });
        try
        {
            await service.AddAsync(user);
        }
        catch (Exception _)
        {
            ModelState.AddModelError("Error", "Er ging iets fout bij het registreren");
            return View(model);
        }

        return await Login(new LoginViewModel { Email = model.Email, Password = model.Password});
    }

    private void SetTokenCookie(User? user)
    {
        var cookieOptions = new CookieOptions { HttpOnly = true };
        if (user == null)
            Response.Cookies.Delete("Token");
        else
            Response.Cookies.Append("Token", new JwtService(user).Token!, cookieOptions);
    }

    public IActionResult Profile()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Profile", model);
        }

        var token = HttpContext.Request.Cookies["Token"];
        var userId = new JwtService(token).GetUserId();
        var user = await service.GetByIdAsync(userId.Value);

        if (user == null)
        {
            return NotFound();
        }

        if (!PasswordHasher.VerifyPassword(model.OldPassword, user.Password))
        {
            ModelState.AddModelError("OldPassword", "Incorrect old password");
            return View("Profile", model);
        }

        user.Password = PasswordHasher.HashPassword(model.NewPassword);
        await service.UpdateAsync(user);

        return RedirectToAction("Index", "Home");
    }
}