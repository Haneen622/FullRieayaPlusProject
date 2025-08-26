//using System.ComponentModel.DataAnnotations;

//namespace AdvSwProject.Models
//{
//    public class User
//    {
//        public int Id { get; set; }

//        [Required]
//        public string Username { get; set; }

//        [Required]
//        [EmailAddress]
//        public string Email { get; set; }

//        [Required]
//        public string Password
//        {
//            get; set;
//        }
//    }
//}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdvSwProject.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required, MaxLength(200), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Password { get; set; } = string.Empty;

        // 👇 هذه هي الخصائص اللي ناقصتك:
        public UserProfile? Profile { get; set; }
        public ICollection<UserSkills> UserSkills { get; set; } = new List<UserSkills>();
    }
}
