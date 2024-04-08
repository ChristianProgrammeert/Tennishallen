using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Tennishallen.Data;
using Tennishallen.Data.Base;
using Tennishallen.Data.Models;
using Tennishallen.Models;

namespace Tennishallen.Services
{
    public class InvoiceService(ApplicationDbContext context)
    {
        public async Task<IEnumerable<InvoiceViewModel>> GetAllInvoicesForUserAsync(Guid? userId)
        {
            // Retrieve all past reservations of the user
            var reservations = await context.Reservations
                .Include(r => r.Court)
                .Include(r => r.Coach)
                .Where(r => r.MemberId == userId && r.Date < DateOnly.FromDateTime(DateTime.Now))
                .ToListAsync();

            // Group reservations by month and year
            var groupedReservations = reservations.GroupBy(r => new { r.Date.Year, r.Date.Month });

            // Generate invoices for each group, covering each month from the earliest reservation to the latest
            var invoices = new List<InvoiceViewModel>();
            var earliestYear = reservations.Min(r => r.Date.Year);
            var earliestMonth = reservations.Where(r => r.Date.Year == earliestYear).Min(r => r.Date.Month);
            var latestYear = reservations.Max(r => r.Date.Year);
            var latestMonth = reservations.Where(r => r.Date.Year == latestYear).Max(r => r.Date.Month);

            for (int year = earliestYear; year <= latestYear; year++)
            {
                int startMonth = (year == earliestYear) ? earliestMonth : 1;
                int endMonth = (year == latestYear) ? latestMonth : 12;

                for (int month = startMonth; month <= endMonth; month++)
                {
                    var invoice = await GenerateMonthlyInvoiceAsync(userId, year, month);
                    invoices.Add(invoice);
                }
            }

            // Sort the invoices by month
            invoices = invoices.OrderBy(i => i.Year).ThenBy(i => i.Month).ToList();

            return invoices;
        }

        public async Task<InvoiceViewModel> GenerateMonthlyInvoiceAsync(Guid? userId, int year, int month)
        {
            var user = await context.Users
                .Include(u => u.MemberReservations)
                .Include(u => u.CoachReservations)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                // Handle user not found
            }

            var courtReservations = user.MemberReservations
                .Where(r => r.Date.Year == year && r.Date.Month == month && r.CoachId == null)
                .ToList();

            var lessons = user.MemberReservations
                .Where(r => r.Date.Year == year && r.Date.Month == month && r.CoachId != null)
                .ToList();

            var totalCost = courtReservations.Sum(r => r.Price) + lessons.Sum(r => r.Price);

            var invoice = new InvoiceViewModel
            {
                UserId = user.Id,
                FullName = user.Email,
                Email = user.Email,
                Month = month,
                Year = year,
                TotalCost = totalCost,
                CourtReservations = courtReservations,
                Lessons = lessons
            };

            return invoice;
        }
    }
}