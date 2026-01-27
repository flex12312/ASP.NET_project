using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stepik.Db.Interfaces;
using stepik_asp.Helpers;
using stepik_asp.Models;

namespace stepik_asp.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly IFavoritesRepository _favoritesRepository;
        private readonly IProductsRepository _productsRepository;

        public FavoriteController(IFavoritesRepository IfavoritesRepository, IProductsRepository productsRepository)
        {
            _favoritesRepository = IfavoritesRepository;
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
            var(userId, guestId) = GetUserIdentifiers();
            var favorite = _favoritesRepository.TryGetByUserId(userId, guestId);

            return View(favorite.ToFavoriteViewModel());
        }
        public IActionResult Add(int productId)
        {
            var (userId, guestId) = GetUserIdentifiers();
            var product = _productsRepository.TryGetById(productId);
            if (product == null)
            {
                return RedirectToAction("Index","Home"); 
            }
            
            _favoritesRepository.Add(product, userId, guestId);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Delete(int productId)
        {
            var (userId, guestId) = GetUserIdentifiers();
            _favoritesRepository.Delete(productId, userId, guestId);
            return RedirectToAction("Index");
        }
        public IActionResult Clear()
        {
            var (userId, guestId) = GetUserIdentifiers();
            _favoritesRepository.Clear(userId, guestId);
            return RedirectToAction("Index");
        }
    }
}