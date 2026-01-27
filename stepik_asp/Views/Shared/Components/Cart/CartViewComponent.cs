using Microsoft.AspNetCore.Mvc;
using stepik.Db.Interfaces;
using stepik_asp.Helpers;
using stepik_asp.Interfaces;

namespace stepik_asp.Views.Shared.Components.Cart
{
    public class CartViewComponent(ICartsRepository cartsRepository) : ViewComponent
    {
        private readonly ICartsRepository _cartsRepository = cartsRepository;

        public IViewComponentResult Invoke()
        {
            string? userId = User.Identity.IsAuthenticated ? UserClaimsPrincipal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value : null;
            string? guestId = Request.Cookies["GuestId"];
            var cart = _cartsRepository.TryGetByUserId(userId, guestId);
            var productsCount = cart?.ToCartViewModel()?.Quantity ?? 0;
            return View("Cart", productsCount);
        }

    }
}