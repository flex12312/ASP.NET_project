using Microsoft.AspNetCore.Mvc;
using stepik.Db.Interfaces;
using stepik_asp.Helpers;

namespace stepik_asp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsRepository _productsRepository;

        public ProductController(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }
        public IActionResult Index(int id)
        {
            var product = _productsRepository.TryGetById(id);

            return View(product?.ToProductViewModel());
        }


    }
}