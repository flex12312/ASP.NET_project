using Microsoft.EntityFrameworkCore;
using stepik.Db;
using stepik.Db.Interfaces;
using stepik.Db.Models;

namespace stepik.Db.Repositories
{
    public class FavoritesDbRepository : IFavoritesRepository
    {
        private readonly DatabaseContext _databaseContext;

        public FavoritesDbRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public void Add(Product product, string userId)
        {
            var favorite = TryGetByUserId(userId);
            if (favorite == null)
            {
                favorite = new Favorite() {  Id = Guid.NewGuid() , Items = [product], UserId = userId };
                _databaseContext.Favorites.Add(favorite);
            }
            else
            {
                var existingFav = favorite.Items.FirstOrDefault(fav => fav.Id == product.Id);

                if (existingFav == null)
                {
                    favorite.Items.Add(product);
                }
            }
            _databaseContext.SaveChanges();
        }

        public void Clear(string userId)
        {
            var favorite = TryGetByUserId(userId);
            if (favorite != null)
            {
                _databaseContext.Favorites.Remove(favorite);
                _databaseContext.SaveChanges();
            }
        }

        public void Delete(int productId, string userId)
        {
            var favorite = TryGetByUserId(userId);
            favorite?.Items.RemoveAll(fav => fav.Id == productId);
            _databaseContext.SaveChanges();
        }

        public Favorite? TryGetByUserId(string userId)
        {
            return _databaseContext.Favorites.Include(x => x.Items).FirstOrDefault(x => x.UserId == userId);
        }
    }
}