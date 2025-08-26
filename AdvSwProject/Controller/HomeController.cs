


using AdvSwProject.Data;
using AdvSwProject.Models;
using AdvSwProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvSwProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataDbContext _db;
        public HomeController(DataDbContext db)
        {
            _db = db;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Profile Page
        public IActionResult profile()
        {
            // يفتح Views/Home/Profile.cshtml
            return View("profile");
        }

        // Memory Page
        public IActionResult memory()
        {
            // يفتح Views/Home/Memory.cshtml
            return View("memory");
        }
        public IActionResult Athkari() {return View(); }

        // الصفحات الداخلية
        public IActionResult altheker() { return View(); }
        public IActionResult quran() { return View();}
        public IActionResult emergency() { return View(); }
        public IActionResult Tutorial() { return View(); }
        public IActionResult duaa() { return View(); }
        public IActionResult messa() { return View(); }
        public IActionResult nawm() { return View(); }
        public IActionResult sabah() { return View(); }
        public IActionResult music() { return View(); }
        public IActionResult games() { return View(); }
        public IActionResult Theater_Drama() { return View(); }
        public IActionResult books() { return View(); }
        // Landing Page (Index)
        public async Task<IActionResult> Index()
        {
            var meds = await _db.Medications.ToListAsync();
            var apps = await _db.Appointments.ToListAsync();
            var tips = await _db.HealthTips.ToListAsync(); // ✅ استدعاء HealthTips

            var vm = new HomeViewModel
            {
                Medications = meds.Select(m => new MedicationVm
                {
                    Id = m.Id,
                    MedicineName = m.MedicineName,
                    Dosage = m.Dosage,
                    BeforeAfterEating = m.BeforeAfterEating,
                    MedicationTimes = (m.TimesCsv ?? "")
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Select(TimeSpan.Parse).ToList()
                }).ToList(),

                Appointments = apps.Select(a => new AppointmentVm
                {
                    Id = a.Id,
                    Name = a.Name,
                    Address = a.Address,
                    Date = a.Date,
                    Time = a.Time
                }).ToList(),

                HealthTips = tips // ✅ أضفناها للـ ViewModel
            };

            return View(vm);
        }
    }
}
