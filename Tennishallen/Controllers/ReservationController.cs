using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Tennishallen.Data;
using Tennishallen.Data.Models;
using Tennishallen.Data.Filters;
using Tennishallen.Models;
using Tennishallen.Data.Services;

namespace Tennishallen.Controllers
{
    //[AuthFilter(Group.GroupName.Admin, Group.GroupName.Assistent, Group.GroupName.Dentist)]
    public class ReservationController(ApplicationDbContext context) : Controller
    {
        AppointmentService appointmentService = new(context);
        AuthService userService = new(context);

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["Token"];
            var id = new JwtService().GetUserId(token);
            var group = new JwtService().GetUserGroups(token);

            List<Appointment> appointments;
            if (group.Contains(Group.GroupName.Assistent))
                appointments = (await appointmentService.GetAllAsync(appointment => appointment.Room, a => a.Dentist, a => a.Patient, a => a.Treatments)).ToList();
            else
                appointments = await appointmentService.GetAppointmentByUser(id);
            return View(appointments);
        }


        private async Task fillViewBag()
        {
            ViewBag.Dentists = await appointmentService.GetAllDentists();
            ViewBag.Patients = await appointmentService.GetAllPatients();
            ViewBag.Treatments = await appointmentService.GetAllTreatments();
            ViewBag.Rooms = await appointmentService.GetAllRooms();
        }

        public async Task<IActionResult> Create()
        {
            await fillViewBag();
            return View(new Appointment());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Appointment model)
        {
            var treatmentIds = HttpContext.Request.Form.Keys
                .Where(k => k.StartsWith("treatment-"))
                .Select(s => int.Parse(s.Replace("treatment-", ""))
                );
            model.Treatments = await treatmentService.FindByIds(treatmentIds);
            if (!ModelState.IsValid)
            {
                await fillViewBag();
                return View(model);
            }
            model = await appointmentService.AddAsync(model);
            return RedirectToAction("View", new { id = model.Id });
        }

        /// <summary>
        /// View edit page for the appointment with the given id, if id is given.
        /// </summary>
        //public async Task<IActionResult> Edit(int? appointmentId)
        //{
        //    Appointment? appointment = appointmentId == null
        //    ? null
        //    : await appointmentService.GetByIdAsync(appointmentId.Value);
        //    return View(appointment);

        //    // Check if appointmentId is provided
        //    if (appointmentId == null)
        //    {
        //        // If appointmentId is not provided, create a new appointment
        //        var model = new CreateAppointmentViewModel
        //        {
        //            Treatments = await appointmentService.GetAllTreatments(),
        //            Dentists = await appointmentService.GetAllDentists(),
        //            Patients = await appointmentService.GetAllPatients(),
        //            Rooms = await appointmentService.GetAllRooms() // Add this line to get all rooms
        //        };

        //        return View("EditAppointment", model);
        //    }
        //    else
        //    {
        //        // If appointmentId is provided, retrieve the existing appointment
        //        Appointment appointment = await appointmentService.GetByIdAsync(appointmentId.Value);

        //        // Populate the ViewModel with data from the existing appointment
        //        var model = new CreateAppointmentViewModel
        //        {
        //            Treatments = await appointmentService.GetAllTreatments(),
        //            Dentists = await appointmentService.GetAllDentists(),
        //            Patients = await appointmentService.GetAllPatients(),
        //            Rooms = await appointmentService.GetAllRooms(), // Add this line to get all rooms
        //            SelectedTreatmentIds = appointment.Treatments.Select(t => t.Id).ToList(),
        //            DentistId = appointment.DentistId,
        //            PatientId = appointment.PatientId,
        //            RoomId = appointment.RoomId,
        //            DateTime = appointment.DateTime,
        //            Note = appointment.Note
        //        };

        //        return View("EditAppointment", model);
        //    }
        //}

        //// Action to handle the submission of the appointment edit form
        //[HttpPost]
        //public async Task<IActionResult> Edit(int? id, Appointment appointment)
        //{
        //    appointment = id == null
        //    ? await appointmentService.AddAsync(appointment)
        //    : await appointmentService.UpdateAsync(appointment);
        //    return RedirectToAction("View", new { id = appointment.Id });

        //    if (ModelState.IsValid)
        //    {
        //        Appointment appointment;

        //        // Check if the id is null
        //        if (id == null)
        //        {
        //            // If id is null, it means it's a new appointment, so add it
        //            appointment = new Appointment
        //            {
        //                DentistId = model.DentistId,
        //                PatientId = model.PatientId,
        //                RoomId = model.RoomId,
        //                DateTime = model.DateTime,
        //                Note = model.Note,
        //                Treatments = model.SelectedTreatmentIds.Select(tid => new Treatment { Id = tid }).ToList()
        //            };
        //            appointment = await appointmentService.AddAsync(appointment);
        //        }
        //        else
        //        {
        //            // If id is not null, it means it's an existing appointment, so update it
        //            appointment = await appointmentService.GetByIdAsync(id.Value);
        //            appointment.DentistId = model.DentistId;
        //            appointment.PatientId = model.PatientId;
        //            appointment.RoomId = model.RoomId;
        //            appointment.DateTime = model.DateTime;
        //            appointment.Note = model.Note;
        //            appointment.Treatments = model.SelectedTreatmentIds.Select(tid => new Treatment { Id = tid }).ToList();
        //            appointment = await appointmentService.UpdateAsync(appointment);
        //        }

        //        // After updating or adding, redirect to appointments list or details page
        //        return RedirectToAction("Index");
        //    }

        //    // If model state is not valid, return to the form with validation errors
        //    model.Treatments = await appointmentService.GetAllTreatments();
        //    model.Dentists = await appointmentService.GetAllDentists();
        //    model.Patients = await appointmentService.GetAllPatients();
        //    model.Rooms = await appointmentService.GetAllRooms(); // Add this line to get all rooms

        //    return View("EditAppointment", model);
        //}
        public async Task<ViewResult> View(int id)
        {
            return View(await appointmentService.GetAppointmentById(id));
        }

        public IActionResult Edit()
        {
            throw new NotImplementedException();
        }
    }
}