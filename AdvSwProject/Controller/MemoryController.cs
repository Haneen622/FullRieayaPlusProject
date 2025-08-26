using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdvSwProject.Data;
using AdvSwProject.Models;

namespace AdvSwProject.Controllers
{
    public class MemoryController : Controller
    {
        private readonly DataDbContext _db;
        private readonly IWebHostEnvironment _env;
        public MemoryController(DataDbContext db, IWebHostEnvironment env)
        {
            _db = db; _env = env;
        }

        // helper: user id من السيشن
        private int? CurrentUserId => HttpContext.Session.GetInt32("UserId");

        // GET /Memory
        public async Task<IActionResult> Index()
        {
            var uid = CurrentUserId;
            if (uid is null) return RedirectToAction("Auth", "Account", new { form = "login" });

            var photos = await _db.MemoryPhotos
                .Where(p => p.UserId == uid.Value)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(photos);
        }

        // POST /Memory/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file, string? caption)
        {
            var uid = CurrentUserId;
            if (uid is null) return RedirectToAction("Auth", "Account", new { form = "login" });

            if (file == null || file.Length == 0)
            {
                TempData["Err"] = "Please choose an image.";
                return RedirectToAction(nameof(Index));
            }

            // السماح بالامتدادات التالية فقط
            var allowed = new[] { ".png", ".jpg", ".jpeg", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowed.Contains(ext))
            {
                TempData["Err"] = "Only PNG/JPG/WebP are allowed.";
                return RedirectToAction(nameof(Index));
            }

            // مجلد التخزين داخل wwwroot
            var folder = Path.Combine(_env.WebRootPath, "uploads", "memories");
            Directory.CreateDirectory(folder);

            // اسم ملف آمن وفريد
            var fileName = $"{Guid.NewGuid()}{ext}";
            var absPath = Path.Combine(folder, fileName);
            var relPath = $"/uploads/memories/{fileName}"; // نخزّنه في DB

            // احفظ الملف
            using (var stream = System.IO.File.Create(absPath))
                await file.CopyToAsync(stream);

            _db.MemoryPhotos.Add(new MemoryPhoto
            {
                UserId = uid.Value,
                FilePath = relPath,
                Caption = string.IsNullOrWhiteSpace(caption) ? null : caption.Trim()
            });
            await _db.SaveChangesAsync();

            TempData["Ok"] = "Photo saved.";
            return RedirectToAction(nameof(Index));
        }

        // POST /Memory/EditCaption/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCaption(int id, string? caption)
        {
            var uid = CurrentUserId;
            if (uid is null) return RedirectToAction("Auth", "Account", new { form = "login" });

            var photo = await _db.MemoryPhotos.FirstOrDefaultAsync(p => p.Id == id && p.UserId == uid.Value);
            if (photo == null) return NotFound();

            photo.Caption = string.IsNullOrWhiteSpace(caption) ? null : caption.Trim();
            await _db.SaveChangesAsync();

            TempData["Ok"] = "Caption updated.";
            return RedirectToAction(nameof(Index));
        }

        // POST /Memory/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var uid = CurrentUserId;
            if (uid is null) return RedirectToAction("Auth", "Account", new { form = "login" });

            var photo = await _db.MemoryPhotos.FirstOrDefaultAsync(p => p.Id == id && p.UserId == uid.Value);
            if (photo == null) return NotFound();

            // احذف الملف من القرص
            var abs = Path.Combine(_env.WebRootPath, photo.FilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (System.IO.File.Exists(abs))
                System.IO.File.Delete(abs);

            _db.MemoryPhotos.Remove(photo);
            await _db.SaveChangesAsync();

            TempData["Ok"] = "Photo deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
