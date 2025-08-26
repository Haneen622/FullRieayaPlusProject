//using Microsoft.AspNetCore.Mvc;

//namespace AdvSwProject.Controllers
//{
//    public class EmergencyContactsController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdvSwProject.Data;
using AdvSwProject.Models;

namespace AdvSwProject.Controllers
{
    public class EmergencyContactsController : Controller
    {
        private readonly DataDbContext _db;
        public EmergencyContactsController(DataDbContext db) => _db = db;

        // GET: /EmergencyContacts
        public IActionResult Index() => View(); // يعرض الـ View

        // ---- Endpoints للواجهة (JSON) ----

        // GET: /EmergencyContacts/List
        [HttpGet]
        public async Task<IActionResult> List()
            => Json(await _db.EmergencyContacts.AsNoTracking().ToListAsync());

        // POST: /EmergencyContacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] EmergencyContact c)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _db.EmergencyContacts.Add(c);
            await _db.SaveChangesAsync();
            return Json(c);
        }

        // POST: /EmergencyContacts/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [FromBody] EmergencyContact c)
        {
            if (id != c.Id) return BadRequest();
            if (!await _db.EmergencyContacts.AnyAsync(x => x.Id == id)) return NotFound();

            _db.Entry(c).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return Json(new { ok = true });
        }

        // POST: /EmergencyContacts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.EmergencyContacts.FindAsync(id);
            if (entity is null) return NotFound();
            _db.Remove(entity);
            await _db.SaveChangesAsync();
            return Json(new { ok = true });
        }
    }
}
