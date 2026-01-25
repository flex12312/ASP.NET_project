using Microsoft.AspNetCore.Mvc;
using stepik.Db.Interfaces;
using stepik_asp.Helpers;

namespace stepik_asp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductsRepository _productsRepository;

        public HomeController(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }
        public IActionResult Index()
        {
            var products = _productsRepository.GetAll();
            return View(products.ToProductViewModels());

        }

        public IActionResult Search(string query)
        {
            if (query == null)
            {
                return View();
            }

            var products = _productsRepository.Search(query);

            return View(products.ToProductViewModels());
        }
    }

}