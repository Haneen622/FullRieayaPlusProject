using Microsoft.AspNetCore.Mvc;
using AdvSwProject.Data;
using AdvSwProject.Models;

namespace AdvSwProject.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly DataDbContext _db;
        public AppointmentsController(DataDbContext db) { _db = db; }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string Name, string Address, DateTime Date, TimeSpan Time)
        {
            var app = new Appointment { Name = Name, Address = Address, Date = Date, Time = Time };
            _db.Appointments.Add(app);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var app = await _db.Appointments.FindAsync(id);
            if (app != null)
            {
                _db.Appointments.Remove(app);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
