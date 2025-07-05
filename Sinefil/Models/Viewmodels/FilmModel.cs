using System.ComponentModel.DataAnnotations;

namespace Sinefil.Models.Viewmodels
{
    public class FilmModel
    {

        public int FilmId { get; set; }

        [Required(ErrorMessage = "Film adı zorunludur")]
        [StringLength(50, ErrorMessage = "Film adı en fazla 100 karakter olabilir.")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "Sadece harf ve boşluk kullanabilirsiniz.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Yönetmen adı zorunludur")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "Sadece harf ve boşluk kullanabilirsiniz.")]
        [StringLength(50)]
        public string  Director { get; set; }

        [Required(ErrorMessage="Yayın yılı zorunludur")]
        [Range(1900,2100, ErrorMessage ="Geçerli bir yıl giriniz")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Tür  zorunludur.")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "Sadece harf ve boşluk kullanabilirsiniz.")]
        [StringLength(50, ErrorMessage = "Tür en fazla 50 karakter olabilir.")]
        public string Genre { get; set; }

        [Required(ErrorMessage ="Açıklma alanı zorunludur")]
        [StringLength(500, ErrorMessage="Açıklama en fazla 500 karakter olabilir")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Resim URL'si zorunludur.")]
        [RegularExpression(@"^.*\.(jpg|jpeg|png|gif|bmp)$", ErrorMessage = "Geçerli bir resim URL'si giriniz (jpg, png, gif, bmp uzantılı).")]
        [Url(ErrorMessage = "Geçerli bir URL giriniz")]
        public string ImgUrl  { get; set; }
    }
}
