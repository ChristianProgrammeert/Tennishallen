﻿using Microsoft.AspNetCore.Mvc;
using Tennishallen.Data;
using Tennishallen.Data.Models;
using Tennishallen.Data.Services;
using Tennishallen.Data.Utils;
using Tennishallen.Services;

namespace Tennishallen.Controllers;

[AuthFilter(Group.GroupName.Admin, Group.GroupName.Coach, Group.GroupName.Member)]
public class InvoiceController(ApplicationDbContext context) : Controller
{
    private readonly AuthService authService = new(context);
    private readonly InvoiceService invoiceService = new(context);

    /// <summary>
    ///     Show the user all their reservations,
    ///     or the admin all users.
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Index()
    {
        var jwt = new JwtService(Request);
        if (!jwt.ValidateToken()) return RedirectToAction("Login", "Auth");
        if (jwt.GetUserGroups()!.Contains(Group.GroupName.Admin))
            return View(await authService.GetAllAsync());
        return RedirectToAction("User", new { guid = jwt.GetUserId()!.Value });
    }

    /// <summary>
    ///     Show all dates where there's an invoice
    /// </summary>
    /// <param name="guid">The id of the user to show the invoices from</param>
    /// <returns></returns>
    public async Task<IActionResult> User(Guid guid)
    {
        var user = await authService.GetByIdAsync(guid); //FIXME: Can be done inline. :)
        ViewBag.user = user;
        return View(await invoiceService.GetUserInvoiceDateTimes(guid));
    }

    /// <summary>
    ///     View the invoice from the given user on the given month
    /// </summary>
    /// <param name="guid">The id of the user to show the invoices</param>
    /// <param name="month">The month of the invoice</param>
    /// <returns></returns>
    public async Task<IActionResult> View(Guid guid, DateOnly month)
    {
        ViewBag.user = await authService.GetByIdAsync(guid);
        var reservation = await invoiceService.GetUserReservationsByMonth(guid, month);
        return View(reservation);
    }
}