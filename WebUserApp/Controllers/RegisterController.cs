using Microsoft.AspNetCore.Mvc;
using DataLayer;
using DataLayer.DatabaseEntites;
using WebUserApp.Models;


namespace WebUserApp.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View();

            Dictionary<string, string> loginData = new Dictionary<string, string>();
            loginData.Add("email", model.email);
            loginData.Add("password", model.password);
            List<User> users = DBConnection.Select<User>(loginData);


            if (users.Count == 1)
            {
                UserSingleton.Instance.loggedUser = users[0];
                return RedirectToAction("Welcome", "Home");
            }

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            if (!ModelState.IsValid)
                return View();

            User newuser = new User()
            {
                Admin = false,
                Email = model.email,
                FirstName = model.firstName,
                LastName = model.lastName,
                Password = model.password,
                UserName = model.username
            };

            DBConnection.Insert(newuser);

            UserSingleton.Instance.loggedUser = newuser;
            return RedirectToAction("Welcome", "Home");

        }

        [HttpPost]
        public IActionResult Logout()
        {
            UserSingleton.Instance.loggedUser = null;
            return RedirectToAction("Index", "Home");
        }


    }
}
