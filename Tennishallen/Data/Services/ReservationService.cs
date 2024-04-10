using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Tennishallen.Data;
using Tennishallen.Data.Base;
using Tennishallen.Data.Models;
using Tennishallen.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tennishallen.Services
{
    public class ReservationService(ApplicationDbContext context) : BaseRepository<Reservation, int>(context)
    {

	    /// <summary>
	    /// Get all reservations with their member court and coach eager loaded
	    /// </summary>
	    /// <returns></returns>
        public async Task<List<Reservation>> GetAllLessonsReservations()
        {
            // Retrieve appointments from the database
            var all = await GetAllAsync(reservation => reservation.Member, reservation => reservation.Court, r => r.Coach);
            return all.ToList();
        }

	    /// <summary>
	    /// Get reservation by the id
	    /// </summary>
	    /// <param name="id"></param>
	    /// <returns></returns>
        public async Task<Reservation?> GetReservationsById(int id)
        {
            // Retrieve appointments from the database by user id.
            return await GetByIdAsync(id, reservation => reservation.Member, reservation => reservation.Court, r => r.Coach);
        }

	    /// <summary>
	    /// Get all courts
	    /// </summary>
	    /// <returns></returns>
        internal async Task<List<Court>> GetAllCourts()
        {
            return await context.Courts.ToListAsync();
        }
	    
		/// <summary>
		/// Get all coaches
		/// </summary>
		/// <returns></returns>
        public async Task<List<User>> GetAllCoaches()
        {
            // Retrieve dentists from the database
            return await context.Users.Where(u => u.Groups.Any(g => g.Name == Group.GroupName.Coach)).ToListAsync();
        }
	
		/// <summary>
		/// get all members
		/// </summary>
		/// <returns></returns>
        public async Task<List<User>> GetAllMembers()
        {
            // Retrieve patients from the database
            return await context.Users.Where(u => u.Groups.Any(g => g.Name == Group.GroupName.Member)).ToListAsync();
        }

		/// <summary>
		/// Get all lessons from a user
		/// </summary>
		/// <param name="id">the id of the user</param>
		/// <returns></returns>
		internal async Task<List<Reservation>> GetLessonByUser(Guid? id)
		{
			return await context.Reservations
						 .Where(a => a.MemberId == id || a.CoachId == id)
						 .Include(a => a.Court)
						 .Include(a => a.Coach)
                         .Include(a => a.Member)
						 .ToListAsync();
		}
	}
}