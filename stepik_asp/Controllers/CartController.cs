using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stepik.Db.Interfaces;
using stepik_asp.Helpers;
using stepik_asp.Interfaces;
using stepik_asp.Models;

namespace stepik_asp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartsRepository _cartsRepository;
        private readonly IProductsRepository _productsRepository;

        public CartController(ICartsRepository cartsRepository, IProductsRepository productsRepository)
        {
            _cartsRepository = cartsRepository;
            _productsRepository = productsRepository;
        }
        private (string? userId, string? guestId) GetUserIdentifiers()
        {
            string? userId = User.Identity.IsAuthenticated ? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value : null;

            string? guestId = Request.Cookies["GuestId"];

            if (userId == null && guestId == null)
            {
                guestId = Guid.NewGuid().ToString();
                Response.Cookies.Append("GuestId", guestId, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddHours(8)
                });
            }

            return (userId, guestId);
        }
        public IActionResult Index()
        {
            var (userId, guestId) = GetUserIdentifiers();
            var cart = _cartsRepository.TryGetByUserId(userId, guestId);
            return View(cart?.ToCartViewModel() ?? new CartViewModel());
        }

        public IActionResult Add(int productId)
        {
            var (userId, guestId) = GetUserIdentifiers();
            var product = _productsRepository.TryGetById(productId);

            _cartsRepository.Add(product, userId, guestId);
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            var (userId, guestId) = GetUserIdentifiers();

            _cartsRepository.Clear(userId, guestId);
            return RedirectToAction("Index");
        }

        public IActionResult Decrease(int productId)
        {
            var (userId, guestId) = GetUserIdentifiers();

            _cartsRepository.Decrease(productId, userId, guestId);
            return RedirectToAction("Index");
        }
    }
}