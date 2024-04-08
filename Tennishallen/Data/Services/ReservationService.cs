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
    public class ReservationService(ApplicationDbContext context) : BaseRepository<Reservation, int>(context)
    {

        public async Task<List<Reservation>> GetAllLessonsReservations()
        {
            // Retrieve appointments from the database
            var all = await GetAllAsync(reservation => reservation.Coach, reservation => reservation.Member, reservation => reservation.Court);
            return all.ToList();
        }
        public async Task<List<Reservation>> GetAllCourtsReservations()
        {
            // Retrieve appointments from the database
            var all = await GetAllAsync(reservation => reservation.Member, reservation => reservation.Court);
            return all.ToList();
        }

        public async Task<Reservation?> GetLessonsById(int id)
        {
            // Retrieve appointments from the database by user id.
            return await GetByIdAsync(id, reservation => reservation.Coach, reservation => reservation.Member, reservation => reservation.Court);
        }
        public async Task<Reservation?> GetCourtsById(int id)
        {
            // Retrieve appointments from the database by user id.
            return await GetByIdAsync(id, reservation => reservation.Member, reservation => reservation.Court);
        }

        internal async Task<List<Court>> GetAllCourts()
        {
            return await context.Courts.ToListAsync();
        }

        //public Task<List<Reservation>> GetAllTreatments()
        //{
        //    // Retrieve treatments from the database including appointments
        //    return context.Treatments.ToListAsync();
        //}

        public async Task<List<User>> GetAllCoaches()
        {
            // Retrieve dentists from the database
            return await context.Users.Where(u => u.Groups.Any(g => g.Name == Group.GroupName.Coach)).ToListAsync();
        }

        public async Task<List<User>> GetAllMembers()
        {
            // Retrieve patients from the database
            return await context.Users.Where(u => u.Groups.Any(g => g.Name == Group.GroupName.Member)).ToListAsync();
        }

		internal async Task<List<Reservation>> GetLessonByUser(Guid? id)
		{
			return await context.Reservations
						 .Where(a => a.MemberId == id)
						 .Include(a => a.Court)
						 .Include(a => a.Coach)
                         .Include(a => a.Member)
						 .ToListAsync();
		}
		internal async Task<List<Reservation>> GetCourtsByUser(Guid? id)
		{
			return await context.Reservations
						 .Where(a => a.MemberId == id)
						 .Include(a => a.Court)
						 .ToListAsync();
		}
	}
}