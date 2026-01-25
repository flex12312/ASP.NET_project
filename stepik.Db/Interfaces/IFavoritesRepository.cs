using stepik.Db.Models;

namespace stepik.Db.Interfaces
{
    public interface IFavoritesRepository
    {
        void Add(Product product, string userId);
        Favorite? TryGetByUserId(string userId);
        void Delete(int productId, string userId);
        void Clear(string userId);
    }
}