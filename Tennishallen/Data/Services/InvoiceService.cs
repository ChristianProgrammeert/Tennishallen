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
		public async Task<HashSet<DateOnly>> GetAllInvoicesForUserAsync(Guid? userId)
		{
            var datetimes = await context.Reservations
                .Select(r => r.Date)
                .Where(r => r.ToDateTime(TimeOnly.MinValue) < DateTime.Now)
                .ToListAsync();
            var set = new HashSet<DateOnly>();

            foreach (var datetime in datetimes)
            {
                set.Add(new DateOnly(datetime.Year, datetime.Month, 1));
            }
            return set;

			//// Retrieve all past reservations of the user
			//var reservations = await context.Reservations
			//	.Include(r => r.Court)
			//	.Include(r => r.Coach)
			//	.Where(r => r.MemberId == userId && r.Date < DateOnly.FromDateTime(DateTime.Now))
			//	.ToListAsync();

			//// Group reservations by month and year
			//var groupedReservations = reservations.GroupBy(r => new { r.Date.Year, r.Date.Month });

			//// Generate invoices for each group, covering each month from the earliest reservation to the latest
			//var invoices = new List<InvoiceViewModel>();
   //         var earliest = reservations.Min(r => r.Date);
   //         var latest = reservations.Max(r => r.Date);

			//for (int year = earliest.Year; year <= latest.Year; year++)
			//{
   //             //int startMonth = (year == earliest.Year) ? earliest.Month : 1;
   //             //int endMonth = (year == latest.Year) ? latest.Month : 12;

   //             //for (int month = startMonth; month <= endMonth; month++)
   //             //{
   //             //	var invoice = await GenerateMonthlyInvoiceAsync(userId, year, month);
   //             //	invoices.Add(invoice);
   //             //}

   //             for (int i = 1; i < 12; i++)
   //             {

   //             }
			//}

			//// Sort the invoices by month
			//invoices = invoices.OrderBy(i => i.Year).ThenBy(i => i.Month).ToList();

			//return invoices;
		}

		public async Task<InvoiceViewModel> GenerateMonthlyInvoiceAsync(Guid? userId, int year, int month)
        {
            var user = await context.Users
                .Include(u => u.MemberReservations)
                .Include(u => u.CoachReservations)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
               
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