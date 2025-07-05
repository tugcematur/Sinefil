using System.ComponentModel.DataAnnotations;

namespace Sinefil.Models.Viewmodels
{
    public class ReviewModel
    {
        public int FilmId { get; set; }
        public int ReviewId { get; set; } 

        [Required(ErrorMessage = "Yorum boş bırakılamaz")]
        public string Comment { get; set; }

        [Range(1,10,ErrorMessage = "Puan 1 ile 10 arasında olmalıdır")]
        public int? Rating { get; set; }

        public int UserId { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
