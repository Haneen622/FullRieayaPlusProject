

using AdvSwProject.Data;
using AdvSwProject.Models;
using AdvSwProject.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvSwProject.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DataDbContext _db;
        public ProfileController(DataDbContext db) => _db = db;


        // GET: /Profile/Me  → يعرض بروفايل المستخدم الحالي من السيشن
        [HttpGet]
        public IActionResult Details()
    => RedirectToAction(nameof(Me));   // /Profile/Details → /Profile/Me

        public async Task<IActionResult> Me()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId is null)
                return RedirectToAction("Auth", "Account", new { form = "login" });

            var user = await _db.Users
                .Include(u => u.Profile)
                .Include(u => u.UserSkills).ThenInclude(us => us.Skill)
                .FirstOrDefaultAsync(u => u.Id == userId.Value);

            if (user == null) return NotFound();

            // إنشاء بروفايل أول مرة إن لم يوجد
            if (user.Profile == null)
            {
                user.Profile = new UserProfile { UserId = user.Id };
                _db.UserProfiles.Add(user.Profile);
                await _db.SaveChangesAsync();
            }

            return View("Details", user); // نستخدم نفس الـView Details.cshtml
        }

        // POST: حفظ التعديلات للمستخدم الحالي (من المودال)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileEditVm vm)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId is null)
                return RedirectToAction("Auth", "Account", new { form = "login" });

            // حمّل/أنشئ البروفايل
            var prof = await _db.UserProfiles.FindAsync(userId.Value);
            if (prof == null)
            {
                prof = new UserProfile { UserId = userId.Value };
                _db.UserProfiles.Add(prof);
            }

            // حدّث الحقول
            prof.FullName = vm.FullName;
            prof.Age = vm.Age;
            prof.City = vm.City;
            prof.About = vm.About;
            prof.AvatarUrl = vm.AvatarUrl;

            // حدّث المهارات: امسح القديم وأضف الجديد
            var old = _db.UserSkills.Where(us => us.UserId == userId.Value);
            _db.UserSkills.RemoveRange(old);

            var names = (vm.SkillsCsv ?? "")
                .Split(new[] { ',', '،' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => s.Length > 0)
                .Distinct();

            foreach (var name in names)
            {
                var skill = await _db.Skills.FirstOrDefaultAsync(s => s.Name == name);
                if (skill == null)
                {
                    skill = new Skill { Name = name };
                    _db.Skills.Add(skill);
                    await _db.SaveChangesAsync(); // نضمن Id للمهارة الجديدة
                }

                // 👇 انتبهي: اسم الكيان الصحيح UserSkill (مش UserSkills)
                _db.UserSkills.Add(new UserSkills
                {
                    UserId = userId.Value,
                    SkillId = skill.Id
                });
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Me));
        }

        //here
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMe()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId is null)
                return RedirectToAction("Auth", "Account", new { form = "login" });

            // حمّل المستخدم مع البيانات التابعة اللي لازم تنحذف (عدّل حسب جداولك)
            var user = await _db.Users
                .Include(u => u.Profile)
                .Include(u => u.UserSkills)   // لو كيانك اسمه UserSkill غيّر السطر
                .FirstOrDefaultAsync(u => u.Id == userId.Value);

            if (user == null)
                return RedirectToAction("Index", "Home");

            // استخدم ترانزاكشن للامان
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                // احذف التبعيات إذا ما عندك Cascade Delete في الـ FK
                if (user.Profile != null) _db.UserProfiles.Remove(user.Profile);
                if (user.UserSkills?.Any() == true) _db.UserSkills.RemoveRange(user.UserSkills);

                // TODO: احذف أي كيانات تابعة أخرى تخص المستخدم (Posts, Comments, …) إن وجدت

                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                // ممكن تعرضي رسالة خطأ مناسبة
                TempData["Err"] = "Error occured Try again Later";
                return RedirectToAction(nameof(Me));
            }

            // تسجيل الخروج وتنظيف السيشن/الكوكي
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["Msg"] = "Account Deleted successfully";
            return RedirectToAction("Auth", "Account");
        }

    }
}
