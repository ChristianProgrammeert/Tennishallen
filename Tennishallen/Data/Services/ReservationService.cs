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
    public class ReservationService(ApplicationDbContext context) : BaseRepository<Appointment, int>(context)
    {

        public async Task<List<Appointment>> GetAllAppointments()
        {
            // Retrieve appointments from the database
            var all = await GetAllAsync(appointment => appointment.Dentist, appointment => appointment.Patient, appointment => appointment.Room);
            return all.ToList();
        }

        public async Task<Appointment?> GetAppointmentById(int id)
        {
            // Retrieve appointments from the database by user id.
            return await GetByIdAsync(id, appointment => appointment.Dentist, appointment => appointment.Patient, appointment => appointment.Room);
        }

        internal async Task<List<Room>> GetAllRooms()
        {
            return await context.Rooms.ToListAsync();
        }

        //public async Task<List<Treatment>> GetAllTreatmentsForAppointment(int appointmentId)
        //{
        //    // Retrieve treatments from the database including appointments
        //    return await context.Treatments
        //        .Where(t => t.Appointments.Any(a => a.Id == appointmentId))
        //        .ToListAsync();
        //}

        //public async Task<List<Appointment>> GetUserAppointments(Guid currentuserid)
        //{
        //    // Retrieve appointments from the database
        //    return await context.Appointments.Where(u => u.PatientId == currentuserid).ToListAsync();
        //}

        //public async Task<List<Appointment>> GetDentistAppointments(Guid currentuserid)
        //{
        //    // Retrieve appointments from the database
        //    return await context.Appointments.Where(u => u.DentistId == currentuserid).ToListAsync();
        //}

        public Task<List<Treatment>> GetAllTreatments()
        {
            // Retrieve treatments from the database including appointments
            return context.Treatments.ToListAsync();
        }

        public async Task<List<User>> GetAllDentists()
        {
            // Retrieve dentists from the database
            return await context.Users.Where(u => u.Groups.Any(g => g.Name == Group.GroupName.Dentist)).ToListAsync();
        }

        public async Task<List<User>> GetAllPatients()
        {
            // Retrieve patients from the database
            return await context.Users.Where(u => u.Groups.Any(g => g.Name == Group.GroupName.Patient)).ToListAsync();
        }

        //public async Task CreateAppointment(CreateAppointmentViewModel model)
        //{
        //    // Create a new appointment based on the model data
        //    Appointment appointment = new Appointment
        //    {
        //        //public Guid DentistId { get; set; }
        //        //public User Dentist { get; set; }
        //        //public Guid PatientId { get; set; }
        //        //public User Patient { get; set; }
        //        //public int RoomId { get; set; }
        //        //public Room Room { get; set; }
        //        //public DateTime DateTime { get; set; }
        //        //public string Note { get; set; }
        //        //public Collection<Treatment> Treatments { get; set; }
        //        DentistId = model.DentistId,
        //        PatientId = model.PatientId,
        //        RoomId = model.RoomId,
        //        DateTime = model.DateTime,


        //    };

        //    // Add the appointment to the database
        //    context.Appointments.Add(appointment);
        //    await context.SaveChangesAsync();
        //}


        public async Task<List<Appointment>> GetAppointmentByUser(string? id)
        {
            return await context.Appointments
                .Where(a => a.DentistId == Guid.Parse(id) || a.PatientId == Guid.Parse(id))
                .Include(a => a.Room)
                .Include(a => a.Treatments)
                .Include(a => a.Dentist)
                .Include(a => a.Patient)
                .ToListAsync();
        }
    }
}