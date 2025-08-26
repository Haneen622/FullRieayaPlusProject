using System.ComponentModel.DataAnnotations;

namespace AdvSwProject.Models
{
    public class Skill
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;   // سنضع Unique Index لها في الـDbContext

        public ICollection<UserSkills> UserSkills { get; set; } = new List<UserSkills>();
    }
}
