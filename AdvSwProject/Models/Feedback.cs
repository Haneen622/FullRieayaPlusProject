using System.ComponentModel.DataAnnotations;

namespace AdvSwProject.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; } = "";

        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = "";

        [MaxLength(20)]
        public string Phone { get; set; } = "";

        [Required, MaxLength(1000)]
        public string Message { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
