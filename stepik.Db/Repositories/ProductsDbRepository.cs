using stepik.Db.Interfaces;
using stepik.Db.Models;

namespace stepik.Db.Repositories
{
    public class ProductsDbRepository : IProductsRepository
    {
        private readonly DatabaseContext _databaseContext;

        public ProductsDbRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<Product> GetAll() => _databaseContext.Products.ToList();

        public Product? TryGetById(int productId) => 
            _databaseContext.Products.FirstOrDefault(product => product.Id == productId);

        public void Add(Product product)
        {
            _databaseContext.Products.Add(product);
            _databaseContext.SaveChanges(); 
        }

        public void Delete(int productId)
        {
            var existingProduct = TryGetById(productId);
            if (existingProduct != null)
            {
                _databaseContext.Products.Remove(existingProduct);
                _databaseContext.SaveChanges();  
            }
        }

        public void EditProduct(Product product)
        {
            var excitingProduct = TryGetById(product.Id);
            if (excitingProduct != null)
            {
                excitingProduct.Name = product.Name;
                excitingProduct.Cost = product.Cost;
                excitingProduct.Description = product.Description;
                excitingProduct.ImagePath = product.ImagePath;

                _databaseContext.SaveChanges(); 
            }
        }

        public List<Product> Search(string text)
        {
            var products = GetAll().Where(product => product.Name!.Contains(text, StringComparison.CurrentCultureIgnoreCase));
            return products.ToList() ?? [];
        }
    }
}