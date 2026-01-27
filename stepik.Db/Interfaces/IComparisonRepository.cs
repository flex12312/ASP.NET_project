using stepik.Db.Models;

namespace stepik.Db.Interfaces
{
    public interface IComparisonRepository
    {
        void Add(Product product, string? userId, string? guestId);
        Comparison? TryGetByUserId(string? userId, string? guestId);
        void Delete(int productId, string? userId, string? guestId);
        void Clear(string? userId, string? guestId);
        public void Merge(string? guestId, string? userId);
    }
}