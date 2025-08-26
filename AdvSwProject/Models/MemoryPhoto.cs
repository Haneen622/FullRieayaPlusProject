using System;
using System.ComponentModel.DataAnnotations;

namespace AdvSwProject.Models
{
    public class MemoryPhoto
    {
        public int Id { get; set; }

        // المالك (من جدول Users)
        public int UserId { get; set; }

        [Required, MaxLength(500)]
        public string FilePath { get; set; } = ""; // مثال: /uploads/memories/abc.jpg (مسار نسبي داخل wwwroot)

        [MaxLength(500)]
        public string? Caption { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
    }
}
