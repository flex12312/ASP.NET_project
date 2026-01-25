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

        public void Add(Product product, string userId)
        {
            var comparison = TryGetByUserId(userId);
            if (comparison == null)
            {
                comparison = new Comparison() { Id = Guid.NewGuid(), Items = [product], UserId = userId };
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

        public void Clear(string userId)
        {
            var comparison = TryGetByUserId(userId);
            if (comparison != null)
            {
                _databaseContext.Comparisons.Remove(comparison);
                _databaseContext.SaveChanges();
            }
        }

        public void Delete(int productId, string userId)
        {
            var comparison = TryGetByUserId(userId);
            comparison?.Items.RemoveAll(fav => fav.Id == productId);
            _databaseContext.SaveChanges();
        }

        public Comparison? TryGetByUserId(string userId)
        {
            return _databaseContext.Comparisons.Include(x => x.Items).FirstOrDefault(x => x.UserId == userId);
        }
    }
}