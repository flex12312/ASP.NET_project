using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stepik.Db.Interfaces;
using stepik_asp.Helpers;
using stepik_asp.Models;

namespace stepik_asp.Controllers
{
    [Authorize]
    public class ComparisonController(IProductsRepository _productsRepository, IComparisonRepository _comparisonRepository) : Controller
    {
        public IActionResult Index()
        {
            var comparison = _comparisonRepository.TryGetByUserId(Constants.UserId);
            return View(comparison.ToComparisonViewModel());
        }
        public IActionResult Add(int productId)
        {
            var product = _productsRepository.TryGetById(productId);
            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }

            _comparisonRepository.Add(product, Constants.UserId);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Delete(int productId)
        {
            _comparisonRepository.Delete(productId, Constants.UserId);
            return RedirectToAction("Index");
        }
        public IActionResult Clear()
        {
            _comparisonRepository.Clear(Constants.UserId);
            return RedirectToAction("Index");
        }
    }
}