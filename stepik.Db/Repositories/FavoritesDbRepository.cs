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

        public void Add(Product product, string? userId, string? guestId)
        {
            var favorite = TryGetByUserId(userId, guestId);
            if (favorite == null)
            {
                favorite = new Favorite() {  Id = Guid.NewGuid() , Items = [product], UserId = userId, GuestId = userId == null ? guestId : null };
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

        public void Clear(string? userId, string? guestId)
        {
            var favorite = TryGetByUserId(userId, guestId);
            if (favorite != null)
            {
                _databaseContext.Favorites.Remove(favorite);
                _databaseContext.SaveChanges();
            }
        }

        public void Delete(int productId, string? userId, string? guestId)
        {
            var favorite = TryGetByUserId(userId, guestId);
            favorite?.Items.RemoveAll(fav => fav.Id == productId);
            _databaseContext.SaveChanges();
        }

        public Favorite? TryGetByUserId(string? userId, string? guestId)
        {
            return _databaseContext.Favorites.Include(x => x.Items).FirstOrDefault(x => (userId != null && x.UserId == userId) ||(guestId != null && x.GuestId == guestId));
        }
        public void Merge(string? guestId, string? userId)
        {
            var guestComparison = _databaseContext.Favorites.Include(x => x.Items).FirstOrDefault(x => x.GuestId == guestId);
            var userComparison = _databaseContext.Favorites.Include(x => x.Items).FirstOrDefault(x => x.UserId == userId);

            if (guestComparison == null) return;
            if (userComparison == null)
            {
                guestComparison.UserId = userId;
                guestComparison.GuestId = null;
            }
            else
            {
                foreach (var item in guestComparison.Items)
                {
                    if (!userComparison.Items.Any(x => x.Id == item.Id))
                    {
                        userComparison.Items.Add(item);
                    }
                }
                _databaseContext.Favorites.Remove(guestComparison);
            }
            _databaseContext.SaveChanges();
        }
    }
}