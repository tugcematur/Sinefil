using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sinefil.Models.Data;
using Sinefil.Models.Data.Class;
using Sinefil.Models.DTO;
using Sinefil.Models.Viewmodels;
using System.IO;

namespace Sinefil.Controllers
{
    public class FilmController : Controller
    {
        SinefilContext _db;
        public FilmController(SinefilContext db)
        {
            _db = db;
        }

        public IActionResult List(string search)
        {
            var film = _db.Film.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var normalizedSearch = search?.Trim().ToLower();
                film = film.Where(f => f.Title.ToLower().Contains(normalizedSearch));
            }

            var filmlist = film.Select(f => new FilmDTO
                {
                    FilmId = f.FilmId,
                    Title = f.Title,
                    Director = f.Director,
                    Year = f.Year,
                    Genre = f.Genre,
                    Description = f.Description,
                    ImgUrl = f.ImgUrl,
                    AverageRating = f.Reviews.Any() ? f.Reviews.Average(r => (double?)r.Rating) ?? 0 : 0


                }).OrderByDescending(f => f.AverageRating).ToList();

            return View(filmlist);

        }

        public IActionResult Details(int id)
        {
            var film = _db.Set<Film>()
                .Include(r => r.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefault(f => f.FilmId == id);

            if (film == null)
            {
                return NotFound();
            }
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            double averageRating = film.Reviews.Any() ? film.Reviews.Average(r => (double?)r.Rating) ?? 0 :0;
            ViewBag.AverageRating = averageRating;
            ViewBag.ReviewCount = film.Reviews?.Count() ?? 0;


            return View(film);

        }
        public IActionResult Add()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            return View();
        }

        [HttpPost]

        public IActionResult Add(FilmModel model)
        {


            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            //    ViewBag.Errors = errors;
            //    return View(model);
            //}


            var film = new Film
            {
                Title = model.Title,
                Director = model.Director,
                Year = model.Year,
                Genre = model.Genre,
                Description = model.Description,
                ImgUrl = model.ImgUrl

            };

            _db.Set<Film>().Add(film);
            _db.SaveChanges();

            TempData["ToastMessage"] = "Kayıt Başarılı";
            return RedirectToAction("Index", "Film");
        }

        public IActionResult AddReview(int id)
        {
            var film = _db.Set<Film>().Find(id);

            if (film == null)
            {
                return NotFound();
            }

            var model = new ReviewModel
            {
                FilmId = id
            };

            return View(model);
        }

        [HttpPost]

        public IActionResult AddReview(ReviewModel model)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool alreadyReviewed = _db.Set<Review>().Any(r => r.FilmId == model.FilmId && r.UserId == userId.Value);

            if (alreadyReviewed)
            {
                TempData["ToastMessage"] = "Bu filme daha önce yorum yaptınız.";
                TempData["ToastType"] = "danger";
                return RedirectToAction("Details", "Film", new { id = model.FilmId });
            }

            var review = new Review
            {
                FilmId = model.FilmId,
                Comment = model.Comment,
                Rating = model.Rating,
                UserId = userId.Value,
                CreateAt = DateTime.Now
            };
            _db.Set<Review>().Add(review);
            _db.SaveChanges();

            TempData["ToastMessage"] = "Yorumunuz başarıyla eklendi.";

            return RedirectToAction("Details", "Film", new { id = model.FilmId });

        }

        public IActionResult DeleteReview(int id)
        {
            var review = _db.Set<Review>().FirstOrDefault(r => r.ReviewId == id);

            if (review == null)
            {
                return View();
            }

            _db.Set<Review>().Remove(review);
            _db.SaveChanges();

            return RedirectToAction("Details", "Film", new { id = review.FilmId });
        }

        public IActionResult EditReview(int id)
        {
            var review = _db.Set<Review>().FirstOrDefault(r => r.ReviewId == id);


            if (review == null)
            {
                return View();
            }

            var model = new ReviewModel
            {
                FilmId = review.FilmId,
                ReviewId = review.ReviewId,
                Comment = review.Comment,
                Rating = review.Rating

            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditReview(ReviewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Comment))
            {
                ViewBag.Message = "Lütfen boş alan bırakmayınız!";
                return View();
            }

            var review = _db.Set<Review>().FirstOrDefault(r => r.ReviewId == model.ReviewId);

            int? userId = HttpContext.Session.GetInt32("UserId");

            if (review == null)
            {
                return View(model);
            }

            review.Comment = model.Comment;
            review.Rating = model.Rating;

            _db.Set<Review>().Update(review);
            _db.SaveChanges();

            TempData["ToastMessage"] = "Yorumunuz başarıyla güncellendi.";
            return RedirectToAction("Details", "Film", new { id = review.FilmId });
        }

        public IActionResult DeleteFilm(int id)
        {
            var film = _db.Set<Film>().FirstOrDefault(f => f.FilmId == id);

            if (film == null)
            {
                return View();
            }

            _db.Set<Film>().Remove(film);
            _db.SaveChanges();

            return RedirectToAction("List", "Film");
        }

        public IActionResult EditFilm(int id)
        {
            var film = _db.Set<Film>().FirstOrDefault(f => f.FilmId == id);

            if (film == null)
            {
                return View();
            }

            var model = new FilmModel
            {
                FilmId=film.FilmId,
                Title = film.Title,
                Director = film.Director,
                Year = film.Year,
                Genre = film.Genre,
                Description = film.Description,
                ImgUrl = film.ImgUrl
            };

            return View(model);
        }

        [HttpPost]

        public IActionResult EditFilm(FilmModel model)
        {
            var film = _db.Set<Film>().FirstOrDefault(f => f.FilmId == model.FilmId);

            if (film == null)
            {
                return View(model);
            }

            film.Title = model.Title;
            film.Director = model.Director;
            film.Year = model.Year;
            film.Genre = model.Genre;
            film.Description = model.Description;
            film.ImgUrl = model.ImgUrl;

            _db.Set<Film>().Update(film);
            _db.SaveChanges();

            TempData["ToastMessage"] = "Güncelleme Başarılı";
            

            return RedirectToAction("List", "Film");
        }

       
    }
}
