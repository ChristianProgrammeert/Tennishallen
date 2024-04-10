﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Tennishallen.Data;
using Tennishallen.Data.Models;
using Tennishallen.Data.Utils;
using Tennishallen.Models;
using Tennishallen.Data.Services;
using Tennishallen.Services;

namespace Tennishallen.Controllers
{
    [AuthFilter(Group.GroupName.Admin, Group.GroupName.Coach, Group.GroupName.Member)]
    public class InvoiceController(ApplicationDbContext context) : Controller
    {
        InvoiceService invoiceService = new(context);
        AuthService authService = new(context);

        public async Task<IActionResult> Index()
        {
            var jwt = new JwtService(Request);
            if (!jwt.ValidateToken()) return RedirectToAction("Login", "Auth");
            if (jwt.GetUserGroups()!.Contains(Group.GroupName.Admin))
                return View(await authService.GetAllAsync());
            return await User(jwt.GetUserId()!.Value);
        }

        public async Task<IActionResult> User(Guid guid)
        {
            ViewBag.user = await authService.GetByIdAsync(guid);
            return View(await invoiceService.GetUserInvoiceDateTimes(guid));
        }
        
        public async Task<IActionResult> View(Guid guid, DateOnly month)
        {
            ViewBag.user = await authService.GetByIdAsync(guid);
            return View(await invoiceService.GetUserReservationsByMonth(guid, month));

        }
    }
}