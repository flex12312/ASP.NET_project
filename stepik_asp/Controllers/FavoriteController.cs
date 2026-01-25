using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stepik.Db.Interfaces;
using stepik_asp.Helpers;
using stepik_asp.Models;

namespace stepik_asp.Controllers
{
    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly IFavoritesRepository _favoritesRepository;
        private readonly IProductsRepository _productsRepository;

        public FavoriteController(IFavoritesRepository IfavoritesRepository, IProductsRepository productsRepository)
        {
            _favoritesRepository = IfavoritesRepository;
            _productsRepository = productsRepository;
        }

        public IActionResult Index()
        {
            var favorite = _favoritesRepository.TryGetByUserId(Constants.UserId);

            return View(favorite.ToFavoriteViewModel());
        }
        public IActionResult Add(int productId)
        {
            var product = _productsRepository.TryGetById(productId);
            if (product == null)
            {
                return RedirectToAction("Index","Home"); 
            }
            
            _favoritesRepository.Add(product, Constants.UserId);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Delete(int productId)
        {
            _favoritesRepository.Delete(productId,Constants.UserId);
            return RedirectToAction("Index");
        }
        public IActionResult Clear()
        {
            _favoritesRepository.Clear(Constants.UserId);
            return RedirectToAction("Index");
        }
    }
}