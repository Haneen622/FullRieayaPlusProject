

using AdvSwProject;
using AdvSwProject.Data;
using AdvSwProject.Data.Interceptors;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Session (للعمل مع HttpContext.Session)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromHours(8);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

// DbContext (بدون NoTracking عالميًا عشان التحديثات تشتغل طبيعي)
builder.Services.AddDbContext<DataDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
       .AddInterceptors(new softDeleteInterceptor())
);

// Cookie Auth (جاهز لو حبيتي تنتقلي للكوكي لاحقًا)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        // خليه يوجّه لنفس صفحة الـAuth عند طلب تسجيل الدخول
        o.LoginPath = "/Account/Auth?form=login";
        o.AccessDeniedPath = "/Account/Auth?form=login";
        o.SlidingExpiration = true;
        o.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session لازم قبل ما نوصل للأكشنز
app.UseSession();

// لو استخدمتي الكوكي لاحقًا (ما بضر يكونوا مفعّلين)
app.UseAuthentication();
app.UseAuthorization();

// الصفحة الافتراضية على شاشة Auth
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Auth}/{id?}"
);
//SimpleTest.Run();

app.Run();
