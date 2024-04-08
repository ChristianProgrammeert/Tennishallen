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
            var invoices = invoiceService.GetAllReservationsForUserAndMonthsAsync(months: 1);   
            return View(invoices);
        }

        public IActionResult ViewInvoice(Guid userId, int year, int month)
        {
            var invoice = invoiceService.GenerateMonthlyInvoiceAsync(userId, year, month);
            return View(invoice);
        }

        public async Task<IActionResult> Monthly(Guid userId, int year, int month)
        {
            var invoice = await invoiceService.GenerateMonthlyInvoiceAsync(userId, year, month);
            return View(invoice);
        }
    }
}