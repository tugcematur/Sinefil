using System.ComponentModel.DataAnnotations.Schema;

namespace Sinefil.Models.Data.Class
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string Comment { get; set; }
        public int? Rating { get; set; }
        public DateTime CreateAt { get; set; }
        public int UserId { get; set; }
        public int FilmId { get; set; }


        [ForeignKey("FilmId")]
        public Film Film { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
