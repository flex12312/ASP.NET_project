using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using stepik.Db;
using stepik.Db.Interfaces;
using stepik.Db.Models;
using stepik_asp.Areas.AdminPanel.Models;
using stepik_asp.Helpers;
using stepik_asp.Models;

namespace stepik_asp.Areas.AdminPanel.Controllers
{
    [Area(Consts.AdminRoleName)]
    [Authorize(Roles = Consts.AdminRoleName)]
    public class ProductController(IProductsRepository _productsRepository,
        IWebHostEnvironment _appEnvironment) : Controller
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
        public async Task<IActionResult> AddProduct(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null)
                {
                    var uploadsFolder = Path.Combine(_appEnvironment.WebRootPath + "/images/products/");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Guid.NewGuid() + "." + product.ImageFile.FileName.Split('.').Last();
                    using (var fileStream = new FileStream(uploadsFolder + fileName, FileMode.Create))
                    {
                         await product.ImageFile.CopyToAsync(fileStream);
                    }
                    product.ImagePath = "/images/products/" + fileName;
                }
                _productsRepository.Add(product.ToProductDb());
                return RedirectToAction(nameof(Products)); 
            }
            return View(product);
        }


        public IActionResult EditProduct(int id)
        {
            var existingProduct = _productsRepository.TryGetById(id);
            return View(existingProduct?.ToProductViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null)
                {
                    var oldProduct = _productsRepository.TryGetById(product.Id);
                    if (oldProduct != null && !string.IsNullOrEmpty(oldProduct.ImagePath))
                    {
                        if (oldProduct.ImagePath.StartsWith("/images/products/"))
                        {
                            var oldFilePath = Path.Combine(_appEnvironment.WebRootPath, oldProduct.ImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath)) 
                                System.IO.File.Delete(oldFilePath);
                        }
                    }
                    var uploadsFolder = Path.Combine(_appEnvironment.WebRootPath, "images", "products");
                    if (!Directory.Exists(uploadsFolder)) 
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid() + "." + product.ImageFile.FileName.Split('.').Last();
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }
                    product.ImagePath = "/images/products/" + fileName;
                }
                _productsRepository.EditProduct(product.ToProductDb());
                return RedirectToAction(nameof(Products));
            }
            return View(product);
        }
        #endregion
    }
}