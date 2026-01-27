using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using stepik.Db.Interfaces;
using stepik.Db.Models;
using stepik.Db.Repositories;
using stepik_asp.Models;

namespace stepik_asp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ICartsRepository _cartsRepository;

        public AccountController(UserManager<User> userManager,SignInManager<User> signInManager,ICartsRepository cartsRepository) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cartsRepository = cartsRepository;
        }

        public IActionResult Autorization(string returnUrl)
        {
            return View(new AutorizationViewModel { ReturnUrl = returnUrl ?? "/" });
        }

        [HttpPost]
        public async Task<IActionResult> Autorization(AutorizationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.Login)
                     ?? await _userManager.FindByEmailAsync(model.Login);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName,
                    model.Password,
                    model.IsRememberMe,
                    false);

                if (result.Succeeded)
                {
                    string guestId = Request.Cookies["GuestId"];
                    if (!string.IsNullOrEmpty(guestId))
                    {
                        _cartsRepository.Merge(guestId, user.Id); 
                        Response.Cookies.Delete("GuestId");       
                    }
                    return Redirect(model.ReturnUrl ?? "/");
                }
            }

            ModelState.AddModelError("", "Неверный логин или пароль");
            return View(model);
        }

        public IActionResult Registration(string returnUrl)
        {
            return View(new RegistrationViewModel
            {
                UserName = "",        
                Email = "",
                Password = "",
                ConfirmPassword = "",
                Phone = "",
                FirstName = "",
                LastName = "",
                ReturnUrl = returnUrl  
            });
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (model.UserName == model.Password)
            {
                ModelState.AddModelError("", "Имя пользователя и пароль не должны совпадать");
            }

            if (ModelState.IsValid)
            {
                if (await _userManager.FindByNameAsync(model.UserName) != null)
                {
                    ModelState.AddModelError("Login", "Пользователь с таким именем уже существует");
                    return View(model);
                }

                if (await _userManager.FindByEmailAsync(model.Email) != null)
                {
                    ModelState.AddModelError("Email", "Пользователь с таким email уже существует");
                    return View(model);
                }

                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    FirstName = model.FirstName ?? "DefaultFirstName", 
                    LastName = model.LastName ?? "DefaultLastName"
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    string guestId = Request.Cookies["GuestId"];
                    if (!string.IsNullOrEmpty(guestId))
                    {
                        _cartsRepository.Merge(guestId, user.Id);
                        Response.Cookies.Delete("GuestId");
                    }
                    return Redirect(model.ReturnUrl ?? "/Home");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}