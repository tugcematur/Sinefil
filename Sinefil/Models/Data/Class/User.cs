using Sinefil.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sinefil.Models.Data.Class
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; } 

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
