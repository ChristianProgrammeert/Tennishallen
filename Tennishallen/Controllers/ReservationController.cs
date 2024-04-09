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
    //[AuthFilter(Group.GroupName.Admin, Group.GroupName.Assistent, Group.GroupName.Dentist)]
    public class ReservationController(ApplicationDbContext context) : Controller
    {
        ReservationService reservationService = new(context);
        AuthService userService = new(context);

        // [AuthFilter(Group.GroupName.Admin, Group.GroupName.Member, Group.GroupName.Coach)]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["Token"];
            var id = new JwtService(Request).GetUserId();
            var group = new JwtService(Request).GetUserGroups();

            IEnumerable<Reservation> reservation;
            if (group.Contains(Group.GroupName.Admin) || group.Contains(Group.GroupName.Member))
				reservation = await reservationService.GetAllAsync(a => a.Court, a => a.Coach, a => a.Member);
            else
				reservation = await reservationService.GetLessonByUser(id);
            return View(reservation.ToList());
        }


        private async Task fillViewBag()
        {
            ViewBag.Members = await reservationService.GetAllMembers();
            ViewBag.Coaches = await reservationService.GetAllCoaches();
            ViewBag.Courts = await reservationService.GetAllCourts();
            ViewBag.Reservations = await reservationService.GetAllLessonsReservations();
        }

        public async Task<IActionResult> Create()
        {
            await fillViewBag();
            return View(new Reservation());
        }

        [HttpPost]
		public async Task<IActionResult> Create(Reservation model)
		{
			var userId = new JwtService(Request).GetUserId();

			model.MemberId = userId.Value;

			model = await reservationService.AddAsync(model);

			return RedirectToAction("View", new { id = model.Id });
		}

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

        public async Task<ViewResult> View(int id)
        {
            return View(await reservationService.GetReservationsById(id));
        }

        public IActionResult Edit()
                {
            throw new NotImplementedException();
        }
    }
}