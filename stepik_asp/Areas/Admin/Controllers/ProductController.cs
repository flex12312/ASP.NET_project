using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using stepik.Db;
using stepik.Db.Interfaces;
using stepik_asp.Areas.AdminPanel.Models;
using stepik_asp.Helpers;
using stepik_asp.Models;

namespace stepik_asp.Areas.AdminPanel.Controllers
{
    [Area(Consts.AdminRoleName)]
    [Authorize(Roles = Consts.AdminRoleName)]
    public class ProductController(IProductsRepository _productsRepository) : Controller
    {
        #region Products
        public IActionResult Products()
        {
            var products = _productsRepository.GetAll();
            return View(products.ToProductViewModels());
        }

        public IActionResult DeleteProduct(int id)
        {
            _productsRepository.Delete(id);
            return RedirectToAction("Products");
        }

        
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddProduct(ProductViewModel product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            _productsRepository.Add(product.ToProductDb());
            return RedirectToAction(nameof(Products));
        }


        public IActionResult EditProduct(int id)
        {
            var existingProduct = _productsRepository.TryGetById(id);
            return View(existingProduct?.ToProductViewModel());
        }

        [HttpPost]
        public IActionResult EditProduct(ProductViewModel product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            _productsRepository.EditProduct(product.ToProductDb());
            return RedirectToAction(nameof(Products));
        }
        #endregion
    }
}