namespace Sinefil.Models.Data.Class
{
    public class Film
    {
        public int FilmId { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
     
    }
}
