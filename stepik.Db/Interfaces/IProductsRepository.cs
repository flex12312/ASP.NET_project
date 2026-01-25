using stepik.Db.Models;

namespace stepik.Db.Interfaces
{
    public interface IProductsRepository
    {
        List<Product> GetAll();
        Product? TryGetById(int productId);
        void Add(Product product);
        void Delete(int id);
        void EditProduct(Product product);
        List<Product> Search(string? query);
    }
}