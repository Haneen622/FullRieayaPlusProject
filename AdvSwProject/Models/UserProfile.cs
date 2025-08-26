using System.ComponentModel.DataAnnotations;

namespace AdvSwProject.Models
{
    public class UserProfile
    {
        [Key]
        public int UserId { get; set; }          // PK + FK إلى User

        [MaxLength(150)]
        public string? FullName { get; set; }

        public int? Age { get; set; }

        [MaxLength(150)]
        public string? City { get; set; }

        public string? About { get; set; }       // NVARCHAR(MAX)

        [MaxLength(300)]
        public string? AvatarUrl { get; set; }

        public User User { get; set; } = null!;
    }
}
