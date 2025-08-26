
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AdvSwProject.Data;
using AdvSwProject.Extenshions.MappingExtenshion;
using AdvSwProject.ViewModels;
using System.Linq;

public class AccountController : Controller
{
    private readonly DataDbContext _context;

    public AccountController(DataDbContext context)
    {
        _context = context;
    }

    // GET: /Account/Auth?form=login|signup
    [HttpGet]
    public IActionResult Auth(string form = "login")
    {
        ViewData["Form"] = (form ?? "login").ToLowerInvariant();
        return View(new UserViewmodel());
    }

    // (اختياري) توجيه سريع إلى Signup
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(UserViewmodel user) => Signup(user);

    // POST: /Account/Signup
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Signup(UserViewmodel user)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Form"] = "signup";
            return View("Auth", user);
        }

        var exists = _context.Users.FirstOrDefault(u => u.Email == user.Email);
        if (exists != null)
        {
            ModelState.AddModelError(nameof(user.Email), "هذا البريد مستخدم بالفعل");
            ViewData["Form"] = "signup";
            return View("Auth", user);
        }

        _context.Users.Add(user.toModel());
        _context.SaveChanges();

        TempData["Ok"] = "تم إنشاء الحساب. يمكنك تسجيل الدخول الآن.";
        return RedirectToAction("Auth", new { form = "login" });
    }

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string email, string password, bool rememberMe)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "wrong email or password");
            ViewData["Form"] = "login";
            return View("Auth", new UserViewmodel { Email = email });
        }

        // ✅ خزّن الـ UserId (الأهم) وباقي المعلومات في Session
        HttpContext.Session.SetInt32("UserId", user.Id);              // 👈 الجديد
        HttpContext.Session.SetString("UserEmail", user.Email);
        HttpContext.Session.SetString("Username", user.Username ?? "");

        // ✅ أنشئ سجل بروفايل أول مرة إن لم يوجد
        var hasProfile = _context.UserProfiles.Any(p => p.UserId == user.Id);
        if (!hasProfile)
        {
            _context.UserProfiles.Add(new AdvSwProject.Models.UserProfile { UserId = user.Id });
            _context.SaveChanges();
        }

        // ✅ بعد اللوج إن: روح مباشرة على بروفايلي أنا
        //return RedirectToAction("Me", "Profile");
        return RedirectToAction("Index", "Home");

    }

    // POST: /Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Auth", new { form = "login" });
    }
}
