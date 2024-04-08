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
        public async Task<IEnumerable<Reservation>> GetAllReservationsForUserAndMonthsAsync(Guid userId, int months)
        {
            // Calculate the start date by subtracting the specified number of months from the current date
            var startDate = DateTime.Today.AddMonths(-months);

            // Retrieve all reservations for the given user within the specified range of months
            return await context.Reservations
                .Include(r => r.Member)
                .Where(r => r.MemberId == userId && r.Date >= startDate)
                .ToListAsync();
        }

        public async Task<InvoiceViewModel> GenerateMonthlyInvoiceAsync(Guid userId, int year, int month)
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