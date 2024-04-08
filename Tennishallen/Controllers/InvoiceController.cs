using Microsoft.AspNetCore.Identity;
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
    //[AuthFilter(Group.GroupName.Admin, Group.GroupName.Assistent, Group.GroupName.Dentist)]
    public class InvoiceController(ApplicationDbContext context) : Controller
    {
        InvoiceService invoiceService = new(context);

        public IActionResult Index()
        {
            var token = HttpContext.Request.Cookies["Token"];
            var userId = new JwtService(token).GetUserId();
            var invoices = invoiceService.GetAllInvoicesForUserAsync(userId);
            return View(invoices);
        }

        public async Task<IActionResult> View(int id)
        {
            var token = HttpContext.Request.Cookies["Token"];
            var userId = new JwtService(token).GetUserId();
            var invoice = await invoiceService.GenerateMonthlyInvoiceAsync(userId, year, month);
            return View(invoice);
        }
    }
}