

using Microsoft.EntityFrameworkCore;
using AdvSwProject.Models;

namespace AdvSwProject.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        // الجداول الموجودة
        public DbSet<User> Users { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HealthTip> HealthTips { get; set; }

        // الجديدة
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<UserSkills> UserSkills { get; set; }        // 👈 مفرد
        public DbSet<MemoryPhoto> MemoryPhotos { get; set; }    // للـ My Memories
        public DbSet<EmergencyContact> EmergencyContacts => Set<EmergencyContact>();
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // أسماء الجداول (اختياري توضيح)
            b.Entity<User>().ToTable("Users");
            b.Entity<UserProfile>().ToTable("UserProfiles");
            b.Entity<Skill>().ToTable("Skills");
            b.Entity<UserSkills>().ToTable("UserSkills");
            b.Entity<MemoryPhoto>().ToTable("MemoryPhotos");

            // ---------------- 1-إلى-1: User <-> UserProfile ----------------
            b.Entity<User>()
             .HasOne(u => u.Profile)
             .WithOne(p => p.User)
             .HasForeignKey<UserProfile>(p => p.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            // ---------------- many-to-many عبر UserSkills ----------------
            // مفتاح مركّب
            b.Entity<UserSkills>()
             .HasKey(us => new { us.UserId, us.SkillId });

            // UserSkills -> User
            b.Entity<UserSkills>()
             .HasOne(us => us.User)
             .WithMany(u => u.UserSkills)
             .HasForeignKey(us => us.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            // UserSkills -> Skill
            b.Entity<UserSkills>()
             .HasOne(us => us.Skill)
             .WithMany(s => s.UserSkills)
             .HasForeignKey(us => us.SkillId)
             .OnDelete(DeleteBehavior.Cascade);

            // ---------------- MemoryPhoto ----------------
            b.Entity<MemoryPhoto>(e =>
            {
                e.Property(x => x.FilePath).HasMaxLength(500).IsRequired();
                e.Property(x => x.Caption).HasMaxLength(500);
                e.HasOne(x => x.User)
                 .WithMany() // ما بدنا ناڤيجेशन عكسي
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(x => new { x.UserId, x.CreatedAt });
            });

            // فهارس وقيود مفيدة
            b.Entity<Skill>().HasIndex(s => s.Name).IsUnique();
            b.Entity<User>().HasIndex(u => u.Username).IsUnique();
            b.Entity<User>().HasIndex(u => u.Email).IsUnique();

            // تأكيد الأطوال
            b.Entity<User>(e =>
            {
                e.Property(x => x.Username).HasMaxLength(100).IsRequired();
                e.Property(x => x.Email).HasMaxLength(200).IsRequired();
                e.Property(x => x.Password).HasMaxLength(200).IsRequired();
            });
            b.Entity<UserProfile>(e =>
            {
                e.Property(x => x.FullName).HasMaxLength(150);
                e.Property(x => x.City).HasMaxLength(150);
                e.Property(x => x.AvatarUrl).HasMaxLength(300);
            });
            b.Entity<Skill>(e =>
            {
                e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            });
        }
    }
}
