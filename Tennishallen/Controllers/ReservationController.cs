using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Tennishallen.Data;
using Tennishallen.Data.Models;
using Tennishallen.Models;
using Tennishallen.Data.Services;
using Tennishallen.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Tennishallen.Models.Court;
using Tennishallen.Data.Utils;

namespace Tennishallen.Controllers
{
    [AuthFilter(Group.GroupName.Admin, Group.GroupName.Coach, Group.GroupName.Member)]
    public class ReservationController(ApplicationDbContext context) : Controller
    {
        ReservationService reservationService = new(context);
        AuthService userService = new(context);

        
        /// <summary>
        /// Show the user their reservations
        /// if admin show **all** reservations
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["Token"];
            var id = new JwtService(Request).GetUserId();
            var group = new JwtService(Request).GetUserGroups();

            IEnumerable<Reservation> reservation;
            if (group.Contains(Group.GroupName.Admin))
				reservation = await reservationService.GetAllAsync(a => a.Court, a => a.Coach, a => a.Member);
            else
				reservation = await reservationService.GetLessonByUser(id);
            return View(reservation.ToList());
        }

        /// <summary>
        /// Fill the viewbag with all selectable members
        /// </summary>
        private async Task fillViewBag()
        {
            ViewBag.Members = await reservationService.GetAllMembers();
            ViewBag.Coaches = await reservationService.GetAllCoaches();
            ViewBag.Courts = await reservationService.GetAllCourts();
            ViewBag.Reservations = await reservationService.GetAllLessonsReservations();
        }

        /// <summary>
        /// Show the user a form to create a reservation
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Create()
        {
            await fillViewBag();
            return View(new Reservation());
        }
        
        
        /// <summary>
        /// Create a new reservation based on the model
        /// show that there are problems
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
		public async Task<IActionResult> Create(Reservation model)
		{
			var userId = new JwtService(Request).GetUserId();

			model.MemberId = userId.Value;

			model = await reservationService.AddAsync(model);

			return RedirectToAction("View", new { id = model.Id });
		}
        /// <summary>
        /// Delete the reservation with the given id
        /// </summary>
        /// <param name="id">the id of the reservation</param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await reservationService.DeleteAsync(id);
            }
            catch (Exception _)
            {
                return RedirectToAction("View", id);
            }

            return RedirectToAction("Index");
        }
        
        
        /// <summary>
        /// Show the reservation with the given id
        /// </summary>
        /// <param name="id">The id of the reservation</param>
        /// <returns></returns>
        public async Task<ViewResult> View(int id)
        {
            return View(await reservationService.GetReservationsById(id));
        }

            
        /// <summary>
        /// View edit page with the room with the given id, if id is given.
        /// </summary>
        /// <param name="appointmentId">the id of the reservation to edit</param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int appointmentId)
        {
            await fillViewBag();

            Reservation? reservation = appointmentId == 0
                ? null
                : await reservationService.GetByIdAsync(appointmentId);
            return View(reservation);
        }


            
        /// <summary>
        /// When an edit is done, add it to the service when id is not set, else update the room with the given id.
        /// Redirect to the view page of the new or updated room.
        /// </summary>
        /// <param name="appointmentId">the id of the reservation to edit</param>
        /// <param name="reservation">the model with user filled data</param>
        [HttpPost]
        public async Task<IActionResult> Edit(int appointmentId, Reservation reservation)
        {
            var userId = new JwtService(Request).GetUserId();

            reservation.MemberId = userId.Value;
            if (reservation.Id == 0 && appointmentId != 0)
            {
                reservation.Id = appointmentId;
            }
            reservation = appointmentId == 0
                ? await reservationService.AddAsync(reservation)
                : await reservationService.UpdateAsync(reservation);
            return RedirectToAction("View", new { id = reservation.Id });
        }
    }
}