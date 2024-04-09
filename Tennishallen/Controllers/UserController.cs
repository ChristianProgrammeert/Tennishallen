using Microsoft.AspNetCore.Mvc;
using Tennishallen.Data;
using Tennishallen.Data.Models;
using Tennishallen.Data.Services;
using Tennishallen.Data.Utils;
using Tennishallen.Models.User;

namespace Tennishallen.Controllers;

[AuthFilter(Group.GroupName.Admin)]
public class UserController(ApplicationDbContext context) : Controller
{
    private readonly AuthService authService = new(context);

    /// <summary>
    /// Show all users
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Index()
    {
        return View(await authService.GetAllAsync(u => u.Groups));
    }

    
    /// <summary>
    /// View a specific user
    /// </summary>
    /// <param name="id">the id of the user to show</param>
    /// <returns></returns>
    public async Task<IActionResult> View(Guid id)
    {
        var user = await authService.GetByIdAsync(id, u => u.Groups);
        return user == null
            ? RedirectToAction("Index")
            : View(user);

    }

    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await authService.DeleteAsync(id);
        }
        catch (Exception e)
        {
            return RedirectToAction("View", id);
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(Guid guid)
    {
        var user = await authService.GetByIdAsync(guid, u => u.Groups);
        if (user == null) return RedirectToAction("Index");
        return View(
                new UserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Birthdate = user.Birthdate.ToDateTime(TimeOnly.MinValue),
                    Address = user.Address,
                    PostalCode = user.PostalCode,
                    City = user.City,
                    HourlyWage = user.HourlyWage,
                    IsActive = user.Active,
                    IsAdmin = user.Groups.Any(group => group.Name == Group.GroupName.Admin),
                    IsCoach = user.Groups.Any(group => group.Name == Group.GroupName.Coach),
                    IsMember = user.Groups.Any(group => group.Name == Group.GroupName.Member),
                }
            );
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(Guid guid, UserViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = await authService.GetByIdAsync(guid, u => u.Groups);
        user.Groups = new();
        if(model.IsAdmin) user.Groups.Add(new Group{Name = Group.GroupName.Admin});
        if(model.IsCoach) user.Groups.Add(new Group{Name = Group.GroupName.Coach});
        if(model.IsMember) user.Groups.Add(new Group{Name = Group.GroupName.Member});
        user.Id = guid;
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        if(model.Password != null) user.Password = PasswordHasher.HashPassword(model.Password);
        user.Email = model.Email;
        user.Phone = model.Phone;
        user.Birthdate = DateOnly.FromDateTime(model.Birthdate);
        user.Address = model.Address;
        user.PostalCode = model.PostalCode;
        user.City = model.City;
        user.Active = model.IsActive;
        user.HourlyWage = model.HourlyWage;
        try
        {
            await authService.UpdateAsync(user);
        }
        catch (Exception _)
        {
            return View(model);
        }
        return RedirectToAction("view", guid);
    }
    

    public async Task<IActionResult> Create()
    {
        return View(
                new UserViewModel()
            );
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(UserViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = new User();
        user.Groups = new();
        if(model.IsAdmin) user.Groups.Add(new Group{Name = Group.GroupName.Admin});
        if(model.IsCoach) user.Groups.Add(new Group{Name = Group.GroupName.Coach});
        if(model.IsMember) user.Groups.Add(new Group{Name = Group.GroupName.Member});
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        if(model.Password != null) user.Password = PasswordHasher.HashPassword(model.Password);
        user.Email = model.Email;
        user.Phone = model.Phone;
        user.Birthdate = DateOnly.FromDateTime(model.Birthdate);
        user.Address = model.Address;
        user.PostalCode = model.PostalCode;
        user.City = model.City;
        user.Active = model.IsActive;
        user.HourlyWage = model.HourlyWage;
        try
        {
            user = await authService.AddAsync(user);
        }
        catch (Exception _)
        {
            return View(model);
        }
        return RedirectToAction("view", user.Id);
    }
}