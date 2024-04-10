using Microsoft.AspNetCore.Mvc;
using Tennishallen.Data;
using Tennishallen.Data.Models;
using Tennishallen.Data.Services;
using Tennishallen.Data.Utils;
using Tennishallen.Models.Court;

namespace Tennishallen.Controllers;

public class CourtController(ApplicationDbContext context) : Controller
{
    private readonly CourtService courtService = new(context);


    /// <summary>
    ///     Show all Courts
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Index()
    {
        return View(await courtService.GetAllAsync());
    }

    /// <summary>
    ///     View a specific Court
    /// </summary>
    /// <param name="id">The id of the court to show.</param>
    /// <returns></returns>
    public async Task<IActionResult> View(int id)
    {
        var court = await courtService.GetByIdAsync(id);
        if (court == null) return RedirectToAction("Index");
        return View(court);
    }


    /// <summary>
    ///     Setup a CourtViewModel to create or update a court
    /// </summary>
    /// <returns></returns>
    [AuthFilter(Group.GroupName.Admin)]
    public async Task<IActionResult> Create()
    {
        return View(new CourtViewModel());
    }


    /// <summary>
    ///     Save or update the new court and redirect to the view
    ///     if the model is invalid show the problems to the user
    /// </summary>
    /// <param name="model">The model of the new court</param>
    /// <returns></returns>
    [HttpPost]
    [AuthFilter(Group.GroupName.Admin)]
    public async Task<IActionResult> Create(CourtViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var court = new Court
        {
            Name = model.Name,
            Description = model.Description,
            Price = model.Price
        };
        try
        {
            court = await courtService.AddAsync(court);
        }
        catch (Exception _)
        {
            return View(model);
        }

        return RedirectToAction("View", court.Id);
    }

    /// <summary>
    ///     Delete the court
    /// </summary>
    /// <param name="id">The id of the court to delete</param>
    /// <returns></returns>
    [AuthFilter(Group.GroupName.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await courtService.DeleteAsync(id);
        }
        catch (Exception _)
        {
            return RedirectToAction("View", id);
        }

        return RedirectToAction("Index");
    }


    /// <summary>
    ///     Start editing the court with the given id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AuthFilter(Group.GroupName.Admin)]
    public async Task<IActionResult> Edit(int id)
    {
        var court = await courtService.GetByIdAsync(id);
        if (court == null) return RedirectToAction("Index");
        return View(
            new CourtViewModel
            {
                Id = id,
                Name = court.Name,
                Price = court.Price,
                Description = court.Description
            }
        );
    }

    /// <summary>
    ///     Update the court with the given id with values in model
    /// </summary>
    /// <param name="id">the id of the court to update</param>
    /// <param name="model">the model with the values to update</param>
    /// <returns></returns>
    [AuthFilter(Group.GroupName.Admin)]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, CourtViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var court = new Court
        {
            Id = id,
            Name = model.Name,
            Description = model.Description,
            Price = model.Price
        };

        try
        {
            court = await courtService.UpdateAsync(court);
        }
        catch (Exception _)
        {
            return View(model);
        }

        return RedirectToAction("View", court.Id);
    }
}