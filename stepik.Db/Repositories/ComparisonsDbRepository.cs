using Microsoft.EntityFrameworkCore;
using stepik.Db;
using stepik.Db.Interfaces;
using stepik.Db.Models;

namespace stepik.Db.Repositories
{
    public class ComparisonsDbRepository : IComparisonRepository
    {
        private readonly DatabaseContext _databaseContext;
        public ComparisonsDbRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public void Add(Product product, string? userId, string? guestId)
        {
            var comparison = TryGetByUserId(userId, guestId);
            if (comparison == null)
            {
                comparison = new Comparison()
                {
                    Id = Guid.NewGuid(),
                    Items = new List<Product> { product },
                    UserId = userId,
                    GuestId = userId == null ? guestId : null 
                };
                _databaseContext.Comparisons.Add(comparison);
            }
            else
            {
                var existingComp = comparison.Items.FirstOrDefault(fav => fav.Id == product.Id);
                if (existingComp == null)
                {
                    comparison.Items.Add(product);
                }
            }
            _databaseContext.SaveChanges();
        }

        public void Clear(string? userId, string? guestId)
        {
            var comparison = TryGetByUserId(userId, guestId);
            if (comparison != null)
            {
                _databaseContext.Comparisons.Remove(comparison);
                _databaseContext.SaveChanges();
            }
        }

        public void Delete(int productId, string? userId, string? guestId)
        {
            var comparison = TryGetByUserId(userId, guestId);
            comparison?.Items.RemoveAll(fav => fav.Id == productId);
            _databaseContext.SaveChanges();
        }

        public Comparison? TryGetByUserId(string? userId, string? guestId)
        {
            return _databaseContext.Comparisons.Include(x => x.Items).FirstOrDefault(x => (userId != null && x.UserId == userId) ||(guestId != null && x.GuestId == guestId));
        }
        public void Merge(string? guestId, string? userId)
        {
            var guestComparison = _databaseContext.Comparisons.Include(x => x.Items).FirstOrDefault(x => x.GuestId == guestId);
            var userComparison = _databaseContext.Comparisons.Include(x => x.Items).FirstOrDefault(x => x.UserId == userId);

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
                _databaseContext.Comparisons.Remove(guestComparison);
            }
            _databaseContext.SaveChanges();
        }
    }
}