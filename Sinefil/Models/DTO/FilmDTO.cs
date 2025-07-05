namespace Sinefil.Models.DTO
{
    public class FilmDTO
    {
        public int FilmId { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }

        public double AverageRating { get; set; }

        public int ReviewCount { get; set; }
    }
}
