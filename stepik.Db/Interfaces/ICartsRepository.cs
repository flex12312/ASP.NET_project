using stepik.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stepik.Db.Interfaces
{
    public interface ICartsRepository
    {
        void Add(Product product, string? userId, string? guestId);
        Cart? TryGetByUserId(string? userId, string? guestId = null);
        void Decrease(int productId, string? userId, string? guestId);
        void Clear(string? userId, string? guestId);
        public void Merge(string guestId, string userId);
    }

}
