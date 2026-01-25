using stepik.Db.Models;

namespace stepik.Db.Interfaces
{
    public interface IComparisonRepository
    {
        void Add(Product product, string userId);
        Comparison? TryGetByUserId(string userId);
        void Delete(int productId, string userId);
        void Clear(string userId);
    }
}