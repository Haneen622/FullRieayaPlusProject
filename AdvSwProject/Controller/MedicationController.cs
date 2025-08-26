using AdvSwProject.Data;
using AdvSwProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdvSwProject.Controllers
{
    public class MedicationController : Controller
    {
        private readonly DataDbContext _db;
        public MedicationController(DataDbContext db) { _db = db; }

        [HttpPost, ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Add(string MedicineName, string Dosage, string BeforeAfterEating, List<string> MedicationTimes)
        {
            var med = new Medication
            {
                MedicineName = MedicineName,
                Dosage = Dosage,
                BeforeAfterEating = BeforeAfterEating,
                TimesCsv = string.Join(",", MedicationTimes.Where(t => !string.IsNullOrWhiteSpace(t)))
            };
            _db.Medications.Add(med);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Delete(int id)
        {
            var med = await _db.Medications.FindAsync(id);
            if (med != null)
            {
                _db.Medications.Remove(med);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
