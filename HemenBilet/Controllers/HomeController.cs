using HemenBilet.Data;
using HemenBilet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HemenBilet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var u = UserRepository.Users()
                    .Find(i => i.UserName.Trim() == user.UserName.Trim() && i.Password.Trim() == user.Password.Trim());


                if (u != null)
                {
                    // return RedirectToAction("List", "Post");
                }
                else
                {
                    ModelState.AddModelError(" ", "Kullanıcı Adı Veya Şifre Hatalı");
                }
            }
            return View(user);
        }
        public IActionResult Signup()
        {
            return View(new User());
        }
        [HttpPost]
        public IActionResult Signup(User u)
        {
            if (ModelState.IsValid)
            {
                var lastUser = UserRepository.Users().OrderByDescending(p => p.UserId).FirstOrDefault();
                var newUserId = (lastUser != null) ? lastUser.UserId + 1 : 1;
                User user = new User()
                {
                    UserName = u.UserName,
                    UserId = newUserId,
                    Password = u.Password,
                    Email = u.Email,
                    Phone = u.Phone
                };

                UserRepository.AddUser(user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(u);
            }
        }
    }
}
