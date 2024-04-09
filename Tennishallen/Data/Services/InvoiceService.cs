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
        public async Task<HashSet<DateOnly>> GetUserInvoiceDateTimes(Guid guid)
        {
            var datetimes = await context.Reservations
                .Where(r => r.MemberId == guid)
                .Select(r => r.Date)
                .Where(r => r < DateOnly.FromDateTime(DateTime.Now))
                .Select(r => new DateOnly(r.Year, r.Month, 1))
                .ToListAsync();
            return datetimes.ToHashSet();
        }

        public async Task<List<Reservation>> GetUserReservationsByMonth(Guid guid, DateOnly month)
        {
            return await context.Reservations
                .Where(r => r.MemberId == guid)
                .Where(r => r.Date.Month == month.Month && r.Date.Year == month.Year)
                .Include(r => r.Court)
                .Include(r => r.Coach)
                .ToListAsync();
        }
    }
}