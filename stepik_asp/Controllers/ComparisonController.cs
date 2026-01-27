using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stepik.Db.Interfaces;
using stepik_asp.Helpers;
using stepik_asp.Models;

namespace stepik_asp.Controllers
{
    public class ComparisonController(IProductsRepository _productsRepository, IComparisonRepository _comparisonRepository) : Controller
    {
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
            var comparison = _comparisonRepository.TryGetByUserId(userId, guestId);
            return View(comparison.ToComparisonViewModel() );
        }
        public IActionResult Add(int productId)
        {
            var (userId, guestId) = GetUserIdentifiers();
            var product = _productsRepository.TryGetById(productId);
            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }

            _comparisonRepository.Add(product, userId, guestId);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Delete(int productId)
        {
            var (userId, guestId) = GetUserIdentifiers();
            _comparisonRepository.Delete(productId, userId, guestId);
            return RedirectToAction("Index");
        }
        public IActionResult Clear()
        {
            var (userId, guestId) = GetUserIdentifiers();
            _comparisonRepository.Clear(userId, guestId);
            return RedirectToAction("Index");
        }
    }
}