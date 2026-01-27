using Microsoft.EntityFrameworkCore;
using stepik.Db.Interfaces;
using stepik.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stepik.Db.Repositories
{
    public class CartsDbRepository : ICartsRepository
    {
        private readonly DatabaseContext _databaseContext;

        public CartsDbRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Cart? TryGetByUserId(string? userId, string? guestId)
        {
            return _databaseContext.Carts
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .FirstOrDefault(x => (userId != null && x.UserId == userId) ||
                                     (guestId != null && x.GuestId == guestId));
        }

        public void Add(Product product, string? userId, string? guestId)
        {
            var existingCart = TryGetByUserId(userId, guestId);

            if (existingCart == null)
            {
                existingCart = new Cart()
                {
                    UserId = userId,
                    GuestId = userId == null ? guestId : null, 
                    Items = new List<CartItem>()
                };
                _databaseContext.Carts.Add(existingCart);
            }

            var existingCartItem = existingCart.Items
                .FirstOrDefault(item => item.Product.Id == product.Id);

            if (existingCartItem == null)
            {
                existingCart.Items.Add(new CartItem
                {
                    Product = product,
                    Quantity = 1,
                    Cart = existingCart
                });
            }
            else
            {
                existingCartItem.Quantity++;
            }

            _databaseContext.SaveChanges();
        }

        public void Decrease(int productId, string? userId, string? guestId)
        {
            var existingCart = TryGetByUserId(userId, guestId);

            var existingCartItem = existingCart?.Items
                .FirstOrDefault(item => item.Product.Id == productId);

            if (existingCartItem == null)
            {
                return;
            }

            existingCartItem.Quantity--;

            if (existingCartItem.Quantity == 0)
            {
                existingCart?.Items.Remove(existingCartItem);
            }

            _databaseContext.SaveChanges();  
        }

        public void Clear(string? userId, string? guestId)
        {
            var existingCart = TryGetByUserId(userId, guestId);

            if (existingCart != null)
            {
                _databaseContext.Carts.Remove(existingCart);

                _databaseContext.SaveChanges(); 
            }
        }
        public void Merge(string guestId, string userId)
        {
            var guestCart = _databaseContext.Carts
                .Include(x => x.Items)
                .FirstOrDefault(x => x.GuestId == guestId);
            var userCart = _databaseContext.Carts
                .Include(x => x.Items)
                .FirstOrDefault(x => x.UserId == userId);
            if (guestCart == null) return; 
            if (userCart == null)
            {
                guestCart.UserId = userId;
                guestCart.GuestId = null;
            }
            else
            {
                foreach (var guestItem in guestCart.Items)
                {
                    var userItem = userCart.Items
                        .FirstOrDefault(x => x.Product.Id == guestItem.Product.Id);
                    if (userItem != null)
                    {
                        userItem.Quantity += guestItem.Quantity;
                    }
                    else
                    {
                        guestItem.Cart = userCart;
                        userCart.Items.Add(guestItem);
                    }
                }
                _databaseContext.Carts.Remove(guestCart);
            }
            _databaseContext.SaveChanges();
        }
    }
}
