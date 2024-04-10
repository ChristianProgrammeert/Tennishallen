using Microsoft.EntityFrameworkCore;
using Tennishallen.Data;
using Tennishallen.Data.Models;

namespace Tennishallen.Services;

public class InvoiceService(ApplicationDbContext context)
{
    /// <summary>
    ///     Get all datetimes where there has been a reservtion by the user
    /// </summary>
    /// <param name="guid">the id of the user</param>
    /// <returns></returns>
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

    /// <summary>
    ///     get all reservations of the user on the given month
    /// </summary>
    /// <param name="guid">the id of the user</param>
    /// <param name="month">the month to get the resrvations of</param>
    /// <returns></returns>
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