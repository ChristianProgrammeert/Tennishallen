using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Tennishallen.Data;
using Tennishallen.Data.Models;
using Tennishallen.Models;
using Tennishallen.Data.Services;
using Tennishallen.Services;

namespace Tennishallen.Controllers
{
    //[AuthFilter(Group.GroupName.Admin, Group.GroupName.Assistent, Group.GroupName.Dentist)]
    public class ReservationController(ApplicationDbContext context) : Controller
    {
        ReservationService reservationService = new(context);
        AuthService userService = new(context);

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["Token"];
            var id = new JwtService(Request).GetUserId();
            var group = new JwtService(Request).GetUserGroups();

            List<Reservation> reservation;
            if (group.Contains(Group.GroupName.Admin) || group.Contains(Group.GroupName.Member))
				reservation = (await reservationService.GetAllAsync(appointment => appointment.Court, a => a.Coach, a => a.Member)).ToList();
            else
				reservation = await reservationService.GetLessonByUser(id);
            return View(reservationService);
        }


        private async Task fillViewBag()
        {
            ViewBag.Members = await reservationService.GetAllMembers();
            ViewBag.Coaches = await reservationService.GetAllCoaches();
            ViewBag.Courts = await reservationService.GetAllCourts();
        }

        public async Task<IActionResult> Create()
        {
            await fillViewBag();
            return View(new Reservation());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reservation model)
        {
            model = await reservationService.AddAsync(model);
            return RedirectToAction("View", new { id = model.Id });
        }

        public async Task<ViewResult> View(int id)
        {
            return View(await reservationService.GetLessonsById(id));
        }

        public IActionResult Edit()
        {
            throw new NotImplementedException();
        }
    }
}