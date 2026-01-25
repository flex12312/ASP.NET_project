using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stepik.Db.Interfaces;
using stepik_asp.Helpers;
using stepik_asp.Interfaces;

namespace stepik_asp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartsRepository _cartsRepository;
        private readonly IProductsRepository _productsRepository;

        public CartController(ICartsRepository cartsRepository, IProductsRepository productsRepository)
        {
            _cartsRepository = cartsRepository;
            _productsRepository = productsRepository;
        }
        public IActionResult Index()
        {
            var cart = _cartsRepository.TryGetByUserId(Constants.UserId);
            return View(cart.ToCartViewModel());
        }
        public IActionResult Add(int productId)
        {
            var product = _productsRepository.TryGetById(productId);
            _cartsRepository.Add(product, Constants.UserId);
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            _cartsRepository.Clear(Constants.UserId);
            return RedirectToAction("Index");
        }

        public IActionResult Decrease(int productId)
        {
            _cartsRepository.Decrease(productId, Constants.UserId);
            return RedirectToAction("Index");
        }
    }
}