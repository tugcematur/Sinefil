using Microsoft.AspNetCore.Mvc;
using Sinefil.Models.Data;
using Sinefil.Models.Data.Class;
using Sinefil.Models.Viewmodels;

namespace Sinefil.Controllers
{
    public class AccountController : Controller
    {
        SinefilContext _db;
        public AccountController(SinefilContext db)
        {
            _db = db;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserModel model)
        {
           

            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                ViewBag.Message = "Lütfen boş alan bırakmayın";
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = _db.Set<User>().FirstOrDefault(u => u.Email == model.Email);

            if (existingUser!= null)
            {
                ViewBag.Messsage = "Bu email adresi zaten kayıttlı";
                return View();
            }

            var newUser = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                Role = Models.Enums.UserRole.User
            };

            _db.Set<User>().Add(newUser);
            _db.SaveChanges();

            TempData["ToastMessage"] = "Kayıt Başarılı";

            return  RedirectToAction( "Login", "Account" );
        }

        public IActionResult Login()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Login(UserModel model)
        {
           

            if(string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password)){

                ViewBag.Message = "Lütfen boş alan bırakmayın";
                return View();
            }

           


            var user = _db.Set<User>().FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (user == null)
            {

                ViewBag.Message = "Geçersiz kullanıcı adı ya da şifre!";
                return View();
            }

            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            //    ViewBag.Errors = errors;
            //    return View(model);
            //}


            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("Role", user.Role.ToString());
            return RedirectToAction("Index", "Home");


        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
