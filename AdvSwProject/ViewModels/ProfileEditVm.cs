using System.ComponentModel.DataAnnotations;

namespace AdvSwProject.ViewModels
{
    public class ProfileEditVm
    {
        [Required]
        public int UserId { get; set; }

        [MaxLength(150)]
        public string? FullName { get; set; }

        [Range(1, 120)]
        public int? Age { get; set; }

        [MaxLength(150)]
        public string? City { get; set; }

        public string? About { get; set; }

        [Url, MaxLength(300)]
        public string? AvatarUrl { get; set; }

        // “Reading, Walking, …” نفككها لاحقاً في الكنترولر إلى Skills
        public string? SkillsCsv { get; set; }
    }
}
