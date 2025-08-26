using Microsoft.AspNetCore.Mvc;
using AdvSwProject.Data;
using AdvSwProject.Models;

namespace AdvSwProject.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly DataDbContext _db;
        public FeedbackController(DataDbContext db) => _db = db;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send([FromBody] Feedback model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _db.Feedbacks.Add(model);
            await _db.SaveChangesAsync();

            return Json(new { ok = true, message = "Feedback saved successfully" });
        }
    }
}
